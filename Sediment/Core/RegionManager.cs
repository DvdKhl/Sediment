using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class RegionManager {
		private World world;

		public RegionManager(World world) {
			this.world = world;
		}

		public Region this[int x, int z] {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
	}
}
