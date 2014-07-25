using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class World {
		public Multiverse Multiverse { get; private set; }
		public WorldInfo Info { get; private set; }

		public RegionManager RegionManager { get; private set; }
		public ChunkManager ChunkManager { get; private set; }
		public BlockManager BlockManager { get; private set; }

		public World(Multiverse multiverse, WorldInfo info) {
			this.Multiverse = multiverse;
			this.Info = info;

			RegionManager = new RegionManager(this);
			ChunkManager = new ChunkManager(this);
			BlockManager = new BlockManager(this);
		}
	}
}
