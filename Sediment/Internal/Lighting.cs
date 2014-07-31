using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Internal {
	public class Lighting {
		private static readonly byte[] maxLookup;
		private static readonly byte[] subtractLookup;
		private static readonly bool[] greaterThanLookup;

		static Lighting() {
			maxLookup = new byte[256 * 256];
			subtractLookup = new byte[256 * 256];
			greaterThanLookup = new bool[256 * 256];

			int i = 0;
			for(int a1 = 0; a1 < 16; a1++) {
				for(int a2 = 0; a2 < 16; a2++) {
					for(int b1 = 0; b1 < 16; b1++) {
						for(int b2 = 0; b2 < 16; b2++) {
							greaterThanLookup[i] = a2 > b2 || a1 > b1;
							subtractLookup[i] = (byte)(Math.Max(a2 - b2, 0) | (Math.Max(a1 - b1, 0) << 4));
							maxLookup[i] = (byte)(Math.Max(a2, b2) | (Math.Max(a1, b1) << 4));
							i++;
						}
					}
				}
			}


		}

		public Lighting() {
			for(int i = 0; i < pendingPriorityBlocks.Length; i++) {
				pendingPriorityBlocks[i] = new List<int>(64);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte Max(byte a, byte b) { return maxLookup[b | (a << 8)]; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte Subtract(byte a, byte b) { return subtractLookup[b | (a << 8)]; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsGreater(byte a, byte b) { return greaterThanLookup[b | (a << 8)]; }

		int[] heightMap;
		ushort[] blockIds;
		byte[] lighting, opacity;
		List<int>[] pendingPriorityBlocks = new List<int>[16];
		LightingOptions options;

		public void LightChunk(ushort[] blockIds, int[] heightMap, LightingOptions options, byte[] lighting) {
			if((options & LightingOptions.NoDiffusion) == 0) throw new NotImplementedException();

			if(heightMap == null) throw new ArgumentNullException("heightMap");
			if(blockIds == null) throw new ArgumentNullException("blockIds");
			if(lighting == null) throw new ArgumentNullException("lighting");

			if(blockIds.Length != Chunk.BlockCount) throw new ArgumentException("Wrong length", "blockIds");
			if(heightMap.Length != Chunk.BlockXCount * Chunk.BlockZCount) throw new ArgumentException("Wrong length", "heightMap");
			if(lighting.Length != Chunk.BlockCount) throw new ArgumentException("Wrong length", "lighting");

			foreach(var pendingBlocks in pendingPriorityBlocks) pendingBlocks.Clear();
			Array.Clear(lighting, 0, lighting.Length);
			opacity = new byte[Chunk.BlockCount];

			this.blockIds = blockIds;
			this.heightMap = heightMap;
			this.lighting = lighting;
			this.options = options;


			//Set vertical skylight rays
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)];

					for(int y = Chunk.BlockYCount - 1; y >= height; y--) {
						var blockIndex = Chunk.ToIndex(x, y, z);
						lighting[blockIndex] = 0x0F;
						opacity[blockIndex] = 0x11;
					}
				}
			}

		
			//Set light emitting blocks and store opacity values
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)];
					for(int y = 0; y < height; y++) {
						var blockIndex = Chunk.ToIndex(x, y, z);
						var blockId = blockIds[blockIndex];

						Blocks.BlockInfo blockInfo;
						if(!Blocks.ById.TryGetValue(blockId, out blockInfo)) {
							Blocks.BlockInfo[] blockInfos;
							if(Blocks.ByPartialId.TryGetValue(blockId, out blockInfos)) {
								blockInfo = blockInfos[0];
							} else blockInfo = Blocks.Air;
						}

						if(blockInfo.Luminance > lighting[blockIndex]) {
							//Add to pending block since it won't be processed by skylight
							pendingPriorityBlocks[blockInfo.Luminance].Add(blockIndex);
						}
						lighting[blockIndex] |= (byte)(blockInfo.Luminance << 4);

						//Change to aaaabbbb format and set at least 1 for opacity
						opacity[blockIndex] = (byte)Math.Max((byte)1, Math.Min(blockInfo.Opacity, (byte)15));
						opacity[blockIndex] |= (byte)(opacity[blockIndex] << 4);
					}
				}
			}

			//TODO: Skylight shining on non fully opaque blocks

			//Process Skylight
			//TODO: Full skylight processing (e.g. vertical lightrays entering caves)
			if((options & LightingOptions.OnlyVerticalSkylight) == 0) throw new NotImplementedException();
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)];
					if(height >= Chunk.BlockXZCount) continue;

					var blockIndex = Chunk.ToIndex(x, height, z);
					LightChunk(blockIndex, lighting[blockIndex]);
				}
			}

			//for(int z = 0; z < 16; z++) {
			//	for(int x = 0; x < 16; x++) {
			//		Console.Write(lighting[Chunk.ToIndex(x, 0xfb, z)].ToString("X"));
			//	}
			//	Console.WriteLine();
			//}

			//Process remaining blocklights
			for(int i = pendingPriorityBlocks.Length - 1; i >= 0; i--) {
				foreach(var blockIndex in pendingPriorityBlocks[i]) {
					LightChunk(blockIndex, lighting[blockIndex]);
				}
			}
		}

		private void LightChunk(int blockIndex, byte luminance) {
			//Left and right halfes (except middle plane)
			LightChunkPrevX(blockIndex, luminance);
			LightChunkNextX(blockIndex, luminance);

			//Top and Bottom half planes (except middle line)
			LightChunkPrevY(blockIndex, luminance);
			LightChunkNextY(blockIndex, luminance);

			//Front and Back half lines (except middle point)
			LightChunkPrevZ(blockIndex, luminance);
			LightChunkNextZ(blockIndex, luminance);

			//Middle point
			lighting[blockIndex] = Max(luminance, lighting[blockIndex]);
		}

		private void LightChunkPrevX(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtLeftYZPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.PrevX(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				LightChunkPrevY(neighborBlockIndex, neighborLuminance);
				LightChunkNextY(neighborBlockIndex, neighborLuminance);
				LightChunkPrevZ(neighborBlockIndex, neighborLuminance);
				LightChunkNextZ(neighborBlockIndex, neighborLuminance);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}

		}

		private void LightChunkNextX(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtRightYZPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.NextX(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				LightChunkPrevY(neighborBlockIndex, neighborLuminance);
				LightChunkNextY(neighborBlockIndex, neighborLuminance);
				LightChunkPrevZ(neighborBlockIndex, neighborLuminance);
				LightChunkNextZ(neighborBlockIndex, neighborLuminance);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}

		}

		private void LightChunkPrevY(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtBottomXZPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.PrevY(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				LightChunkPrevZ(neighborBlockIndex, neighborLuminance);
				LightChunkNextZ(neighborBlockIndex, neighborLuminance);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}
		}

		private void LightChunkNextY(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtTopXZPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.NextY(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				LightChunkPrevZ(neighborBlockIndex, neighborLuminance);
				LightChunkNextZ(neighborBlockIndex, neighborLuminance);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}
		}

		private void LightChunkPrevZ(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtBackYXPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.PrevZ(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}
		}

		private void LightChunkNextZ(int blockIndex, byte luminance) {
			var neighborLuminance = luminance;
			var neighborBlockIndex = blockIndex;
			while(neighborLuminance != 0 && !Chunk.IsAtFrontYXPlane(neighborBlockIndex)) {
				neighborBlockIndex = Chunk.NextZ(neighborBlockIndex);
				neighborLuminance = Subtract(neighborLuminance, opacity[neighborBlockIndex]);
				if(IsGreater(lighting[neighborBlockIndex], neighborLuminance)) break;

				lighting[neighborBlockIndex] = Max(neighborLuminance, lighting[neighborBlockIndex]);

				//if(IsGreater(opacity[neighborBlockIndex], 0x11) && neighborLuminance != 0) LightChunk(neighborBlockIndex, neighborLuminance);
			}
		}
	}

	public enum LightingOptions {
		None,
		OnlyVerticalSkylight = 1,
		NoDiffusion = 2,
		NoStiching = 4,
		Fastest = OnlyVerticalSkylight | NoDiffusion | NoStiching
	}
}
