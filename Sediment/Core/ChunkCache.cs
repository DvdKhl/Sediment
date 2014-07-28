using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class ChunkCache {
		private Dictionary<ulong, LinkedListNode<Chunk>> index;
		private LinkedList<Chunk> list;

		public int Capacity { get; private set; }

		public ChunkCache(int capacity) {
			this.Capacity = capacity;

			index = new Dictionary<ulong, LinkedListNode<Chunk>>(capacity);
			list = new LinkedList<Chunk>();
		}

		public bool TryGetValue(int x, int z, out Chunk chunk) {
			LinkedListNode<Chunk> node;
			if(!index.TryGetValue((uint)x | (ulong)z << 32, out node)) {
				chunk = null;
				return false;
			}

			chunk = node.Value;
			list.Remove(node);
			list.AddFirst(node);

			return true;
		}

		public void Add(Chunk chunk) {
			if(list.Count >= Capacity) {
				var removeChunk = list.Last.Value;
				//removeChunk.Save();
				index.Remove((uint)removeChunk.X | (ulong)removeChunk.Z << 32);
			}

			var node = list.AddFirst(chunk);
			index.Add((uint)chunk.X | (ulong)chunk.Z << 32, node);
		}

		public IEnumerable<Chunk> DirtyChunks {
			get { foreach(var chunk in list) if(chunk.IsDirty) yield return chunk; }
		}

	}
}
