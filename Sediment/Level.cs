using NBTLib;
using Sediment.Core;
using Sediment.Core.Entities;
using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment {
	public class Level {
		public LevelInfo Info { get; private set; }
		public string RootPath { get; private set; }

		public WorldManager WorldManager { get; private set; }
		public PlayerManager PlayerManager { get; private set; }

		private List<NBTNode> unknownTags = new List<NBTNode>();

		private Level(string rootPath, LevelInfo info) {
			this.RootPath = rootPath;
			this.Info = info;

			WorldManager = new WorldManager(this);
			PlayerManager = new PlayerManager(this);


			Gameplay = new GameplaySection();
			Gamerules = new GamerulesSection();
			Weather = new WeatherSection();
			Generation = new GenerationSection();
			Border = new BorderSection();

			using(var fileStream = File.OpenRead(Path.Combine(rootPath, Info.LevelPath)))
			using(var dataStream = new Ionic.Zlib.GZipStream(fileStream, Ionic.Zlib.CompressionMode.Decompress))
			using(var reader = new NBTReader(dataStream)) {
				reader.MoveNext();
				reader.MoveNext();

				while(reader.MoveNext() && reader.Type != NBTType.End) {
					switch(reader.Name) {
						case "version": Version = (int)reader.Value; break;
						case "initialized": Generation.IsInitialized = (byte)reader.Value != 0 ? true : false; break;
						case "LevelName": Name = (string)reader.Value; break;
						case "generatorName": Generation.Name = (string)reader.Value; break;
						case "generatorVersion": Generation.Version = (int)reader.Value; break;
						case "generatorOptions": Generation.Options = (string)reader.Value; break;
						case "RandomSeed": Generation.Seed = (long)reader.Value; break;
						case "MapFeatures": Generation.AllowMapFeatures = (byte)reader.Value != 0 ? true : false; break;
						case "LastPlayed": LastPlayed = DateTimeEx.UnixTime.AddMilliseconds((long)reader.Value); break;
						case "SizeOnDisk": break;
						case "allowCommands": Gameplay.AllowCommands = (byte)reader.Value != 0 ? true : false; break;
						case "hardcore": Gameplay.IsHardcore = (byte)reader.Value != 0 ? true : false; break;
						case "GameType": Gameplay.GameType = (int)reader.Value; break;
						case "Difficulty": Gameplay.Difficulty = (byte)reader.Value; break;
						case "DifficultyLocked": Gameplay.IsDifficultyLocked = (byte)reader.Value != 0 ? true : false; break;
						case "Time": Time = (long)reader.Value; break;
						case "DayTime": DayTime = (long)reader.Value; break;
						case "SpawnX": Gameplay.SpawnX = (int)reader.Value; break;
						case "SpawnY": Gameplay.SpawnY = (int)reader.Value; break;
						case "SpawnZ": Gameplay.SpawnZ = (int)reader.Value; break;
						case "BorderCenterX": Border.CenterX = (double)reader.Value; break;
						case "BorderCenterZ": Border.CenterZ = (double)reader.Value; break;
						case "BorderSize": Border.Size = (double)reader.Value; break;
						case "BorderSafeZone": Border.SafeZone = (double)reader.Value; break;
						case "BorderWarningBlocks": Border.WarningBlocks = (double)reader.Value; break;
						case "BorderWarningTime": Border.WarningTime = (double)reader.Value; break;
						case "BorderSizeLerpTarget": Border.SizeLerpTarget = (double)reader.Value; break;
						case "BorderSizeLerpTime": Border.SizeLerpTime = (int)reader.Value; break;
						case "BorderDamagePerBlock": Border.DamagePerBlock = (double)reader.Value; break;
						case "raining": Weather.IsRaining = (byte)reader.Value != 0 ? true : false; break;
						case "rainTime": Weather.RainTime = (int)reader.Value; break;
						case "thundering": Weather.IsThundering = (byte)reader.Value != 0 ? true : false; break;
						case "thunderTime": Weather.ThunderTime = (int)reader.Value; break;
						case "clearWeatherTime": Weather.ClearTime = (int)reader.Value; break;
						case "GameRules": Gamerules.Read(reader); break;

						default: unknownTags.Add(reader.TreeToStructure()); break;
					}
				}
				reader.MoveNext();
			}
		}

		public int Version { get; set; }
		public long DayTime { get; set; }
		public long Time { get; set; }
		public DateTime LastPlayed { get; set; }
		public string Name { get; set; }
		public GameplaySection Gameplay { get; set; }
		public GamerulesSection Gamerules { get; set; }
		public WeatherSection Weather { get; set; }
		public GenerationSection Generation { get; set; }
		public BorderSection Border { get; set; }
		public Player LocalPlayer { get; set; }

		public void Save() {
			WorldManager.Save();
			//PlayerManager.Save();

			using(var fileStream = File.OpenWrite(Path.Combine(RootPath, Info.LevelPath)))
			using(var dataStream = new Ionic.Zlib.GZipStream(fileStream, Ionic.Zlib.CompressionMode.Compress)) {
				WriteTo(dataStream);
			}

		}
		public void WriteTo(Stream stream) {
			var writer = new NBTWriter(stream);

			writer.WriteCompound("", w => {
				w.WriteCompound("Data", WriteLevel);
			});
		}
		private void WriteLevel(NBTWriter writer) {
			writer.Write("version", Version);
			writer.Write("initialized", Generation.IsInitialized ? (byte)1 : (byte)0);
			writer.Write("LevelName", Name);
			writer.Write("generatorName", Generation.Name);
			writer.Write("generatorVersion", Generation.Version);
			writer.Write("generatorOptions", Generation.Options);
			writer.Write("RandomSeed", Generation.Seed);
			writer.Write("MapFeatures", Generation.AllowMapFeatures ? (byte)1 : (byte)0);
			writer.Write("LastPlayed", (long)(LastPlayed - DateTimeEx.UnixTime).TotalMilliseconds);
			writer.Write("allowCommands", Gameplay.AllowCommands ? (byte)1 : (byte)0);
			writer.Write("hardcore", Gameplay.IsHardcore ? (byte)1 : (byte)0);
			writer.Write("GameType", Gameplay.GameType);
			writer.Write("Difficulty", Gameplay.Difficulty);
			writer.Write("DifficultyLocked", Gameplay.IsDifficultyLocked ? (byte)1 : (byte)0);
			writer.Write("Time", Time);
			writer.Write("DayTime", DayTime);
			writer.Write("SpawnX", Gameplay.SpawnX);
			writer.Write("SpawnY", Gameplay.SpawnY);
			writer.Write("SpawnZ", Gameplay.SpawnZ);
			writer.Write("BorderCenterX", Border.CenterX);
			writer.Write("BorderCenterZ", Border.CenterZ);
			writer.Write("BorderSize", Border.Size);
			writer.Write("BorderSafeZone", Border.SafeZone);
			writer.Write("BorderWarningBlocks", Border.WarningBlocks);
			writer.Write("BorderWarningTime", Border.WarningTime);
			writer.Write("BorderSizeLerpTarget", Border.SizeLerpTarget);
			writer.Write("BorderSizeLerpTime", Border.SizeLerpTime);
			writer.Write("BorderDamagePerBlock", Border.DamagePerBlock);
			writer.Write("raining", Weather.IsRaining ? (byte)1 : (byte)0);
			writer.Write("rainTime", Weather.RainTime);
			writer.Write("thundering", Weather.IsThundering ? (byte)1 : (byte)0);
			writer.Write("thunderTime", Weather.ThunderTime);
			writer.Write("clearWeatherTime", Weather.ClearTime);
			writer.WriteCompound("GameRules", Gamerules.Write);

			foreach(var nodes in unknownTags) writer.Write(nodes);
		}

		#region Sections
		public class GameplaySection {
			public bool AllowCommands { get; set; }
			public byte Difficulty { get; set; }
			public bool IsDifficultyLocked { get; set; }
			public bool IsHardcore { get; set; }
			public int GameType { get; set; }

			public int SpawnX { get; set; }
			public int SpawnY { get; set; }
			public int SpawnZ { get; set; }
		}
		public class WeatherSection {
			public bool IsRaining { get; set; }
			public bool IsThundering { get; set; }
			public int ClearTime { get; set; }
			public int RainTime { get; set; }
			public int ThunderTime { get; set; }

		}
		public class GenerationSection {
			public string Name { get; set; }
			public string Options { get; set; }
			public long Seed { get; set; }
			public bool IsInitialized { get; set; }
			public bool AllowMapFeatures { get; set; }
			public int Version { get; set; }
		}
		public class BorderSection {
			public int SizeLerpTime { get; set; }
			public double CenterX { get; set; }
			public double CenterZ { get; set; }
			public double DamagePerBlock { get; set; }
			public double SafeZone { get; set; }
			public double Size { get; set; }
			public double SizeLerpTarget { get; set; }
			public double WarningBlocks { get; set; }
			public double WarningTime { get; set; }
		}
		public class GamerulesSection {
			public List<NBTNode> UnknownTags { get; private set; }

			public string CommandBlockOutput { get; set; }
			public string DoDaylightCycle { get; set; }
			public string DoFireTick { get; set; }
			public string DoMobLoot { get; set; }
			public string DoMobSpawning { get; set; }
			public string DoTileDrops { get; set; }
			public string KeepInventory { get; set; }
			public string LogAdminCommands { get; set; }
			public string MobGriefing { get; set; }
			public string NaturalRegeneration { get; set; }
			public string RandomTickSpeed { get; set; }
			public string SendCommandFeedback { get; set; }
			public string ShowDeathMessages { get; set; }

			public GamerulesSection() {
				UnknownTags = new List<NBTNode>();
			}

			internal void Read(NBTReader reader) {
				while(reader.MoveNext() && reader.Type != NBTType.End) {
					switch(reader.Name) {
						case "commandBlockOutput": CommandBlockOutput = (string)reader.Value; break;
						case "doDaylightCycle": DoDaylightCycle = (string)reader.Value; break;
						case "doFireTick": DoFireTick = (string)reader.Value; break;
						case "doMobLoot": DoMobLoot = (string)reader.Value; break;
						case "doMobSpawning": DoMobSpawning = (string)reader.Value; break;
						case "doTileDrops": DoTileDrops = (string)reader.Value; break;
						case "keepInventory": KeepInventory = (string)reader.Value; break;
						case "logAdminCommands": LogAdminCommands = (string)reader.Value; break;
						case "mobGriefing": MobGriefing = (string)reader.Value; break;
						case "naturalRegeneration": NaturalRegeneration = (string)reader.Value; break;
						case "randomTickSpeed": RandomTickSpeed = (string)reader.Value; break;
						case "sendCommandFeedback": SendCommandFeedback = (string)reader.Value; break;
						case "showDeathMessages": ShowDeathMessages = (string)reader.Value; break;
						default: UnknownTags.Add(reader.TreeToStructure()); break;
					}
				}
			}

			internal void Write(NBTWriter writer) {
				writer.Write("commandBlockOutput", CommandBlockOutput);
				writer.Write("doDaylightCycle", DoDaylightCycle);
				writer.Write("doFireTick", DoFireTick);
				writer.Write("doMobLoot", DoMobLoot);
				writer.Write("doMobSpawning", DoMobSpawning);
				writer.Write("doTileDrops", DoTileDrops);
				writer.Write("keepInventory", KeepInventory);
				writer.Write("logAdminCommands", LogAdminCommands);
				writer.Write("mobGriefing", MobGriefing);
				writer.Write("naturalRegeneration", NaturalRegeneration);
				writer.Write("randomTickSpeed", RandomTickSpeed);
				writer.Write("sendCommandFeedback", SendCommandFeedback);
				writer.Write("showDeathMessages", ShowDeathMessages);
				foreach(var unknownTag in UnknownTags) writer.Write(unknownTag);
			}
		}
		#endregion

		private static Dictionary<string, Level> levels = new Dictionary<string, Level>();

		public static Level Load(string rootPath) {
			if(levels.ContainsKey(Path.GetFullPath(rootPath))) {
				throw new InvalidOperationException("Already loaded");
			}

			var level = new Level(rootPath, LevelInfo.Default);

			levels.Add(level.RootPath, level);

			return level;
		}

		public static Level Create(string rootPath) {
			using(var memStream = new MemoryStream()) {
				var writer = new NBTWriter(memStream);


				using(var fileStream = File.OpenWrite(Path.Combine(rootPath, LevelInfo.Default.LevelPath)))
				using(var dataStream = new Ionic.Zlib.GZipStream(fileStream, Ionic.Zlib.CompressionMode.Compress)) {
					memStream.CopyTo(dataStream);
				}
			}

			return null;
		}
	}


	public class LevelInfo {
		public static readonly LevelInfo Default = new LevelInfo {
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
