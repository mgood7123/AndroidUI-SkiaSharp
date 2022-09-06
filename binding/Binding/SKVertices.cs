using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace SkiaSharp
{
	public unsafe class SKVertices : SKObject, ISKNonVirtualReferenceCounted, ISKSkipObjectRegistration
	{
		internal SKVertices (IntPtr x, bool owns)
			: base (x, owns)
		{
		}

		// so we can serialize and deserialize this

		private SKVertexMode vmode;
		private SKPoint[] positions;
		private SKPoint[] texCoords;
		private SKColor[] colors;
		private UInt16[] indices;

		public SKVertexMode VMode_FOR_SERIALIZATION
		{
			get { return vmode; }
			set { vmode = value; }
		}

		public SKPoint[] Positions_FOR_SERIALIZATION
		{
			get { return positions; }
			set { positions = value; }
		}

		public SKPoint[] TexCoords_FOR_SERIALIZATION
		{
			get { return texCoords; }
			set { texCoords = value; }
		}

		public SKColor[] Colors_FOR_SERIALIZATION
		{
			get { return colors; }
			set { colors = value; }
		}

		public UInt16[] Indices_FOR_SERIALIZATION
		{
			get { return indices; }
			set { indices = value; }
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		void ISKNonVirtualReferenceCounted.ReferenceNative () => SkiaApi.sk_vertices_ref (Handle);

		void ISKNonVirtualReferenceCounted.UnreferenceNative () => SkiaApi.sk_vertices_unref (Handle);

		public static SKVertices CreateCopy (SKVertexMode vmode, SKPoint[] positions, SKColor[] colors)
		{
			return CreateCopy (vmode, positions, null, colors, null);
		}

		public static SKVertices CreateCopy (SKVertexMode vmode, SKPoint[] positions, SKPoint[] texs, SKColor[] colors)
		{
			return CreateCopy (vmode, positions, texs, colors, null);
		}

		public static SKVertices CreateCopy (SKVertexMode vmode, SKPoint[] positions, SKPoint[] texs, SKColor[] colors, UInt16[] indices)
		{
			if (positions == null)
				throw new ArgumentNullException (nameof (positions));

			if (texs != null && positions.Length != texs.Length)
				throw new ArgumentException ("The number of texture coordinates must match the number of vertices.", nameof (texs));
			if (colors != null && positions.Length != colors.Length)
				throw new ArgumentException ("The number of colors must match the number of vertices.", nameof (colors));

			var vertexCount = positions.Length;
			var indexCount = indices?.Length ?? 0;

			fixed (SKPoint* p = positions)
			fixed (SKPoint* t = texs)
			fixed (SKColor* c = colors)
			fixed (UInt16* i = indices) {
				SKVertices obj = GetObject (SkiaApi.sk_vertices_make_copy (vmode, vertexCount, p, t, (uint*)c, indexCount, i));
				obj.VMode_FOR_SERIALIZATION = vmode;
				obj.Positions_FOR_SERIALIZATION = positions;
				obj.TexCoords_FOR_SERIALIZATION = texs;
				obj.Colors_FOR_SERIALIZATION = colors;
				obj.Indices_FOR_SERIALIZATION = indices;
				return obj;
			}
		}

		internal static SKVertices GetObject (IntPtr handle) =>
			handle == IntPtr.Zero ? null : new SKVertices (handle, true);
	}
}
