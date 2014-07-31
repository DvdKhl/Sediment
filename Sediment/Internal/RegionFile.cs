using Ionic.Zlib;
using NBTLib;
using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Internal {
	internal class RegionFile {
		private const int SectorSize = 4 * 1024;
		private static readonly byte[] ZeroSector = new byte[SectorSize];
		private const int ChunkCount = ChunkXCount * ChunkZCount;
		private const int ChunkXCount = 32;
		private const int ChunkZCount = 32;
		private const int ChunkHeaderLength = 5;

		private LinkedList<TableEntry> list;
		private TableEntry[] table;

		private class TableEntry {
			public int index;
			public int SectorOffset;
			public byte SectorCount;
			public DateTime Timestamp;
			public int Length;
			public byte CompressionType;

			public LinkedListNode<TableEntry> Node;

			public void Reset() {
				SectorOffset = 0;
				SectorCount = 0;
				Timestamp = new DateTime();
				Length = 0;
				CompressionType = 0;
				Node = null;
			}
		}

		private string path;

		public RegionFile(string regionPath) {
			this.path = regionPath;
			list = new LinkedList<TableEntry>();

			Directory.CreateDirectory(Path.GetDirectoryName(regionPath));

			using(var fileStream = new FileStream(regionPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				table = new TableEntry[ChunkXCount * ChunkZCount];

				if(fileStream.Length < SectorSize) {
					fileStream.Write(ZeroSector, 0, SectorSize); //Locations
					fileStream.Write(ZeroSector, 0, SectorSize); //Timestamps
				}

				ReadTable(fileStream);
				BuildLinks();
			}
		}
		private void ReadTable(FileStream fileStream) {
			var tableBin = new byte[SectorSize * 2];
			fileStream.Read(tableBin, 0, tableBin.Length);

			var chunkHeader = new byte[5];
			for(int i = 0; i < ChunkCount; i++) {
				var entry = new TableEntry();
				table[i] = entry;
				entry.index = i;

				entry.SectorOffset = (tableBin[i * 4 + 0] << 16) | (tableBin[i * 4 + 1] << 8) | tableBin[i * 4 + 2];
				entry.SectorCount = entry.SectorOffset < 2 ? (byte)0 : tableBin[i * 4 + 3];

				entry.Timestamp = DateTimeEx.UnixTime.AddSeconds(
					(tableBin[SectorSize + i * 4 + 0] << 24) |
					(tableBin[SectorSize + i * 4 + 1] << 16) |
					(tableBin[SectorSize + i * 4 + 2] << 8) |
					(tableBin[SectorSize + i * 4 + 3] << 0)
				);
				if(entry.SectorOffset == 0) continue;

				fileStream.Position = entry.SectorOffset << 12;
				fileStream.Read(chunkHeader, 0, 5);

				entry.Length =
					(chunkHeader[0] << 24) |
					(chunkHeader[1] << 16) |
					(chunkHeader[2] << 8) |
					(chunkHeader[3] << 0);

				entry.CompressionType = chunkHeader[4];

			}
		}
		private void BuildLinks() {
			var map = new int[ChunkCount];
			for(int i = 0; i < ChunkCount; i++) map[i] = i;
			Array.Sort(map, (a, b) => table[a].SectorOffset.CompareTo(table[b].SectorOffset));

			for(int i = 1; i < ChunkCount - 1; i++) {
				var entry = table[map[i]];
				if(entry.SectorOffset < 2) continue;
				entry.Node = list.AddLast(entry);
			}
		}


		public void StoreChunk(Chunk chunk) {
			byte[] data;
			using(var memStream = new MemoryStream(32 * 1024)) {
				using(var dataStream = new ZlibStream(memStream, CompressionMode.Compress, true)) {
					chunk.WriteTo(dataStream);
				}
				data = memStream.ToArray();
			}

			StoreChunk(chunk.X & 0x1F, chunk.Z & 0x1F, data, 0, data.Length, DateTime.UtcNow);
		}
		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data, int offset, int length) { StoreChunk(localChunkX, localChunkZ, data, offset, length, DateTime.UtcNow); }
		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data, int offset, int length, DateTime timestamp) {
			var entry = table[localChunkX + localChunkZ * ChunkXCount];

			var sectorsNeeded = (byte)((ChunkHeaderLength + length) / SectorSize + 1);
			var oldSectorCount = entry.SectorCount;


			entry.SectorCount = sectorsNeeded;
			entry.Timestamp = timestamp;
			entry.CompressionType = 2;
			entry.Length = length;


			if(oldSectorCount >= sectorsNeeded) {
				StoreChunk(data, offset, length, entry);

			} else if(list.Count == 0) {
				entry.SectorOffset = 2;
				entry.Node = list.AddFirst(entry);
				StoreChunk(data, offset, length, entry);

			} else {
				if(entry.Node != null) {
					list.Remove(entry.Node);
					entry.Node = null;
				}

				var cur = list.First;
				int sectorsAvailable = cur.Value.SectorOffset - 2;
				if(sectorsAvailable >= sectorsNeeded) {
					entry.SectorOffset = 2;
					list.AddFirst(entry);
					StoreChunk(data, offset, length, entry);

				} else {
					while(cur.Next != null && sectorsAvailable < sectorsNeeded) {
						sectorsAvailable = cur.Next.Value.SectorOffset - cur.Value.SectorOffset - cur.Value.SectorCount;
						cur = cur.Next;
					}

					if(sectorsAvailable >= sectorsNeeded) {
						entry.SectorOffset = cur.Previous.Value.SectorOffset + cur.Previous.Value.SectorCount;
						list.AddBefore(cur, entry);

					} else {
						entry.SectorOffset = list.Last.Value.SectorOffset + list.Last.Value.SectorCount;
						list.AddLast(entry);
					}
					StoreChunk(data, offset, length, entry);
				}
			}

		}
		private void StoreChunk(byte[] data, int offset, int length, TableEntry entry) {
			var lengthBin = BitConverter.GetBytes(length + 1);
			if(BitConverter.IsLittleEndian) Array.Reverse(lengthBin);

			using(var fileStream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite)) {
				WriteMeta(entry, fileStream);

				fileStream.Position = entry.SectorOffset << 12;
				fileStream.Write(lengthBin, 0, 4);
				fileStream.WriteByte(entry.CompressionType);
				fileStream.Write(data, offset, length);

				Debug.WriteLine(entry.index + " " + entry.SectorCount + " " + entry.SectorOffset);

				var toPad = SectorSize - ((int)fileStream.Length & (SectorSize - 1));
				if(toPad != SectorSize) {
					fileStream.Position = fileStream.Length;
					fileStream.Write(ZeroSector, 0, toPad);
				}
			}
		}


		public void DeleteChunk(int localChunkX, int localChunkZ) {
			var entry = table[localChunkX + localChunkZ * ChunkXCount];

			list.Remove(entry.Node);
			entry.Reset();

			using(var fileStream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite)) {
				WriteMeta(entry, fileStream);
			}
		}

		public NBTReader CreateChunkReader(int localChunkX, int localChunkZ) {
			var entry = table[localChunkX + localChunkZ * ChunkXCount];
			if(entry.SectorOffset < 2) return null;

			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			fileStream.Position = (entry.SectorOffset << 12) + 5;

			Stream dataStream;
			switch(entry.CompressionType) {
				case 1: dataStream = new GZipStream(fileStream, CompressionMode.Decompress, false); break;
				case 2: dataStream = new ZlibStream(fileStream, CompressionMode.Decompress, false); break;
				default: throw new InvalidOperationException();
			}

			return new NBTReader(dataStream);
		}

		private static void WriteMeta(TableEntry entry, FileStream fileStream) {
			fileStream.Position = entry.index * 4;
			fileStream.WriteByte((byte)(entry.SectorOffset >> 16));
			fileStream.WriteByte((byte)(entry.SectorOffset >> 8));
			fileStream.WriteByte((byte)(entry.SectorOffset >> 0));
			fileStream.WriteByte(entry.SectorCount);

			fileStream.Position = SectorSize + entry.index * 4;
			var unixTimestamp = (int)(entry.Timestamp - DateTimeEx.UnixTime).TotalSeconds;
			fileStream.WriteByte((byte)(unixTimestamp >> 24));
			fileStream.WriteByte((byte)(unixTimestamp >> 16));
			fileStream.WriteByte((byte)(unixTimestamp >> 8));
			fileStream.WriteByte((byte)(unixTimestamp >> 0));
		}
	}
}
