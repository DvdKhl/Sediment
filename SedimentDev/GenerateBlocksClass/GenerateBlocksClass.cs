﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SedimentDev {
	public class GenerateBlocksClass {
		private Dictionary<ushort, XElement> blocks;

		public GenerateBlocksClass(XElement template, XElement blockInformation) {
			var groups = template.Elements().Select(e => new TplGroup(e)).ToArray();

			blocks = blockInformation.Elements().ToDictionary(x => (ushort)(int)x.Element("Id"));

			foreach(var group in groups) {
				Evaluate(group);
			}
		}

		private Stack<TplGroup> parents = new Stack<TplGroup>();

		private void Evaluate(TplGroup node) {
			parents.Push(node);

			foreach(var child in node.Children) {
				if(child is TplGroup) Evaluate((TplGroup)child);
				else if(child is TplOnBlockInfo) Evaluate((TplOnBlockInfo)child);
				else if(child is TplBlockInfoCollection) Evaluate((TplBlockInfoCollection)child);
				else if(child is TplBlockInfo) Evaluate((TplBlockInfo)child);
				else if(child is TplInclude) Evaluate((TplInclude)child);
				else if(child is TplFor) Evaluate((TplFor)child);
				else if(child is TplDataValueRange) Evaluate((TplDataValueRange)child);
			}
		}
		private void Evaluate(TplOnBlockInfo node) {
			parents.Peek().Expressions.AddRange(node.Expressions);
		}
		private void Evaluate(TplBlockInfoCollection node) {
			parents.Peek().Collections[node.Key] = node.Items;
		}
		private void Evaluate(TplInclude node) {
			var collection = parents.First(x => x.Collections.ContainsKey(node.Key)).Collections[node.Key];
			foreach(var item in collection) Evaluate(item);
		}
		private void Evaluate(TplDataValueRange node) {
			var i = (byte)node.Start;
			foreach(var name in node.Names) {
				Evaluate(new TplBlockInfo { Name = name, DataValue = i++ });
			}
		}
		private void Evaluate(TplFor node) {
			var count = node.Body.Expressions.Count;

			parents.Peek().Expressions.AddRange(node.Body.Expressions);
			for(byte i = 0; i < (byte)node.Count; i++) {
				parents.Peek().forIndex = i;
				Evaluate(new TplBlockInfo());
			}
			parents.Peek().forIndex = null;

			parents.Peek().Expressions.RemoveRange(parents.Peek().Expressions.Count - count, count);
		}
		private void Evaluate(TplBlockInfo node) {
			var blockInfo = new BlockInfo();
			blockInfo.Name = node.Name;
			blockInfo.Note = node.Note;
			if(node.BlockId.HasValue) blockInfo.Id = node.BlockId.Value;
			if(node.DataValue.HasValue) blockInfo.Id |= (ushort)(node.DataValue.Value << 12);

			foreach(var parent in parents.Reverse()) {
				foreach(var pair in parent.Expressions) {
					pair.Expression.EvaluateFunction += OnNCalcMethod;

					pair.Expression.Parameters["blockInfoName"] = blockInfo.Name;
					pair.Expression.Parameters["dataValue"] = blockInfo.Id >> 12;
					pair.Expression.Parameters["note"] = blockInfo.Note;
					if(parent.forIndex.HasValue) pair.Expression.Parameters["i"] = parent.forIndex;

					switch(pair.VarName) {
						case "Name": blockInfo.Name = (string)pair.Expression.Evaluate(); break;
						case "BlockId": blockInfo.Id = (ushort)(blockInfo.Id & 0xF000 | (ushort)(int)pair.Expression.Evaluate()); break;
						case "DataValue": blockInfo.Id = (ushort)(blockInfo.Id & 0x0FFF | ((byte)(int)pair.Expression.Evaluate() << 12)); break;
						case "Note": blockInfo.Note = (string)pair.Expression.Evaluate(); break;

						default: throw new InvalidOperationException();
					}
				}
			}

			var exportBlockInfo = blocks[(ushort)(blockInfo.Id & 0xFFF)];
			blockInfo.InternalName = (string)exportBlockInfo.Element("InternalName");
			blockInfo.UsesEntityData = (bool)exportBlockInfo.Element("HasTileEntity");
			blockInfo.Luminance = (int)exportBlockInfo.Element("LightValue");
			blockInfo.Opacity = (int)exportBlockInfo.Element("LightOpacity");
			blockInfo.Hardness = (float)exportBlockInfo.Element("Hardness");
			blockInfo.BlastResistance = (float)exportBlockInfo.Element("BlastResistance");

		}

		private void OnNCalcMethod(string name, NCalc.FunctionArgs args) {
			switch(name) {
				case "ToString":
					if(args.Parameters.Length == 2) {
						var arg0 = args.Parameters[0].Evaluate();
						var arg1 = args.Parameters[1].Evaluate();

						if(arg0 is IFormattable) {
							args.HasResult = true;
							args.Result = ((IFormattable)arg0).ToString((string)arg1, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
						}

					}
					break;

				case "GetGroupName":
					if(args.Parameters.Length == 1) {
						var arg0 = args.Parameters[0].Evaluate();
						if(arg0 is Int32) {
							args.HasResult = true;
							args.Result = parents.ElementAt((int)arg0).Name;
						}
					}
					break;

				//default: throw new InvalidOperationException();
			}
		}
	}

	class BlockInfo {
		public ushort Id;
		public string Name;
		public string Note;

		public string InternalName;
		public bool UsesEntityData;
		public int Luminance;
		public int Opacity;
		public float Hardness;
		public float BlastResistance;
	}


	abstract class TplNode { }

	class TplGroup : TplNode {

		public Dictionary<string, List<TplBlockInfo>> Collections = new Dictionary<string, List<TplBlockInfo>>();
		public List<TplOnBlockInfo.Pair> Expressions = new List<TplOnBlockInfo.Pair>();
		public List<BlockInfo> Blocks = new List<BlockInfo>();
		public int? forIndex = null;

		public List<TplNode> Children = new List<TplNode>();
		public string Name;

		public TplGroup(XElement root) {
			Name = root.Attribute("Name").Value;

			foreach(var elem in root.Elements()) {
				switch(elem.Name.LocalName) {
					case "Group": Children.Add(new TplGroup(elem)); break;
					case "OnBlockInfo": Children.Add(new TplOnBlockInfo(elem)); break;
					case "BlockInfoCollection": Children.Add(new TplBlockInfoCollection(elem)); break;
					case "BlockInfo": Children.Add(new TplBlockInfo(elem)); break;
					case "Include": Children.Add(new TplInclude(elem)); break;
					case "For": Children.Add(new TplFor(elem)); break;
					case "DataValueRange": Children.Add(new TplDataValueRange(elem)); break;
					default: throw new InvalidOperationException();
				}
			}
		}
	}

	class TplOnBlockInfo : TplNode {
		public class Pair {
			public string VarName;
			public NCalc.Expression Expression;
		}

		public List<Pair> Expressions = new List<Pair>();

		public TplOnBlockInfo(XElement root) {
			foreach(var attribute in root.Attributes()) {
				Expressions.Add(new Pair {
					VarName = attribute.Name.LocalName,
					Expression = new NCalc.Expression(attribute.Value)
				});
			}
			foreach(var elem in root.Elements()) {
				Expressions.Add(new Pair {
					VarName = elem.Name.LocalName,
					Expression = new NCalc.Expression(elem.Value)
				});
			}
		}
	}
	class TplBlockInfoCollection : TplNode {
		public string Key;
		public List<TplBlockInfo> Items = new List<TplBlockInfo>();

		public TplBlockInfoCollection(XElement root) {
			Key = root.Attribute("Key").Value;

			foreach(var elem in root.Elements()) {
				Items.Add(new TplBlockInfo(elem));
			}
		}
	}
	class TplBlockInfo : TplNode {
		public string Name;
		public ushort? BlockId;
		public byte? DataValue;
		public string Note;

		public TplBlockInfo() { }
		public TplBlockInfo(XElement root) {
			var tmp = (string)root.Attribute("BlockId"); //TODO
			if(tmp != null && tmp.StartsWith("0x")) {
				tmp = ushort.Parse(tmp.Substring(2), System.Globalization.NumberStyles.HexNumber).ToString();
			}

			Name = (string)root.Attribute("Name");
			BlockId = tmp != null ? ushort.Parse(tmp) : (ushort?)null;
			DataValue = (byte?)(int?)root.Attribute("DataValue") ?? 0;
			Note = (string)root.Attribute("Note");
		}
	}
	class TplInclude : TplNode {
		public string Key;
		public TplInclude(XElement root) {
			Key = root.Attribute("Key").Value;
		}
	}
	class TplFor : TplNode {
		public int Start, Count;
		public TplOnBlockInfo Body;

		public TplFor() { }
		public TplFor(XElement root) {
			Start = (int?)root.Attribute("Start") ?? 0;
			Count = (int?)root.Attribute("Count") ?? 0;
			Body = new TplOnBlockInfo(new XElement("OnBlockInfo", root.Elements()));
		}
	}
	class TplDataValueRange : TplNode {
		public int Start;
		public List<string> Names;

		public TplDataValueRange(XElement root) {
			Start = (int?)root.Attribute("Start") ?? 0;
			Names = root.Attribute("NameList").Value.Split(',').Select(x => x.Trim()).ToList();
		}
	}
}
