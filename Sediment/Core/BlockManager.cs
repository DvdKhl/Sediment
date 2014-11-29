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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return GetChunk(x, z)[x & Chunk.XMask, y, z & Chunk.ZMask]; }
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { GetChunk(x, z)[x & Chunk.XMask, y, z & Chunk.ZMask] = value; }
		}


		private Chunk chunkCache;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Chunk GetChunk(int x, int z) {
			var chunkX = x >> Chunk.XBits;
			var chunkZ = z >> Chunk.ZBits;

			if(chunkCache == null || chunkCache.X != chunkX || chunkCache.Z != chunkZ) {
				chunkCache = world.ChunkManager[chunkX, chunkZ];
			}
			return chunkCache;
		}


		public class HeightMapIndexer {
			private BlockManager blockManager;

			public HeightMapIndexer(BlockManager blockManager) { this.blockManager = blockManager; }

			public int this[int x, int z] {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get { return blockManager.GetChunk(x, z).HeightMap[x & Chunk.XMask, z & Chunk.ZMask]; }
			}

		}

		private HeightMapIndexer heightMapIndexer;
		public HeightMapIndexer HeightMap { get { return heightMapIndexer ?? (heightMapIndexer = new HeightMapIndexer(this)); } }
	}
}
