using System;

namespace SkiaSharp
{
	public static class SKTestBed
	{
		public static void Invoke (SKCanvas canvas, int test_number) =>
			SkiaApi.sk_testbed (canvas == null ? IntPtr.Zero : canvas.Handle, test_number);
	}
}
