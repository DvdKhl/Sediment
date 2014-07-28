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

			var sw = new Stopwatch();

			sw.Restart();
			var blockMan = world.BlockManager;
			for(int i = 0; i < 100; i++) {
				for(int y = 0; y < Chunk.BlockYCount; y++) {
					for(int x = 0; x < Chunk.BlockXCount; x++) {
						for(int z = 0; z < Chunk.BlockZCount; z++) {
							blockMan[x, y, z] = 1;
						}
					}
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);

			var chunk = world.ChunkManager[0, 0];

			sw.Restart();
			for(int i = 0; i < 100; i++) {
				for(int y = 0; y < Chunk.BlockYCount; y++) {
					for(int x = 0; x < Chunk.BlockXCount; x++) {
						for(int z = 0; z < Chunk.BlockZCount; z++) {
							chunk[x, y, z] = 1;
						}
					}
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);

			sw.Restart();
			for(int i = 0; i < 100; i++) {
				for(int j = 0; j < Chunk.BlockCount; j++) {
					chunk[j] = 1;
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);

			Console.Read();
		}
	}
}
