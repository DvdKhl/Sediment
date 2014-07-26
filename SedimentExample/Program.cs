using Sediment;
using Sediment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SedimentExample {
	class Program {
		static void Main(string[] args) {
			var level = Level.Load(@"C:\Users\Arokh\AppData\Roaming\.minecraft\saves\Test");
			var world = level.WorldManager[WorldInfo.Overworld];
			var chunk = world.ChunkManager[0, 0];
		}
	}
}
