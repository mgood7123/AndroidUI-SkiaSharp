using System;

namespace SkiaSharp
{
	// TODO: `FilterColor` may be useful

	public unsafe class SKBlender : SKObject, ISKReferenceCounted
	{
		internal SKBlender (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		public static SKBlender CreateMode (SKBlendMode mode)
		{
			return GetObject (SkiaApi.sk_blender_new_mode (mode));
		}

		public static SKBlender CreateArithmetic (float k1, float k2, float k3, float k4, bool enforcePremul)
		{
			return GetObject (SkiaApi.sk_blender_new_arithmetic (k1, k2, k3, k4, enforcePremul));
		}

		public SKData Serialize ()
		{
			return SKData.GetObject (SkiaApi.sk_blender_serialize (Handle));
		}

		public static SKBlender Deserialize (SKData data)
		{
			return GetObject (SkiaApi.sk_blender_deserialize (data.Handle));
		}

		internal static SKBlender GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKBlender (h, o));
	}
}
