using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class WorldManager {
		public Multiverse Multiverse { get; private set; }

		public World this[WorldInfo type] {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public WorldManager(Multiverse multiverse) {
			Multiverse = multiverse;
		}
	}


	public class WorldInfo {
		public static readonly WorldInfo Overworld = new WorldInfo("region", "r.{0}.{1}.mca", "data/villages.dat");
		public static readonly WorldInfo Netherworld = new WorldInfo("DIM-1/region", "r.{0}.{1}.mca", "data/villages_nether.dat");
		public static readonly WorldInfo TheEnd = new WorldInfo("DIM1/region", "r.{0}.{1}.mca", "data/villages_end.dat");

		public string RegionPath { get; private set; }
		public string VillagesDataPath { get; private set; }
		public string RegionFilePathFormat { get; set; }

		public WorldInfo(string regionPath, string regionFilePathFormat, string villagesDataPath) {
			this.RegionPath = regionPath;
			this.RegionFilePathFormat = regionFilePathFormat;
			this.VillagesDataPath = villagesDataPath;
		}

	}
}
