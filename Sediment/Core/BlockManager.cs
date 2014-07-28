using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class BlockManager {
		private World world;

		public BlockManager(World world) {
			this.world = world;
		}

		public ushort this[int x, int y, int z] {
			get {
				var chunk = world.ChunkManager[x / Chunk.BlockXCount, z / Chunk.BlockZCount];
				return chunk[x & Chunk.XMask, y, z & Chunk.ZMask];
			}
			set {
				var chunk = world.ChunkManager[x / Chunk.BlockXCount, z / Chunk.BlockZCount];
				chunk[x & Chunk.XMask, y, z & Chunk.ZMask] = value;
			}
		}
	}
}
