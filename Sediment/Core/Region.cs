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

		private Region(World world, int x, int z) {

			regionFile = new RegionFile(string.Format(world.Info.RegionFilePathFormat, x, z));
		}
	}
}
