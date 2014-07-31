using Ionic.Zlib;
using Sediment.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class Region {
		public static readonly int ChunkXCount = 32;
		public static readonly int ChunkYCount = 1;
		public static readonly int ChunkZCount = 32;
		public static readonly int ChunkXZCount = ChunkXCount * ChunkZCount;
		public static readonly int ChunkCount = ChunkXCount * ChunkYCount * ChunkZCount;

		public static readonly int BlockXCount = ChunkXCount * 16;
		public static readonly int BlockYCount = ChunkYCount * 256;
		public static readonly int BlockZCount = ChunkZCount * 16;
		public static readonly int BlockXZCount = BlockXCount * BlockZCount;
		public static readonly int BlockCount = BlockXCount * BlockYCount * BlockZCount;

		private World world;
		private RegionFile regionFile;

		public int X { get; private set; }
		public int Z { get; private set; }

		internal Region(World world, int regionX, int regionZ) {
			this.world = world;
			this.X = regionX;
			this.Z = regionZ;

			var regionFileName = string.Format(world.Info.RegionFilePathFormat, regionX, regionZ);
			var regionFilePath = Path.Combine(world.Level.RootPath, world.Info.RegionPath, regionFileName);

			regionFile = new RegionFile(regionFilePath);
		}

		internal NBTLib.NBTReader CreateChunkReader(int localChunkX, int localChunkZ) {
			return regionFile.CreateChunkReader(localChunkX, localChunkZ);
		}

		public static int ToChunkIndex(int x, int z) { return x | (z << 5); }


		internal void SaveChunks(IEnumerable<Chunk> dirtyChunks) {
			if(dirtyChunks.Any(c => c.X >> 5 != X || c.Z >> 5 != Z)) throw new InvalidOperationException("Chunk is not in this region instance");

			var q = (from chunk in dirtyChunks.AsParallel()
					 select new {
						 Chunk = chunk,
						 Data = SerializeChunk(chunk)
					 });

			foreach(var chunkInfo in q) {
				regionFile.StoreChunk(chunkInfo.Chunk.X & 0x1F, chunkInfo.Chunk.Z & 0x1F, chunkInfo.Data, 0, chunkInfo.Data.Length, DateTime.UtcNow);
				chunkInfo.Chunk.MarkPristine();
			}
		}
		internal void SaveChunk(Chunk dirtyChunk) {
			if(dirtyChunk.X >> 5 != X || dirtyChunk.Z >> 5 != Z) throw new InvalidOperationException("Chunk is not in this region instance");

			var data = SerializeChunk(dirtyChunk);
			regionFile.StoreChunk(dirtyChunk.X & 0x1F, dirtyChunk.Z & 0x1F, data, 0, data.Length, DateTime.UtcNow);
		}
		private byte[] SerializeChunk(Chunk chunk) {
			world.OnSavingChunk(chunk);

			byte[] data;
			using(var memStream = new MemoryStream(32 * 1024)) {
				using(var dataStream = new ZlibStream(memStream, CompressionMode.Compress, true)) {
					chunk.WriteTo(dataStream);
				}
				data = memStream.ToArray();
			}
			return data;
		}

	}
}
