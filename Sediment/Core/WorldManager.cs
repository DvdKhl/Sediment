using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class WorldManager {
		public Level Level { get; private set; }

		private Dictionary<WorldInfo, World> openWorlds;

		public World this[WorldInfo info] {
			get {
				World world;
				if(!openWorlds.TryGetValue(info, out world)) {
					world = new World(Level, info);
					openWorlds.Add(info, world);
				}
				return world;
			}

			set { throw new NotImplementedException(); }
		}

		public WorldManager(Level level) {
			Level = level;

			openWorlds = new Dictionary<WorldInfo, World>();
		}

		public void Save() {
			foreach(var world in openWorlds.Values) world.Save();
		}
	}


	public class WorldInfo {
		public static readonly WorldInfo Overworld = new WorldInfo("region", "r.{0}.{1}.mca", "data/villages.dat", new ChunkCache(1024));
		public static readonly WorldInfo Netherworld = new WorldInfo("DIM-1/region", "r.{0}.{1}.mca", "data/villages_nether.dat", new ChunkCache(1024));
		public static readonly WorldInfo TheEnd = new WorldInfo("DIM1/region", "r.{0}.{1}.mca", "data/villages_end.dat", new ChunkCache(1024));

		public string RegionPath { get; private set; }
		public string VillagesDataPath { get; private set; }
		public string RegionFilePathFormat { get; private set; }
		public ChunkCache ChunkCache { get; private set; }

		public WorldInfo WithChunkCache(ChunkCache chunkCache) {
			return new WorldInfo(RegionPath, RegionFilePathFormat, VillagesDataPath, chunkCache);
		}

		public WorldInfo(string regionPath, string regionFilePathFormat, string villagesDataPath, ChunkCache chunkCache) {
			this.RegionPath = regionPath;
			this.RegionFilePathFormat = regionFilePathFormat;
			this.VillagesDataPath = villagesDataPath;
			this.ChunkCache = chunkCache;
		}
	}
}
