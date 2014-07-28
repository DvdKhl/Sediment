using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sediment.Core {
	public class XZInt {
		public int X { get; private set; }
		public int Z { get; private set; }

		public override bool Equals(object obj) { return Equals(obj as XZInt); }
		public bool Equals(XZInt obj) { return obj != null && obj.X == X && obj.Z == Z; }

		public override int GetHashCode() { return unchecked(X ^ (Z << 16)); }

		public XZInt(int x, int z) {
			this.X = x;
			this.Z = z;
		}
	}
}
