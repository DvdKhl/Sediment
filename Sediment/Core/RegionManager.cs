using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class RegionManager {
		private World world;

		private Dictionary<ulong, Region> regions;

		public IEnumerable<Region> TouchedRegions { get { return regions.Values; } }

		public RegionManager(World world) {
			this.world = world;

			regions = new Dictionary<ulong, Region>();
		}


		internal void Commit() {
			foreach(var region in regions.Values) {
				region.Commit();
			}
		}

		public Region this[int x, int z] {
			get {
				Region region;
				if(!regions.TryGetValue((uint)x | (ulong)z << 32, out region)) {
					region = new Region(world, x, z);
					regions.Add((uint)x | (ulong)z << 32, region);
				}
				return region;
			}
		}
	}
}
