using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.IO;
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

			var regionFileName = string.Format(world.Info.RegionFilePathFormat, regionX, regionZ);
			var regionFilePath = Path.Combine(world.Level.RootPath, world.Info.RegionPath, regionFileName);

			regionFile = new RegionFile(regionFilePath);
		}

		internal NBTLib.NBTReader CreateChunkReader(int localChunkX, int localChunkZ) {
			return regionFile.CreateChunkReader(localChunkX, localChunkZ);
		}

	}
}
