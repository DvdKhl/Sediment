using NBTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBTLib {
	public static class NBTReaderEx {
		public static NBTNode TreeToStructure(this NBTReader reader) {
			var node = new NBTNode {
				Name = reader.Name,
				Type = reader.Type
			};

			node.Value = reader.Value;

			if(reader.Type == NBTType.Compound) {
				var nodes = new List<NBTNode>();
				while(reader.MoveNext() && reader.Type != NBTType.End) {
					nodes.Add(TreeToStructure(reader));
				}
				node.Value = nodes;

			} else if(reader.Type == NBTType.CompoundList) {
				var nodes = new List<NBTNode>();
				var length = (int)reader.Value;
				for(int i = 0; i < length; i++) {
					while(reader.MoveNext() && reader.Type != NBTType.End) {
						nodes.Add(TreeToStructure(reader));
					}
				}
			}

			return node;
		}
	}

	public class NBTNode {
		public string Name;
		public NBTType Type;
		public object Value;
	}
}
