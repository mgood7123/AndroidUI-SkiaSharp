﻿using System;
using System.ComponentModel;

namespace SkiaSharp
{
	// TODO: carefully consider the `PeekPixels`, `ReadPixels`

	public unsafe class SKCanvas : SKObject
	{
		private const int PatchCornerCount = 4;
		private const int PatchCubicsCount = 12;
		private const double RadiansCircle = 2.0 * Math.PI;
		private const double DegreesCircle = 360.0;

		internal SKCanvas (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKCanvas (SKBitmap bitmap)
			: this (IntPtr.Zero, true)
		{
			if (bitmap == null)
				throw new ArgumentNullException (nameof (bitmap));
			Handle = SkiaApi.sk_canvas_new_from_bitmap (bitmap.Handle);
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			SkiaApi.sk_canvas_destroy (Handle);

		virtual public void Discard () =>
			SkiaApi.sk_canvas_discard (Handle);

		// QuickReject

		virtual public bool QuickReject (SKRect rect)
		{
			return SkiaApi.sk_canvas_quick_reject (Handle, &rect);
		}

		public bool QuickReject (SKPath path)
		{
			if (path == null)
				throw new ArgumentNullException (nameof (path));
			return path.IsEmpty || QuickReject (path.Bounds);
		}

		// Save*

		virtual public int Save ()
		{
			if (Handle == IntPtr.Zero)
				throw new ObjectDisposedException ("SKCanvas");
			return SkiaApi.sk_canvas_save (Handle);
		}

		virtual public int SaveLayer (SKRect limit, SKPaint paint)
		{
			return SkiaApi.sk_canvas_save_layer (Handle, &limit, paint == null ? IntPtr.Zero : paint.Handle);
		}

		virtual public int SaveLayer (SKPaint paint)
		{
			return SkiaApi.sk_canvas_save_layer (Handle, null, paint == null ? IntPtr.Zero : paint.Handle);
		}

		public int SaveLayer () =>
			SaveLayer (null);

		// DrawColor

		virtual public void DrawColor (SKColor color, SKBlendMode mode = SKBlendMode.Src) =>
			SkiaApi.sk_canvas_draw_color (Handle, (uint)color, mode);

		virtual public void DrawColor (SKColorF color, SKBlendMode mode = SKBlendMode.Src) =>
			SkiaApi.sk_canvas_draw_color4f (Handle, color, mode);

		// DrawLine

		public void DrawLine (SKPoint p0, SKPoint p1, SKPaint paint)
		{
			DrawLine (p0.X, p0.Y, p1.X, p1.Y, paint);
		}

		virtual public void DrawLine (float x0, float y0, float x1, float y1, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_line (Handle, x0, y0, x1, y1, paint.Handle);
		}

		// Clear

		public void Clear () =>
			Clear (SKColors.Empty);

		virtual public void Clear (SKColor color) =>
			SkiaApi.sk_canvas_clear (Handle, (uint)color);

		virtual public void Clear (SKColorF color) =>
			SkiaApi.sk_canvas_clear_color4f (Handle, color);

		// Restore*

		virtual public void Restore ()
		{
			SkiaApi.sk_canvas_restore (Handle);
		}

		virtual public void RestoreToCount (int count)
		{
			SkiaApi.sk_canvas_restore_to_count (Handle, count);
		}

		// Translate

		virtual public void Translate (float dx, float dy)
		{
			if (dx == 0 && dy == 0)
				return;

			SkiaApi.sk_canvas_translate (Handle, dx, dy);
		}

		virtual public void Translate (SKPoint point)
		{
			if (point.IsEmpty)
				return;

			SkiaApi.sk_canvas_translate (Handle, point.X, point.Y);
		}

		// Scale

		virtual public void Scale (float s)
		{
			if (s == 1)
				return;

			SkiaApi.sk_canvas_scale (Handle, s, s);
		}

		virtual public void Scale (float sx, float sy)
		{
			if (sx == 1 && sy == 1)
				return;

			SkiaApi.sk_canvas_scale (Handle, sx, sy);
		}

		virtual public void Scale (SKPoint size)
		{
			if (size.IsEmpty)
				return;

			SkiaApi.sk_canvas_scale (Handle, size.X, size.Y);
		}

		public void Scale (float sx, float sy, float px, float py)
		{
			if (sx == 1 && sy == 1)
				return;

			Translate (px, py);
			Scale (sx, sy);
			Translate (-px, -py);
		}

		// Rotate*

		virtual public void RotateDegrees (float degrees)
		{
			if (degrees % DegreesCircle == 0)
				return;

			SkiaApi.sk_canvas_rotate_degrees (Handle, degrees);
		}

		virtual public void RotateRadians (float radians)
		{
			if (radians % RadiansCircle == 0)
				return;

			SkiaApi.sk_canvas_rotate_radians (Handle, radians);
		}

		public void RotateDegrees (float degrees, float px, float py)
		{
			if (degrees % DegreesCircle == 0)
				return;

			Translate (px, py);
			RotateDegrees (degrees);
			Translate (-px, -py);
		}

		public void RotateRadians (float radians, float px, float py)
		{
			if (radians % RadiansCircle == 0)
				return;

			Translate (px, py);
			RotateRadians (radians);
			Translate (-px, -py);
		}

		// Skew

		virtual public void Skew (float sx, float sy)
		{
			if (sx == 0 && sy == 0)
				return;

			SkiaApi.sk_canvas_skew (Handle, sx, sy);
		}

		virtual public void Skew (SKPoint skew)
		{
			if (skew.IsEmpty)
				return;

			SkiaApi.sk_canvas_skew (Handle, skew.X, skew.Y);
		}

		// Concat

		virtual public void Concat (ref SKMatrix m)
		{
			fixed (SKMatrix* ptr = &m) {
				SkiaApi.sk_canvas_concat (Handle, ptr);
			}
		}

		// Clip*

		virtual public void ClipRect (SKRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			SkiaApi.sk_canvas_clip_rect_with_operation (Handle, &rect, operation, antialias);
		}

		virtual public void ClipRoundRect (SKRoundRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			if (rect == null)
				throw new ArgumentNullException (nameof (rect));

			SkiaApi.sk_canvas_clip_rrect_with_operation (Handle, rect.Handle, operation, antialias);
		}

		virtual public void ClipPath (SKPath path, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			if (path == null)
				throw new ArgumentNullException (nameof (path));

			SkiaApi.sk_canvas_clip_path_with_operation (Handle, path.Handle, operation, antialias);
		}

		virtual public void ClipRegion (SKRegion region, SKClipOperation operation = SKClipOperation.Intersect)
		{
			if (region == null)
				throw new ArgumentNullException (nameof (region));

			SkiaApi.sk_canvas_clip_region (Handle, region.Handle, operation);
		}

		public SKRect LocalClipBounds {
			get {
				GetLocalClipBounds (out var bounds);
				return bounds;
			}
		}

		public SKRectI DeviceClipBounds {
			get {
				GetDeviceClipBounds (out var bounds);
				return bounds;
			}
		}

		virtual public bool IsClipEmpty => SkiaApi.sk_canvas_is_clip_empty (Handle);

		virtual public bool IsClipRect => SkiaApi.sk_canvas_is_clip_rect (Handle);

		virtual public bool GetLocalClipBounds (out SKRect bounds)
		{
			fixed (SKRect* b = &bounds) {
				return SkiaApi.sk_canvas_get_local_clip_bounds (Handle, b);
			}
		}

		virtual public bool GetDeviceClipBounds (out SKRectI bounds)
		{
			fixed (SKRectI* b = &bounds) {
				return SkiaApi.sk_canvas_get_device_clip_bounds (Handle, b);
			}
		}

		// DrawPaint

		virtual public void DrawPaint (SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_paint (Handle, paint.Handle);
		}

		// DrawRegion

		virtual public void DrawRegion (SKRegion region, SKPaint paint)
		{
			if (region == null)
				throw new ArgumentNullException (nameof (region));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_region (Handle, region.Handle, paint.Handle);
		}

		// DrawRect

		virtual public void DrawRect (float x, float y, float w, float h, SKPaint paint)
		{
			DrawRect (SKRect.Create (x, y, w, h), paint);
		}

		virtual public void DrawRect (SKRect rect, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_rect (Handle, &rect, paint.Handle);
		}

		// DrawRoundRect

		virtual public void DrawRoundRect (SKRoundRect rect, SKPaint paint)
		{
			if (rect == null)
				throw new ArgumentNullException (nameof (rect));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_rrect (Handle, rect.Handle, paint.Handle);
		}

		public void DrawRoundRect (float x, float y, float w, float h, float rx, float ry, SKPaint paint)
		{
			DrawRoundRect (SKRect.Create (x, y, w, h), rx, ry, paint);
		}

		virtual public void DrawRoundRect (SKRect rect, float rx, float ry, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_round_rect (Handle, &rect, rx, ry, paint.Handle);
		}

		public void DrawRoundRect (SKRect rect, SKSize r, SKPaint paint)
		{
			DrawRoundRect (rect, r.Width, r.Height, paint);
		}

		// DrawOval

		public void DrawOval (float cx, float cy, float rx, float ry, SKPaint paint)
		{
			DrawOval (new SKRect (cx - rx, cy - ry, cx + rx, cy + ry), paint);
		}

		public void DrawOval (SKPoint c, SKSize r, SKPaint paint)
		{
			DrawOval (c.X, c.Y, r.Width, r.Height, paint);
		}

		virtual public void DrawOval (SKRect rect, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_oval (Handle, &rect, paint.Handle);
		}

		// DrawCircle

		virtual public void DrawCircle (float cx, float cy, float radius, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_circle (Handle, cx, cy, radius, paint.Handle);
		}

		public void DrawCircle (SKPoint c, float radius, SKPaint paint)
		{
			DrawCircle (c.X, c.Y, radius, paint);
		}

		// DrawPath

		virtual public void DrawPath (SKPath path, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			if (path == null)
				throw new ArgumentNullException (nameof (path));
			SkiaApi.sk_canvas_draw_path (Handle, path.Handle, paint.Handle);
		}

		// DrawPoints

		virtual public void DrawPoints (SKPointMode mode, SKPoint[] points, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			if (points == null)
				throw new ArgumentNullException (nameof (points));
			fixed (SKPoint* p = points) {
				SkiaApi.sk_canvas_draw_points (Handle, mode, (IntPtr)points.Length, p, paint.Handle);
			}
		}

		// DrawPoint

		public void DrawPoint (SKPoint p, SKPaint paint)
		{
			DrawPoint (p.X, p.Y, paint);
		}

		virtual public void DrawPoint (float x, float y, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_point (Handle, x, y, paint.Handle);
		}

		public void DrawPoint (SKPoint p, SKColor color)
		{
			DrawPoint (p.X, p.Y, color);
		}

		public void DrawPoint (float x, float y, SKColor color)
		{
			using (var paint = new SKPaint { Color = color, BlendMode = SKBlendMode.Src }) {
				DrawPoint (x, y, paint);
			}
		}

		// DrawImage

		public void DrawImage (SKImage image, SKPoint p, SKPaint paint = null)
		{
			DrawImage (image, p.X, p.Y, paint);
		}

		virtual public void DrawImage (SKImage image, float x, float y, SKPaint paint = null)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			SkiaApi.sk_canvas_draw_image (Handle, image.Handle, x, y, paint == null ? IntPtr.Zero : paint.Handle);
		}

		virtual public void DrawImage (SKImage image, SKRect dest, SKPaint paint = null)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			SkiaApi.sk_canvas_draw_image_rect (Handle, image.Handle, null, &dest, paint == null ? IntPtr.Zero : paint.Handle);
		}

		virtual public void DrawImage (SKImage image, SKRect source, SKRect dest, SKPaint paint = null)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			SkiaApi.sk_canvas_draw_image_rect (Handle, image.Handle, &source, &dest, paint == null ? IntPtr.Zero : paint.Handle);
		}

		// DrawPicture

		public void DrawPicture (SKPicture picture, float x, float y, SKPaint paint = null)
		{
			var matrix = SKMatrix.CreateTranslation (x, y);
			DrawPicture (picture, ref matrix, paint);
		}

		public void DrawPicture (SKPicture picture, SKPoint p, SKPaint paint = null)
		{
			DrawPicture (picture, p.X, p.Y, paint);
		}

		virtual public void DrawPicture (SKPicture picture, ref SKMatrix matrix, SKPaint paint = null)
		{
			if (picture == null)
				throw new ArgumentNullException (nameof (picture));
			fixed (SKMatrix* m = &matrix) {
				SkiaApi.sk_canvas_draw_picture (Handle, picture.Handle, m, paint == null ? IntPtr.Zero : paint.Handle);
			}
		}

		virtual public void DrawPicture (SKPicture picture, SKPaint paint = null)
		{
			if (picture == null)
				throw new ArgumentNullException (nameof (picture));
			SkiaApi.sk_canvas_draw_picture (Handle, picture.Handle, null, paint == null ? IntPtr.Zero : paint.Handle);
		}

		// DrawDrawable

		virtual public void DrawDrawable (SKDrawable drawable, ref SKMatrix matrix)
		{
			if (drawable == null)
				throw new ArgumentNullException (nameof (drawable));
			fixed (SKMatrix* m = &matrix) {
				SkiaApi.sk_canvas_draw_drawable (Handle, drawable.Handle, m);
			}
		}

		public void DrawDrawable (SKDrawable drawable, float x, float y)
		{
			if (drawable == null)
				throw new ArgumentNullException (nameof (drawable));
			var matrix = SKMatrix.CreateTranslation (x, y);
			DrawDrawable (drawable, ref matrix);
		}

		public void DrawDrawable (SKDrawable drawable, SKPoint p)
		{
			if (drawable == null)
				throw new ArgumentNullException (nameof (drawable));
			var matrix = SKMatrix.CreateTranslation (p.X, p.Y);
			DrawDrawable (drawable, ref matrix);
		}

		// DrawBitmap

		public void DrawBitmap (SKBitmap bitmap, SKPoint p, SKPaint paint = null) =>
			DrawBitmap (bitmap, p.X, p.Y, paint);

		public void DrawBitmap (SKBitmap bitmap, float x, float y, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImage (image, x, y, paint);
		}

		public void DrawBitmap (SKBitmap bitmap, SKRect dest, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImage (image, dest, paint);
		}

		public void DrawBitmap (SKBitmap bitmap, SKRect source, SKRect dest, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImage (image, source, dest, paint);
		}

		// DrawSurface

		public void DrawSurface (SKSurface surface, SKPoint p, SKPaint paint = null)
		{
			DrawSurface (surface, p.X, p.Y, paint);
		}

		virtual public void DrawSurface (SKSurface surface, float x, float y, SKPaint paint = null)
		{
			if (surface == null)
				throw new ArgumentNullException (nameof (surface));

			surface.Draw (this, x, y, paint);
		}

		// DrawText (SKTextBlob)

		virtual public void DrawText (SKTextBlob text, float x, float y, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			SkiaApi.sk_canvas_draw_text_blob (Handle, text.Handle, x, y, paint.Handle);
		}

		// DrawText

		public void DrawText (string text, SKPoint p, SKPaint paint)
		{
			DrawText (text, p.X, p.Y, paint);
		}

		public void DrawText (string text, float x, float y, SKPaint paint)
		{
			DrawText (text, x, y, paint.GetFont (), paint);
		}

		public void DrawText (string text, float x, float y, SKFont font, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (font == null)
				throw new ArgumentNullException (nameof (font));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			if (paint.TextAlign != SKTextAlign.Left) {
				var width = font.MeasureText (text);
				if (paint.TextAlign == SKTextAlign.Center)
					width *= 0.5f;
				x -= width;
			}

			using var blob = SKTextBlob.Create (text, font);
			if (blob == null)
				return;

			DrawText (blob, x, y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawText (byte[] text, SKPoint p, SKPaint paint)
		{
			DrawText (text, p.X, p.Y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawText (byte[] text, float x, float y, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			if (paint.TextAlign != SKTextAlign.Left) {
				var width = paint.MeasureText (text);
				if (paint.TextAlign == SKTextAlign.Center)
					width *= 0.5f;
				x -= width;
			}

			using var blob = SKTextBlob.Create (text, paint.TextEncoding, paint.GetFont ());
			if (blob == null)
				return;

			DrawText (blob, x, y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawText (IntPtr buffer, int length, SKPoint p, SKPaint paint)
		{
			DrawText (buffer, length, p.X, p.Y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawText (IntPtr buffer, int length, float x, float y, SKPaint paint)
		{
			if (buffer == IntPtr.Zero && length != 0)
				throw new ArgumentNullException (nameof (buffer));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			if (paint.TextAlign != SKTextAlign.Left) {
				var width = paint.MeasureText (buffer, length);
				if (paint.TextAlign == SKTextAlign.Center)
					width *= 0.5f;
				x -= width;
			}

			using var blob = SKTextBlob.Create (buffer, length, paint.TextEncoding, paint.GetFont ());
			if (blob == null)
				return;

			DrawText (blob, x, y, paint);
		}

		// DrawPositionedText

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawPositionedText (string text, SKPoint[] points, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			if (points == null)
				throw new ArgumentNullException (nameof (points));

			using var blob = SKTextBlob.CreatePositioned (text, paint.GetFont (), points);
			if (blob == null)
				return;

			DrawText (blob, 0, 0, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawPositionedText (byte[] text, SKPoint[] points, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			if (points == null)
				throw new ArgumentNullException (nameof (points));

			using var blob = SKTextBlob.CreatePositioned (text, paint.TextEncoding, paint.GetFont (), points);
			if (blob == null)
				return;

			DrawText (blob, 0, 0, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawText(SKTextBlob, float, float, SKPaint) instead.")]
		public void DrawPositionedText (IntPtr buffer, int length, SKPoint[] points, SKPaint paint)
		{
			if (buffer == IntPtr.Zero && length != 0)
				throw new ArgumentNullException (nameof (buffer));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			if (points == null)
				throw new ArgumentNullException (nameof (points));

			using var blob = SKTextBlob.CreatePositioned (buffer, length, paint.TextEncoding, paint.GetFont (), points);
			if (blob == null)
				return;

			DrawText (blob, 0, 0, paint);
		}

		// DrawTextOnPath

		public void DrawTextOnPath (string text, SKPath path, SKPoint offset, SKPaint paint)
		{
			DrawTextOnPath (text, path, offset, true, paint);
		}

		public void DrawTextOnPath (string text, SKPath path, float hOffset, float vOffset, SKPaint paint)
		{
			DrawTextOnPath (text, path, new SKPoint (hOffset, vOffset), true, paint);
		}

		public void DrawTextOnPath (string text, SKPath path, SKPoint offset, bool warpGlyphs, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			DrawTextOnPath (text, path, offset, warpGlyphs, paint.GetFont (), paint);
		}

		public void DrawTextOnPath (string text, SKPath path, SKPoint offset, bool warpGlyphs, SKFont font, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (path == null)
				throw new ArgumentNullException (nameof (path));
			if (font == null)
				throw new ArgumentNullException (nameof (font));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			if (warpGlyphs) {
				using var textPath = font.GetTextPathOnPath (text, path, paint.TextAlign, offset);
				DrawPath (textPath, paint);
			} else {
				using var blob = SKTextBlob.CreatePathPositioned (text, font, path, paint.TextAlign, offset);
				if (blob != null)
					DrawText (blob, 0, 0, paint);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawTextOnPath(string, SKPath, SKPoint, SKPaint) instead.")]
		public void DrawTextOnPath (byte[] text, SKPath path, SKPoint offset, SKPaint paint)
		{
			DrawTextOnPath (text, path, offset.X, offset.Y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawTextOnPath(string, SKPath, float, float, SKPaint) instead.")]
		public void DrawTextOnPath (byte[] text, SKPath path, float hOffset, float vOffset, SKPaint paint)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));
			if (path == null)
				throw new ArgumentNullException (nameof (path));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			fixed (byte* t = text) {
				DrawTextOnPath ((IntPtr)t, text.Length, path, hOffset, vOffset, paint);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawTextOnPath(string, SKPath, SKPoint, SKPaint) instead.")]
		public void DrawTextOnPath (IntPtr buffer, int length, SKPath path, SKPoint offset, SKPaint paint)
		{
			DrawTextOnPath (buffer, length, path, offset.X, offset.Y, paint);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use DrawTextOnPath(string, SKPath, float, float, SKPaint) instead.")]
		public void DrawTextOnPath (IntPtr buffer, int length, SKPath path, float hOffset, float vOffset, SKPaint paint)
		{
			if (buffer == IntPtr.Zero && length != 0)
				throw new ArgumentNullException (nameof (buffer));
			if (path == null)
				throw new ArgumentNullException (nameof (path));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			var font = paint.GetFont ();

			using var textPath = font.GetTextPathOnPath (buffer, length, paint.TextEncoding, path, paint.TextAlign, new SKPoint (hOffset, vOffset));
			DrawPath (textPath, paint);
		}

		// Flush

		virtual public void Flush ()
		{
			SkiaApi.sk_canvas_flush (Handle);
		}

		// Draw*Annotation

		virtual public void DrawAnnotation (SKRect rect, string key, SKData value)
		{
			var bytes = StringUtilities.GetEncodedText (key, SKTextEncoding.Utf8, true);
			fixed (byte* b = bytes) {
				SkiaApi.sk_canvas_draw_annotation (base.Handle, &rect, b, value == null ? IntPtr.Zero : value.Handle);
			}
		}

		virtual public void DrawUrlAnnotation (SKRect rect, SKData value)
		{
			SkiaApi.sk_canvas_draw_url_annotation (Handle, &rect, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKData DrawUrlAnnotation (SKRect rect, string value)
		{
			var data = SKData.FromCString (value);
			DrawUrlAnnotation (rect, data);
			return data;
		}

		virtual public void DrawNamedDestinationAnnotation (SKPoint point, SKData value)
		{
			SkiaApi.sk_canvas_draw_named_destination_annotation (Handle, &point, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKData DrawNamedDestinationAnnotation (SKPoint point, string value)
		{
			var data = SKData.FromCString (value);
			DrawNamedDestinationAnnotation (point, data);
			return data;
		}

		virtual public void DrawLinkDestinationAnnotation (SKRect rect, SKData value)
		{
			SkiaApi.sk_canvas_draw_link_destination_annotation (Handle, &rect, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKData DrawLinkDestinationAnnotation (SKRect rect, string value)
		{
			var data = SKData.FromCString (value);
			DrawLinkDestinationAnnotation (rect, data);
			return data;
		}

		// Draw*NinePatch

		public void DrawBitmapNinePatch (SKBitmap bitmap, SKRectI center, SKRect dst, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImageNinePatch (image, center, dst, paint);
		}

		virtual public void DrawImageNinePatch (SKImage image, SKRectI center, SKRect dst, SKPaint paint = null)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			// the "center" rect must fit inside the image "rect"
			if (!SKRect.Create (image.Width, image.Height).Contains (center))
				throw new ArgumentException ("Center rectangle must be contained inside the image bounds.", nameof (center));

			SkiaApi.sk_canvas_draw_image_nine (Handle, image.Handle, &center, &dst, paint == null ? IntPtr.Zero : paint.Handle);
		}

		// Draw*Lattice

		public void DrawBitmapLattice (SKBitmap bitmap, int[] xDivs, int[] yDivs, SKRect dst, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImageLattice (image, xDivs, yDivs, dst, paint);
		}

		public void DrawImageLattice (SKImage image, int[] xDivs, int[] yDivs, SKRect dst, SKPaint paint = null)
		{
			var lattice = new SKLattice {
				XDivs = xDivs,
				YDivs = yDivs
			};
			DrawImageLattice (image, lattice, dst, paint);
		}

		public void DrawBitmapLattice (SKBitmap bitmap, SKLattice lattice, SKRect dst, SKPaint paint = null)
		{
			using var image = SKImage.FromBitmap (bitmap);
			DrawImageLattice (image, lattice, dst, paint);
		}

		virtual public void DrawImageLattice (SKImage image, SKLattice lattice, SKRect dst, SKPaint paint = null)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			if (lattice.XDivs == null)
				throw new ArgumentNullException (nameof (lattice.XDivs));
			if (lattice.YDivs == null)
				throw new ArgumentNullException (nameof (lattice.YDivs));

			fixed (int* x = lattice.XDivs)
			fixed (int* y = lattice.YDivs)
			fixed (SKLatticeRectType* r = lattice.RectTypes)
			fixed (SKColor* c = lattice.Colors) {
				var nativeLattice = new SKLatticeInternal {
					fBounds = null,
					fRectTypes = r,
					fXCount = lattice.XDivs.Length,
					fXDivs = x,
					fYCount = lattice.YDivs.Length,
					fYDivs = y,
					fColors = (uint*)c,
				};
				if (lattice.Bounds != null) {
					var bounds = lattice.Bounds.Value;
					nativeLattice.fBounds = &bounds;
				}
				SkiaApi.sk_canvas_draw_image_lattice (Handle, image.Handle, &nativeLattice, &dst, paint == null ? IntPtr.Zero : paint.Handle);
			}
		}

		// *Matrix

		virtual public void ResetMatrix ()
		{
			SkiaApi.sk_canvas_reset_matrix (Handle);
		}

		virtual public void SetMatrix (SKMatrix matrix)
		{
			SkiaApi.sk_canvas_set_matrix (Handle, &matrix);
		}

		virtual public SKMatrix TotalMatrix {
			get {
				SKMatrix matrix;
				SkiaApi.sk_canvas_get_total_matrix (Handle, &matrix);
				return matrix;
			}
		}

		// SaveCount

		virtual public int SaveCount => SkiaApi.sk_canvas_get_save_count (Handle);

		// DrawVertices

		public void DrawVertices (SKVertexMode vmode, SKPoint[] vertices, SKColor[] colors, SKPaint paint)
		{
			var vert = SKVertices.CreateCopy (vmode, vertices, colors);
			DrawVertices (vert, SKBlendMode.Modulate, paint);
		}

		public void DrawVertices (SKVertexMode vmode, SKPoint[] vertices, SKPoint[] texs, SKColor[] colors, SKPaint paint)
		{
			var vert = SKVertices.CreateCopy (vmode, vertices, texs, colors);
			DrawVertices (vert, SKBlendMode.Modulate, paint);
		}

		public void DrawVertices (SKVertexMode vmode, SKPoint[] vertices, SKPoint[] texs, SKColor[] colors, UInt16[] indices, SKPaint paint)
		{
			var vert = SKVertices.CreateCopy (vmode, vertices, texs, colors, indices);
			DrawVertices (vert, SKBlendMode.Modulate, paint);
		}

		public void DrawVertices (SKVertexMode vmode, SKPoint[] vertices, SKPoint[] texs, SKColor[] colors, SKBlendMode mode, UInt16[] indices, SKPaint paint)
		{
			var vert = SKVertices.CreateCopy (vmode, vertices, texs, colors, indices);
			DrawVertices (vert, mode, paint);
		}

		virtual public void DrawVertices (SKVertices vertices, SKBlendMode mode, SKPaint paint)
		{
			if (vertices == null)
				throw new ArgumentNullException (nameof (vertices));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_vertices (Handle, vertices.Handle, mode, paint.Handle);
		}

		// DrawArc

		virtual public void DrawArc (SKRect oval, float startAngle, float sweepAngle, bool useCenter, SKPaint paint)
		{
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));
			SkiaApi.sk_canvas_draw_arc (Handle, &oval, startAngle, sweepAngle, useCenter, paint.Handle);
		}

		// DrawRoundRectDifference

		virtual public void DrawRoundRectDifference (SKRoundRect outer, SKRoundRect inner, SKPaint paint)
		{
			if (outer == null)
				throw new ArgumentNullException (nameof (outer));
			if (inner == null)
				throw new ArgumentNullException (nameof (inner));
			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			SkiaApi.sk_canvas_draw_drrect (Handle, outer.Handle, inner.Handle, paint.Handle);
		}

		// DrawAtlas

		public void DrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKPaint paint) =>
			DrawAtlas (atlas, sprites, transforms, null, SKBlendMode.Dst, null, paint);

		public void DrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKPaint paint) =>
			DrawAtlas (atlas, sprites, transforms, colors, mode, null, paint);

		public void DrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKRect cullRect, SKPaint paint) =>
			DrawAtlas (atlas, sprites, transforms, colors, mode, &cullRect, paint);

		virtual public void DrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKRect* cullRect, SKPaint paint)
		{
			if (atlas == null)
				throw new ArgumentNullException (nameof (atlas));
			if (sprites == null)
				throw new ArgumentNullException (nameof (sprites));
			if (transforms == null)
				throw new ArgumentNullException (nameof (transforms));

			if (transforms.Length != sprites.Length)
				throw new ArgumentException ("The number of transforms must match the number of sprites.", nameof (transforms));
			if (colors != null && colors.Length != sprites.Length)
				throw new ArgumentException ("The number of colors must match the number of sprites.", nameof (colors));

			fixed (SKRect* s = sprites)
			fixed (SKRotationScaleMatrix* t = transforms)
			fixed (SKColor* c = colors) {
				SkiaApi.sk_canvas_draw_atlas (Handle, atlas.Handle, t, s, (uint*)c, transforms.Length, mode, cullRect, paint.Handle);
			}
		}

		// DrawPatch

		public void DrawPatch (SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKPaint paint) =>
			DrawPatch (cubics, colors, texCoords, SKBlendMode.Modulate, paint);

		virtual public void DrawPatch (SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKBlendMode mode, SKPaint paint)
		{
			if (cubics == null)
				throw new ArgumentNullException (nameof (cubics));
			if (cubics.Length != PatchCubicsCount)
				throw new ArgumentException ($"Cubics must have a length of {PatchCubicsCount}.", nameof (cubics));

			if (colors != null && colors.Length != PatchCornerCount)
				throw new ArgumentException ($"Colors must have a length of {PatchCornerCount}.", nameof (colors));

			if (texCoords != null && texCoords.Length != PatchCornerCount)
				throw new ArgumentException ($"Texture coordinates must have a length of {PatchCornerCount}.", nameof (texCoords));

			if (paint == null)
				throw new ArgumentNullException (nameof (paint));

			fixed (SKPoint* cubes = cubics)
			fixed (SKColor* cols = colors)
			fixed (SKPoint* coords = texCoords) {
				SkiaApi.sk_canvas_draw_patch (Handle, cubes, (uint*)cols, coords, mode, paint.Handle);
			}
		}

		internal static SKCanvas GetObject (IntPtr handle, bool owns = true, bool unrefExisting = true) =>
			GetOrAddObject (handle, owns, unrefExisting, (h, o) => new SKCanvas (h, o));
	}

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

		public SKCanvas GetNativeObject () => canvasRef;

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

		protected override void Dispose(bool disposing)
		{
			if (owns_canvas)
				canvasRef.Dispose ();
		}

		public override void Discard()
		{
			canvasRef.Discard ();
		}

		public override bool QuickReject(SKRect rect)
		{
			return canvasRef.QuickReject (rect);
		}

		public override int Save()
		{
			return canvasRef.Save ();
		}

		public override int SaveLayer(SKRect limit, SKPaint paint)
		{
			return canvasRef.SaveLayer (limit, paint);
		}

		public override int SaveLayer(SKPaint paint)
		{
			return canvasRef.SaveLayer(paint);
		}

		public override void DrawColor(SKColor color, SKBlendMode mode = SKBlendMode.Src)
		{
			canvasRef.DrawColor(color, mode);
		}

		public override void DrawColor(SKColorF color, SKBlendMode mode = SKBlendMode.Src)
		{
			canvasRef.DrawColor(color, mode);
		}

		public override void DrawLine(float x0, float y0, float x1, float y1, SKPaint paint)
		{
			canvasRef.DrawLine(x0, y0, x1, y1, paint);
		}

		public override void Clear(SKColor color)
		{
			canvasRef.Clear(color);
		}

		public override void Clear(SKColorF color)
		{
			canvasRef.Clear(color);
		}

		public override void Restore()
		{
			canvasRef.Restore();
		}

		public override void RestoreToCount(int count)
		{
			canvasRef.RestoreToCount(count);
		}

		public override void Translate(float dx, float dy)
		{
			canvasRef.Translate(dx, dy);
		}

		public override void Translate(SKPoint point)
		{
			canvasRef.Translate(point);
		}

		public override void Scale(float s)
		{
			canvasRef.Scale(s);
		}

		public override void Scale(float sx, float sy)
		{
			canvasRef.Scale(sx, sy);
		}

		public override void Scale(SKPoint size)
		{
			canvasRef.Scale(size);
		}

		public override void RotateDegrees(float degrees)
		{
			canvasRef.RotateDegrees(degrees);
		}

		public override void RotateRadians(float radians)
		{
			canvasRef.RotateRadians(radians);
		}

		public override void Skew(float sx, float sy)
		{
			canvasRef.Skew(sx, sy);
		}

		public override void Skew(SKPoint skew)
		{
			canvasRef.Skew(skew);
		}

		public override void Concat(ref SKMatrix m)
		{
			canvasRef.Concat(ref m);
		}

		public override void ClipRect(SKRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipRect(rect, operation, antialias);
		}

		public override void ClipRoundRect(SKRoundRect rect, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipRoundRect(rect, operation, antialias);
		}

		public override void ClipPath(SKPath path, SKClipOperation operation = SKClipOperation.Intersect, bool antialias = false)
		{
			canvasRef.ClipPath(path, operation, antialias);
		}

		public override void ClipRegion(SKRegion region, SKClipOperation operation = SKClipOperation.Intersect)
		{
			canvasRef.ClipRegion(region, operation);
		}

		public override bool GetLocalClipBounds(out SKRect bounds)
		{
			return canvasRef.GetLocalClipBounds(out bounds);
		}

		public override bool GetDeviceClipBounds(out SKRectI bounds)
		{
			return canvasRef.GetDeviceClipBounds(out bounds);
		}

		public override void DrawPaint(SKPaint paint)
		{
			canvasRef.DrawPaint(paint);
		}

		public override void DrawRegion(SKRegion region, SKPaint paint)
		{
			canvasRef.DrawRegion(region, paint);
		}

		public override void DrawRect(float x, float y, float w, float h, SKPaint paint)
		{
			canvasRef.DrawRect(x, y, w, h, paint);
		}

		public override void DrawRect(SKRect rect, SKPaint paint)
		{
			canvasRef.DrawRect(rect, paint);
		}

		public override void DrawRoundRect(SKRoundRect rect, SKPaint paint)
		{
			canvasRef.DrawRoundRect(rect, paint);
		}

		public override void DrawRoundRect(SKRect rect, float rx, float ry, SKPaint paint)
		{
			canvasRef.DrawRoundRect(rect, rx, ry, paint);
		}

		public override void DrawOval(SKRect rect, SKPaint paint)
		{
			canvasRef.DrawOval(rect, paint);
		}

		public override void DrawCircle(float cx, float cy, float radius, SKPaint paint)
		{
			canvasRef.DrawCircle(cx, cy, radius, paint);
		}

		public override void DrawPath(SKPath path, SKPaint paint)
		{
			canvasRef.DrawPath(path, paint);
		}

		public override void DrawPoints(SKPointMode mode, SKPoint[] points, SKPaint paint)
		{
			canvasRef.DrawPoints(mode, points, paint);
		}

		public override void DrawPoint(float x, float y, SKPaint paint)
		{
			canvasRef.DrawPoint(x, y, paint);
		}

		public override void DrawImage(SKImage image, float x, float y, SKPaint paint = null)
		{
			canvasRef.DrawImage(image, x, y, paint);
		}

		public override void DrawImage(SKImage image, SKRect dest, SKPaint paint = null)
		{
			canvasRef.DrawImage(image, dest, paint);
		}

		public override void DrawImage(SKImage image, SKRect source, SKRect dest, SKPaint paint = null)
		{
			canvasRef.DrawImage(image, source, dest, paint);
		}

		public override void DrawPicture(SKPicture picture, ref SKMatrix matrix, SKPaint paint = null)
		{
			canvasRef.DrawPicture(picture, ref matrix, paint);
		}

		public override void DrawPicture(SKPicture picture, SKPaint paint = null)
		{
			canvasRef.DrawPicture(picture, paint);
		}

		public override void DrawDrawable(SKDrawable drawable, ref SKMatrix matrix)
		{
			canvasRef.DrawDrawable(drawable, ref matrix);
		}

		public override void DrawSurface(SKSurface surface, float x, float y, SKPaint paint = null)
		{
			canvasRef.DrawSurface(surface, x, y, paint);
		}

		public override void DrawText(SKTextBlob text, float x, float y, SKPaint paint)
		{
			canvasRef.DrawText(text, x, y, paint);
		}

		public override void Flush()
		{
			canvasRef.Flush();
		}

		public override void DrawAnnotation(SKRect rect, string key, SKData value)
		{
			canvasRef.DrawAnnotation(rect, key, value);
		}

		public override void DrawUrlAnnotation(SKRect rect, SKData value)
		{
			canvasRef.DrawUrlAnnotation(rect, value);
		}

		public override void DrawNamedDestinationAnnotation(SKPoint point, SKData value)
		{
			canvasRef.DrawNamedDestinationAnnotation(point, value);
		}

		public override void DrawLinkDestinationAnnotation(SKRect rect, SKData value)
		{
			canvasRef.DrawLinkDestinationAnnotation(rect, value);
		}

		public override void DrawImageNinePatch(SKImage image, SKRectI center, SKRect dst, SKPaint paint = null)
		{
			canvasRef.DrawImageNinePatch(image, center, dst, paint);
		}

		public override void DrawImageLattice(SKImage image, SKLattice lattice, SKRect dst, SKPaint paint = null)
		{
			canvasRef.DrawImageLattice(image, lattice, dst, paint);
		}

		public override void ResetMatrix()
		{
			canvasRef.ResetMatrix();
		}

		public override void SetMatrix(SKMatrix matrix)
		{
			canvasRef.SetMatrix(matrix);
		}

		public override void DrawVertices(SKVertices vertices, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawVertices(vertices, mode, paint);
		}

		public override void DrawArc(SKRect oval, float startAngle, float sweepAngle, bool useCenter, SKPaint paint)
		{
			canvasRef.DrawArc(oval, startAngle, sweepAngle, useCenter, paint);
		}

		public override void DrawRoundRectDifference(SKRoundRect outer, SKRoundRect inner, SKPaint paint)
		{
			canvasRef.DrawRoundRectDifference(outer, inner, paint);
		}

		public override unsafe void DrawAtlas(SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKRect* cullRect, SKPaint paint)
		{
			canvasRef.DrawAtlas(atlas, sprites, transforms, colors, mode, cullRect, paint);
		}

		public override void DrawPatch(SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKBlendMode mode, SKPaint paint)
		{
			canvasRef.DrawPatch(cubics, colors, texCoords, mode, paint);
		}
	}

	public class SKAutoCanvasRestore : IDisposable
	{
		private SKCanvas canvas;
		private readonly int saveCount;

		public SKAutoCanvasRestore (SKCanvas canvas)
			: this (canvas, true)
		{
		}

		public SKAutoCanvasRestore (SKCanvas canvas, bool doSave)
		{
			this.canvas = canvas;
			this.saveCount = 0;

			if (canvas != null) {
				saveCount = canvas.SaveCount;
				if (doSave) {
					canvas.Save ();
				}
			}
		}

		public void Dispose ()
		{
			Restore ();
		}

		/// <summary>
		/// Perform the restore now, instead of waiting for the Dispose.
		/// Will only do this once.
		/// </summary>
		public void Restore ()
		{
			if (canvas != null) {
				canvas.RestoreToCount (saveCount);
				canvas = null;
			}
		}
	}
}
