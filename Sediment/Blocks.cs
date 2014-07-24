using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
			public static class Oak {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 17, DataValue = 0, Name = "Vertical", InternalName = "minecraft:log", Note = "Oak, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 17, DataValue = 4, Name = "HorizontalEastWest", InternalName = "minecraft:log", Note = "Oak, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 17, DataValue = 8, Name = "HorizontalNorthSouth", InternalName = "minecraft:log", Note = "Oak, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 17, DataValue = 12, Name = "BarkOnly", InternalName = "minecraft:log", Note = "Oak, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
			public static class Spruce {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 17, DataValue = 1, Name = "Vertical", InternalName = "minecraft:log", Note = "Spruce, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 17, DataValue = 5, Name = "HorizontalEastWest", InternalName = "minecraft:log", Note = "Spruce, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 17, DataValue = 9, Name = "HorizontalNorthSouth", InternalName = "minecraft:log", Note = "Spruce, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 17, DataValue = 13, Name = "BarkOnly", InternalName = "minecraft:log", Note = "Spruce, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
			public static class Birch {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 17, DataValue = 2, Name = "Vertical", InternalName = "minecraft:log", Note = "Birch, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 17, DataValue = 6, Name = "HorizontalEastWest", InternalName = "minecraft:log", Note = "Birch, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 17, DataValue = 10, Name = "HorizontalNorthSouth", InternalName = "minecraft:log", Note = "Birch, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 17, DataValue = 14, Name = "BarkOnly", InternalName = "minecraft:log", Note = "Birch, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
			public static class Jungle {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 17, DataValue = 3, Name = "Vertical", InternalName = "minecraft:log", Note = "Jungle, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 17, DataValue = 7, Name = "HorizontalEastWest", InternalName = "minecraft:log", Note = "Jungle, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 17, DataValue = 11, Name = "HorizontalNorthSouth", InternalName = "minecraft:log", Note = "Jungle, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 17, DataValue = 15, Name = "BarkOnly", InternalName = "minecraft:log", Note = "Jungle, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
			public static class Acacia {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 162, DataValue = 0, Name = "Vertical", InternalName = "minecraft:log2", Note = "Acacia, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 162, DataValue = 4, Name = "HorizontalEastWest", InternalName = "minecraft:log2", Note = "Acacia, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 162, DataValue = 8, Name = "HorizontalNorthSouth", InternalName = "minecraft:log2", Note = "Acacia, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 162, DataValue = 12, Name = "BarkOnly", InternalName = "minecraft:log2", Note = "Acacia, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
			public static class DarkOak {
				public static readonly BlockInfo Vertical = new BlockInfo { TypeId = 162, DataValue = 1, Name = "Vertical", InternalName = "minecraft:log2", Note = "DarkOak, Vertical", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalEastWest = new BlockInfo { TypeId = 162, DataValue = 5, Name = "HorizontalEastWest", InternalName = "minecraft:log2", Note = "DarkOak, Horizontal East/West", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo HorizontalNorthSouth = new BlockInfo { TypeId = 162, DataValue = 9, Name = "HorizontalNorthSouth", InternalName = "minecraft:log2", Note = "DarkOak, Horizontal North/South", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
				public static readonly BlockInfo BarkOnly = new BlockInfo { TypeId = 162, DataValue = 13, Name = "BarkOnly", InternalName = "minecraft:log2", Note = "DarkOak, Bark Only", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 10f };
			}
		}
		public static class Leaves {
			public static class Oak {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 18, DataValue = 0, Name = "Decayable", InternalName = "minecraft:leaves", Note = "Oak, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 18, DataValue = 4, Name = "Permanent", InternalName = "minecraft:leaves", Note = "Oak, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 18, DataValue = 8, Name = "DecayableChecked", InternalName = "minecraft:leaves", Note = "Oak, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 18, DataValue = 12, Name = "PermanentChecked", InternalName = "minecraft:leaves", Note = "Oak, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
			public static class Spruce {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 18, DataValue = 1, Name = "Decayable", InternalName = "minecraft:leaves", Note = "Spruce, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 18, DataValue = 5, Name = "Permanent", InternalName = "minecraft:leaves", Note = "Spruce, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 18, DataValue = 9, Name = "DecayableChecked", InternalName = "minecraft:leaves", Note = "Spruce, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 18, DataValue = 13, Name = "PermanentChecked", InternalName = "minecraft:leaves", Note = "Spruce, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
			public static class Birch {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 18, DataValue = 2, Name = "Decayable", InternalName = "minecraft:leaves", Note = "Birch, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 18, DataValue = 6, Name = "Permanent", InternalName = "minecraft:leaves", Note = "Birch, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 18, DataValue = 10, Name = "DecayableChecked", InternalName = "minecraft:leaves", Note = "Birch, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 18, DataValue = 14, Name = "PermanentChecked", InternalName = "minecraft:leaves", Note = "Birch, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
			public static class Jungle {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 18, DataValue = 3, Name = "Decayable", InternalName = "minecraft:leaves", Note = "Jungle, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 18, DataValue = 7, Name = "Permanent", InternalName = "minecraft:leaves", Note = "Jungle, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 18, DataValue = 11, Name = "DecayableChecked", InternalName = "minecraft:leaves", Note = "Jungle, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 18, DataValue = 15, Name = "PermanentChecked", InternalName = "minecraft:leaves", Note = "Jungle, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
			public static class Acacia {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 161, DataValue = 0, Name = "Decayable", InternalName = "minecraft:leaves2", Note = "Acacia, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 161, DataValue = 4, Name = "Permanent", InternalName = "minecraft:leaves2", Note = "Acacia, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 161, DataValue = 8, Name = "DecayableChecked", InternalName = "minecraft:leaves2", Note = "Acacia, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 161, DataValue = 12, Name = "PermanentChecked", InternalName = "minecraft:leaves2", Note = "Acacia, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
			public static class DarkOak {
				public static readonly BlockInfo Decayable = new BlockInfo { TypeId = 161, DataValue = 1, Name = "Decayable", InternalName = "minecraft:leaves2", Note = "DarkOak, Decaying with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo Permanent = new BlockInfo { TypeId = 161, DataValue = 5, Name = "Permanent", InternalName = "minecraft:leaves2", Note = "DarkOak, Permanent with no checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo DecayableChecked = new BlockInfo { TypeId = 161, DataValue = 9, Name = "DecayableChecked", InternalName = "minecraft:leaves2", Note = "DarkOak, Decaying with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
				public static readonly BlockInfo PermanentChecked = new BlockInfo { TypeId = 161, DataValue = 13, Name = "PermanentChecked", InternalName = "minecraft:leaves2", Note = "DarkOak, Permanent with checks", UsesEntityData = false, Luminance = 0, Opacity = 1, Hardness = 0.2f, BlastResistance = 1f };
			}
		}
		public static class Planks {
			public static readonly BlockInfo Oak = new BlockInfo { TypeId = 5, DataValue = 0, Name = "Oak", InternalName = "minecraft:planks", Note = "Oak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
			public static readonly BlockInfo Spruce = new BlockInfo { TypeId = 5, DataValue = 1, Name = "Spruce", InternalName = "minecraft:planks", Note = "Spruce", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
			public static readonly BlockInfo Birch = new BlockInfo { TypeId = 5, DataValue = 2, Name = "Birch", InternalName = "minecraft:planks", Note = "Birch", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
			public static readonly BlockInfo Jungle = new BlockInfo { TypeId = 5, DataValue = 3, Name = "Jungle", InternalName = "minecraft:planks", Note = "Jungle", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
			public static readonly BlockInfo Acacia = new BlockInfo { TypeId = 5, DataValue = 4, Name = "Acacia", InternalName = "minecraft:planks", Note = "Acacia", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
			public static readonly BlockInfo DarkOak = new BlockInfo { TypeId = 5, DataValue = 5, Name = "DarkOak", InternalName = "minecraft:planks", Note = "DarkOak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
		}
		public static class Sapling {
			public static readonly BlockInfo Oak = new BlockInfo { TypeId = 6, DataValue = 0, Name = "Oak", InternalName = "minecraft:sapling", Note = "Oak", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Spruce = new BlockInfo { TypeId = 6, DataValue = 1, Name = "Spruce", InternalName = "minecraft:sapling", Note = "Spruce", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Birch = new BlockInfo { TypeId = 6, DataValue = 2, Name = "Birch", InternalName = "minecraft:sapling", Note = "Birch", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Jungle = new BlockInfo { TypeId = 6, DataValue = 3, Name = "Jungle", InternalName = "minecraft:sapling", Note = "Jungle", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Acacia = new BlockInfo { TypeId = 6, DataValue = 4, Name = "Acacia", InternalName = "minecraft:sapling", Note = "Acacia", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo DarkOak = new BlockInfo { TypeId = 6, DataValue = 5, Name = "DarkOak", InternalName = "minecraft:sapling", Note = "DarkOak", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
		}
		public static class Stone {
			public static readonly BlockInfo Normal = new BlockInfo { TypeId = 1, DataValue = 0, Name = "Normal", InternalName = "minecraft:stone", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo Granite = new BlockInfo { TypeId = 1, DataValue = 1, Name = "Granite", InternalName = "minecraft:stone", Note = "Granite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo PolishedGranite = new BlockInfo { TypeId = 1, DataValue = 2, Name = "PolishedGranite", InternalName = "minecraft:stone", Note = "PolishedGranite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo Diorite = new BlockInfo { TypeId = 1, DataValue = 3, Name = "Diorite", InternalName = "minecraft:stone", Note = "Diorite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo PolishedDiorite = new BlockInfo { TypeId = 1, DataValue = 4, Name = "PolishedDiorite", InternalName = "minecraft:stone", Note = "PolishedDiorite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo Andesite = new BlockInfo { TypeId = 1, DataValue = 5, Name = "Andesite", InternalName = "minecraft:stone", Note = "Andesite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
			public static readonly BlockInfo PolishedAndesite = new BlockInfo { TypeId = 1, DataValue = 6, Name = "PolishedAndesite", InternalName = "minecraft:stone", Note = "PolishedAndesite", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
		}
		public static class Dirt {
			public static readonly BlockInfo Normal = new BlockInfo { TypeId = 3, DataValue = 0, Name = "Normal", InternalName = "minecraft:dirt", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.5f, BlastResistance = 2.5f };
			public static readonly BlockInfo Coarse = new BlockInfo { TypeId = 3, DataValue = 1, Name = "Coarse", InternalName = "minecraft:dirt", Note = "Coarse", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.5f, BlastResistance = 2.5f };
			public static readonly BlockInfo Podzol = new BlockInfo { TypeId = 3, DataValue = 2, Name = "Podzol", InternalName = "minecraft:dirt", Note = "Podzol", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.5f, BlastResistance = 2.5f };
		}
		public static class Water {
			public static class Flowing {
				public static readonly BlockInfo Horizontal0 = new BlockInfo { TypeId = 8, DataValue = 0, Name = "Horizontal0", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal1 = new BlockInfo { TypeId = 8, DataValue = 1, Name = "Horizontal1", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal2 = new BlockInfo { TypeId = 8, DataValue = 2, Name = "Horizontal2", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal3 = new BlockInfo { TypeId = 8, DataValue = 3, Name = "Horizontal3", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal4 = new BlockInfo { TypeId = 8, DataValue = 4, Name = "Horizontal4", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal5 = new BlockInfo { TypeId = 8, DataValue = 5, Name = "Horizontal5", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal6 = new BlockInfo { TypeId = 8, DataValue = 6, Name = "Horizontal6", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal7 = new BlockInfo { TypeId = 8, DataValue = 7, Name = "Horizontal7", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical8 = new BlockInfo { TypeId = 8, DataValue = 8, Name = "Vertical8", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical9 = new BlockInfo { TypeId = 8, DataValue = 9, Name = "Vertical9", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalA = new BlockInfo { TypeId = 8, DataValue = 10, Name = "VerticalA", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalB = new BlockInfo { TypeId = 8, DataValue = 11, Name = "VerticalB", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalC = new BlockInfo { TypeId = 8, DataValue = 12, Name = "VerticalC", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalD = new BlockInfo { TypeId = 8, DataValue = 13, Name = "VerticalD", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalE = new BlockInfo { TypeId = 8, DataValue = 14, Name = "VerticalE", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalF = new BlockInfo { TypeId = 8, DataValue = 15, Name = "VerticalF", InternalName = "minecraft:flowing_water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
			}
			public static class Stationary {
				public static readonly BlockInfo Horizontal0 = new BlockInfo { TypeId = 9, DataValue = 0, Name = "Horizontal0", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal1 = new BlockInfo { TypeId = 9, DataValue = 1, Name = "Horizontal1", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal2 = new BlockInfo { TypeId = 9, DataValue = 2, Name = "Horizontal2", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal3 = new BlockInfo { TypeId = 9, DataValue = 3, Name = "Horizontal3", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal4 = new BlockInfo { TypeId = 9, DataValue = 4, Name = "Horizontal4", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal5 = new BlockInfo { TypeId = 9, DataValue = 5, Name = "Horizontal5", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal6 = new BlockInfo { TypeId = 9, DataValue = 6, Name = "Horizontal6", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal7 = new BlockInfo { TypeId = 9, DataValue = 7, Name = "Horizontal7", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical8 = new BlockInfo { TypeId = 9, DataValue = 8, Name = "Vertical8", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical9 = new BlockInfo { TypeId = 9, DataValue = 9, Name = "Vertical9", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalA = new BlockInfo { TypeId = 9, DataValue = 10, Name = "VerticalA", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalB = new BlockInfo { TypeId = 9, DataValue = 11, Name = "VerticalB", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalC = new BlockInfo { TypeId = 9, DataValue = 12, Name = "VerticalC", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalD = new BlockInfo { TypeId = 9, DataValue = 13, Name = "VerticalD", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalE = new BlockInfo { TypeId = 9, DataValue = 14, Name = "VerticalE", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalF = new BlockInfo { TypeId = 9, DataValue = 15, Name = "VerticalF", InternalName = "minecraft:water", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 3, Hardness = 100f, BlastResistance = 500f };
			}
		}
		public static class Lava {
			public static class Flowing {
				public static readonly BlockInfo Horizontal0 = new BlockInfo { TypeId = 10, DataValue = 0, Name = "Horizontal0", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal1 = new BlockInfo { TypeId = 10, DataValue = 1, Name = "Horizontal1", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal2 = new BlockInfo { TypeId = 10, DataValue = 2, Name = "Horizontal2", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal3 = new BlockInfo { TypeId = 10, DataValue = 3, Name = "Horizontal3", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal4 = new BlockInfo { TypeId = 10, DataValue = 4, Name = "Horizontal4", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal5 = new BlockInfo { TypeId = 10, DataValue = 5, Name = "Horizontal5", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal6 = new BlockInfo { TypeId = 10, DataValue = 6, Name = "Horizontal6", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal7 = new BlockInfo { TypeId = 10, DataValue = 7, Name = "Horizontal7", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical8 = new BlockInfo { TypeId = 10, DataValue = 8, Name = "Vertical8", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical9 = new BlockInfo { TypeId = 10, DataValue = 9, Name = "Vertical9", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalA = new BlockInfo { TypeId = 10, DataValue = 10, Name = "VerticalA", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalB = new BlockInfo { TypeId = 10, DataValue = 11, Name = "VerticalB", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalC = new BlockInfo { TypeId = 10, DataValue = 12, Name = "VerticalC", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalD = new BlockInfo { TypeId = 10, DataValue = 13, Name = "VerticalD", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalE = new BlockInfo { TypeId = 10, DataValue = 14, Name = "VerticalE", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalF = new BlockInfo { TypeId = 10, DataValue = 15, Name = "VerticalF", InternalName = "minecraft:flowing_lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
			}
			public static class Stationary {
				public static readonly BlockInfo Horizontal0 = new BlockInfo { TypeId = 11, DataValue = 0, Name = "Horizontal0", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal1 = new BlockInfo { TypeId = 11, DataValue = 1, Name = "Horizontal1", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal2 = new BlockInfo { TypeId = 11, DataValue = 2, Name = "Horizontal2", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal3 = new BlockInfo { TypeId = 11, DataValue = 3, Name = "Horizontal3", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal4 = new BlockInfo { TypeId = 11, DataValue = 4, Name = "Horizontal4", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal5 = new BlockInfo { TypeId = 11, DataValue = 5, Name = "Horizontal5", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal6 = new BlockInfo { TypeId = 11, DataValue = 6, Name = "Horizontal6", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Horizontal7 = new BlockInfo { TypeId = 11, DataValue = 7, Name = "Horizontal7", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical8 = new BlockInfo { TypeId = 11, DataValue = 8, Name = "Vertical8", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo Vertical9 = new BlockInfo { TypeId = 11, DataValue = 9, Name = "Vertical9", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalA = new BlockInfo { TypeId = 11, DataValue = 10, Name = "VerticalA", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalB = new BlockInfo { TypeId = 11, DataValue = 11, Name = "VerticalB", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalC = new BlockInfo { TypeId = 11, DataValue = 12, Name = "VerticalC", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalD = new BlockInfo { TypeId = 11, DataValue = 13, Name = "VerticalD", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalE = new BlockInfo { TypeId = 11, DataValue = 14, Name = "VerticalE", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
				public static readonly BlockInfo VerticalF = new BlockInfo { TypeId = 11, DataValue = 15, Name = "VerticalF", InternalName = "minecraft:lava", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 100f, BlastResistance = 500f };
			}
		}
		public static class Sand {
			public static readonly BlockInfo Normal = new BlockInfo { TypeId = 12, DataValue = 0, Name = "Normal", InternalName = "minecraft:sand", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.5f, BlastResistance = 2.5f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 12, DataValue = 1, Name = "Red", InternalName = "minecraft:sand", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.5f, BlastResistance = 2.5f };
		}
		public static class Wool {
			public static readonly BlockInfo White = new BlockInfo { TypeId = 35, DataValue = 0, Name = "White", InternalName = "minecraft:wool", Note = "White", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Orange = new BlockInfo { TypeId = 35, DataValue = 1, Name = "Orange", InternalName = "minecraft:wool", Note = "Orange", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Magenta = new BlockInfo { TypeId = 35, DataValue = 2, Name = "Magenta", InternalName = "minecraft:wool", Note = "Magenta", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo LightBlue = new BlockInfo { TypeId = 35, DataValue = 3, Name = "LightBlue", InternalName = "minecraft:wool", Note = "LightBlue", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Yellow = new BlockInfo { TypeId = 35, DataValue = 4, Name = "Yellow", InternalName = "minecraft:wool", Note = "Yellow", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Lime = new BlockInfo { TypeId = 35, DataValue = 5, Name = "Lime", InternalName = "minecraft:wool", Note = "Lime", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Pink = new BlockInfo { TypeId = 35, DataValue = 6, Name = "Pink", InternalName = "minecraft:wool", Note = "Pink", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Gray = new BlockInfo { TypeId = 35, DataValue = 7, Name = "Gray", InternalName = "minecraft:wool", Note = "Gray", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo LightGray = new BlockInfo { TypeId = 35, DataValue = 8, Name = "LightGray", InternalName = "minecraft:wool", Note = "LightGray", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Cyan = new BlockInfo { TypeId = 35, DataValue = 9, Name = "Cyan", InternalName = "minecraft:wool", Note = "Cyan", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Purple = new BlockInfo { TypeId = 35, DataValue = 10, Name = "Purple", InternalName = "minecraft:wool", Note = "Purple", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Blue = new BlockInfo { TypeId = 35, DataValue = 11, Name = "Blue", InternalName = "minecraft:wool", Note = "Blue", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Brown = new BlockInfo { TypeId = 35, DataValue = 12, Name = "Brown", InternalName = "minecraft:wool", Note = "Brown", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Green = new BlockInfo { TypeId = 35, DataValue = 13, Name = "Green", InternalName = "minecraft:wool", Note = "Green", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 35, DataValue = 14, Name = "Red", InternalName = "minecraft:wool", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Black = new BlockInfo { TypeId = 35, DataValue = 15, Name = "Black", InternalName = "minecraft:wool", Note = "Black", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
		}
		public static class StainedClay {
			public static readonly BlockInfo White = new BlockInfo { TypeId = 159, DataValue = 0, Name = "White", InternalName = "minecraft:stained_hardened_clay", Note = "White", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Orange = new BlockInfo { TypeId = 159, DataValue = 1, Name = "Orange", InternalName = "minecraft:stained_hardened_clay", Note = "Orange", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Magenta = new BlockInfo { TypeId = 159, DataValue = 2, Name = "Magenta", InternalName = "minecraft:stained_hardened_clay", Note = "Magenta", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo LightBlue = new BlockInfo { TypeId = 159, DataValue = 3, Name = "LightBlue", InternalName = "minecraft:stained_hardened_clay", Note = "LightBlue", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Yellow = new BlockInfo { TypeId = 159, DataValue = 4, Name = "Yellow", InternalName = "minecraft:stained_hardened_clay", Note = "Yellow", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Lime = new BlockInfo { TypeId = 159, DataValue = 5, Name = "Lime", InternalName = "minecraft:stained_hardened_clay", Note = "Lime", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Pink = new BlockInfo { TypeId = 159, DataValue = 6, Name = "Pink", InternalName = "minecraft:stained_hardened_clay", Note = "Pink", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Gray = new BlockInfo { TypeId = 159, DataValue = 7, Name = "Gray", InternalName = "minecraft:stained_hardened_clay", Note = "Gray", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo LightGray = new BlockInfo { TypeId = 159, DataValue = 8, Name = "LightGray", InternalName = "minecraft:stained_hardened_clay", Note = "LightGray", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Cyan = new BlockInfo { TypeId = 159, DataValue = 9, Name = "Cyan", InternalName = "minecraft:stained_hardened_clay", Note = "Cyan", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Purple = new BlockInfo { TypeId = 159, DataValue = 10, Name = "Purple", InternalName = "minecraft:stained_hardened_clay", Note = "Purple", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Blue = new BlockInfo { TypeId = 159, DataValue = 11, Name = "Blue", InternalName = "minecraft:stained_hardened_clay", Note = "Blue", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Brown = new BlockInfo { TypeId = 159, DataValue = 12, Name = "Brown", InternalName = "minecraft:stained_hardened_clay", Note = "Brown", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Green = new BlockInfo { TypeId = 159, DataValue = 13, Name = "Green", InternalName = "minecraft:stained_hardened_clay", Note = "Green", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 159, DataValue = 14, Name = "Red", InternalName = "minecraft:stained_hardened_clay", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
			public static readonly BlockInfo Black = new BlockInfo { TypeId = 159, DataValue = 15, Name = "Black", InternalName = "minecraft:stained_hardened_clay", Note = "Black", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.25f, BlastResistance = 21f };
		}
		public static class StainedGlass {
			public static readonly BlockInfo White = new BlockInfo { TypeId = 95, DataValue = 0, Name = "White", InternalName = "minecraft:stained_glass", Note = "White", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Orange = new BlockInfo { TypeId = 95, DataValue = 1, Name = "Orange", InternalName = "minecraft:stained_glass", Note = "Orange", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Magenta = new BlockInfo { TypeId = 95, DataValue = 2, Name = "Magenta", InternalName = "minecraft:stained_glass", Note = "Magenta", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo LightBlue = new BlockInfo { TypeId = 95, DataValue = 3, Name = "LightBlue", InternalName = "minecraft:stained_glass", Note = "LightBlue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Yellow = new BlockInfo { TypeId = 95, DataValue = 4, Name = "Yellow", InternalName = "minecraft:stained_glass", Note = "Yellow", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Lime = new BlockInfo { TypeId = 95, DataValue = 5, Name = "Lime", InternalName = "minecraft:stained_glass", Note = "Lime", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Pink = new BlockInfo { TypeId = 95, DataValue = 6, Name = "Pink", InternalName = "minecraft:stained_glass", Note = "Pink", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Gray = new BlockInfo { TypeId = 95, DataValue = 7, Name = "Gray", InternalName = "minecraft:stained_glass", Note = "Gray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo LightGray = new BlockInfo { TypeId = 95, DataValue = 8, Name = "LightGray", InternalName = "minecraft:stained_glass", Note = "LightGray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Cyan = new BlockInfo { TypeId = 95, DataValue = 9, Name = "Cyan", InternalName = "minecraft:stained_glass", Note = "Cyan", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Purple = new BlockInfo { TypeId = 95, DataValue = 10, Name = "Purple", InternalName = "minecraft:stained_glass", Note = "Purple", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Blue = new BlockInfo { TypeId = 95, DataValue = 11, Name = "Blue", InternalName = "minecraft:stained_glass", Note = "Blue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Brown = new BlockInfo { TypeId = 95, DataValue = 12, Name = "Brown", InternalName = "minecraft:stained_glass", Note = "Brown", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Green = new BlockInfo { TypeId = 95, DataValue = 13, Name = "Green", InternalName = "minecraft:stained_glass", Note = "Green", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 95, DataValue = 14, Name = "Red", InternalName = "minecraft:stained_glass", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Black = new BlockInfo { TypeId = 95, DataValue = 15, Name = "Black", InternalName = "minecraft:stained_glass", Note = "Black", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
		}
		public static class StainedGlassPane {
			public static readonly BlockInfo White = new BlockInfo { TypeId = 160, DataValue = 0, Name = "White", InternalName = "minecraft:stained_glass_pane", Note = "White", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Orange = new BlockInfo { TypeId = 160, DataValue = 1, Name = "Orange", InternalName = "minecraft:stained_glass_pane", Note = "Orange", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Magenta = new BlockInfo { TypeId = 160, DataValue = 2, Name = "Magenta", InternalName = "minecraft:stained_glass_pane", Note = "Magenta", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo LightBlue = new BlockInfo { TypeId = 160, DataValue = 3, Name = "LightBlue", InternalName = "minecraft:stained_glass_pane", Note = "LightBlue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Yellow = new BlockInfo { TypeId = 160, DataValue = 4, Name = "Yellow", InternalName = "minecraft:stained_glass_pane", Note = "Yellow", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Lime = new BlockInfo { TypeId = 160, DataValue = 5, Name = "Lime", InternalName = "minecraft:stained_glass_pane", Note = "Lime", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Pink = new BlockInfo { TypeId = 160, DataValue = 6, Name = "Pink", InternalName = "minecraft:stained_glass_pane", Note = "Pink", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Gray = new BlockInfo { TypeId = 160, DataValue = 7, Name = "Gray", InternalName = "minecraft:stained_glass_pane", Note = "Gray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo LightGray = new BlockInfo { TypeId = 160, DataValue = 8, Name = "LightGray", InternalName = "minecraft:stained_glass_pane", Note = "LightGray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Cyan = new BlockInfo { TypeId = 160, DataValue = 9, Name = "Cyan", InternalName = "minecraft:stained_glass_pane", Note = "Cyan", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Purple = new BlockInfo { TypeId = 160, DataValue = 10, Name = "Purple", InternalName = "minecraft:stained_glass_pane", Note = "Purple", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Blue = new BlockInfo { TypeId = 160, DataValue = 11, Name = "Blue", InternalName = "minecraft:stained_glass_pane", Note = "Blue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Brown = new BlockInfo { TypeId = 160, DataValue = 12, Name = "Brown", InternalName = "minecraft:stained_glass_pane", Note = "Brown", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Green = new BlockInfo { TypeId = 160, DataValue = 13, Name = "Green", InternalName = "minecraft:stained_glass_pane", Note = "Green", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 160, DataValue = 14, Name = "Red", InternalName = "minecraft:stained_glass_pane", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
			public static readonly BlockInfo Black = new BlockInfo { TypeId = 160, DataValue = 15, Name = "Black", InternalName = "minecraft:stained_glass_pane", Note = "Black", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.3f, BlastResistance = 1.5f };
		}
		public static class Carpet {
			public static readonly BlockInfo White = new BlockInfo { TypeId = 171, DataValue = 0, Name = "White", InternalName = "minecraft:carpet", Note = "White", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Orange = new BlockInfo { TypeId = 171, DataValue = 1, Name = "Orange", InternalName = "minecraft:carpet", Note = "Orange", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Magenta = new BlockInfo { TypeId = 171, DataValue = 2, Name = "Magenta", InternalName = "minecraft:carpet", Note = "Magenta", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo LightBlue = new BlockInfo { TypeId = 171, DataValue = 3, Name = "LightBlue", InternalName = "minecraft:carpet", Note = "LightBlue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Yellow = new BlockInfo { TypeId = 171, DataValue = 4, Name = "Yellow", InternalName = "minecraft:carpet", Note = "Yellow", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Lime = new BlockInfo { TypeId = 171, DataValue = 5, Name = "Lime", InternalName = "minecraft:carpet", Note = "Lime", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Pink = new BlockInfo { TypeId = 171, DataValue = 6, Name = "Pink", InternalName = "minecraft:carpet", Note = "Pink", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Gray = new BlockInfo { TypeId = 171, DataValue = 7, Name = "Gray", InternalName = "minecraft:carpet", Note = "Gray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo LightGray = new BlockInfo { TypeId = 171, DataValue = 8, Name = "LightGray", InternalName = "minecraft:carpet", Note = "LightGray", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Cyan = new BlockInfo { TypeId = 171, DataValue = 9, Name = "Cyan", InternalName = "minecraft:carpet", Note = "Cyan", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Purple = new BlockInfo { TypeId = 171, DataValue = 10, Name = "Purple", InternalName = "minecraft:carpet", Note = "Purple", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Blue = new BlockInfo { TypeId = 171, DataValue = 11, Name = "Blue", InternalName = "minecraft:carpet", Note = "Blue", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Brown = new BlockInfo { TypeId = 171, DataValue = 12, Name = "Brown", InternalName = "minecraft:carpet", Note = "Brown", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Green = new BlockInfo { TypeId = 171, DataValue = 13, Name = "Green", InternalName = "minecraft:carpet", Note = "Green", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Red = new BlockInfo { TypeId = 171, DataValue = 14, Name = "Red", InternalName = "minecraft:carpet", Note = "Red", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Black = new BlockInfo { TypeId = 171, DataValue = 15, Name = "Black", InternalName = "minecraft:carpet", Note = "Black", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
		}
		public static class Torch {
			public static readonly BlockInfo East = new BlockInfo { TypeId = 50, DataValue = 1, Name = "East", InternalName = "minecraft:torch", Note = "East", UsesEntityData = false, Luminance = 14, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo West = new BlockInfo { TypeId = 50, DataValue = 2, Name = "West", InternalName = "minecraft:torch", Note = "West", UsesEntityData = false, Luminance = 14, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo South = new BlockInfo { TypeId = 50, DataValue = 3, Name = "South", InternalName = "minecraft:torch", Note = "South", UsesEntityData = false, Luminance = 14, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo North = new BlockInfo { TypeId = 50, DataValue = 4, Name = "North", InternalName = "minecraft:torch", Note = "North", UsesEntityData = false, Luminance = 14, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Standing = new BlockInfo { TypeId = 50, DataValue = 5, Name = "Standing", InternalName = "minecraft:torch", Note = "Standing", UsesEntityData = false, Luminance = 14, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
		}
		public static class RedstoneTorch {
			public static class Active {
				public static readonly BlockInfo East = new BlockInfo { TypeId = 76, DataValue = 1, Name = "East", InternalName = "minecraft:redstone_torch", Note = "East", UsesEntityData = false, Luminance = 7, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo West = new BlockInfo { TypeId = 76, DataValue = 2, Name = "West", InternalName = "minecraft:redstone_torch", Note = "West", UsesEntityData = false, Luminance = 7, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo South = new BlockInfo { TypeId = 76, DataValue = 3, Name = "South", InternalName = "minecraft:redstone_torch", Note = "South", UsesEntityData = false, Luminance = 7, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo North = new BlockInfo { TypeId = 76, DataValue = 4, Name = "North", InternalName = "minecraft:redstone_torch", Note = "North", UsesEntityData = false, Luminance = 7, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Standing = new BlockInfo { TypeId = 76, DataValue = 5, Name = "Standing", InternalName = "minecraft:redstone_torch", Note = "Standing", UsesEntityData = false, Luminance = 7, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			}
			public static class Inactive {
				public static readonly BlockInfo East = new BlockInfo { TypeId = 75, DataValue = 1, Name = "East", InternalName = "minecraft:unlit_redstone_torch", Note = "East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo West = new BlockInfo { TypeId = 75, DataValue = 2, Name = "West", InternalName = "minecraft:unlit_redstone_torch", Note = "West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo South = new BlockInfo { TypeId = 75, DataValue = 3, Name = "South", InternalName = "minecraft:unlit_redstone_torch", Note = "South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo North = new BlockInfo { TypeId = 75, DataValue = 4, Name = "North", InternalName = "minecraft:unlit_redstone_torch", Note = "North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Standing = new BlockInfo { TypeId = 75, DataValue = 5, Name = "Standing", InternalName = "minecraft:unlit_redstone_torch", Note = "Standing", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			}
		}
		public static class Slab {
			public static class Stone {
				public static class Single {
					public static readonly BlockInfo Normal = new BlockInfo { TypeId = 44, DataValue = 0, Name = "Normal", InternalName = "minecraft:stone_slab", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Sandstone = new BlockInfo { TypeId = 44, DataValue = 1, Name = "Sandstone", InternalName = "minecraft:stone_slab", Note = "Sandstone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Wooden = new BlockInfo { TypeId = 44, DataValue = 2, Name = "Wooden", InternalName = "minecraft:stone_slab", Note = "Wooden", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Cobblestone = new BlockInfo { TypeId = 44, DataValue = 3, Name = "Cobblestone", InternalName = "minecraft:stone_slab", Note = "Cobblestone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Brick = new BlockInfo { TypeId = 44, DataValue = 4, Name = "Brick", InternalName = "minecraft:stone_slab", Note = "Brick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo StoneBrick = new BlockInfo { TypeId = 44, DataValue = 5, Name = "StoneBrick", InternalName = "minecraft:stone_slab", Note = "StoneBrick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo NetherBrick = new BlockInfo { TypeId = 44, DataValue = 6, Name = "NetherBrick", InternalName = "minecraft:stone_slab", Note = "NetherBrick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Quartz = new BlockInfo { TypeId = 44, DataValue = 7, Name = "Quartz", InternalName = "minecraft:stone_slab", Note = "Quartz", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
				public static class Double {
					public static readonly BlockInfo Normal = new BlockInfo { TypeId = 43, DataValue = 0, Name = "Normal", InternalName = "minecraft:double_stone_slab", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Sandstone = new BlockInfo { TypeId = 43, DataValue = 1, Name = "Sandstone", InternalName = "minecraft:double_stone_slab", Note = "Sandstone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Wooden = new BlockInfo { TypeId = 43, DataValue = 2, Name = "Wooden", InternalName = "minecraft:double_stone_slab", Note = "Wooden", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Cobblestone = new BlockInfo { TypeId = 43, DataValue = 3, Name = "Cobblestone", InternalName = "minecraft:double_stone_slab", Note = "Cobblestone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Brick = new BlockInfo { TypeId = 43, DataValue = 4, Name = "Brick", InternalName = "minecraft:double_stone_slab", Note = "Brick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo StoneBrick = new BlockInfo { TypeId = 43, DataValue = 5, Name = "StoneBrick", InternalName = "minecraft:double_stone_slab", Note = "StoneBrick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo NetherBrick = new BlockInfo { TypeId = 43, DataValue = 6, Name = "NetherBrick", InternalName = "minecraft:double_stone_slab", Note = "NetherBrick", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo Quartz = new BlockInfo { TypeId = 43, DataValue = 7, Name = "Quartz", InternalName = "minecraft:double_stone_slab", Note = "Quartz", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo SmoothStone = new BlockInfo { TypeId = 43, DataValue = 8, Name = "SmoothStone", InternalName = "minecraft:double_stone_slab", Note = "SmoothStone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo SmoothSandstone = new BlockInfo { TypeId = 43, DataValue = 9, Name = "SmoothSandstone", InternalName = "minecraft:double_stone_slab", Note = "SmoothSandstone", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo TileQuartz = new BlockInfo { TypeId = 43, DataValue = 10, Name = "TileQuartz", InternalName = "minecraft:double_stone_slab", Note = "TileQuartz", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
			}
			public static class Wood {
				public static class Single {
					public static readonly BlockInfo Oak = new BlockInfo { TypeId = 126, DataValue = 0, Name = "Oak", InternalName = "minecraft:wooden_slab", Note = "Oak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Spruce = new BlockInfo { TypeId = 126, DataValue = 1, Name = "Spruce", InternalName = "minecraft:wooden_slab", Note = "Spruce", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Birch = new BlockInfo { TypeId = 126, DataValue = 2, Name = "Birch", InternalName = "minecraft:wooden_slab", Note = "Birch", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Jungle = new BlockInfo { TypeId = 126, DataValue = 3, Name = "Jungle", InternalName = "minecraft:wooden_slab", Note = "Jungle", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Acacia = new BlockInfo { TypeId = 126, DataValue = 4, Name = "Acacia", InternalName = "minecraft:wooden_slab", Note = "Acacia", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo DarkOak = new BlockInfo { TypeId = 126, DataValue = 5, Name = "DarkOak", InternalName = "minecraft:wooden_slab", Note = "DarkOak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class Double {
					public static readonly BlockInfo Oak = new BlockInfo { TypeId = 125, DataValue = 0, Name = "Oak", InternalName = "minecraft:double_wooden_slab", Note = "Oak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Spruce = new BlockInfo { TypeId = 125, DataValue = 1, Name = "Spruce", InternalName = "minecraft:double_wooden_slab", Note = "Spruce", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Birch = new BlockInfo { TypeId = 125, DataValue = 2, Name = "Birch", InternalName = "minecraft:double_wooden_slab", Note = "Birch", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Jungle = new BlockInfo { TypeId = 125, DataValue = 3, Name = "Jungle", InternalName = "minecraft:double_wooden_slab", Note = "Jungle", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo Acacia = new BlockInfo { TypeId = 125, DataValue = 4, Name = "Acacia", InternalName = "minecraft:double_wooden_slab", Note = "Acacia", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo DarkOak = new BlockInfo { TypeId = 125, DataValue = 5, Name = "DarkOak", InternalName = "minecraft:double_wooden_slab", Note = "DarkOak", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
		}
		public static class Fire {
			public static readonly BlockInfo Tick0 = new BlockInfo { TypeId = 51, DataValue = 0, Name = "Tick0", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick1 = new BlockInfo { TypeId = 51, DataValue = 1, Name = "Tick1", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick2 = new BlockInfo { TypeId = 51, DataValue = 2, Name = "Tick2", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick3 = new BlockInfo { TypeId = 51, DataValue = 3, Name = "Tick3", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick4 = new BlockInfo { TypeId = 51, DataValue = 4, Name = "Tick4", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick5 = new BlockInfo { TypeId = 51, DataValue = 5, Name = "Tick5", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick6 = new BlockInfo { TypeId = 51, DataValue = 6, Name = "Tick6", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick7 = new BlockInfo { TypeId = 51, DataValue = 7, Name = "Tick7", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick8 = new BlockInfo { TypeId = 51, DataValue = 8, Name = "Tick8", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Tick9 = new BlockInfo { TypeId = 51, DataValue = 9, Name = "Tick9", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickA = new BlockInfo { TypeId = 51, DataValue = 10, Name = "TickA", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickB = new BlockInfo { TypeId = 51, DataValue = 11, Name = "TickB", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickC = new BlockInfo { TypeId = 51, DataValue = 12, Name = "TickC", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickD = new BlockInfo { TypeId = 51, DataValue = 13, Name = "TickD", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickE = new BlockInfo { TypeId = 51, DataValue = 14, Name = "TickE", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo TickF = new BlockInfo { TypeId = 51, DataValue = 15, Name = "TickF", InternalName = "minecraft:fire", Note = "", UsesEntityData = false, Luminance = 15, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
		}
		public static class Sandstone {
			public static readonly BlockInfo Normal = new BlockInfo { TypeId = 24, DataValue = 0, Name = "Normal", InternalName = "minecraft:sandstone", Note = "Normal", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Chiseled = new BlockInfo { TypeId = 24, DataValue = 1, Name = "Chiseled", InternalName = "minecraft:sandstone", Note = "Chiseled", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
			public static readonly BlockInfo Smooth = new BlockInfo { TypeId = 24, DataValue = 2, Name = "Smooth", InternalName = "minecraft:sandstone", Note = "Smooth", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
		}
		public static class Bed {
			public static class Foot {
				public static class Unoccupied {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 26, DataValue = 0, Name = "East", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 26, DataValue = 1, Name = "West", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 26, DataValue = 2, Name = "South", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 26, DataValue = 3, Name = "North", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
				}
				public static class Occupied {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 26, DataValue = 4, Name = "East", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 26, DataValue = 5, Name = "West", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 26, DataValue = 6, Name = "South", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 26, DataValue = 7, Name = "North", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
				}
			}
			public static class Head {
				public static class Unoccupied {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 26, DataValue = 8, Name = "East", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 26, DataValue = 9, Name = "West", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 26, DataValue = 10, Name = "South", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 26, DataValue = 11, Name = "North", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
				}
				public static class Occupied {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 26, DataValue = 12, Name = "East", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 26, DataValue = 13, Name = "West", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 26, DataValue = 14, Name = "South", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 26, DataValue = 15, Name = "North", InternalName = "minecraft:bed", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.2f, BlastResistance = 1f };
				}
			}
		}
		public static class TallGrass {
			public static readonly BlockInfo Shrub = new BlockInfo { TypeId = 31, DataValue = 0, Name = "Shrub", InternalName = "minecraft:tallgrass", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Normal = new BlockInfo { TypeId = 31, DataValue = 1, Name = "Normal", InternalName = "minecraft:tallgrass", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Fern = new BlockInfo { TypeId = 31, DataValue = 2, Name = "Fern", InternalName = "minecraft:tallgrass", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo BiomeShrub = new BlockInfo { TypeId = 31, DataValue = 3, Name = "BiomeShrub", InternalName = "minecraft:tallgrass", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
		}
		public static class Flower {
			public static class Small {
				public static readonly BlockInfo Dandelion = new BlockInfo { TypeId = 37, DataValue = 0, Name = "Dandelion", InternalName = "minecraft:yellow_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Poppy = new BlockInfo { TypeId = 38, DataValue = 0, Name = "Poppy", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo BlueOrchid = new BlockInfo { TypeId = 38, DataValue = 1, Name = "BlueOrchid", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Allium = new BlockInfo { TypeId = 38, DataValue = 2, Name = "Allium", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo AzureBluet = new BlockInfo { TypeId = 38, DataValue = 3, Name = "AzureBluet", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo RedTulip = new BlockInfo { TypeId = 38, DataValue = 4, Name = "RedTulip", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo OrangeTulip = new BlockInfo { TypeId = 38, DataValue = 5, Name = "OrangeTulip", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo WhiteTulip = new BlockInfo { TypeId = 38, DataValue = 6, Name = "WhiteTulip", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo PinkTulip = new BlockInfo { TypeId = 38, DataValue = 7, Name = "PinkTulip", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo OxeyeDaisy = new BlockInfo { TypeId = 38, DataValue = 8, Name = "OxeyeDaisy", InternalName = "minecraft:red_flower", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			}
			public static class Large {
				public static readonly BlockInfo Sunflower = new BlockInfo { TypeId = 175, DataValue = 0, Name = "Sunflower", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Lilac = new BlockInfo { TypeId = 175, DataValue = 1, Name = "Lilac", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo DoubleTallgrass = new BlockInfo { TypeId = 175, DataValue = 2, Name = "DoubleTallgrass", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo LargeFern = new BlockInfo { TypeId = 175, DataValue = 3, Name = "LargeFern", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo RoseBush = new BlockInfo { TypeId = 175, DataValue = 4, Name = "RoseBush", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Peony = new BlockInfo { TypeId = 175, DataValue = 5, Name = "Peony", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
				public static readonly BlockInfo Top = new BlockInfo { TypeId = 175, DataValue = 8, Name = "Top", InternalName = "minecraft:double_plant", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			}
			public static class Pot {
			}
		}
		public static class Piston {
			public static readonly BlockInfo Extension = new BlockInfo { TypeId = 36, DataValue = 0, Name = "Extension", InternalName = "minecraft:piston_extension", Note = "Piston Extension", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = -1f, BlastResistance = 0f };
			public static class Normal {
				public static class Retracted {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 33, DataValue = 0, Name = "Down", InternalName = "minecraft:piston", Note = "Retracted Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 33, DataValue = 1, Name = "Up", InternalName = "minecraft:piston", Note = "Retracted Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 33, DataValue = 2, Name = "North", InternalName = "minecraft:piston", Note = "Retracted North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 33, DataValue = 3, Name = "South", InternalName = "minecraft:piston", Note = "Retracted South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 33, DataValue = 4, Name = "West", InternalName = "minecraft:piston", Note = "Retracted West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 33, DataValue = 5, Name = "East", InternalName = "minecraft:piston", Note = "Retracted East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
				public static class Extended {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 33, DataValue = 8, Name = "Down", InternalName = "minecraft:piston", Note = "Extended Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 33, DataValue = 9, Name = "Up", InternalName = "minecraft:piston", Note = "Extended Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 33, DataValue = 10, Name = "North", InternalName = "minecraft:piston", Note = "Extended North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 33, DataValue = 11, Name = "South", InternalName = "minecraft:piston", Note = "Extended South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 33, DataValue = 12, Name = "West", InternalName = "minecraft:piston", Note = "Extended West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 33, DataValue = 13, Name = "East", InternalName = "minecraft:piston", Note = "Extended East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
			}
			public static class Sticky {
				public static class Retracted {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 29, DataValue = 0, Name = "Down", InternalName = "minecraft:sticky_piston", Note = "Retracted Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 29, DataValue = 1, Name = "Up", InternalName = "minecraft:sticky_piston", Note = "Retracted Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 29, DataValue = 2, Name = "North", InternalName = "minecraft:sticky_piston", Note = "Retracted North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 29, DataValue = 3, Name = "South", InternalName = "minecraft:sticky_piston", Note = "Retracted South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 29, DataValue = 4, Name = "West", InternalName = "minecraft:sticky_piston", Note = "Retracted West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 29, DataValue = 5, Name = "East", InternalName = "minecraft:sticky_piston", Note = "Retracted East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
				public static class Extended {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 29, DataValue = 8, Name = "Down", InternalName = "minecraft:sticky_piston", Note = "Extended Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 29, DataValue = 9, Name = "Up", InternalName = "minecraft:sticky_piston", Note = "Extended Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 29, DataValue = 10, Name = "North", InternalName = "minecraft:sticky_piston", Note = "Extended North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 29, DataValue = 11, Name = "South", InternalName = "minecraft:sticky_piston", Note = "Extended South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 29, DataValue = 12, Name = "West", InternalName = "minecraft:sticky_piston", Note = "Extended West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 29, DataValue = 13, Name = "East", InternalName = "minecraft:sticky_piston", Note = "Extended East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
			}
			public static class Head {
				public static class Normal {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 34, DataValue = 0, Name = "Down", InternalName = "minecraft:piston_head", Note = "Normal Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 34, DataValue = 1, Name = "Up", InternalName = "minecraft:piston_head", Note = "Normal Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 34, DataValue = 2, Name = "North", InternalName = "minecraft:piston_head", Note = "Normal North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 34, DataValue = 3, Name = "South", InternalName = "minecraft:piston_head", Note = "Normal South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 34, DataValue = 4, Name = "West", InternalName = "minecraft:piston_head", Note = "Normal West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 34, DataValue = 5, Name = "East", InternalName = "minecraft:piston_head", Note = "Normal East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
				public static class Sticky {
					public static readonly BlockInfo Down = new BlockInfo { TypeId = 34, DataValue = 8, Name = "Down", InternalName = "minecraft:piston_head", Note = "Sticky Down", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo Up = new BlockInfo { TypeId = 34, DataValue = 9, Name = "Up", InternalName = "minecraft:piston_head", Note = "Sticky Up", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 34, DataValue = 10, Name = "North", InternalName = "minecraft:piston_head", Note = "Sticky North", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 34, DataValue = 11, Name = "South", InternalName = "minecraft:piston_head", Note = "Sticky South", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 34, DataValue = 12, Name = "West", InternalName = "minecraft:piston_head", Note = "Sticky West", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
					public static readonly BlockInfo East = new BlockInfo { TypeId = 34, DataValue = 13, Name = "East", InternalName = "minecraft:piston_head", Note = "Sticky East", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.5f, BlastResistance = 2.5f };
				}
			}
		}
		public static class Stairs {
			public static class Oak {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 67, DataValue = 0, Name = "East", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 67, DataValue = 1, Name = "West", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 67, DataValue = 2, Name = "South", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 67, DataValue = 3, Name = "North", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 67, DataValue = 4, Name = "East", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 67, DataValue = 5, Name = "West", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 67, DataValue = 6, Name = "South", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 67, DataValue = 7, Name = "North", InternalName = "minecraft:stone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
			}
			public static class Brick {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 108, DataValue = 0, Name = "East", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 108, DataValue = 1, Name = "West", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 108, DataValue = 2, Name = "South", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 108, DataValue = 3, Name = "North", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 108, DataValue = 4, Name = "East", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 108, DataValue = 5, Name = "West", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 108, DataValue = 6, Name = "South", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 108, DataValue = 7, Name = "North", InternalName = "minecraft:brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
			}
			public static class StoneBrick {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 109, DataValue = 0, Name = "East", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 109, DataValue = 1, Name = "West", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 109, DataValue = 2, Name = "South", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 109, DataValue = 3, Name = "North", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 109, DataValue = 4, Name = "East", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 109, DataValue = 5, Name = "West", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 109, DataValue = 6, Name = "South", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 109, DataValue = 7, Name = "North", InternalName = "minecraft:stone_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 1.5f, BlastResistance = 30f };
				}
			}
			public static class NetherBrick {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 114, DataValue = 0, Name = "East", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 114, DataValue = 1, Name = "West", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 114, DataValue = 2, Name = "South", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 114, DataValue = 3, Name = "North", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 114, DataValue = 4, Name = "East", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 114, DataValue = 5, Name = "West", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 114, DataValue = 6, Name = "South", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 114, DataValue = 7, Name = "North", InternalName = "minecraft:nether_brick_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 30f };
				}
			}
			public static class Sandstone {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 128, DataValue = 0, Name = "East", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 128, DataValue = 1, Name = "West", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 128, DataValue = 2, Name = "South", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 128, DataValue = 3, Name = "North", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 128, DataValue = 4, Name = "East", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 128, DataValue = 5, Name = "West", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 128, DataValue = 6, Name = "South", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 128, DataValue = 7, Name = "North", InternalName = "minecraft:sandstone_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.8f, BlastResistance = 4f };
				}
			}
			public static class Spruce {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 134, DataValue = 0, Name = "East", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 134, DataValue = 1, Name = "West", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 134, DataValue = 2, Name = "South", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 134, DataValue = 3, Name = "North", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 134, DataValue = 4, Name = "East", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 134, DataValue = 5, Name = "West", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 134, DataValue = 6, Name = "South", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 134, DataValue = 7, Name = "North", InternalName = "minecraft:spruce_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
			public static class Birch {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 135, DataValue = 0, Name = "East", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 135, DataValue = 1, Name = "West", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 135, DataValue = 2, Name = "South", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 135, DataValue = 3, Name = "North", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 135, DataValue = 4, Name = "East", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 135, DataValue = 5, Name = "West", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 135, DataValue = 6, Name = "South", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 135, DataValue = 7, Name = "North", InternalName = "minecraft:birch_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
			public static class Jungle {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 136, DataValue = 0, Name = "East", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 136, DataValue = 1, Name = "West", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 136, DataValue = 2, Name = "South", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 136, DataValue = 3, Name = "North", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 136, DataValue = 4, Name = "East", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 136, DataValue = 5, Name = "West", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 136, DataValue = 6, Name = "South", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 136, DataValue = 7, Name = "North", InternalName = "minecraft:jungle_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
			public static class Acacia {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 163, DataValue = 0, Name = "East", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 163, DataValue = 1, Name = "West", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 163, DataValue = 2, Name = "South", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 163, DataValue = 3, Name = "North", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 163, DataValue = 4, Name = "East", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 163, DataValue = 5, Name = "West", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 163, DataValue = 6, Name = "South", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 163, DataValue = 7, Name = "North", InternalName = "minecraft:acacia_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
			public static class DarkOak {
				public static class Normal {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 164, DataValue = 0, Name = "East", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 164, DataValue = 1, Name = "West", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 164, DataValue = 2, Name = "South", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 164, DataValue = 3, Name = "North", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
				public static class UpsideDown {
					public static readonly BlockInfo East = new BlockInfo { TypeId = 164, DataValue = 4, Name = "East", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo West = new BlockInfo { TypeId = 164, DataValue = 5, Name = "West", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo South = new BlockInfo { TypeId = 164, DataValue = 6, Name = "South", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
					public static readonly BlockInfo North = new BlockInfo { TypeId = 164, DataValue = 7, Name = "North", InternalName = "minecraft:dark_oak_stairs", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 2f, BlastResistance = 15f };
				}
			}
		}
		public static class RedstoneWire {
			public static readonly BlockInfo Power0 = new BlockInfo { TypeId = 55, DataValue = 0, Name = "Power0", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power1 = new BlockInfo { TypeId = 55, DataValue = 1, Name = "Power1", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power2 = new BlockInfo { TypeId = 55, DataValue = 2, Name = "Power2", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power3 = new BlockInfo { TypeId = 55, DataValue = 3, Name = "Power3", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power4 = new BlockInfo { TypeId = 55, DataValue = 4, Name = "Power4", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power5 = new BlockInfo { TypeId = 55, DataValue = 5, Name = "Power5", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power6 = new BlockInfo { TypeId = 55, DataValue = 6, Name = "Power6", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power7 = new BlockInfo { TypeId = 55, DataValue = 7, Name = "Power7", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power8 = new BlockInfo { TypeId = 55, DataValue = 8, Name = "Power8", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo Power9 = new BlockInfo { TypeId = 55, DataValue = 9, Name = "Power9", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerA = new BlockInfo { TypeId = 55, DataValue = 10, Name = "PowerA", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerB = new BlockInfo { TypeId = 55, DataValue = 11, Name = "PowerB", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerC = new BlockInfo { TypeId = 55, DataValue = 12, Name = "PowerC", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerD = new BlockInfo { TypeId = 55, DataValue = 13, Name = "PowerD", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerE = new BlockInfo { TypeId = 55, DataValue = 14, Name = "PowerE", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
			public static readonly BlockInfo PowerF = new BlockInfo { TypeId = 55, DataValue = 15, Name = "PowerF", InternalName = "minecraft:redstone_wire", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0f, BlastResistance = 0f };
		}
		public static class Farmland {
			public static readonly BlockInfo Wetness0 = new BlockInfo { TypeId = 60, DataValue = 0, Name = "Wetness0", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness1 = new BlockInfo { TypeId = 60, DataValue = 1, Name = "Wetness1", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness2 = new BlockInfo { TypeId = 60, DataValue = 2, Name = "Wetness2", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness3 = new BlockInfo { TypeId = 60, DataValue = 3, Name = "Wetness3", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness4 = new BlockInfo { TypeId = 60, DataValue = 4, Name = "Wetness4", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness5 = new BlockInfo { TypeId = 60, DataValue = 5, Name = "Wetness5", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness6 = new BlockInfo { TypeId = 60, DataValue = 6, Name = "Wetness6", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness7 = new BlockInfo { TypeId = 60, DataValue = 7, Name = "Wetness7", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness8 = new BlockInfo { TypeId = 60, DataValue = 8, Name = "Wetness8", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo Wetness9 = new BlockInfo { TypeId = 60, DataValue = 9, Name = "Wetness9", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessA = new BlockInfo { TypeId = 60, DataValue = 10, Name = "WetnessA", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessB = new BlockInfo { TypeId = 60, DataValue = 11, Name = "WetnessB", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessC = new BlockInfo { TypeId = 60, DataValue = 12, Name = "WetnessC", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessD = new BlockInfo { TypeId = 60, DataValue = 13, Name = "WetnessD", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessE = new BlockInfo { TypeId = 60, DataValue = 14, Name = "WetnessE", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
			public static readonly BlockInfo WetnessF = new BlockInfo { TypeId = 60, DataValue = 15, Name = "WetnessF", InternalName = "minecraft:farmland", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.6f, BlastResistance = 3f };
		}
		public static class SignPost {
			public static readonly BlockInfo S = new BlockInfo { TypeId = 63, DataValue = 0, Name = "S", InternalName = "minecraft:standing_sign", Note = "S", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SSW = new BlockInfo { TypeId = 63, DataValue = 1, Name = "SSW", InternalName = "minecraft:standing_sign", Note = "SSW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SW = new BlockInfo { TypeId = 63, DataValue = 2, Name = "SW", InternalName = "minecraft:standing_sign", Note = "SW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SWW = new BlockInfo { TypeId = 63, DataValue = 3, Name = "SWW", InternalName = "minecraft:standing_sign", Note = "SWW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo W = new BlockInfo { TypeId = 63, DataValue = 4, Name = "W", InternalName = "minecraft:standing_sign", Note = "W", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NWW = new BlockInfo { TypeId = 63, DataValue = 5, Name = "NWW", InternalName = "minecraft:standing_sign", Note = "NWW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NW = new BlockInfo { TypeId = 63, DataValue = 6, Name = "NW", InternalName = "minecraft:standing_sign", Note = "NW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NNW = new BlockInfo { TypeId = 63, DataValue = 7, Name = "NNW", InternalName = "minecraft:standing_sign", Note = "NNW", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo N = new BlockInfo { TypeId = 63, DataValue = 8, Name = "N", InternalName = "minecraft:standing_sign", Note = "N", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NNE = new BlockInfo { TypeId = 63, DataValue = 9, Name = "NNE", InternalName = "minecraft:standing_sign", Note = "NNE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NE = new BlockInfo { TypeId = 63, DataValue = 10, Name = "NE", InternalName = "minecraft:standing_sign", Note = "NE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo NEE = new BlockInfo { TypeId = 63, DataValue = 11, Name = "NEE", InternalName = "minecraft:standing_sign", Note = "NEE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo E = new BlockInfo { TypeId = 63, DataValue = 12, Name = "E", InternalName = "minecraft:standing_sign", Note = "E", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SEE = new BlockInfo { TypeId = 63, DataValue = 13, Name = "SEE", InternalName = "minecraft:standing_sign", Note = "SEE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SE = new BlockInfo { TypeId = 63, DataValue = 14, Name = "SE", InternalName = "minecraft:standing_sign", Note = "SE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
			public static readonly BlockInfo SSE = new BlockInfo { TypeId = 63, DataValue = 15, Name = "SSE", InternalName = "minecraft:standing_sign", Note = "SSE", UsesEntityData = true, Luminance = 0, Opacity = 0, Hardness = 1f, BlastResistance = 5f };
		}
		public static class Snow {
			public static readonly BlockInfo Height0 = new BlockInfo { TypeId = 78, DataValue = 0, Name = "Height0", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height1 = new BlockInfo { TypeId = 78, DataValue = 1, Name = "Height1", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height2 = new BlockInfo { TypeId = 78, DataValue = 2, Name = "Height2", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height3 = new BlockInfo { TypeId = 78, DataValue = 3, Name = "Height3", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height4 = new BlockInfo { TypeId = 78, DataValue = 4, Name = "Height4", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height5 = new BlockInfo { TypeId = 78, DataValue = 5, Name = "Height5", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height6 = new BlockInfo { TypeId = 78, DataValue = 6, Name = "Height6", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Height7 = new BlockInfo { TypeId = 78, DataValue = 7, Name = "Height7", InternalName = "minecraft:snow_layer", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 0, Hardness = 0.1f, BlastResistance = 0.5f };
			public static readonly BlockInfo Block = new BlockInfo { TypeId = 80, DataValue = 0, Name = "Block", InternalName = "minecraft:snow", Note = "", UsesEntityData = false, Luminance = 0, Opacity = 255, Hardness = 0.2f, BlastResistance = 1f };
		}
		public static class Crop {
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

			public int TypeId { get { return Id & 0xFFF; } internal set { Id = (ushort)((Id & 0xF000) | value); } }
			public int DataValue { get { return Id >> 12; } internal set { Id = (ushort)((Id & 0x0FFF) | (value << 12)); } }

			public BlockInfo() {

			}
		}
	}
}