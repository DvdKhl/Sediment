using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Internal {
	public abstract class Freezable {
		public bool IsFrozen { get; private set; }
		public void Freeze() { IsFrozen = true; }
		protected void WritePreamble() { if(IsFrozen) throw new InvalidOperationException("The Freezable instance is frozen and cannot have its members written to."); }
	}
}
