using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace SkiaSharp
{
	public unsafe class SKCanvasForwarder : SKCanvas
	{
		SKCanvas canvasRef;
		bool owns_canvas;

		public override bool IsClipEmpty => canvasRef.IsClipEmpty;

		public override bool IsClipRect => canvasRef.IsClipRect;

		public override SKMatrix TotalMatrix => canvasRef.TotalMatrix;

		public override int SaveCount => canvasRef.SaveCount;

		internal SKCanvasForwarder (IntPtr handle)
			: base (handle, true)
		{
		}

		public SKCanvasForwarder ()
			: this (IntPtr.Zero)
		{
			canvasRef = null;
			owns_canvas = false;
		}

		public SKCanvasForwarder (SKCanvas canvas)
			: this (IntPtr.Zero)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));
			canvasRef = canvas;
			owns_canvas = true;
		}

		public SKCanvasForwarder (SKCanvas canvas, bool ownsCanvas)
			: this (IntPtr.Zero)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));
			canvasRef = canvas;
			owns_canvas = ownsCanvas;
		}

		public virtual void SetNativeObject (SKCanvas canvas)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));

			if (canvasRef != null) {
				throw new InvalidOperationException ("a canvas has already been assigned, please call ReleaseNativeObject() first");
			}
			canvasRef = canvas;
			owns_canvas = true;
		}

		public virtual void SetNativeObject (SKCanvas canvas, bool ownsCanvas)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));

			if (canvasRef != null) {
				throw new InvalidOperationException ("a canvas has already been assigned, please call ReleaseNativeObject() first");
			}
			canvasRef = canvas;
			owns_canvas = ownsCanvas;
		}

		public SKCanvas GetNativeObject () => canvasRef;

		public bool OwnsNativeObject => owns_canvas;

		/// <summary>
		/// Releases the SKCanvas object, it will not be disposed nor discarded
		/// <br></br>
		/// it is UB to call any methods except SetNativeObject after calling this method
		/// </summary>
		public virtual SKCanvas ReleaseNativeObject ()
		{
			SKCanvas r = canvasRef;
			canvasRef = null;
			owns_canvas = false;
			return r;
		}

		protected override void Dispose (bool disposing)
		{
			if (owns_canvas)
				canvasRef.Dispose ();
		}

		public override void Discard ()
		{
			canvasRef.Discard ();
		}

		public override bool QuickReject (SKRect rect)
		{
			return canvasRef.QuickReject (rect);
		}

		public override SKImageInfo Info {
			get {
				return canvasRef.Info;
			}
		}

		public override int Save ()
		{
			return canvasRef.Save ();
		}

		public override int SaveLayer (SKRect limit, SKPaint paint)
		{
			return canvasRef.SaveLayer (limit, paint);
		}

		public override int SaveLayer (SKPaint paint)
		{
			return canvasRef.SaveLayer (paint);
		}

		public override void DrawColor (SKColor color, SKBlendMode mode = SKBlendMode.Src)
		{
			canvasRef.DrawColor (color, mode);
		}

		public override void DrawColor (SKColorF color, SKBlendMode mode = SKBlendMode.Src)
		{
			canvasRef.DrawColor (color, mode);
		}

		public override void DrawLine (float x0, float y0, float x1, float y1, SKPaint paint)
		{
			canvasRef.DrawLine (x0, y0, x1, y1, paint);
		}

		public override void Clear (SKColor color)
		{
			canvasRef.Clear (color);
		}

		public override void Clear (SKColorF color)
		{
			canvasRef.Clear (color);
		}

		public override void Restore ()
		{
			canvasRef.Restore ();
		}

		public override void RestoreToCount (int count)
		{
			canvasRef.RestoreToCount (count);
		}

		public override void Translate (float dx, float dy)
		{
			canvasRef.Translate (dx, dy);
		}

		public override void Translate (SKPoint point)
		{
			canvasRef.Translate (point);
		}

		public override void Scale (float s)
		{
			canvasRef.Scale (s);
		}

		public override void Scale (float sx, float sy)
		{
			canvasRef.Scale (sx, sy);
		}

		public override void Scale (SKPoint size)
		{
			canvasRef.Scale (size);
		}

		public override void RotateDegrees (float degrees)
		{
			canvasRef.RotateDegrees (degrees);
		}

		public override void RotateRadians (float radians)
		{
			canvasRef.RotateRadians (radians);
		}

		public override void Skew (float sx, float sy)
		{
			canvasRef.Skew (sx, sy);
		}

		public override void Skew (SKPoint skew)
		{
			canvasRef.Skew (skew);
		}

		public override void Concat (ref SKMatrix m)
		{
			canvasRef.Concat (ref m);
		}

		public override void ClipRect (SKRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipRect (rect, operation, antialias);
		}

		public override void ClipRoundRect (SKRoundRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipRoundRect (rect, operation, antialias);
		}

		public override void ClipPath (SKPath path, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipPath (path, operation, antialias);
		}

		public override void ClipRegion (SKRegion region, SKClipOperation operation = SKClipOperation.Intersect)
		{
			canvasRef.ClipRegion (region, operation);
		}

		public override bool GetLocalClipBounds (out SKRect bounds)
		{
			return canvasRef.GetLocalClipBounds (out bounds);
		}

		public override bool GetDeviceClipBounds (out SKRectI bounds)
		{
			return canvasRef.GetDeviceClipBounds (out bounds);
		}

		public override void DrawPaint (SKPaint paint)
		{
			canvasRef.DrawPaint (paint);
		}

		public override void DrawRegion (SKRegion region, SKPaint paint)
		{
			canvasRef.DrawRegion (region, paint);
		}

		public override void DrawRect (float x, float y, float w, float h, SKPaint paint)
		{
			canvasRef.DrawRect (x, y, w, h, paint);
		}

		public override void DrawRect (SKRect rect, SKPaint paint)
		{
			canvasRef.DrawRect (rect, paint);
		}

		public override void DrawRoundRect (SKRoundRect rect, SKPaint paint)
		{
			canvasRef.DrawRoundRect (rect, paint);
		}

		public override void DrawRoundRect (SKRect rect, float rx, float ry, SKPaint paint)
		{
			canvasRef.DrawRoundRect (rect, rx, ry, paint);
		}

		public override void DrawOval (SKRect rect, SKPaint paint)
		{
			canvasRef.DrawOval (rect, paint);
		}

		public override void DrawCircle (float cx, float cy, float radius, SKPaint paint)
		{
			canvasRef.DrawCircle (cx, cy, radius, paint);
		}

		public override void DrawPath (SKPath path, SKPaint paint)
		{
			canvasRef.DrawPath (path, paint);
		}

		public override void DrawPoints (SKPointMode mode, SKPoint[] points, SKPaint paint)
		{
			canvasRef.DrawPoints (mode, points, paint);
		}

		public override void DrawPoint (float x, float y, SKPaint paint)
		{
			canvasRef.DrawPoint (x, y, paint);
		}

		public override void DrawImage (SKImage image, float x, float y, SKPaint paint = null)
		{
			canvasRef.DrawImage (image, x, y, paint);
		}

		public override void DrawImage (SKImage image, SKRect dest, SKPaint paint = null)
		{
			canvasRef.DrawImage (image, dest, paint);
		}

		public override void DrawImage (SKImage image, SKRect source, SKRect dest, SKPaint paint = null)
		{
			canvasRef.DrawImage (image, source, dest, paint);
		}

		public override void DrawPicture (SKPicture picture, ref SKMatrix matrix, SKPaint paint = null)
		{
			canvasRef.DrawPicture (picture, ref matrix, paint);
		}

		public override void DrawPicture (SKPicture picture, SKPaint paint = null)
		{
			canvasRef.DrawPicture (picture, paint);
		}

		public override void DrawDrawable (SKDrawable drawable, ref SKMatrix matrix)
		{
			canvasRef.DrawDrawable (drawable, ref matrix);
		}

		public override void DrawSurface (SKSurface surface, float x, float y, SKPaint paint = null)
		{
			canvasRef.DrawSurface (surface, x, y, paint);
		}

		public override void DrawText (SKTextBlob text, float x, float y, SKPaint paint)
		{
			canvasRef.DrawText (text, x, y, paint);
		}

		public override void Flush ()
		{
			canvasRef.Flush ();
		}

		public override void DrawAnnotation (SKRect rect, string key, SKData value)
		{
			canvasRef.DrawAnnotation (rect, key, value);
		}

		public override void DrawUrlAnnotation (SKRect rect, SKData value)
		{
			canvasRef.DrawUrlAnnotation (rect, value);
		}

		public override void DrawNamedDestinationAnnotation (SKPoint point, SKData value)
		{
			canvasRef.DrawNamedDestinationAnnotation (point, value);
		}

		public override void DrawLinkDestinationAnnotation (SKRect rect, SKData value)
		{
			canvasRef.DrawLinkDestinationAnnotation (rect, value);
		}

		public override void DrawImageNinePatch (SKImage image, SKRectI center, SKRect dst, SKPaint paint = null)
		{
			canvasRef.DrawImageNinePatch (image, center, dst, paint);
		}

		public override void DrawImageLattice (SKImage image, SKLattice lattice, SKRect dst, SKPaint paint = null)
		{
			canvasRef.DrawImageLattice (image, lattice, dst, paint);
		}

		public override void ResetMatrix ()
		{
			canvasRef.ResetMatrix ();
		}

		virtual public SKSurface Surface {
			get {
				return canvasRef.Surface;
			}
		}

		virtual public SKSizeI BaseLayerSize {
			get
			{
				return canvasRef.BaseLayerSize;
			}
		}

		public override void SetMatrix (SKMatrix matrix)
		{
			canvasRef.SetMatrix (matrix);
		}

		public override void DrawVertices (SKVertices vertices, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawVertices (vertices, mode, paint);
		}

		public override void DrawArc (SKRect oval, float startAngle, float sweepAngle, bool useCenter, SKPaint paint)
		{
			canvasRef.DrawArc (oval, startAngle, sweepAngle, useCenter, paint);
		}

		public override void DrawRoundRectDifference (SKRoundRect outer, SKRoundRect inner, SKPaint paint)
		{
			canvasRef.DrawRoundRectDifference (outer, inner, paint);
		}

		public override unsafe void DrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKRect* cullRect, SKPaint paint)
		{
			canvasRef.DrawAtlas (atlas, sprites, transforms, colors, mode, cullRect, paint);
		}

		public override void DrawPatch (SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawPatch (cubics, colors, texCoords, mode, paint);
		}
	}
}
