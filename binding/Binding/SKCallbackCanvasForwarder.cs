using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace SkiaSharp
{
	public class SKCallbackCanvasForwarder : SKCallbackCanvas
	{
		SKCanvas canvasRef;
		bool owns_canvas;

		public SKCallbackCanvasForwarder (int width, int height) : base (width, height)
		{
			canvasRef = null;
			owns_canvas = false;
		}

		public SKCallbackCanvasForwarder (SKCanvas canvas, int width, int height) : base (width, height)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));
			canvasRef = canvas;
			owns_canvas = true;
		}

		public SKCallbackCanvasForwarder (SKCanvas canvas, bool ownCanvas, int width, int height) : base (width, height)
		{
			if (canvas == null)
				throw new ArgumentNullException (nameof (canvas));
			canvasRef = canvas;
			owns_canvas = ownCanvas;
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
			base.Dispose (disposing);
		}

		protected override void OnConcat (SKMatrix44 matrix)
		{
			canvasRef.Concat (matrix);
		}

		protected override void OnClipRect (SKRect rect, SKClipOperation operation, bool antiAlias)
		{
			canvasRef.ClipRect (rect, operation, antiAlias);
		}

		protected override void OnClipRoundRect (SKRoundRect rrect, SKClipOperation operation, bool antiAlias)
		{
			canvasRef.ClipRoundRect (rrect, operation, antiAlias);
		}

		protected override void OnClipPath (SKPath path, SKClipOperation operation, bool antiAlias)
		{
			canvasRef.ClipPath (path, operation, antiAlias);
		}

		protected override void OnClipShader (SKShader shader, SKClipOperation operation)
		{
			canvasRef.ClipShader (shader, operation);
		}

		protected override void OnClipRegion (SKRegion region, SKClipOperation operation)
		{
			canvasRef.ClipRegion (region, operation);
		}

		protected override void OnDrawAnnotation (SKRect rect, string key, SKData value)
		{
			canvasRef.DrawAnnotation (rect, key, value);
		}

		protected override void OnDrawArc (SKRect rect, float startAngle, float sweepAngle, bool useCenter, SKPaint paint)
		{
			canvasRef.DrawArc (rect, startAngle, sweepAngle, useCenter, paint);
		}

		protected override unsafe void OnDrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKSamplingOptions samplingOptions, SKRect* cullRect, SKPaint paint)
		{
			canvasRef.DrawAtlas (atlas, sprites, transforms, colors, mode, samplingOptions, cullRect, paint);
		}

		protected override void OnDrawDrawable (SKDrawable drawable, SKMatrix matrix)
		{
			canvasRef.DrawDrawable (drawable, ref matrix);
		}

		protected override void OnDrawImageRect (SKImage image, SKRect dest, SKSamplingOptions samplingOptions, SKPaint paint, SKSrcRectConstraint constraint)
		{
			canvasRef.DrawImage (image, dest, samplingOptions, paint, constraint);
		}

		protected override void OnDrawImageRect (SKImage image, SKRect src, SKRect dest, SKSamplingOptions samplingOptions, SKPaint paint, SKSrcRectConstraint constraint)
		{
			canvasRef.DrawImage (image, src, dest, samplingOptions, paint, constraint);
		}

		protected override void OnDrawImageLattice (SKImage image, SKLattice lattice, SKRect dest, SKFilterMode filter, SKPaint paint)
		{
			canvasRef.DrawImageLattice (image, lattice, dest, filter, paint);
		}

		protected override void OnDrawImage (SKImage image, float x, float y, SKSamplingOptions samplingOptions, SKPaint paint)
		{
			canvasRef.DrawImage (image, x, y, samplingOptions, paint);
		}

		protected override void OnDrawOval (SKRect rect, SKPaint paint)
		{
			canvasRef.DrawOval (rect, paint);
		}

		protected override void OnDrawPaint (SKPaint paint)
		{
			canvasRef.DrawPaint (paint);
		}

		protected override void OnDrawPath (SKPath path, SKPaint paint)
		{
			canvasRef.DrawPath (path, paint);
		}

		protected override void OnDrawPatch (SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawPatch (cubics, colors, texCoords, mode, paint);
		}

		protected override void OnDrawPoints (SKPointMode pointmode, SKPoint[] points, SKPaint paint)
		{
			canvasRef.DrawPoints (pointmode, points, paint);
		}

		protected override void OnDrawRect (SKRect rect, SKPaint paint)
		{
			canvasRef.DrawRect (rect, paint);
		}

		protected override void OnDrawRegion (SKRegion region, SKPaint paint)
		{
			canvasRef.DrawRegion (region, paint);
		}

		protected override void OnDrawRoundRect (SKRoundRect rect, SKPaint paint)
		{
			canvasRef.DrawRoundRect (rect, paint);
		}

		protected override void OnDrawRoundRectDifference (SKRoundRect outer, SKRoundRect inner, SKPaint paint)
		{
			canvasRef.DrawRoundRectDifference (outer, inner, paint);
		}

		protected override void OnDrawSlug (IntPtr slug)
		{
			canvasRef.DrawSlug (slug);
		}

		protected override void OnDrawText (SKTextBlob blob, float x, float y, SKPaint paint)
		{
			canvasRef.DrawText (blob, x, y, paint);
		}

		protected override void OnDrawVertices (SKVertices vertices, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawVertices (vertices, mode, paint);
		}

		protected override void OnFlush ()
		{
			canvasRef.Flush ();
		}

		protected override void OnRestore ()
		{
			canvasRef.Restore ();
		}

		protected override void OnSave ()
		{
			canvasRef.Save ();
		}

		protected override void OnSaveLayer (SKPaint paint)
		{
			canvasRef.SaveLayer (paint);
		}

		protected override void OnSaveLayer (SKRect limit, SKPaint paint)
		{
			canvasRef.SaveLayer (limit, paint);
		}

		protected override void OnScale (float sx, float sy)
		{
			canvasRef.Scale (sx, sy);
		}

		protected override void OnSetMatrix (SKMatrix44 matrix)
		{
			canvasRef.SetMatrix (matrix);
		}

		protected override void OnTranslate (float dx, float dy)
		{
			canvasRef.Translate (dx, dy);
		}
	}
}
