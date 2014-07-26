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
							greaterThanLookup[i] = a1 > a2 || b1 > b2;
							subtractLookup[i] = (byte)(Math.Max(a1 - a2, 0) | (Math.Max(b1 - b2, 0) << 4));
							maxLookup[i] = (byte)(Math.Max(a1, a2) | (Math.Max(b1, b2) << 4));
							i++;
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte Max(byte a, byte b) { return maxLookup[a | (b << 8)]; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static byte Subtract(byte a, byte b) { return subtractLookup[a | (b << 8)]; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsGreater(byte a, byte b) { return greaterThanLookup[a | (b << 8)]; }

		int[] heightMap;
		ushort[] blockIds;
		byte[] lighting, opacity;
		List<int> pendingBlocks = new List<int>(1024);
		LightingOptions options;

		public void LightChunk(ushort[] blockIds, int[] heightMap, LightingOptions options, byte[] lighting) {
			if(heightMap == null) throw new ArgumentNullException("heightMap");
			if(blockIds == null) throw new ArgumentNullException("blockIds");
			if(lighting == null) throw new ArgumentNullException("lighting");

			if(blockIds.Length != Chunk.BlockCount) throw new ArgumentException("Wrong length", "blockIds");
			if(heightMap.Length != Chunk.BlockXCount * Chunk.BlockZCount) throw new ArgumentException("Wrong length", "heightMap");
			if(lighting.Length != Chunk.BlockCount) throw new ArgumentException("Wrong length", "lighting");

			pendingBlocks.Clear();

			this.blockIds = blockIds;
			this.heightMap = heightMap;
			this.lighting = lighting;
			this.options = options;

			//Set vertical skylight rays
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)];
					for(int y = Chunk.BlockYCount - 1; y >= height; y--) {
						lighting[Chunk.ToIndex(x, y, z)] = 0x0F;
					}
				}
			}

			opacity = new byte[Chunk.BlockCount];

			//Set light emitting blocks and store opacity values
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)] + 1;
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
							pendingBlocks.Add(blockIndex);
						}
						lighting[blockIndex] |= (byte)(blockInfo.Luminance << 4);

						//Change to aaaabbbb format and set at least 1 for opacity
						opacity[blockIndex] = (byte)Math.Max(1, Math.Min(blockInfo.Opacity, 15));
						opacity[blockIndex] |= (byte)(opacity[blockIndex] << 4);
					}
				}
			}

			//TODO: Skylight shining on non fully opaque blocks

			//Process Skylight
			for(int x = 0; x < Chunk.BlockXCount; x++) {
				for(int z = 0; z < Chunk.BlockZCount; z++) {
					var height = heightMap[x | (z * Chunk.BlockXCount)];
					var blockIndex = Chunk.ToIndex(x, height, z);
					LightChunk(blockIndex, lighting[blockIndex]);
				}
			}

			//Process remaining blocklights
			foreach(var blockIndex in pendingBlocks) {
				LightChunk(blockIndex, lighting[blockIndex]);
			}
		}


		private void LightChunk(int blockIndex, byte luminance) {
			byte newLuminance;
			int neighborBlockIndex;

			lighting[blockIndex] = luminance;

			if(!Chunk.IsAtLeftYZPlane(blockIndex)) {
				neighborBlockIndex = Chunk.PrevX(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}

			if(!Chunk.IsAtRightYZPlane(blockIndex)) {
				neighborBlockIndex = Chunk.NextX(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}


			if(!Chunk.IsAtBackYXPlane(blockIndex)) {
				neighborBlockIndex = Chunk.PrevZ(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}

			if(!Chunk.IsAtFrontYXPlane(blockIndex)) {
				neighborBlockIndex = Chunk.NextZ(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}


			if(!Chunk.IsAtBottomXZPlane(blockIndex)) {
				neighborBlockIndex = Chunk.PrevY(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}

			if(!Chunk.IsAtTopXZPlane(blockIndex)) {
				neighborBlockIndex = Chunk.NextY(blockIndex);
				newLuminance = Subtract(luminance, opacity[neighborBlockIndex]);
				if(IsGreater(newLuminance, lighting[neighborBlockIndex])) {
					LightChunk(neighborBlockIndex, newLuminance);
				}
			}
		}
	}

	public enum LightingOptions { None, OnlyVerticalSkylight, NoStiching }
}
