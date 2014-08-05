using Sediment.Internal;
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
		public void Commit() {
			throw new NotImplementedException();
		}
	}


	public class WorldInfo : Freezable {
		public static readonly WorldInfo Overworld = new WorldInfo {
			RegionPath = "region",
			RegionFilePathFormat = "r.{0}.{1}.mca",
			VillagesDataPath = "data/villages.dat",
			ChunkCache = new ChunkCache(1024)
		};
		public static readonly WorldInfo Netherworld = new WorldInfo {
			RegionPath = "DIM-1/region",
			RegionFilePathFormat = "r.{0}.{1}.mca",
			VillagesDataPath = "data/villages_nether.dat",
			ChunkCache = new ChunkCache(1024)
		};
		public static readonly WorldInfo TheEnd = new WorldInfo {
			RegionPath = "DIM1/region",
			RegionFilePathFormat = "r.{0}.{1}.mca",
			VillagesDataPath = "data/villages_end.dat",
			ChunkCache = new ChunkCache(1024)
		};

		static WorldInfo() {
			Overworld.Freeze();
			Netherworld.Freeze();
			TheEnd.Freeze();
		}

		public string RegionPath { get { return regionPath; } set { WritePreamble(); regionPath = value; } } private string regionPath;
		public string VillagesDataPath { get { return villagesDataPath; } set { WritePreamble(); villagesDataPath = value; } } private string villagesDataPath;
		public string RegionFilePathFormat { get { return regionFilePathFormat; } set { WritePreamble(); regionFilePathFormat = value; } } private string regionFilePathFormat;
		public ChunkCache ChunkCache { get { return chunkCache; } set { WritePreamble(); chunkCache = value; } } private ChunkCache chunkCache;

		public override WorldInfo UnfrozenCopy() {
			return new WorldInfo { 
				RegionPath = RegionPath,
				RegionFilePathFormat = RegionFilePathFormat,
				VillagesDataPath = VillagesDataPath, 
				ChunkCache = ChunkCache 
			};
		}
	}
}
