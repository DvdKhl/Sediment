using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class World {
		public event EventHandler<Chunk> SavingChunk = delegate { };

		public Level Level { get; private set; }
		public WorldInfo Info { get; private set; }

		public RegionManager RegionManager { get; private set; }
		public ChunkManager ChunkManager { get; private set; }
		public BlockManager BlockManager { get; private set; }

		public World(Level level, WorldInfo info) {
			if(!info.IsFrozen) throw new ArgumentException("Not frozen", "info");

			this.Level = level;
			this.Info = info;

			RegionManager = new RegionManager(this);
			ChunkManager = new ChunkManager(this);
			BlockManager = new BlockManager(this);

			Info.ChunkCache.EvictingChunk += EvictingChunkHandler;
		}

		private void EvictingChunkHandler(object sender, Chunk chunk) { if(chunk.IsDirty) Save(); }

		public void Save() {
			foreach(var regionChunks in Info.ChunkCache.DirtyChunks.GroupBy(c => c.Region)) {
				regionChunks.Key.SaveChunks(regionChunks);
			}
		}
		public void Commit() {
			throw new NotImplementedException();
		}


		internal void OnSavingChunk(Chunk chunk) { SavingChunk(this, chunk); }
	}
}
