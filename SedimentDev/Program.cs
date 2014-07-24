using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SedimentDev {
	class Program {
		static void Main(string[] args) {
			var gen = new GenerateBlocksClass(XElement.Load("GenerateBlocksClass/Template.xml"), XElement.Load("GenerateBlocksClass/MinecraftExport.xml"));
		}
	}
}
