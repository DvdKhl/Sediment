using Sediment.Core;
using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment {
	public class Multiverse {
		private static Dictionary<string, Multiverse> openMultiverses = new Dictionary<string, Multiverse>();

		public MultiverseInfo Info { get; private set; }
		public string RootPath { get; private set; }

		public WorldManager WorldManager { get; private set; }
		public PlayerManager PlayerManager { get; private set; }


		private LevelFile levelFile;


		private Multiverse(string rootPath, MultiverseInfo info) {
			this.RootPath = rootPath;
			this.Info = info;

			WorldManager = new WorldManager(this);
			PlayerManager = new PlayerManager(this);

			levelFile = new LevelFile(info.LevelPath);
		}


		public Multiverse Load(string rootPath) {
			if(openMultiverses.ContainsKey(Path.GetFullPath(rootPath))) {
				throw new InvalidOperationException("Already loaded");
			}

			var multiverse = new Multiverse(rootPath, MultiverseInfo.Default);

			openMultiverses.Add(multiverse.RootPath, multiverse);

			return multiverse;
		}
	}

	public class MultiverseInfo {
		public static readonly MultiverseInfo Default = new MultiverseInfo {
			LevelPath = "level.dat",
			PlayerDataPath = "playerdata",
			StatisticsDataPath = "stats",
			FortressGenerationPath = "data/Fortress.dat",
			MineshaftGenerationPath = "data/Mineshaft.dat",
			StrongholdGenerationPath = "data/Stronghold.dat",
			VillageGenerationPath = "data/Village.dat",
		};

		public string LevelPath { get; internal set; }
		public string PlayerDataPath { get; internal set; }
		public string StatisticsDataPath { get; internal set; }
		public string VillageGenerationPath { get; internal set; }
		public string FortressGenerationPath { get; internal set; }
		public string MineshaftGenerationPath { get; internal set; }
		public string StrongholdGenerationPath { get; internal set; }
	}
}
