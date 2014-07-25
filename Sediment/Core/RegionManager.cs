using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class RegionManager {
		private World world;

		private Dictionary<XZInt, Region> regions;

		public RegionManager(World world) {
			this.world = world;

			regions = new Dictionary<XZInt, Region>();
		}

		public Region this[int x, int z] {
			get {
				Region region;
				var pos = new XZInt(x, z);
				if(!regions.TryGetValue(pos, out region)) {
					region = new Region(world, x, z);
					regions.Add(pos, region);
				}
				return region;
			}

			set { throw new NotImplementedException(); }
		}
	}
}
