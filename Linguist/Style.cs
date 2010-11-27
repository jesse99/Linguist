using System;
using System.Windows.Media;

namespace Linguist
{
	internal sealed class Style
	{
		public Color? BackColor { get; set; }
		public Color? ForeColor { get; set; }

		public bool Italic { get; set; }
		public bool Bold { get; set; }

		public string FontName { get; set; }
		public double PointSize { get; set; }
	}
}
