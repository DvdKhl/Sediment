using NBTLib;
using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class Chunk {
		public static readonly int BlockXCount = 16;
		public static readonly int BlockYCount = 256;
		public static readonly int BlockZCount = 16;
		public static readonly int BlockXZCount = BlockXCount * BlockZCount;
		public static readonly int BlockCount = BlockXCount * BlockYCount * BlockZCount;
		public static readonly int SectionCount = 16;
		public static readonly int SectionBlockXCount = 16;
		public static readonly int SectionBlockYCount = 16;
		public static readonly int SectionBlockZCount = 16;
		public static readonly int SectionBlockCount = SectionBlockXCount * SectionBlockYCount * SectionBlockZCount;
		public static readonly int SectionBlockHalfCount = SectionBlockCount >> 1;

		public static readonly int XMask = 0xF;
		public static readonly int ZMask = 0xF;

		public static readonly int XBits = 4;
		public static readonly int ZBits = 4;

		public Region Region { get; private set; }

		public int X { get; private set; }
		public int Z { get; private set; }

		private long lastUpdate;
		public long LastUpdate {
			get { return lastUpdate; }
			set { lastUpdate = value; MarkDirty(); }
		}

		public bool IsDirty { get; private set; }

		private bool isTerrainPopulated;
		public bool IsTerrainPopulated {
			get { return isTerrainPopulated; }
			set { isTerrainPopulated = value; MarkDirty(); }
		}

		private bool isLightPopulated;
		public bool IsLightPopulated {
			get { return isLightPopulated; }
			set { isLightPopulated = value; MarkDirty(); }
		}

		private long inhabitedTime;
		public long InhabitedTime {
			get { return inhabitedTime; }
			set { inhabitedTime = value; MarkDirty(); }
		}

		private byte version;
		public byte Version {
			get { return version; }
			set { version = value; MarkDirty(); }
		}

		private List<Entity> entities = new List<Entity>();
		private List<TileEntity> tileEntities = new List<TileEntity>();

		private ushort[] blockIds;
		private byte[] lightingData, biomeIds;
		private int[] heightMapData;
		private bool[] hasSection;
		private List<NBTNode> unknownChunkTags, unknownSectionTags;

		private XZData<byte> biomeIndexer;
		public XZData<byte> Biome {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return biomeIndexer ?? (biomeIndexer = new XZData<byte>(biomeIds, BlockXCount)); }
		}

		private XZData<int> heightMapIndexer;
		public XZData<int> HeightMap {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return heightMapIndexer ?? (heightMapIndexer = new XZData<int>(heightMapData, BlockXCount)); }
		}

		private XYZData<byte> lightingIndexer;
		public XYZData<byte> Lighting {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return lightingIndexer ?? (lightingIndexer = new XYZData<byte>(lightingData, BlockXCount, BlockZCount)); }
		}

		public void WriteTo(Stream stream) {
			var writer = new NBTWriter(stream);

			writer.WriteCompound("", w => {
				w.WriteCompound("Level", WriteChunk);
			});
		}
		private void WriteChunk(NBTWriter writer) {
			writer.Write("V", Version);
			writer.Write("xPos", X);
			writer.Write("zPos", Z);
			writer.Write("LastUpdate", LastUpdate);
			writer.Write("InhabitedTime", InhabitedTime);

			writer.Write("TerrainPopulated", (byte)(IsTerrainPopulated ? 1 : 0));
			writer.Write("LightPopulated", (byte)(IsLightPopulated ? 1 : 0));

			writer.Write("Sections", hasSection, WriteSections); //TODO: Write only non-empty sections
			writer.Write("HeightMap", heightMapData);
			writer.Write("Biomes", biomeIds);

			writer.Write("TileEntities", tileEntities.Count, WriteTileEntities); //TODO: Write only non-empty sections
			writer.Write("Entities", entities.Count, WriteEntities); //TODO: Write only non-empty sections

			foreach(var unknownTag in unknownChunkTags) writer.Write(unknownTag);
		}

		private void WriteEntities(NBTWriter writer, int i) {
		}
		private void WriteTileEntities(NBTWriter writer, int i) {
		}

		private void WriteSections(NBTWriter writer, int y) {

			writer.Write("Y", (byte)y);

			var secBlocks = new byte[SectionBlockCount];
			for(int i = 0; i < secBlocks.Length; i++) secBlocks[i] = (byte)blockIds[(y << 12) | i];
			writer.Write("Blocks", secBlocks);

			var secAdd = new byte[SectionBlockCount / 2];
			for(int i = 0; i < secAdd.Length; i++) {
				secAdd[i] = (byte)((blockIds[(y << 12) | (i * 2 + 0)] >> 8) & 0x0F);
				secAdd[i] |= (byte)((blockIds[(y << 12) | (i * 2 + 1)] >> 4) & 0xF0);
			}
			//writer.Write("Add", secAdd);

			var secData = new byte[SectionBlockCount / 2];
			for(int i = 0; i < secAdd.Length; i++) {
				secData[i] = (byte)((blockIds[(y << 12) | (i * 2 + 0)] >> 12) & 0x0F);
				secData[i] |= (byte)((blockIds[(y << 12) | (i * 2 + 1)] >> 8) & 0xF0);
			}
			writer.Write("Data", secData);

			var secSkyLight = new byte[SectionBlockCount / 2];
			for(int i = 0; i < secAdd.Length; i++) {
				secSkyLight[i] = (byte)(lightingData[(y << 12) | (i * 2 + 0)] & 0x0F);
				secSkyLight[i] |= (byte)(lightingData[(y << 12) | (i * 2 + 1)] << 4);
			}
			writer.Write("BlockLight", secSkyLight);

			var secBlockLight = new byte[SectionBlockCount / 2];
			for(int i = 0; i < secAdd.Length; i++) {
				secBlockLight[i] = (byte)(lightingData[(y << 12) | (i * 2 + 0)] >> 4);
				secBlockLight[i] |= (byte)(lightingData[(y << 12) | (i * 2 + 1)] & 0xF0);
			}
			writer.Write("SkyLight", secBlockLight);

			foreach(var unknownTag in unknownSectionTags) writer.Write(unknownTag);
		}

		internal Chunk(int x, int z, Region region) {
			this.Region = region;
			this.X = x;
			this.Z = z;

			blockIds = new ushort[BlockCount];
			lightingData = new byte[BlockCount];
			hasSection = new bool[SectionCount];

			unknownChunkTags = new List<NBTNode>();
			unknownSectionTags = new List<NBTNode>();

			biomeIds = new byte[BlockXZCount];
			heightMapData = new int[BlockXZCount];

			version = 1;
		}

		internal Chunk(NBTReader reader, Region region) {
			this.Region = region;

			blockIds = new ushort[BlockCount];
			lightingData = new byte[BlockCount];
			hasSection = new bool[SectionCount];

			unknownChunkTags = new List<NBTNode>();
			unknownSectionTags = new List<NBTNode>();

			byte[][] blocks = new byte[SectionCount][];
			byte[][] add = new byte[SectionCount][];
			byte[][] data = new byte[SectionCount][];
			byte[][] blockLight = new byte[SectionCount][];
			byte[][] skyLight = new byte[SectionCount][];

			using(reader) {
				reader.MoveNext();
				reader.MoveNext();

				int length;
				while(reader.MoveNext() && reader.Type != NBTType.End) {
					switch(reader.Name) {
						case "xPos": X = (int)reader.Value; break;
						case "zPos": Z = (int)reader.Value; break;
						case "LastUpdate": LastUpdate = (long)reader.Value; break;
						case "LightPopulated": isLightPopulated = (byte)reader.Value != 0; break;
						case "TerrainPopulated": isTerrainPopulated = (byte)reader.Value != 0; break;
						case "V": Version = (byte)reader.Value; break;
						case "InhabitedTime": inhabitedTime = (long)reader.Value; break;
						case "Biomes": biomeIds = (byte[])reader.Value; break;
						case "HeightMap": heightMapData = (int[])reader.Value; break;
						case "Entities":
							length = (int)reader.Value;
							for(int i = 0; i < length; i++) {
								while(reader.MoveNext() && reader.Type != NBTType.End) ;
							}
							break;

						case "TileEntities":
							length = (int)reader.Value;
							for(int i = 0; i < length; i++) {
								while(reader.MoveNext() && reader.Type != NBTType.End) ;
							}
							break;

						case "Sections":
							length = (int)reader.Value;
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
										default: unknownSectionTags.Add(reader.TreeToStructure()); break;
									}
								}
								blocks[y] = secBlocks;
								add[y] = secAdd;
								data[y] = secData;
								blockLight[y] = secBlockLight;
								skyLight[y] = secSkyLight;
							}
							break;

						default: unknownChunkTags.Add(reader.TreeToStructure()); break;
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
					for(int i = 0; i < SectionBlockHalfCount; i++) {
						blockIds[(y << 12) | (i * 2 + 0)] |= (ushort)((add[y][i] & 0x0F) << 8);
						blockIds[(y << 12) | (i * 2 + 1)] |= (ushort)((add[y][i] & 0xF0) << 4);
					}
				}
				if(data[y] != null) {
					for(int i = 0; i < SectionBlockHalfCount; i++) {
						blockIds[(y << 12) | (i * 2 + 0)] |= (ushort)((data[y][i] & 0x0F) << 12);
						blockIds[(y << 12) | (i * 2 + 1)] |= (ushort)((data[y][i] & 0xF0) << 8);
					}
				}
				if(skyLight[y] != null) {
					for(int i = 0; i < SectionBlockHalfCount; i++) {
						lightingData[(y << 12) | (i * 2 + 0)] = (byte)(skyLight[y][i] & 0x0F);
						lightingData[(y << 12) | (i * 2 + 1)] = (byte)((skyLight[y][i] & 0xF0) >> 4);
					}
				}
				if(blockLight[y] != null) {
					for(int i = 0; i < SectionBlockHalfCount; i++) {
						lightingData[(y << 12) | (i * 2 + 0)] |= (byte)((blockLight[y][i] & 0x0F) << 4);
						lightingData[(y << 12) | (i * 2 + 1)] |= (byte)(blockLight[y][i] & 0xF0);
					}
				}
			}

			if(biomeIds == null) biomeIds = new byte[BlockXZCount];
			if(heightMapData == null) heightMapData = new int[BlockXZCount];
		}

		public ushort this[int blockIndex] {
			get { return blockIds[blockIndex]; }
			set { blockIds[blockIndex] = value; MarkDirty(); }
		}
		public ushort this[int y, int blockPlaneIndex] {
			get { return blockIds[y << 8 | blockPlaneIndex]; }
			set { blockIds[y << 8 | blockPlaneIndex] = value; MarkDirty(); }
		}
		public ushort this[int x, int y, int z] {
			get { return blockIds[ToIndex(x, y, z)]; }
			set { blockIds[ToIndex(x, y, z)] = value; MarkDirty(); }
		}

		//public void Reload() { throw new NotImplementedException(); }
		//public void Discard() { throw new NotImplementedException(); }
		//public void Save() { throw new NotImplementedException(); }
		public void MarkDirty() { IsDirty = true; }
		internal void MarkPristine() { IsDirty = false; }


		public void UpdateHeightMap() {
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					for(int y = Chunk.BlockYCount - 1; y >= 0; y--) {
						if(!Blocks.ZeroOpacityBlockIds.Contains(blockIds[Chunk.ToIndex(x, y, z)])) {
							heightMapData[x | (z * Chunk.BlockXCount)] = y + 1;
							break;
						}
					}
				}
			}
			MarkDirty();
		}

		public void UpdateLighting() {
			var lighting = new Lighting();
			lighting.LightChunk(blockIds, heightMapData, LightingOptions.Fastest, lightingData);
		}

		public void UpdateHasSections() {
			for(int y = 0; y < SectionCount; y++) {
				var hasSection = false;
				for(int i = 0; i < SectionBlockCount && !hasSection; i++) {
					hasSection |= blockIds[(y << 12) | i] == 0;
				}

				this.hasSection[y] = hasSection;
			}
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
		public static bool IsAtRightYZPlane(int blockIndex) { return (blockIndex & 0x000F) == 0x000F; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtBackYXPlane(int blockIndex) { return (blockIndex & 0x00F0) == 0; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtFrontYXPlane(int blockIndex) { return (blockIndex & 0x00F0) == 0x00F0; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtBottomXZPlane(int blockIndex) { return (blockIndex & 0xFF00) == 0; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAtTopXZPlane(int blockIndex) { return (blockIndex & 0xFF00) == 0xFF00; }

		public class XZData<T> {
			private T[] data;
			private int xLength;

			public T this[int index] {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get { return data[index]; }
			}

			public T this[int x, int z] {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get { return data[x + z * xLength]; }
			}

			public XZData(T[] data, int xLength) {
				this.data = data;
				this.xLength = xLength;
			}
		}
		public class XYZData<T> {
			private T[] data;
			private int xLength, xzLength;

			public T this[int index] {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get { return data[index]; }
			}

			public T this[int x, int y, int z] {
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get { return data[x + z * xLength + y * xzLength]; }
			}

			public XYZData(T[] data, int xLength, int zLength) {
				this.data = data;
				this.xLength = xLength;
				this.xzLength = zLength * xLength;
			}
		}
	}
}
