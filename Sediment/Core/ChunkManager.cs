using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class ChunkManager {
		private World world;
		private ChunkCache cache;

		public ChunkManager(World world) {
			this.world = world;
			cache = new ChunkCache();
		}

		public Chunk this[int x, int z] {
			get {
				Chunk chunk;
				var pos = new XZInt(x, z);
				if(!cache.TryGetValue(pos, out chunk)) {
					var region = world.RegionManager[x >> 5, z >> 5];
					using(var reader = region.CreateChunkReader(x & 0x1F, z & 0x1F)) {
						chunk = new Chunk(reader);
					}
					cache.Add(pos, chunk);
				}
				return chunk;
			}

			set { throw new NotImplementedException(); }
		}

	}
}
