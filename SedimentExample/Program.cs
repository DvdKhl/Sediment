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
			var level = Level.Load(@"C:\Users\Arokh\AppData\Roaming\.minecraft\saves\Test");
			var world = level.WorldManager[WorldInfo.Overworld];

			//FillChunk(level);
			FillChunkAndSaveTest(level, Blocks.Stone.Diorite.Id);
		}

		private static void FillChunkAndSaveTest(Level level, ushort fillBlockId) {
			var world = level.WorldManager[WorldInfo.Overworld];
			var chunk = world.ChunkManager[0, 0];

			for(int j = 0; j < Chunk.BlockCount; j++) {
				chunk[j] = fillBlockId;
			}

			world.Save();
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
	}
}
