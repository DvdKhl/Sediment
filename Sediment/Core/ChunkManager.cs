using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class ChunkManager {
		private World world;

		public ChunkManager(World world) {
			this.world = world;
		}

		public Chunk this[int x, int z] {
			get {
				Chunk chunk;
				if(!world.Info.ChunkCache.TryGetValue(x, z, out chunk)) {
					var region = world.RegionManager[x >> 5, z >> 5];
					using(var reader = region.CreateChunkReader(x & 0x1F, z & 0x1F)) {
						chunk = new Chunk(reader, region);
					}
					world.Info.ChunkCache.Add(chunk);
				}
				return chunk;
			}

			set { throw new NotImplementedException(); }
		}

	}
}
