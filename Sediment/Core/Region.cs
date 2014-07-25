using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class Region {
		private World world;
		private RegionFile regionFile;

		public int X { get; private set; }
		public int Z { get; private set; }

		internal Region(World world, int regionX, int regionZ) {
			this.world = world;
			this.X = regionX;
			this.Z = regionZ;

			regionFile = new RegionFile(string.Format(world.Info.RegionFilePathFormat, regionX, regionZ));
		}
	}
}
