using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sediment {
	public static class Blocks {
		public static Dictionary<ushort, BlockInfo> ById { get; private set; }
		public static Dictionary<ushort, BlockInfo[]> ByPartialId { get; private set; }
		public static Dictionary<string, BlockInfo[]> ByName { get; private set; }

		static Blocks() {
			ById = new Dictionary<ushort, BlockInfo>();

			Action<Type, List<BlockInfo>> getBlockInfos = null;
			getBlockInfos = (type, items) => {
				items.AddRange(
					type.GetFields(BindingFlags.Public | BindingFlags.Static)
					  .Where(x => x.FieldType == typeof(BlockInfo))
					  .Select(x => (BlockInfo)x.GetValue(null))
				);

				foreach(var nestedClass in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static)) {
					getBlockInfos(nestedClass, items);
				}
			};

			var blockInfos = new List<BlockInfo>();
			getBlockInfos(typeof(Blocks), blockInfos);
			foreach(var blockInfo in blockInfos) {
				ById.Add(blockInfo.Id, blockInfo);
			}

			ByPartialId = blockInfos.GroupBy(x => (ushort)(x.Id & 0x0FFF)).ToDictionary(x => x.Key, x => x.ToArray());

			ByName = new Dictionary<string, BlockInfo[]>();
			foreach(var items in ByPartialId.Values) {
				ByName.Add(items[0].InternalName, items);
			}
		}

		public static class Wood {
			internal static readonly string InternalName = "minecraft:log";
			internal static readonly string InternalName2 = "minecraft:log2";

			private static BlockInfo Create(string species, ushort id, string internalName) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Wood (" + species + ")",
					InternalName = internalName,
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				var dataValue = (id & 0xF000) >> 12;
				if(dataValue < 4) blockInfo.Note = "Vertical";
				else if(dataValue < 8) blockInfo.Note = "Horizontal East/West";
				else if(dataValue < 12) blockInfo.Note = "Horizontal North/South";
				else blockInfo.Note = "Bark Only";

				return blockInfo;
			}

			public static class Oak {
				public static readonly BlockInfo Vertical = Create("Oak", 0x0011, InternalName);
				public static readonly BlockInfo HorizontalEastWest = Create("Oak", 0x4011, InternalName);
				public static readonly BlockInfo HorizontalNorthSouth = Create("Oak", 0x8011, InternalName);
				public static readonly BlockInfo BarkOnly = Create("Oak", 0xC011, InternalName);
			}
			public static class Spruce {
				public static readonly BlockInfo Vertical = Create("Spruce", 0x1011, InternalName);
				public static readonly BlockInfo HorizontalEastWest = Create("Spruce", 0x5011, InternalName);
				public static readonly BlockInfo HorizontalNorthSouth = Create("Spruce", 0x9011, InternalName);
				public static readonly BlockInfo BarkOnly = Create("Spruce", 0xD011, InternalName);
			}
			public static class Birch {
				public static readonly BlockInfo Vertical = Create("Birch", 0x2011, InternalName);
				public static readonly BlockInfo HorizontalEastWest = Create("Birch", 0x6011, InternalName);
				public static readonly BlockInfo HorizontalNorthSouth = Create("Birch", 0xA011, InternalName);
				public static readonly BlockInfo BarkOnly = Create("Birch", 0xE011, InternalName);
			}
			public static class Jungle {
				public static readonly BlockInfo Vertical = Create("Jungle", 0x3011, InternalName);
				public static readonly BlockInfo HorizontalEastWest = Create("Jungle", 0x7011, InternalName);
				public static readonly BlockInfo HorizontalNorthSouth = Create("Jungle", 0xB011, InternalName);
				public static readonly BlockInfo BarkOnly = Create("Jungle", 0xF011, InternalName);
			}
			public static class Acacia {
				public static readonly BlockInfo Vertical = Create("Acacia", 0x00A2, InternalName2);
				public static readonly BlockInfo HorizontalEastWest = Create("Acacia", 0x40A2, InternalName2);
				public static readonly BlockInfo HorizontalNorthSouth = Create("Acacia", 0x80A2, InternalName2);
				public static readonly BlockInfo BarkOnly = Create("Acacia", 0xC0A2, InternalName2);
			}
			public static class DarkOak {
				public static readonly BlockInfo Vertical = Create("DarkOak", 0x10A2, InternalName2);
				public static readonly BlockInfo HorizontalEastWest = Create("DarkOak", 0x50A2, InternalName2);
				public static readonly BlockInfo HorizontalNorthSouth = Create("DarkOak", 0x90A2, InternalName2);
				public static readonly BlockInfo BarkOnly = Create("DarkOak", 0xD0A2, InternalName2);
			}
		}
		public static class Leaves {
			internal static readonly string InternalName = "minecraft:leaves";
			internal static readonly string InternalName2 = "minecraft:leaves2";

			private static BlockInfo Create(string species, ushort id, string internalName) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Leaves (" + species + ")",
					InternalName = internalName,
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				var dataValue = (id & 0xF000) >> 12;
				if(dataValue < 4) blockInfo.Note = "Decaying with no checks";
				else if(dataValue < 8) blockInfo.Note = "Permanent with no checks";
				else if(dataValue < 12) blockInfo.Note = "Decaying with checks";
				else blockInfo.Note = "Permanent with checks";

				return blockInfo;
			}

			public static class Oak {
				public static readonly BlockInfo Decayable = Create("Oak", 0x0012, InternalName);
				public static readonly BlockInfo Permanent = Create("Oak", 0x4012, InternalName);
				public static readonly BlockInfo DecayableChecked = Create("Oak", 0x8012, InternalName);
				public static readonly BlockInfo PermanentChecked = Create("Oak", 0xC012, InternalName);
			}
			public static class Spruce {
				public static readonly BlockInfo Decayable = Create("Spruce", 0x1012, InternalName);
				public static readonly BlockInfo Permanent = Create("Spruce", 0x5012, InternalName);
				public static readonly BlockInfo DecayableChecked = Create("Spruce", 0x9012, InternalName);
				public static readonly BlockInfo PermanentChecked = Create("Spruce", 0xD012, InternalName);
			}
			public static class Birch {
				public static readonly BlockInfo Decayable = Create("Birch", 0x2012, InternalName);
				public static readonly BlockInfo Permanent = Create("Birch", 0x6012, InternalName);
				public static readonly BlockInfo DecayableChecked = Create("Birch", 0xA012, InternalName);
				public static readonly BlockInfo PermanentChecked = Create("Birch", 0xE012, InternalName);
			}
			public static class Jungle {
				public static readonly BlockInfo Decayable = Create("Jungle", 0x3012, InternalName);
				public static readonly BlockInfo Permanent = Create("Jungle", 0x7012, InternalName);
				public static readonly BlockInfo DecayableChecked = Create("Jungle", 0xB012, InternalName);
				public static readonly BlockInfo PermanentChecked = Create("Jungle", 0xF012, InternalName);
			}
			public static class Acacia {
				public static readonly BlockInfo Decayable = Create("Acacia", 0x00A1, InternalName2);
				public static readonly BlockInfo Permanent = Create("Acacia", 0x40A1, InternalName2);
				public static readonly BlockInfo DecayableChecked = Create("Acacia", 0x80A1, InternalName2);
				public static readonly BlockInfo PermanentChecked = Create("Acacia", 0xC0A1, InternalName2);
			}
			public static class DarkOak {
				public static readonly BlockInfo Decayable = Create("DarkOak", 0x10A1, InternalName2);
				public static readonly BlockInfo Permanent = Create("DarkOak", 0x50A1, InternalName2);
				public static readonly BlockInfo DecayableChecked = Create("DarkOak", 0x90A1, InternalName2);
				public static readonly BlockInfo PermanentChecked = Create("DarkOak", 0xD0A1, InternalName2);
			}
		}
		public static class Planks {
			private static BlockInfo Create(string woodType, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Planks (" + woodType + ")",
					InternalName = "minecraft:planks",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Oak = Create("Oak", 0x0005);
			public static readonly BlockInfo Spruce = Create("Spruce ", 0x1005);
			public static readonly BlockInfo Birch = Create("Birch", 0x2005);
			public static readonly BlockInfo Jungle = Create("Jungle", 0x3005);
			public static readonly BlockInfo Acacia = Create("Acacia", 0x4005);
			public static readonly BlockInfo DarkOak = Create("DarkOak", 0x5005);
		}
		public static class Saplings {
			private static BlockInfo Create(string species, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Sapling (" + species + ")",
					InternalName = "minecraft:sapling",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Oak = Create("Oak", 0x0006);
			public static readonly BlockInfo Spruce = Create("Spruce ", 0x1006);
			public static readonly BlockInfo Birch = Create("Birch", 0x2006);
			public static readonly BlockInfo Jungle = Create("Jungle", 0x3006);
			public static readonly BlockInfo Acacia = Create("Acacia", 0x4006);
			public static readonly BlockInfo DarkOak = Create("DarkOak", 0x5006);

		}
		public static class Stone {
			private static BlockInfo Create(string stoneType, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Stone (" + stoneType + ")",
					InternalName = "minecraft:stone",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Normal = Create("Normal", 0x0001);
			public static readonly BlockInfo Granite = Create("Granite", 0x1001);
			public static readonly BlockInfo PolishedGranite = Create("Polished Granite", 0x2001);
			public static readonly BlockInfo Diorite = Create("Diorite", 0x3001);
			public static readonly BlockInfo PolishedDiorite = Create("Polished Diorite", 0x4001);
			public static readonly BlockInfo Andesite = Create("Andesite", 0x5001);
			public static readonly BlockInfo PolishedAndesite = Create("Polished Andesite", 0x6001);

		}
		public static class Dirt {
			private static BlockInfo Create(string dirtType, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Dirt (" + dirtType + ")",
					InternalName = "minecraft:dirt",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Normal = Create("Normal", 0x0003);
			public static readonly BlockInfo CoarseDirt = Create("Coarse Dirt", 0x1003);
			public static readonly BlockInfo Podzol = Create("Podzol", 0x2003);

		}
		public static class Water {
			private static BlockInfo Create(ushort id) {
				var isFlowing = (id & 0xF) == 8;
				var isFalling = (id & 0x800) != 0;
				var level = (id & 0x700) >> 12;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = (isFlowing ? "Flowing Water" : "Stationary Water") + " (" + (isFalling ? "Falling" : level.ToString()) + ")",
					InternalName = isFlowing ? "minecraft:flowing_water" : "minecraft:water",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Flowing0 = Create(0x0008);
			public static readonly BlockInfo Flowing1 = Create(0x1008);
			public static readonly BlockInfo Flowing2 = Create(0x2008);
			public static readonly BlockInfo Flowing3 = Create(0x3008);
			public static readonly BlockInfo Flowing4 = Create(0x4008);
			public static readonly BlockInfo Flowing5 = Create(0x5008);
			public static readonly BlockInfo Flowing6 = Create(0x6008);
			public static readonly BlockInfo Flowing7 = Create(0x7008);
			public static readonly BlockInfo FlowingFalling0 = Create(0x8008);
			public static readonly BlockInfo FlowingFalling1 = Create(0x9008);
			public static readonly BlockInfo FlowingFalling2 = Create(0xA008);
			public static readonly BlockInfo FlowingFalling3 = Create(0xB008);
			public static readonly BlockInfo FlowingFalling4 = Create(0xC008);
			public static readonly BlockInfo FlowingFalling5 = Create(0xD008);
			public static readonly BlockInfo FlowingFalling6 = Create(0xE008);
			public static readonly BlockInfo FlowingFalling7 = Create(0xF008);

			public static readonly BlockInfo Stationary0 = Create(0x0009);
			public static readonly BlockInfo Stationary1 = Create(0x1009);
			public static readonly BlockInfo Stationary2 = Create(0x2009);
			public static readonly BlockInfo Stationary3 = Create(0x3009);
			public static readonly BlockInfo Stationary4 = Create(0x4009);
			public static readonly BlockInfo Stationary5 = Create(0x5009);
			public static readonly BlockInfo Stationary6 = Create(0x6009);
			public static readonly BlockInfo Stationary7 = Create(0x7009);
			public static readonly BlockInfo StationaryFalling0 = Create(0x8009);
			public static readonly BlockInfo StationaryFalling1 = Create(0x9009);
			public static readonly BlockInfo StationaryFalling2 = Create(0xA009);
			public static readonly BlockInfo StationaryFalling3 = Create(0xB009);
			public static readonly BlockInfo StationaryFalling4 = Create(0xC009);
			public static readonly BlockInfo StationaryFalling5 = Create(0xD009);
			public static readonly BlockInfo StationaryFalling6 = Create(0xE009);
			public static readonly BlockInfo StationaryFalling7 = Create(0xF009);
		}
		public static class Lava {
			private static BlockInfo Create(ushort id) {
				var isFlowing = (id & 0xF) == 0xA;
				var isFalling = (id & 0x800) != 0;
				var level = (id & 0x700) >> 12;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = (isFlowing ? "Flowing Lava" : "Stationary Lava") + " (" + (isFalling ? "Falling" : level.ToString()) + ")",
					InternalName = isFlowing ? "minecraft:flowing_lava" : "minecraft:lava",
					BlastResistance = 10,
					Hardness = 2,
					FullStackCount = 64,
				};

				return blockInfo;
			}

			public static readonly BlockInfo Flowing0 = Create(0x000A);
			public static readonly BlockInfo Flowing1 = Create(0x100A);
			public static readonly BlockInfo Flowing2 = Create(0x200A);
			public static readonly BlockInfo Flowing3 = Create(0x300A);
			public static readonly BlockInfo Flowing4 = Create(0x400A);
			public static readonly BlockInfo Flowing5 = Create(0x500A);
			public static readonly BlockInfo Flowing6 = Create(0x600A);
			public static readonly BlockInfo Flowing7 = Create(0x700A);
			public static readonly BlockInfo FlowingFalling0 = Create(0x800A);
			public static readonly BlockInfo FlowingFalling1 = Create(0x900A);
			public static readonly BlockInfo FlowingFalling2 = Create(0xA00A);
			public static readonly BlockInfo FlowingFalling3 = Create(0xB00A);
			public static readonly BlockInfo FlowingFalling4 = Create(0xC00A);
			public static readonly BlockInfo FlowingFalling5 = Create(0xD00A);
			public static readonly BlockInfo FlowingFalling6 = Create(0xE00A);
			public static readonly BlockInfo FlowingFalling7 = Create(0xF00A);

			public static readonly BlockInfo Stationary0 = Create(0x000B);
			public static readonly BlockInfo Stationary1 = Create(0x100B);
			public static readonly BlockInfo Stationary2 = Create(0x200B);
			public static readonly BlockInfo Stationary3 = Create(0x300B);
			public static readonly BlockInfo Stationary4 = Create(0x400B);
			public static readonly BlockInfo Stationary5 = Create(0x500B);
			public static readonly BlockInfo Stationary6 = Create(0x600B);
			public static readonly BlockInfo Stationary7 = Create(0x700B);
			public static readonly BlockInfo StationaryFalling0 = Create(0x800B);
			public static readonly BlockInfo StationaryFalling1 = Create(0x900B);
			public static readonly BlockInfo StationaryFalling2 = Create(0xA00B);
			public static readonly BlockInfo StationaryFalling3 = Create(0xB00B);
			public static readonly BlockInfo StationaryFalling4 = Create(0xC00B);
			public static readonly BlockInfo StationaryFalling5 = Create(0xD00B);
			public static readonly BlockInfo StationaryFalling6 = Create(0xE00B);
			public static readonly BlockInfo StationaryFalling7 = Create(0xF00B);
		}

		public class BlockInfo {
			public ushort Id { get; internal set; }
			public string Name { get; internal set; }
			public string InternalName { get; internal set; }
			public string Note { get; internal set; }

			public bool UsesEntityData { get; internal set; }
			public int Tick { get; internal set; }
			public int Luminance { get; internal set; }
			public int TransmitsLight { get; internal set; }
			public float Hardness { get; internal set; }
			public float BlastResistance { get; internal set; }
			public int FullStackCount { get; internal set; }


			public BlockInfo() {

			}
		}
	}
}
