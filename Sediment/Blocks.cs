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
		public static class Sapling {
			private static BlockInfo Create(string species, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Sapling (" + species + ")",
					InternalName = "minecraft:sapling",
					BlastResistance = 10,
					Hardness = 2,

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

				};

				return blockInfo;
			}

			public static class Flowing {
				public static readonly BlockInfo Horizontal0 = Create(0x0008);
				public static readonly BlockInfo Horizontal1 = Create(0x1008);
				public static readonly BlockInfo Horizontal2 = Create(0x2008);
				public static readonly BlockInfo Horizontal3 = Create(0x3008);
				public static readonly BlockInfo Horizontal4 = Create(0x4008);
				public static readonly BlockInfo Horizontal5 = Create(0x5008);
				public static readonly BlockInfo Horizontal6 = Create(0x6008);
				public static readonly BlockInfo Horizontal7 = Create(0x7008);
				public static readonly BlockInfo Vertical0 = Create(0x8008);
				public static readonly BlockInfo Vertical1 = Create(0x9008);
				public static readonly BlockInfo Vertical2 = Create(0xA008);
				public static readonly BlockInfo Vertical3 = Create(0xB008);
				public static readonly BlockInfo Vertical4 = Create(0xC008);
				public static readonly BlockInfo Vertical5 = Create(0xD008);
				public static readonly BlockInfo Vertical6 = Create(0xE008);
				public static readonly BlockInfo Vertical7 = Create(0xF008);
			}
			public static class Stationary {
				public static readonly BlockInfo Horizontal0 = Create(0x0009);
				public static readonly BlockInfo Horizontal1 = Create(0x1009);
				public static readonly BlockInfo Horizontal2 = Create(0x2009);
				public static readonly BlockInfo Horizontal3 = Create(0x3009);
				public static readonly BlockInfo Horizontal4 = Create(0x4009);
				public static readonly BlockInfo Horizontal5 = Create(0x5009);
				public static readonly BlockInfo Horizontal6 = Create(0x6009);
				public static readonly BlockInfo Horizontal7 = Create(0x7009);
				public static readonly BlockInfo Vertical0 = Create(0x8009);
				public static readonly BlockInfo Vertical1 = Create(0x9009);
				public static readonly BlockInfo Vertical2 = Create(0xA009);
				public static readonly BlockInfo Vertical3 = Create(0xB009);
				public static readonly BlockInfo Vertical4 = Create(0xC009);
				public static readonly BlockInfo Vertical5 = Create(0xD009);
				public static readonly BlockInfo Vertical6 = Create(0xE009);
				public static readonly BlockInfo Vertical7 = Create(0xF009);
			}
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

				};

				return blockInfo;
			}

			public static class Flowing {
				public static readonly BlockInfo Horizontal0 = Create(0x000A);
				public static readonly BlockInfo Horizontal1 = Create(0x100A);
				public static readonly BlockInfo Horizontal2 = Create(0x200A);
				public static readonly BlockInfo Horizontal3 = Create(0x300A);
				public static readonly BlockInfo Horizontal4 = Create(0x400A);
				public static readonly BlockInfo Horizontal5 = Create(0x500A);
				public static readonly BlockInfo Horizontal6 = Create(0x600A);
				public static readonly BlockInfo Horizontal7 = Create(0x700A);
				public static readonly BlockInfo Vertical0 = Create(0x800A);
				public static readonly BlockInfo Vertical1 = Create(0x900A);
				public static readonly BlockInfo Vertical2 = Create(0xA00A);
				public static readonly BlockInfo Vertical3 = Create(0xB00A);
				public static readonly BlockInfo Vertical4 = Create(0xC00A);
				public static readonly BlockInfo Vertical5 = Create(0xD00A);
				public static readonly BlockInfo Vertical6 = Create(0xE00A);
				public static readonly BlockInfo Vertical7 = Create(0xF00A);
			}
			public static class Stationary {
				public static readonly BlockInfo Horizontal0 = Create(0x000B);
				public static readonly BlockInfo Horizontal1 = Create(0x100B);
				public static readonly BlockInfo Horizontal2 = Create(0x200B);
				public static readonly BlockInfo Horizontal3 = Create(0x300B);
				public static readonly BlockInfo Horizontal4 = Create(0x400B);
				public static readonly BlockInfo Horizontal5 = Create(0x500B);
				public static readonly BlockInfo Horizontal6 = Create(0x600B);
				public static readonly BlockInfo Horizontal7 = Create(0x700B);
				public static readonly BlockInfo Vertical0 = Create(0x800B);
				public static readonly BlockInfo Vertical1 = Create(0x900B);
				public static readonly BlockInfo Vertical2 = Create(0xA00B);
				public static readonly BlockInfo Vertical3 = Create(0xB00B);
				public static readonly BlockInfo Vertical4 = Create(0xC00B);
				public static readonly BlockInfo Vertical5 = Create(0xD00B);
				public static readonly BlockInfo Vertical6 = Create(0xE00B);
				public static readonly BlockInfo Vertical7 = Create(0xF00B);
			}
		}
		public static class Sand {
			private static BlockInfo Create(string sandType, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Sand (" + sandType + ")",
					InternalName = "minecraft:sand",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Normal = Create("Normal", 0x000C);
			public static readonly BlockInfo Red = Create("Red", 0x100C);

		}
		public static class Wool {
			private static BlockInfo Create(string color, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Wool (" + color + ")",
					InternalName = "minecraft:wool",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo White = Create("White", 0x0023);
			public static readonly BlockInfo Orange = Create("Orange", 0x1023);
			public static readonly BlockInfo Magenta = Create("Magenta", 0x2023);
			public static readonly BlockInfo LightBlue = Create("Light Blue", 0x3023);
			public static readonly BlockInfo Yellow = Create("Yellow", 0x4023);
			public static readonly BlockInfo Lime = Create("Lime", 0x5023);
			public static readonly BlockInfo Pink = Create("Pink", 0x6023);
			public static readonly BlockInfo Gray = Create("Gray", 0x7023);
			public static readonly BlockInfo LightGray = Create("Light Gray", 0x8023);
			public static readonly BlockInfo Cyan = Create("Cyan", 0x9023);
			public static readonly BlockInfo Purple = Create("Purple", 0xA023);
			public static readonly BlockInfo Blue = Create("Blue", 0xB023);
			public static readonly BlockInfo Brown = Create("Brown", 0xC023);
			public static readonly BlockInfo Green = Create("Green", 0xD023);
			public static readonly BlockInfo Red = Create("Red", 0xE023);
			public static readonly BlockInfo Black = Create("Black", 0xF023);
		}
		public static class StainedClay {
			private static BlockInfo Create(string color, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Stained Clay (" + color + ")",
					InternalName = "minecraft:stained_hardened_clay",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo White = Create("White", 0x009F);
			public static readonly BlockInfo Orange = Create("Orange", 0x109F);
			public static readonly BlockInfo Magenta = Create("Magenta", 0x209F);
			public static readonly BlockInfo LightBlue = Create("Light Blue", 0x309F);
			public static readonly BlockInfo Yellow = Create("Yellow", 0x409F);
			public static readonly BlockInfo Lime = Create("Lime", 0x509F);
			public static readonly BlockInfo Pink = Create("Pink", 0x609F);
			public static readonly BlockInfo Gray = Create("Gray", 0x709F);
			public static readonly BlockInfo LightGray = Create("Light Gray", 0x809F);
			public static readonly BlockInfo Cyan = Create("Cyan", 0x909F);
			public static readonly BlockInfo Purple = Create("Purple", 0xA09F);
			public static readonly BlockInfo Blue = Create("Blue", 0xB09F);
			public static readonly BlockInfo Brown = Create("Brown", 0xC09F);
			public static readonly BlockInfo Green = Create("Green", 0xD09F);
			public static readonly BlockInfo Red = Create("Red", 0xE09F);
			public static readonly BlockInfo Black = Create("Black", 0xF09F);
		}
		public static class StainedGlass {
			private static BlockInfo Create(string color, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Stained Glass (" + color + ")",
					InternalName = "minecraft:stained_glass",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo White = Create("White", 0x005F);
			public static readonly BlockInfo Orange = Create("Orange", 0x105F);
			public static readonly BlockInfo Magenta = Create("Magenta", 0x205F);
			public static readonly BlockInfo LightBlue = Create("Light Blue", 0x305F);
			public static readonly BlockInfo Yellow = Create("Yellow", 0x405F);
			public static readonly BlockInfo Lime = Create("Lime", 0x505F);
			public static readonly BlockInfo Pink = Create("Pink", 0x605F);
			public static readonly BlockInfo Gray = Create("Gray", 0x705F);
			public static readonly BlockInfo LightGray = Create("Light Gray", 0x805F);
			public static readonly BlockInfo Cyan = Create("Cyan", 0x905F);
			public static readonly BlockInfo Purple = Create("Purple", 0xA05F);
			public static readonly BlockInfo Blue = Create("Blue", 0xB05F);
			public static readonly BlockInfo Brown = Create("Brown", 0xC05F);
			public static readonly BlockInfo Green = Create("Green", 0xD05F);
			public static readonly BlockInfo Red = Create("Red", 0xE05F);
			public static readonly BlockInfo Black = Create("Black", 0xF05F);
		}
		public static class StainedGlassPane {
			private static BlockInfo Create(string color, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Stained Glass Pane (" + color + ")",
					InternalName = "minecraft:stained_glass_pane",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo White = Create("White", 0x00A0);
			public static readonly BlockInfo Orange = Create("Orange", 0x10A0);
			public static readonly BlockInfo Magenta = Create("Magenta", 0x20A0);
			public static readonly BlockInfo LightBlue = Create("Light Blue", 0x30A0);
			public static readonly BlockInfo Yellow = Create("Yellow", 0x40A0);
			public static readonly BlockInfo Lime = Create("Lime", 0x50A0);
			public static readonly BlockInfo Pink = Create("Pink", 0x60A0);
			public static readonly BlockInfo Gray = Create("Gray", 0x70A0);
			public static readonly BlockInfo LightGray = Create("Light Gray", 0x80A0);
			public static readonly BlockInfo Cyan = Create("Cyan", 0x90A0);
			public static readonly BlockInfo Purple = Create("Purple", 0xA0A0);
			public static readonly BlockInfo Blue = Create("Blue", 0xB0A0);
			public static readonly BlockInfo Brown = Create("Brown", 0xC0A0);
			public static readonly BlockInfo Green = Create("Green", 0xD0A0);
			public static readonly BlockInfo Red = Create("Red", 0xE0A0);
			public static readonly BlockInfo Black = Create("Black", 0xF0A0);
		}
		public static class Carpet {
			private static BlockInfo Create(string color, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Carpet (" + color + ")",
					InternalName = "minecraft:carpet",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo White = Create("White", 0x00AB);
			public static readonly BlockInfo Orange = Create("Orange", 0x10AB);
			public static readonly BlockInfo Magenta = Create("Magenta", 0x20AB);
			public static readonly BlockInfo LightBlue = Create("Light Blue", 0x30AB);
			public static readonly BlockInfo Yellow = Create("Yellow", 0x40AB);
			public static readonly BlockInfo Lime = Create("Lime", 0x50AB);
			public static readonly BlockInfo Pink = Create("Pink", 0x60AB);
			public static readonly BlockInfo Gray = Create("Gray", 0x70AB);
			public static readonly BlockInfo LightGray = Create("Light Gray", 0x80AB);
			public static readonly BlockInfo Cyan = Create("Cyan", 0x90AB);
			public static readonly BlockInfo Purple = Create("Purple", 0xA0AB);
			public static readonly BlockInfo Blue = Create("Blue", 0xB0AB);
			public static readonly BlockInfo Brown = Create("Brown", 0xC0AB);
			public static readonly BlockInfo Green = Create("Green", 0xD0AB);
			public static readonly BlockInfo Red = Create("Red", 0xE0AB);
			public static readonly BlockInfo Black = Create("Black", 0xF0AB);
		}
		public static class Torch {
			private static BlockInfo Create(ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Torch",
					InternalName = "minecraft:torch",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo East = Create(0x1032);
			public static readonly BlockInfo West = Create(0x2032);
			public static readonly BlockInfo South = Create(0x3032);
			public static readonly BlockInfo North = Create(0x4032);
			public static readonly BlockInfo Standing = Create(0x5032);

		}
		public static class RedstoneTorch {
			private static BlockInfo Create(ushort id) {
				var isLit = (id & 0xFFF) == 0x4C;
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Redstone Torch " + (isLit ? "(Active)" : "(Inactive)"),
					InternalName = isLit ? "minecraft:lit_redstone_torch" : "minecraft:unlit_redstone_torch",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Lit {
				public static readonly BlockInfo East = Create(0x104C);
				public static readonly BlockInfo West = Create(0x204C);
				public static readonly BlockInfo South = Create(0x304C);
				public static readonly BlockInfo North = Create(0x404C);
				public static readonly BlockInfo Standing = Create(0x504C);
			}
			public static class Unlit {
				public static readonly BlockInfo East = Create(0x104B);
				public static readonly BlockInfo West = Create(0x204B);
				public static readonly BlockInfo South = Create(0x304B);
				public static readonly BlockInfo North = Create(0x404B);
				public static readonly BlockInfo Standing = Create(0x504B);
			}
		}
		public static class Slab {
			private static BlockInfo Create(string type, ushort id) {
				var isSingle = (id & 0xFFF) == 0x7E || (id & 0xFFF) == 0x2C;
				var isWooden = (id & 0xFFF) == 0x7D || (id & 0xFFF) == 0x7E;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = (isWooden ? "Wooden " : "Stone ") + (isSingle ? "" : "Double ") + "Slab (" + type + ")",
					InternalName = "minecraft:" + (isWooden ? "wooden" : "stone") + (isSingle ? "" : "_double") + "_slab",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Stone {
				public static class Single {
					public static readonly BlockInfo Normal = Create("Normal", 0x002C);
					public static readonly BlockInfo Sandstone = Create("Sandstone", 0x102C);
					public static readonly BlockInfo Wooden = Create("Wooden", 0x202C);
					public static readonly BlockInfo Cobblestone = Create("Cobblestone", 0x302C);
					public static readonly BlockInfo Brick = Create("Brick", 0x402C);
					public static readonly BlockInfo StoneBrick = Create("Stone Brick", 0x502C);
					public static readonly BlockInfo NetherBrick = Create("Nether Brick", 0x602C);
					public static readonly BlockInfo Quartz = Create("Quartz", 0x702C);
				}
				public static class Double {
					public static readonly BlockInfo Normal = Create("Normal", 0x002B);
					public static readonly BlockInfo Sandstone = Create("Sandstone ", 0x102B);
					public static readonly BlockInfo Wooden = Create("Wooden", 0x202B);
					public static readonly BlockInfo Cobblestone = Create("Cobblestone", 0x302B);
					public static readonly BlockInfo Brick = Create("Brick", 0x402B);
					public static readonly BlockInfo StoneBrick = Create("Stone Brick", 0x502B);
					public static readonly BlockInfo NetherBrick = Create("Nether Brick", 0x602B);
					public static readonly BlockInfo Quartz = Create("Quartz", 0x702B);
					public static readonly BlockInfo SmoothStone = Create("Quartz", 0x802B);
					public static readonly BlockInfo SmoothSandstone = Create("Quartz", 0x902B);
					public static readonly BlockInfo TileQuartz = Create("Quartz", 0xA02B);
				}
			}
			public static class Wood {
				public static class Single {
					public static readonly BlockInfo Oak = Create("Oak", 0x007E);
					public static readonly BlockInfo Spruce = Create("Spruce ", 0x107E);
					public static readonly BlockInfo Birch = Create("Birch", 0x207E);
					public static readonly BlockInfo Jungle = Create("Jungle", 0x307E);
					public static readonly BlockInfo Acacia = Create("Acacia", 0x407E);
					public static readonly BlockInfo DarkOak = Create("DarkOak", 0x507E);
				}
				public static class Double {
					public static readonly BlockInfo Oak = Create("Oak", 0x007D);
					public static readonly BlockInfo Spruce = Create("Spruce ", 0x107D);
					public static readonly BlockInfo Birch = Create("Birch", 0x207D);
					public static readonly BlockInfo Jungle = Create("Jungle", 0x307D);
					public static readonly BlockInfo Acacia = Create("Acacia", 0x407D);
					public static readonly BlockInfo DarkOak = Create("DarkOak", 0x507D);
				}
			}
		}
		public static class Fire {
			private static BlockInfo Create(ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Fire",
					InternalName = "minecraft:fire",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Tick0 = Create(0x0033);
			public static readonly BlockInfo Tick1 = Create(0x1033);
			public static readonly BlockInfo Tick2 = Create(0x2033);
			public static readonly BlockInfo Tick3 = Create(0x3033);
			public static readonly BlockInfo Tick4 = Create(0x4033);
			public static readonly BlockInfo Tick5 = Create(0x5033);
			public static readonly BlockInfo Tick6 = Create(0x6033);
			public static readonly BlockInfo Tick7 = Create(0x7033);
			public static readonly BlockInfo Tick8 = Create(0x8033);
			public static readonly BlockInfo Tick9 = Create(0x9033);
			public static readonly BlockInfo TickA = Create(0xA033);
			public static readonly BlockInfo TickB = Create(0xB033);
			public static readonly BlockInfo TickC = Create(0xC033);
			public static readonly BlockInfo TickD = Create(0xD033);
			public static readonly BlockInfo TickE = Create(0xE033);
			public static readonly BlockInfo TickF = Create(0xF033);
		}
		public static class Sandstone {
			private static BlockInfo Create(string type, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Sandstone (" + type + ")",
					InternalName = "minecraft:sandstone",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Normal = Create("Normal", 0x0018);
			public static readonly BlockInfo Chiseled = Create("Chiseled", 0x1018);
			public static readonly BlockInfo Smooth = Create("Smooth", 0x2018);
		}
		public static class Bed {
			private static BlockInfo Create(string type, ushort id) {
				var isOccupied = (id & 0x4000) != 0;
				var isHead = (id & 0x8000) != 0;


				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Bed (" + type + (isHead ? " Head" : " Foot") + (isOccupied ? " Occupied" : " Unoccupied") + ")",
					InternalName = "minecraft:bed",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Foot {
				public static class Unoccupied {
					public static readonly BlockInfo South = Create("South", 0x001A);
					public static readonly BlockInfo West = Create("West", 0x101A);
					public static readonly BlockInfo North = Create("North", 0x201A);
					public static readonly BlockInfo East = Create("East", 0x301A);
				}
				public static class Occupied {
					public static readonly BlockInfo South = Create("South", 0x401A);
					public static readonly BlockInfo West = Create("West", 0x501A);
					public static readonly BlockInfo North = Create("North", 0x601A);
					public static readonly BlockInfo East = Create("East", 0x701A);
				}
			}
			public static class Head {
				public static class Unoccupied {
					public static readonly BlockInfo South = Create("South", 0x801A);
					public static readonly BlockInfo West = Create("West", 0x901A);
					public static readonly BlockInfo North = Create("North", 0xA01A);
					public static readonly BlockInfo East = Create("East", 0xB01A);
				}
				public static class Occupied {
					public static readonly BlockInfo South = Create("South", 0xC01A);
					public static readonly BlockInfo West = Create("West", 0xD01A);
					public static readonly BlockInfo North = Create("North", 0xE01A);
					public static readonly BlockInfo East = Create("East", 0xF01A);
				}
			}
		}
		public static class TallGrass {
			private static BlockInfo Create(string type, ushort id) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "TallGrass (" + type + ")",
					InternalName = "minecraft:tallgrass",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Shrub = Create("Shrub", 0x001F);
			public static readonly BlockInfo Normal = Create("Normal", 0x101F);
			public static readonly BlockInfo Fern = Create("Fern", 0x201F);
			public static readonly BlockInfo BiomeShrub = Create("Biome Shrub", 0x301F);
		}
		public static class Flower {
			private static BlockInfo Create(string type, ushort id) {
				var isYellowFlower = (id & 0xFFF) == 0x25;
				var isLarge = (id & 0xFFF) == 0xAF;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Flower (" + type + ")",
					InternalName = isYellowFlower ? "minecraft:yellow_flower" : (isLarge ? "minecraft:double_plant" : "minecraft:red_flower"),
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}
			public static class Small {
				public static readonly BlockInfo Dandelion = Create("Dandelion", 0x0025);
				public static readonly BlockInfo Poppy = Create("Poppy", 0x0026);
				public static readonly BlockInfo BlueOrchid = Create("Blue Orchid", 0x1026);
				public static readonly BlockInfo Allium = Create("Allium", 0x2026);
				public static readonly BlockInfo AzureBluet = Create("Azure Bluet", 0x3026);
				public static readonly BlockInfo RedTulip = Create("Red Tulip", 0x4026);
				public static readonly BlockInfo OrangeTulip = Create("Orange Tulip", 0x5026);
				public static readonly BlockInfo WhiteTulip = Create("White Tulip", 0x6026);
				public static readonly BlockInfo PinkTulip = Create("Pink Tulip", 0x7026);
				public static readonly BlockInfo OxeyeDaisy = Create("Oxeye Daisy", 0x8026);
			}
			public static class Large {
				public static readonly BlockInfo Sunflower = Create("Sunflower", 0x00AF);
				public static readonly BlockInfo Lilac = Create("Lilac", 0x10AF);
				public static readonly BlockInfo DoubleTallgrass = Create("Double Tallgrass", 0x20AF);
				public static readonly BlockInfo LargeFern = Create("Large Fern", 0x30AF);
				public static readonly BlockInfo RoseBush = Create("Rose Bush", 0x40AF);
				public static readonly BlockInfo Peony = Create("Peony", 0x50AF);
				public static readonly BlockInfo Top = Create("Top", 0x80AF);
			}
			public static class Pot {
			}
		}
		public static class Piston {
			private static BlockInfo Create(string type, ushort id) {
				var internalName = "";
				var isSticky = false;
				var isHead = false;
				var isExtension = false;

				switch(id & 0xFFF) {
					case 0x1D: internalName = "minecraft:sticky_piston"; isSticky = true; break;
					case 0x21: internalName = "minecraft:piston"; break;
					case 0x22: internalName = "minecraft:piston_head"; isHead = true; isSticky = (id & 0x800) != 0; break;
					case 0x24: internalName = "minecraft:piston_extension"; isExtension = true; break;
				}

				var blockInfo = new BlockInfo {
					Id = id,
					Name = isExtension ? "Piston Extension" : "Piston (" + type + (isHead ? " Head" : " Body") + (isSticky ? " Sticky" : "") + ")",
					InternalName = internalName,
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Normal {
				public static class Retracted {
					public static readonly BlockInfo Down = Create("Down", 0x0021);
					public static readonly BlockInfo Up = Create("Up", 0x1021);
					public static readonly BlockInfo North = Create("North", 0x2021);
					public static readonly BlockInfo South = Create("South", 0x3021);
					public static readonly BlockInfo West = Create("West", 0x4021);
					public static readonly BlockInfo East = Create("East", 0x5021);
				}
				public static class Extended {
					public static readonly BlockInfo Down = Create("Down", 0x8021);
					public static readonly BlockInfo Up = Create("Up", 0x9021);
					public static readonly BlockInfo North = Create("North", 0xA021);
					public static readonly BlockInfo South = Create("South", 0xB021);
					public static readonly BlockInfo West = Create("West", 0xC021);
					public static readonly BlockInfo East = Create("East", 0xD021);
				}
			}
			public static class Sticky {
				public static class Retracted {
					public static readonly BlockInfo Down = Create("Down", 0x001D);
					public static readonly BlockInfo Up = Create("Up", 0x101D);
					public static readonly BlockInfo North = Create("North", 0x201D);
					public static readonly BlockInfo South = Create("South", 0x301D);
					public static readonly BlockInfo West = Create("West", 0x401D);
					public static readonly BlockInfo East = Create("East", 0x501D);
				}
				public static class Extended {
					public static readonly BlockInfo Down = Create("Down", 0x801D);
					public static readonly BlockInfo Up = Create("Up", 0x901D);
					public static readonly BlockInfo North = Create("North", 0xA01D);
					public static readonly BlockInfo South = Create("South", 0xB01D);
					public static readonly BlockInfo West = Create("West", 0xC01D);
					public static readonly BlockInfo East = Create("East", 0xD01D);
				}
			}
			public static class Head {
				public static class Normal {
					public static readonly BlockInfo Down = Create("Down", 0x0022);
					public static readonly BlockInfo Up = Create("Up", 0x1022);
					public static readonly BlockInfo North = Create("North", 0x2022);
					public static readonly BlockInfo South = Create("South", 0x3022);
					public static readonly BlockInfo West = Create("West", 0x4022);
					public static readonly BlockInfo East = Create("East", 0x5022);
				}
				public static class Sticky {
					public static readonly BlockInfo Down = Create("Down", 0x8022);
					public static readonly BlockInfo Up = Create("Up", 0x9022);
					public static readonly BlockInfo North = Create("North", 0xA022);
					public static readonly BlockInfo South = Create("South", 0xB022);
					public static readonly BlockInfo West = Create("West", 0xC022);
					public static readonly BlockInfo East = Create("East", 0xD022);
				}

			}

			public static readonly BlockInfo Extension = Create(null, 0x0024);
		}
		public static class Stairs {
			private static BlockInfo Create(ushort id, string type) {
				var isUpsideDown = (id & 0x8000) != 0;
				string direction = "";
				switch((id >> 12) & 3) {
					case 0: direction = "East"; break;
					case 1: direction = "West"; break;
					case 2: direction = "South"; break;
					case 3: direction = "North"; break;
				}

				string internalType = string.Concat(type.Select((x, i) => (i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()).ToLowerInvariant()));

				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Stairs (" + direction + (isUpsideDown ? " Upright " : " UpsideDown ") + type + ")",
					InternalName = "minecraft:" + internalType + "_stairs",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Oak {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0035, "Oak");
					public static readonly BlockInfo West = Create(0x1035, "Oak");
					public static readonly BlockInfo South = Create(0x2035, "Oak");
					public static readonly BlockInfo North = Create(0x3035, "Oak");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4035, "Oak");
					public static readonly BlockInfo West = Create(0x5035, "Oak");
					public static readonly BlockInfo South = Create(0x6035, "Oak");
					public static readonly BlockInfo North = Create(0x7035, "Oak");
				}
			}
			public static class Stone {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0043, "Stone");
					public static readonly BlockInfo West = Create(0x1043, "Stone");
					public static readonly BlockInfo South = Create(0x2043, "Stone");
					public static readonly BlockInfo North = Create(0x3043, "Stone");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4043, "Stone");
					public static readonly BlockInfo West = Create(0x5043, "Stone");
					public static readonly BlockInfo South = Create(0x6043, "Stone");
					public static readonly BlockInfo North = Create(0x7043, "Stone");
				}
			}
			public static class Brick {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x006C, "Brick");
					public static readonly BlockInfo West = Create(0x106C, "Brick");
					public static readonly BlockInfo South = Create(0x206C, "Brick");
					public static readonly BlockInfo North = Create(0x306C, "Brick");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x406C, "Brick");
					public static readonly BlockInfo West = Create(0x506C, "Brick");
					public static readonly BlockInfo South = Create(0x606C, "Brick");
					public static readonly BlockInfo North = Create(0x706C, "Brick");
				}
			}
			public static class StoneBrick {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x006D, "StoneBrick");
					public static readonly BlockInfo West = Create(0x106D, "StoneBrick");
					public static readonly BlockInfo South = Create(0x206D, "StoneBrick");
					public static readonly BlockInfo North = Create(0x306D, "StoneBrick");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x406D, "StoneBrick");
					public static readonly BlockInfo West = Create(0x506D, "StoneBrick");
					public static readonly BlockInfo South = Create(0x606D, "StoneBrick");
					public static readonly BlockInfo North = Create(0x706D, "StoneBrick");
				}
			}
			public static class NetherBrick {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0072, "NetherBrick");
					public static readonly BlockInfo West = Create(0x1072, "NetherBrick");
					public static readonly BlockInfo South = Create(0x2072, "NetherBrick");
					public static readonly BlockInfo North = Create(0x3072, "NetherBrick");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4072, "NetherBrick");
					public static readonly BlockInfo West = Create(0x5072, "NetherBrick");
					public static readonly BlockInfo South = Create(0x6072, "NetherBrick");
					public static readonly BlockInfo North = Create(0x7072, "NetherBrick");
				}
			}
			public static class Sandstone {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0080, "Sandstone");
					public static readonly BlockInfo West = Create(0x1080, "Sandstone");
					public static readonly BlockInfo South = Create(0x2080, "Sandstone");
					public static readonly BlockInfo North = Create(0x3080, "Sandstone");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4080, "Sandstone");
					public static readonly BlockInfo West = Create(0x5080, "Sandstone");
					public static readonly BlockInfo South = Create(0x6080, "Sandstone");
					public static readonly BlockInfo North = Create(0x7080, "Sandstone");
				}
			}
			public static class Spruce {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0086, "Spruce");
					public static readonly BlockInfo West = Create(0x1086, "Spruce");
					public static readonly BlockInfo South = Create(0x2086, "Spruce");
					public static readonly BlockInfo North = Create(0x3086, "Spruce");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4086, "Spruce");
					public static readonly BlockInfo West = Create(0x5086, "Spruce");
					public static readonly BlockInfo South = Create(0x6086, "Spruce");
					public static readonly BlockInfo North = Create(0x7086, "Spruce");
				}
			}
			public static class Birch {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0087, "Birch");
					public static readonly BlockInfo West = Create(0x1087, "Birch");
					public static readonly BlockInfo South = Create(0x2087, "Birch");
					public static readonly BlockInfo North = Create(0x3087, "Birch");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4087, "Birch");
					public static readonly BlockInfo West = Create(0x5087, "Birch");
					public static readonly BlockInfo South = Create(0x6087, "Birch");
					public static readonly BlockInfo North = Create(0x7087, "Birch");
				}
			}
			public static class Jungle {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x0088, "Jungle");
					public static readonly BlockInfo West = Create(0x1088, "Jungle");
					public static readonly BlockInfo South = Create(0x2088, "Jungle");
					public static readonly BlockInfo North = Create(0x3088, "Jungle");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x4088, "Jungle");
					public static readonly BlockInfo West = Create(0x5088, "Jungle");
					public static readonly BlockInfo South = Create(0x6088, "Jungle");
					public static readonly BlockInfo North = Create(0x7088, "Jungle");
				}
			}
			public static class Quartz {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x009C, "Quartz");
					public static readonly BlockInfo West = Create(0x109C, "Quartz");
					public static readonly BlockInfo South = Create(0x209C, "Quartz");
					public static readonly BlockInfo North = Create(0x309C, "Quartz");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x409C, "Quartz");
					public static readonly BlockInfo West = Create(0x509C, "Quartz");
					public static readonly BlockInfo South = Create(0x609C, "Quartz");
					public static readonly BlockInfo North = Create(0x709C, "Quartz");
				}
			}
			public static class Acacia {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x00A3, "Acacia");
					public static readonly BlockInfo West = Create(0x10A3, "Acacia");
					public static readonly BlockInfo South = Create(0x20A3, "Acacia");
					public static readonly BlockInfo North = Create(0x30A3, "Acacia");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x40A3, "Acacia");
					public static readonly BlockInfo West = Create(0x50A3, "Acacia");
					public static readonly BlockInfo South = Create(0x60A3, "Acacia");
					public static readonly BlockInfo North = Create(0x70A3, "Acacia");
				}
			}
			public static class DarkOak {
				public static class Normal {
					public static readonly BlockInfo East = Create(0x00A4, "DarkOak");
					public static readonly BlockInfo West = Create(0x10A4, "DarkOak");
					public static readonly BlockInfo South = Create(0x20A4, "DarkOak");
					public static readonly BlockInfo North = Create(0x30A4, "DarkOak");
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = Create(0x40A4, "DarkOak");
					public static readonly BlockInfo West = Create(0x50A4, "DarkOak");
					public static readonly BlockInfo South = Create(0x60A4, "DarkOak");
					public static readonly BlockInfo North = Create(0x70A4, "DarkOak");
				}
			}
		}
		public static class RedstoneWire {
			private static BlockInfo Create(ushort id) {
				var power = id >> 12;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Redstone Wire (" + power + ")",
					InternalName = "minecraft:redstone_wire",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Power0 = Create(0x0037);
			public static readonly BlockInfo Power1 = Create(0x1037);
			public static readonly BlockInfo Power2 = Create(0x2037);
			public static readonly BlockInfo Power3 = Create(0x3037);
			public static readonly BlockInfo Power4 = Create(0x4037);
			public static readonly BlockInfo Power5 = Create(0x5037);
			public static readonly BlockInfo Power6 = Create(0x6037);
			public static readonly BlockInfo Power7 = Create(0x7037);
			public static readonly BlockInfo Power8 = Create(0x8037);
			public static readonly BlockInfo Power9 = Create(0x9037);
			public static readonly BlockInfo PowerA = Create(0xA037);
			public static readonly BlockInfo PowerB = Create(0xB037);
			public static readonly BlockInfo PowerC = Create(0xC037);
			public static readonly BlockInfo PowerD = Create(0xD037);
			public static readonly BlockInfo PowerE = Create(0xE037);
			public static readonly BlockInfo PowerF = Create(0xF037);
		}
		public static class Crop {
			private static BlockInfo Create(ushort id, string type) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Fire",
					InternalName = "minecraft:fire",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static class Wheat {
			}
			public static class Carrot {
			}
			public static class Potato {
			}
			public static class PumpkinStem {
			}
			public static class MelonStem {
			}
			public static class SugarCane {
			}
			public static class Cactus {
			}
			public static class NetherWart {
			}
			public static class Cocoa {
			}
			//public static readonly BlockInfo Melon = Create(0xF033, "Melon");
			//public static readonly BlockInfo Pumkin = Create(0xF033, "Pumkin");
			//public static readonly BlockInfo JackoLanters = Create(0xF033, "Pumkin");
		}
		public static class Farmland {
			private static BlockInfo Create(ushort id) {
				var wetness = id >> 12;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Farmland (" + wetness + ")",
					InternalName = "minecraft:farmland",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Wetness0 = Create(0x003C);
			public static readonly BlockInfo Wetness1 = Create(0x103C);
			public static readonly BlockInfo Wetness2 = Create(0x203C);
			public static readonly BlockInfo Wetness3 = Create(0x303C);
			public static readonly BlockInfo Wetness4 = Create(0x403C);
			public static readonly BlockInfo Wetness5 = Create(0x503C);
			public static readonly BlockInfo Wetness6 = Create(0x603C);
			public static readonly BlockInfo Wetness7 = Create(0x703C);
			public static readonly BlockInfo Wetness8 = Create(0x803C);
			public static readonly BlockInfo Wetness9 = Create(0x903C);
			public static readonly BlockInfo WetnessA = Create(0xA03C);
			public static readonly BlockInfo WetnessB = Create(0xB03C);
			public static readonly BlockInfo WetnessC = Create(0xC03C);
			public static readonly BlockInfo WetnessD = Create(0xD03C);
			public static readonly BlockInfo WetnessE = Create(0xE03C);
			public static readonly BlockInfo WetnessF = Create(0xF03C);
		}
		public static class SignPost {
			private static BlockInfo Create(ushort id, string direction) {
				var blockInfo = new BlockInfo {
					Id = id,
					Name = "Sign Post (" + direction + ")",
					InternalName = "minecraft:standing_sign",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo S = Create(0x003F, "S");
			public static readonly BlockInfo SSW = Create(0x103F, "SSW");
			public static readonly BlockInfo SW = Create(0x203F, "SW");
			public static readonly BlockInfo SWW = Create(0x303F, "SWW");
			public static readonly BlockInfo W = Create(0x403F, "W");
			public static readonly BlockInfo NWW = Create(0x503F, "NWW");
			public static readonly BlockInfo NW = Create(0x603F, "NW");
			public static readonly BlockInfo NNW = Create(0x703F, "NNW");
			public static readonly BlockInfo N = Create(0x803F, "N");
			public static readonly BlockInfo NNE = Create(0x903F, "NNE");
			public static readonly BlockInfo NE = Create(0xA03F, "NE");
			public static readonly BlockInfo NEE = Create(0xB03F, "NEE");
			public static readonly BlockInfo E = Create(0xC03F, "E");
			public static readonly BlockInfo SEE = Create(0xD03F, "SEE");
			public static readonly BlockInfo SE = Create(0xE03F, "SE");
			public static readonly BlockInfo SSE = Create(0xF03F, "SSE");
		}
		public static class Door {
			public static class Wood {
				public static class Top {
				}
				public static class Bottom {
				}
			}
			public static class Iron {
				public static class Top {
				}
				public static class Bottom {
				}
			}
		}
		public static class Rail {
			public static class Normal {
			}
			public static class Powered {
			}
			public static class Detector {
			}
			public static class Activator {
			}
		}
		public static class Ladder {
		}
		public static class WallSign {
		}
		public static class Furnace {
		}
		public static class Chest {
			public static class Normal {

			}
			public static class Trapped {

			}
			public static class Ender {

			}

		}
		public static class Dispenser {
		}
		public static class Hopper {
		}
		public static class Dropper {
		}
		public static class Lever {
		}
		public static class PressurePlate {
		}
		public static class Button {
		}
		public static class Snow {
			private static BlockInfo Create(ushort id) {
				var height = id >> 12;

				var blockInfo = new BlockInfo {
					Id = id,
					Name = id == 0x50 ? "Snow Block" : "Snow (" + height + ")",
					InternalName = id == 0x50 ? "minecraft:snow" : "minecraft:snow_layer",
					BlastResistance = 10,
					Hardness = 2,

				};

				return blockInfo;
			}

			public static readonly BlockInfo Height0 = Create(0x004E);
			public static readonly BlockInfo Height1 = Create(0x104E);
			public static readonly BlockInfo Height2 = Create(0x204E);
			public static readonly BlockInfo Height3 = Create(0x304E);
			public static readonly BlockInfo Height4 = Create(0x404E);
			public static readonly BlockInfo Height5 = Create(0x504E);
			public static readonly BlockInfo Height6 = Create(0x604E);
			public static readonly BlockInfo Height7 = Create(0x704E);
			public static readonly BlockInfo Block = Create(0x0050);
		}
		public static class Jukebox {
		}
		public static class Cake {
		}
		public static class Repeater {
			public static class Active {
			}
			public static class Inactive {
			}
		}
		public static class Comparator {
			public static class Normal {
			}
			public static class Active {
			}
		}
		public static class Trapdoor {
			public static class Wood {
			}
			public static class Iron {
			}
		}
		public static class MonsterEgg {
		}
		public static class StoneBrick {
		}
		public static class Mushroom {
			public static class Brown {
			}
			public static class Red {
			}
		}
		public static class Vine {
		}
		public static class FenceGate {
		}
		public static class BrewingStand {
		}
		public static class Cauldron {
		}
		public static class EndPortalBlock {
		}
		public static class Tripwire {
		}
		public static class Wall {
		}
		public static class Head {
		}
		public static class Quartz {
		}
		public static class Coal {
		}
		public static class Anvil {
		}

		public class BlockInfo {
			public ushort Id { get; internal set; }
			public string Name { get; internal set; }
			public string InternalName { get; internal set; }
			public string Note { get; internal set; }

			public bool UsesEntityData { get; internal set; }
			public int Luminance { get; internal set; }
			public int Opacity { get; internal set; }
			public float Hardness { get; internal set; }
			public float BlastResistance { get; internal set; }


			public BlockInfo() {

			}
		}
	}
}
