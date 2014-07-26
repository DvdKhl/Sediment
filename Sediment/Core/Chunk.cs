using NBTLib;
using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class Chunk {
		public static readonly int BlockXCount = 16;
		public static readonly int BlockYCount = 256;
		public static readonly int BlockZCount = 16;
		public static readonly int BlockCount = BlockXCount * BlockYCount * BlockZCount;
		public static readonly int SectionCount = 16;
		public static readonly int SectionBlockXCount = 16;
		public static readonly int SectionBlockYCount = 16;
		public static readonly int SectionBlockZCount = 16;
		public static readonly int SectionBlockCount = SectionBlockXCount * SectionBlockYCount * SectionBlockZCount;

		public int X { get; private set; }
		public int Z { get; private set; }

		public DateTime LastSaveOn { get; private set; }

		public DateTime lastEditOn;
		public DateTime LastEditOn {
			get { return lastEditOn; }
			set { lastEditOn = value.ToUniversalTime(); MarkDirty(); }
		}

		public bool IsDirty { get; private set; }

		public bool isTerrainPopulated;
		public bool IsTerrainPopulated {
			get { return isTerrainPopulated; }
			set { isTerrainPopulated = value; MarkDirty(); }
		}

		public bool isLightPopulated;
		public bool IsLightPopulated {
			get { return isLightPopulated; }
			set { isLightPopulated = value; MarkDirty(); }
		}

		public long inhabitedTime;
		public long InhabitedTime {
			get { return inhabitedTime; }
			set { inhabitedTime = value; MarkDirty(); }
		}

		public byte version;
		public byte Version {
			get { return version; }
			set { version = value; MarkDirty(); }
		}


		private ushort[] blockIds;
		private byte[] lightingData, biomeIds;
		private int[] heightMapData;
		private bool[] hasSection;

		public void Reload() { throw new NotImplementedException(); }
		public void Discard() { throw new NotImplementedException(); }
		public void Save() { throw new NotImplementedException(); }
		public void MarkDirty() { IsDirty = true; lastEditOn = DateTime.UtcNow; }

		internal Chunk(NBTLib.NBTReader reader) {
			blockIds = new ushort[BlockCount];
			lightingData = new byte[BlockCount];
			hasSection = new bool[SectionCount];

			byte[][] blocks = new byte[SectionCount][];
			byte[][] add = new byte[SectionCount][];
			byte[][] data = new byte[SectionCount][];
			byte[][] blockLight = new byte[SectionCount][];
			byte[][] skyLight = new byte[SectionCount][];

			using(reader) {
				reader.MoveNext();
				reader.MoveNext();

				while(reader.MoveNext() && reader.Type != NBTType.End) {
					switch(reader.Name) {
						case "xPos": X = (int)reader.Value; break;
						case "zPos": Z = (int)reader.Value; break;
						case "LastUpdate": LastSaveOn = lastEditOn = DateTimeEx.UnixTime.AddSeconds((long)reader.Value); break;
						case "LightPopulated": isLightPopulated = (byte)reader.Value != 0; break;
						case "TerrainPopulated": isTerrainPopulated = (byte)reader.Value != 0; break;
						case "V": Version = (byte)reader.Value; break;
						case "InhabitedTime": inhabitedTime = (long)reader.Value; break;
						case "Biomes": biomeIds = (byte[])reader.Value; break;
						case "HeightMap": heightMapData = (int[])reader.Value; break;

						case "Sections":
							var length = (int)reader.Value;
							for(int i = 0; i < length; i++) {
								byte y = 0;
								byte[] secBlocks = null, secAdd = null, secData = null, secBlockLight = null, secSkyLight = null;
								while(reader.MoveNext() && reader.Type != NBTType.End) {
									switch(reader.Name) {
										case "Y": y = (byte)reader.Value; break;
										case "Blocks": secBlocks = (byte[])reader.Value; break;
										case "Add": secAdd = (byte[])reader.Value; break;
										case "Data": secData = (byte[])reader.Value; break;
										case "BlockLight": secBlockLight = (byte[])reader.Value; break;
										case "SkyLight": secSkyLight = (byte[])reader.Value; break;
										default: reader.TreeToStructure(); break; //TODO: keep unknown nbt tags
									}
								}
								blocks[y] = secBlocks;
								add[y] = secAdd;
								data[y] = secData;
								blockLight[y] = secBlockLight;
								skyLight[y] = secSkyLight;
							}
							break;


						default: reader.TreeToStructure(); break; //TODO: keep unknown nbt tags
					}
				}
				reader.MoveNext();
			}


			for(int y = 0; y < SectionCount; y++) {
				if(blocks[y] != null) {
					hasSection[y] = true;
					for(int i = 0; i < SectionBlockCount; i++) {
						blockIds[(y << 12) | i] = blocks[y][i];
					}
				}
				if(add[y] != null) {
					for(int i = 0; i < add.Length; i++) {
						blockIds[(y << 12) | (i + 0)] |= (ushort)((add[y][i] & 0x0F) << 16);
						blockIds[(y << 12) | (i + 1)] |= (ushort)((add[y][i] & 0xF0) << 12);
					}
				}
				if(skyLight[y] != null) {
					for(int i = 0; i < skyLight.Length; i++) {
						lightingData[(y << 12) | (i + 0)] = (byte)(skyLight[y][i] & 0x0F);
						lightingData[(y << 12) | (i + 1)] = (byte)((skyLight[y][i] & 0xF0) >> 4);
					}
				}
				if(blockLight[y] != null) {
					for(int i = 0; i < blockLight.Length; i++) {
						lightingData[(y << 12) | (i + 0)] |= (byte)((blockLight[y][i] & 0x0F) << 4);
						lightingData[(y << 12) | (i + 1)] |= (byte)(blockLight[y][i] & 0xF0);
					}
				}
			}
		}

		public void UpdateHeightMap() {
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					for(int y = Chunk.BlockYCount - 1; y >= 0; y--) {

						var bla = blockIds[Chunk.ToIndex(x, y, z)];
						if(!Blocks.ZeroOpacityBlockIds.Contains(blockIds[Chunk.ToIndex(x, y, z)])) {
							heightMapData[x | (z * Chunk.BlockXCount)] = y;
							break;
						}
					}
				}
			}
			MarkDirty();
		}

		public void UpdateLighting() {
			var lighting = new Lighting();
			lighting.LightChunk(blockIds, heightMapData, LightingOptions.None, lightingData);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToIndex(int x, int y, int z) { return x | (z << 4) | (y << 8); }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextX(int blockIndex) { return blockIndex + 1; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PrevX(int blockIndex) { return blockIndex - 1; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextZ(int blockIndex) { return blockIndex + BlockXCount; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PrevZ(int blockIndex) { return blockIndex - BlockXCount; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextY(int blockIndex) { return blockIndex + BlockXCount * BlockZCount; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PrevY(int blockIndex) { return blockIndex - BlockXCount * BlockZCount; }


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtLeftYZPlane(int blockIndex) { return (blockIndex & 0x000F) == 0; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtRightYZPlane(int blockIndex) { return (blockIndex & 0x000F) == 15; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtBackYXPlane(int blockIndex) { return (blockIndex & 0x00F0) == 0; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtFrontYXPlane(int blockIndex) { return (blockIndex & 0x00F0) == 15 << 4; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtBottomXZPlane(int blockIndex) { return (blockIndex & 0xFF00) == 0; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtTopXZPlane(int blockIndex) { return (blockIndex & 0xFF00) == 255 << 8; }

	}
}
