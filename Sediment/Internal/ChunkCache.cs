using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Internal {
	internal class ChunkCache { //TODO
		public event EventHandler EvictRequest;

		private Dictionary<XZInt, Chunk> items;

		public ChunkCache() {
			items = new Dictionary<XZInt, Chunk>();
		}

		public bool TryGetValue(XZInt pos, out Chunk region) {
			return items.TryGetValue(pos, out region);
		}

		public void Add(XZInt pos, Chunk chunk) {
			items.Add(pos, chunk);
		}
	}
}
