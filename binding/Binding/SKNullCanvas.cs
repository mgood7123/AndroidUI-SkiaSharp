using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace SkiaSharp
{
	// An N-Way canvas forwards calls to N canvas's. When N == 0 it's
	// effectively a null canvas.
	public class SKNullCanvas : SKNoDrawCanvas
	{
		internal SKNullCanvas (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKNullCanvas ()
			: this (IntPtr.Zero, true)
		{
			Handle = SkiaApi.sk_nway_canvas_new (0, 0);
		}
	}
}
