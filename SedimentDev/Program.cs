using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SedimentDev {
	class Program {
		static void Main(string[] args) {

			var result = System.Text.RegularExpressions.Regex.Replace(System.IO.File.ReadAllText(@"D:\My Stuff\Projects\visual studio 2013\Projects\Sediment\SedimentDev\GenerateBlocksClass\Template.xml"), @"0x\d\d", m => {
				var tmp = (string)m.Value; //TODO
				if(tmp != null && tmp.StartsWith("0x")) {
					tmp = ushort.Parse(tmp.Substring(2), System.Globalization.NumberStyles.HexNumber).ToString();
				}
				return tmp;
			});

			var gen = new GenerateBlocksClass(XElement.Load("GenerateBlocksClass/Template.xml"), XElement.Load("GenerateBlocksClass/MinecraftExport.xml"));

		}
	}
}
