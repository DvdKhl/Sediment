using Ionic.Zlib;
using NBTLib;
using System;
using System.Collections.Generic;
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

		private bool disposed;

		private TableEntry firstLocationTableEntry;
		private TableEntry[] table;

		private class TableEntry {
			public int SectorOffset;
			public byte SectorCount;
			public DateTime Timestamp;
			public int Length;
			public byte CompressionType;

			public TableEntry Prev, Next;

			public void Reset() {
				SectorOffset = 0;
				SectorCount = 0;
				Timestamp = new DateTime();
				Length = 0;
				CompressionType = 0;
				Prev = Next = null;
			}
		}

		private string path;

		public RegionFile(string regionPath) {
			this.path = regionPath;

			using(var fileStream = new FileStream(regionPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				table = new TableEntry[ChunkXCount * ChunkZCount];

				if(fileStream.Length < SectorSize) {
					throw new InvalidOperationException();
					fileStream.Write(ZeroSector, 0, SectorSize); //Locations
					fileStream.Write(ZeroSector, 0, SectorSize); //Timestamps

				} else {
					ReadTable(fileStream);
					BuildLinks();
				}
			}
		}
		private void ReadTable(FileStream fileStream) {
			var tableBin = new byte[SectorSize * 2];
			fileStream.Read(tableBin, 0, tableBin.Length);

			var chunkHeader = new byte[5];
			for(int i = 0; i < ChunkCount; i++) {
				var entry = new TableEntry();
				entry.SectorOffset = (tableBin[i * 4 + 0] << 16) | (tableBin[i * 4 + 1] << 8) | tableBin[i * 4 + 2];
				entry.SectorCount = tableBin[i * 4 + 3];

				entry.Timestamp = DateTimeEx.UnixTime.AddSeconds(
					(tableBin[SectorSize + i * 4 + 0] << 24) |
					(tableBin[SectorSize + i * 4 + 1] << 16) |
					(tableBin[SectorSize + i * 4 + 2] << 8) |
					(tableBin[SectorSize + i * 4 + 3] << 0)
				);

				fileStream.Position = entry.SectorOffset << 12;
				fileStream.Read(chunkHeader, 0, 5);

				entry.Length =
					(chunkHeader[0] << 24) |
					(chunkHeader[1] << 16) |
					(chunkHeader[2] << 8) |
					(chunkHeader[3] << 0);

				entry.CompressionType = chunkHeader[4];

				table[i] = entry;
			}
		}
		private void BuildLinks() {
			var map = new int[ChunkCount];
			for(int i = 0; i < ChunkCount; i++) map[i] = i;
			Array.Sort(map, (a, b) => table[a].SectorOffset.CompareTo(table[b].SectorOffset));

			for(int i = ChunkCount - 2; i >= 1; i--) { //Set all links except first and last; break when we encounter an unused entry
				var entry = table[map[i]];
				if(entry.SectorCount == 0) {
					firstLocationTableEntry = table[map[i + 1]];
					firstLocationTableEntry.Prev = null;
					break;
				}

				entry.Prev = table[map[i - 1]];
				entry.Next = table[map[i + 1]];
			}

			if(table[map[ChunkCount - 2]].SectorCount != 0) { //Set prev link for last entry if there is one
				table[map[ChunkCount - 1]].Prev = table[map[ChunkCount - 2]];
			}
			if(firstLocationTableEntry == null) { //All entries are not empty
				firstLocationTableEntry = table[map[0]].Next = table[map[1]];
			}
		}


		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data) { StoreChunk(localChunkX, localChunkZ, data, 0, data.Length, DateTime.UtcNow); }
		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data, DateTime timestamp) { StoreChunk(localChunkX, localChunkZ, data, 0, data.Length, timestamp); }
		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data, int offset, int length) { StoreChunk(localChunkX, localChunkZ, data, offset, length, DateTime.UtcNow); }
		public void StoreChunk(int localChunkX, int localChunkZ, byte[] data, int offset, int length, DateTime timestamp) {
			var entry = table[localChunkX + localChunkZ * ChunkXCount];

			var sectorsNeeded = (byte)((ChunkHeaderLength + length) / SectorSize + 1);

			if(entry.SectorCount <= sectorsNeeded) {
				StoreChunk(data, offset, length, timestamp, entry.SectorOffset);

			} else {
				var prev = firstLocationTableEntry;
				var cur = prev.Next;

				int sectorsAvailable = cur.SectorOffset - prev.SectorOffset - prev.SectorCount;
				while(cur != null && sectorsAvailable < sectorsNeeded) {
					sectorsAvailable = cur.SectorOffset - prev.SectorOffset - prev.SectorCount;
					prev = cur;
					cur = cur.Next;
				}

				StoreChunk(data, offset, length, timestamp, prev.SectorOffset + prev.SectorCount);

				entry.SectorOffset = prev.SectorOffset + prev.SectorCount;

				if(prev != entry) {
					entry.Next = prev.Next;
					prev.Next = entry;
				}
			}

			entry.SectorCount = sectorsNeeded;
			entry.Timestamp = timestamp;
			entry.CompressionType = 2;
			entry.Length = length;
		}
		private void StoreChunk(byte[] data, int offset, int length, DateTime timestamp, int sectorOffset) {
			var lengthBin = BitConverter.GetBytes(length);
			if(BitConverter.IsLittleEndian) Array.Reverse(lengthBin);

			using(var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) {
				fileStream.Position = sectorOffset << 12;
				fileStream.Write(lengthBin, 0, 4);
				fileStream.WriteByte(2);
				fileStream.Write(data, offset, length);
			}
		}

		public void DeleteChunk(int localChunkX, int localChunkZ) {
			var entry = table[localChunkX + localChunkZ * ChunkXCount];

			if(entry.Next != null) entry.Next.Prev = entry.Prev;
			if(entry.Prev != null) entry.Prev.Next = entry.Next;
			entry.Reset();
		}

		public NBTReader CreateChunkReader(int localChunkX, int localChunkZ) {
			var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

			var entry = table[localChunkX + localChunkZ * ChunkXCount];
			fileStream.Position = (entry.SectorOffset << 12) + 5;

			Stream dataStream;
			switch(entry.CompressionType) {
				case 1: dataStream = new GZipStream(fileStream, CompressionMode.Decompress, false); break;
				case 2: dataStream = new ZlibStream(fileStream, CompressionMode.Decompress, false); break;
				default: throw new InvalidOperationException();
			}

			return new NBTReader(dataStream);
		}
	}
}
