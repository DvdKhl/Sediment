using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class BlockManager {
		private World world;

		public BlockManager(World world) {
			this.world = world;
		}

		public ushort this[int x, int y, int z] {
			get { return GetChunk(x, z)[x & Chunk.XMask, y, z & Chunk.ZMask]; }
			set { GetChunk(x, z)[x & Chunk.XMask, y, z & Chunk.ZMask] = value; }
		}
		
		
		private Chunk chunkCache;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Chunk GetChunk(int x, int z) {
			var chunkX = x / Chunk.BlockXCount;
			var chunkZ = z / Chunk.BlockZCount;
		
			if(chunkCache == null || chunkCache.X != chunkX || chunkCache.Z != chunkZ) {
				chunkCache = world.ChunkManager[chunkX, chunkZ];
			}
			return chunkCache;
		}

	}
}
