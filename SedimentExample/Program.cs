using Sediment;
using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedimentExample {
	class Program {
		static void Main(string[] args) {
			//var level = Level.Create(@"C:\Users\Arokh\AppData\Roaming\.minecraft\saves\Test2");

			//FillChunk(level);
			//FillChunkAndSaveTest(level, Blocks.Stone.Diorite.Id);
			HeightFun1();
		}

		private static void FillChunkAndSaveTest(Level level, ushort fillBlockId) {
			var world = level.WorldManager[WorldInfo.Overworld];



			for(int z = 0; z < 32; z++) {
				for(int x = 0; x < 32; x++) {
					var chunk = world.ChunkManager[x, z];

					for(int j = 0; j < Chunk.BlockCount; j++) {
						var oldId = chunk[j];
						chunk[j] = fillBlockId;
					}

				}
			}

			level.Save();
		}

		private static void FillChunkSpeedTest(Level level) {
			var world = level.WorldManager[WorldInfo.Overworld];
			var chunk = world.ChunkManager[0, 0];
			chunk.IsLightPopulated = false;

			var sw = new Stopwatch();

			sw.Restart();
			var blockMan = world.BlockManager;
			for(int i = 0; i < 100; i++) {
				for(int y = 0; y < Chunk.BlockYCount; y++) {
					for(int x = 0; x < Chunk.BlockXCount; x++) {
						for(int z = 0; z < Chunk.BlockZCount; z++) {
							blockMan[x, y, z] = Blocks.Stone.Diorite.Id;
						}
					}
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);



			sw.Restart();
			for(int i = 0; i < 100; i++) {
				for(int y = 0; y < Chunk.BlockYCount; y++) {
					for(int x = 0; x < Chunk.BlockXCount; x++) {
						for(int z = 0; z < Chunk.BlockZCount; z++) {
							chunk[x, y, z] = Blocks.Furnace.Active.North.Id;
						}
					}
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);

			sw.Restart();
			for(int i = 0; i < 100; i++) {
				for(int j = 0; j < Chunk.BlockCount; j++) {
					chunk[j] = 2;
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

		private static void HeightFun1() {
			var level = Level.Load(@"C:\Users\Arokh\AppData\Roaming\.minecraft\saves\HeightFun1");
			var world = level.WorldManager[WorldInfo.Overworld];

			world.SavingChunk += (s, c) => {
				c.UpdateHeightMap();
				c.UpdateLighting();

				c.IsTerrainPopulated = true;
				c.IsLightPopulated = true;
			};


			var blockMan = world.BlockManager;

			for(int z = 0; z < 32 * Chunk.BlockZCount; z++) {
				for(int x = 0; x < 32 * Chunk.BlockXCount; x++) {
					var height = Chunk.BlockYCount - 3 - (int)((Math.Sin((x / (32 * 2 * 16d / 3)) * (Math.PI * 2)) * Math.Cos((z / (32 * 2 * 16d / 3)) * (Math.PI * 2) + Math.PI / 2) + 1) * 127);

					int y = 0;
					for(y = 0; y <= height; y++) {
						blockMan[x, y, z] = Blocks.Stone.Normal.Id;
					}
					for(; y < Chunk.BlockYCount; y++) {
						blockMan[x, y, z] = Blocks.Air.Id;
					}
				}
			}

			level.Save();
		}
	}
}
