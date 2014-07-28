using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class World {
		public Level Level { get; private set; }
		public WorldInfo Info { get; private set; }

		public RegionManager RegionManager { get; private set; }
		public ChunkManager ChunkManager { get; private set; }
		public BlockManager BlockManager { get; private set; }

		public World(Level level, WorldInfo info) {
			this.Level = level;
			this.Info = info;

			RegionManager = new RegionManager(this);
			ChunkManager = new ChunkManager(this);
			BlockManager = new BlockManager(this);
		}

		public void Save() {
			foreach(var dirtyChunk in Info.ChunkCache.DirtyChunks) {
				dirtyChunk.Region.SaveChunk(dirtyChunk);
			}
		}
	}
}
