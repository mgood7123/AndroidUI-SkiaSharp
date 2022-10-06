using System;
using System.ComponentModel;

namespace SkiaSharp
{
	public enum SKPaintHinting
	{
		NoHinting = 0,
		Slight = 1,
		Normal = 2,
		Full = 3,
	}

	public unsafe class SKPaint : SKObject, ISKSkipObjectRegistration
	{
		private SKFont font;
		private bool lcdRenderText;

		internal SKPaint (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKPaint ()
			: this (SkiaApi.sk_paint_extra_info_new (), true)
		{
			if (Handle == IntPtr.Zero) {
				throw new InvalidOperationException ("Unable to create a new SKPaint instance.");
			}

			font = new SKFont (FontHandle, false);

			LcdRenderText = font.Edging == SKFontEdging.SubpixelAntialias;
		}

		public SKPaint (SKFont font)
			: this (IntPtr.Zero, true)
		{
			if (font == null)
				throw new ArgumentNullException (nameof (font));

			Handle = SkiaApi.sk_paint_extra_info_new_with_font (font.Handle);

			if (Handle == IntPtr.Zero)
				throw new InvalidOperationException ("Unable to create a new SKPaint instance.");

			font = new SKFont (FontHandle, false);

			LcdRenderText = font.Edging == SKFontEdging.SubpixelAntialias;
		}

		public SKPaint (IntPtr nativeSkFontHandle, IntPtr nativeSkPaintHandle)
			: this (IntPtr.Zero, true)
		{
			Handle = SkiaApi.sk_paint_extra_info_new_with_font_and_paint (nativeSkFontHandle, nativeSkPaintHandle);

			if (Handle == IntPtr.Zero)
				throw new InvalidOperationException ("Unable to create a new SKPaint instance.");

			font = new SKFont (FontHandle, false);

			LcdRenderText = font.Edging == SKFontEdging.SubpixelAntialias;
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			SkiaApi.sk_paint_extra_info_delete (Handle);

		// Reset

		public void Reset () =>
			SkiaApi.sk_paint_extra_info_reset (Handle);

		// properties

		private IntPtr PaintHandle =>
			SkiaApi.sk_paint_extra_info_get_paint (Handle);

		private IntPtr FontHandle =>
			SkiaApi.sk_paint_extra_info_get_font (Handle);

		public SKFont Font => font;

		public bool IsAntialias {
			get => SkiaApi.sk_paint_is_antialias (PaintHandle);
			set {
				SkiaApi.sk_paint_set_antialias (PaintHandle, value);
				UpdateFontEdging (value);
			}
		}

		public bool NothingToDraw {
			get => SkiaApi.sk_paint_nothing_to_draw (PaintHandle);
		}

		public bool IsDither {
			get => SkiaApi.sk_paint_is_dither (PaintHandle);
			set => SkiaApi.sk_paint_set_dither (PaintHandle, value);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete]
		public bool IsVerticalText {
			get => false;
			set { }
		}

		public bool IsLinearText {
			get => font.LinearMetrics;
			set => font.LinearMetrics = value;
		}

		public bool SubpixelText {
			get => font.Subpixel;
			set => font.Subpixel = value;
		}

		public bool LcdRenderText {
			get => lcdRenderText;
			set {
				lcdRenderText = value;
				UpdateFontEdging (IsAntialias);
			}
		}

		public bool IsEmbeddedBitmapText {
			get => font.EmbeddedBitmaps;
			set => font.EmbeddedBitmaps = value;
		}

		public bool IsAutohinted {
			get => font.ForceAutoHinting;
			set => font.ForceAutoHinting = value;
		}

		public SKPaintHinting HintingLevel {
			get => (SKPaintHinting)font.Hinting;
			set => font.Hinting = (SKFontHinting)value;
		}

		public bool FakeBoldText {
			get => font.Embolden;
			set => font.Embolden = value;
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete]
		public bool DeviceKerningEnabled {
			get => false;
			set { }
		}

		public bool IsStroke {
			get => Style != SKPaintStyle.Fill;
			set => Style = value ? SKPaintStyle.Stroke : SKPaintStyle.Fill;
		}

		public SKPaintStyle Style {
			get => SkiaApi.sk_paint_get_style (PaintHandle);
			set => SkiaApi.sk_paint_set_style (PaintHandle, value);
		}

		public byte Alpha {
			get => SkiaApi.sk_paint_get_alpha (PaintHandle);
			set => SkiaApi.sk_paint_set_alpha (PaintHandle, value);
		}

		public float AlphaF {
			get => SkiaApi.sk_paint_get_alphaf (PaintHandle);
			set => SkiaApi.sk_paint_set_alphaf (PaintHandle, value);
		}

		public SKColor Color {
			get => SkiaApi.sk_paint_get_color (PaintHandle);
			set => SkiaApi.sk_paint_set_color (PaintHandle, (uint)value);
		}

		public SKColorF ColorF {
			get {
				SKColorF color4f;
				SkiaApi.sk_paint_get_color4f (PaintHandle, &color4f);
				return color4f;
			}
			set => SkiaApi.sk_paint_set_color4f (PaintHandle, &value, IntPtr.Zero);
		}

		public void SetColor (SKColorF color, SKColorSpace colorspace) =>
			SkiaApi.sk_paint_set_color4f (PaintHandle, &color, colorspace?.Handle ?? IntPtr.Zero);

		public float StrokeWidth {
			get => SkiaApi.sk_paint_get_stroke_width (PaintHandle);
			set => SkiaApi.sk_paint_set_stroke_width (PaintHandle, value);
		}

		public float StrokeMiter {
			get => SkiaApi.sk_paint_get_stroke_miter (PaintHandle);
			set => SkiaApi.sk_paint_set_stroke_miter (PaintHandle, value);
		}

		public SKStrokeCap StrokeCap {
			get => SkiaApi.sk_paint_get_stroke_cap (PaintHandle);
			set => SkiaApi.sk_paint_set_stroke_cap (PaintHandle, value);
		}

		public SKStrokeJoin StrokeJoin {
			get => SkiaApi.sk_paint_get_stroke_join (PaintHandle);
			set => SkiaApi.sk_paint_set_stroke_join (PaintHandle, value);
		}

		public SKShader Shader {
			get => SKShader.GetObject (SkiaApi.sk_paint_get_shader (PaintHandle));
			set => SkiaApi.sk_paint_set_shader (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKMaskFilter MaskFilter {
			get => SKMaskFilter.GetObject (SkiaApi.sk_paint_get_maskfilter (PaintHandle));
			set => SkiaApi.sk_paint_set_maskfilter (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKColorFilter ColorFilter {
			get => SKColorFilter.GetObject (SkiaApi.sk_paint_get_colorfilter (PaintHandle));
			set => SkiaApi.sk_paint_set_colorfilter (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKImageFilter ImageFilter {
			get => SKImageFilter.GetObject (SkiaApi.sk_paint_get_imagefilter (PaintHandle));
			set => SkiaApi.sk_paint_set_imagefilter (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKBlendMode BlendMode {
			get => SkiaApi.sk_paint_get_blendmode_or_default (PaintHandle, SKBlendMode.SrcOver);
			set => SkiaApi.sk_paint_set_blend_mode (PaintHandle, value);
		}

		public SKBlendMode? BlenderAsBlendMode()
		{
			SKBlendMode blendMode;
			SKBlendMode* ptr = &blendMode;
			SkiaApi.sk_paint_as_blendmode (PaintHandle, &ptr);
			return ptr is null ? null : blendMode;
		}

		public SKBlendMode BlenderAsBlendModeOrDefault (SKBlendMode defaultBlendMode)
		{
			return SkiaApi.sk_paint_get_blendmode_or_default (PaintHandle, defaultBlendMode);
		}

		public bool IsSrcOver => SkiaApi.sk_paint_is_src_over (PaintHandle);

		public void SetARGB(byte a, byte r, byte g, byte b) => SkiaApi.sk_paint_set_argb (PaintHandle, a, r, g, b);
		public void SetARGB(float a, float r, float g, float b) => SkiaApi.sk_paint_set_argb (
			PaintHandle,
			(byte)(a * 255.0f + 0.5f),
			(byte)(r * 255.0f + 0.5f),
			(byte)(g * 255.0f + 0.5f),
			(byte)(b * 255.0f + 0.5f)
		);

		public SKBlender Blender {
			get => SKBlender.GetObject (SkiaApi.sk_paint_get_blender (PaintHandle));
			set => SkiaApi.sk_paint_set_blender (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		public SKTypeface Typeface {
			get => font.Typeface;
			set => font.Typeface = value;
		}

		public float TextSize {
			get => font.Size;
			set => font.Size = value;
		}

		public SKTextAlign TextAlign {
			get => SkiaApi.sk_paint_extra_info_get_text_align (Handle);
			set => SkiaApi.sk_paint_extra_info_set_text_align (Handle, value);
		}

		public SKTextEncoding TextEncoding {
			get => SkiaApi.sk_paint_extra_info_get_text_encoding (Handle);
			set => SkiaApi.sk_paint_extra_info_set_text_encoding (Handle, value);
		}

		public float TextScaleX {
			get => font.ScaleX;
			set => font.ScaleX = value;
		}

		public float TextSkewX {
			get => font.SkewX;
			set => font.SkewX = value;
		}

		public SKPathEffect PathEffect {
			get => SKPathEffect.GetObject (SkiaApi.sk_paint_get_path_effect (PaintHandle));
			set => SkiaApi.sk_paint_set_path_effect (PaintHandle, value == null ? IntPtr.Zero : value.Handle);
		}

		// FontSpacing

		public float FontSpacing =>
			font.Spacing;

		// FontMetrics

		public SKFontMetrics FontMetrics {
			get {
				return font.Metrics;
			}
		}

		public float GetFontMetrics (out SKFontMetrics metrics) =>
			font.GetFontMetrics (out metrics);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use GetFontMetrics (out SKFontMetrics) instead.")]
		public float GetFontMetrics (out SKFontMetrics metrics, float scale) =>
			GetFontMetrics (out metrics);

		// Clone

		public SKPaint Clone () =>
			GetObject (SkiaApi.sk_paint_extra_info_clone (Handle));

		// MeasureText

		public float MeasureText (string text) =>
			font.MeasureText (text, this);

		public float MeasureText (ReadOnlySpan<char> text) =>
			font.MeasureText (text, this);

		public float MeasureText (byte[] text) =>
			font.MeasureText (text, TextEncoding, this);

		public float MeasureText (ReadOnlySpan<byte> text) =>
			font.MeasureText (text, TextEncoding, this);

		public float MeasureText (IntPtr buffer, int length) =>
			font.MeasureText (buffer, length, TextEncoding, this);

		public float MeasureText (IntPtr buffer, IntPtr length) =>
			font.MeasureText (buffer, (int)length, TextEncoding, this);

		public float MeasureText (string text, ref SKRect bounds) =>
			font.MeasureText (text, out bounds, this);

		public float MeasureText (ReadOnlySpan<char> text, ref SKRect bounds) =>
			font.MeasureText (text, out bounds, this);

		public float MeasureText (byte[] text, ref SKRect bounds) =>
			font.MeasureText (text, TextEncoding, out bounds, this);

		public float MeasureText (ReadOnlySpan<byte> text, ref SKRect bounds) =>
			font.MeasureText (text, TextEncoding, out bounds, this);

		public float MeasureText (IntPtr buffer, int length, ref SKRect bounds) =>
			font.MeasureText (buffer, length, TextEncoding, out bounds, this);

		public float MeasureText (IntPtr buffer, IntPtr length, ref SKRect bounds) =>
			font.MeasureText (buffer, (int)length, TextEncoding, out bounds, this);

		// BreakText

		public long BreakText (string text, float maxWidth) =>
			font.BreakText (text, maxWidth, out _, this);

		public long BreakText (string text, float maxWidth, out float measuredWidth) =>
			font.BreakText (text, maxWidth, out measuredWidth, this);

		public long BreakText (string text, float maxWidth, out float measuredWidth, out string measuredText)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			var charsRead = font.BreakText (text, maxWidth, out measuredWidth, this);
			if (charsRead == 0) {
				measuredText = string.Empty;
				return 0;
			}
			if (charsRead == text.Length) {
				measuredText = text;
				return text.Length;
			}
			measuredText = text.Substring (0, charsRead);
			return charsRead;
		}

		public long BreakText (ReadOnlySpan<char> text, float maxWidth) =>
			font.BreakText (text, maxWidth, out _, this);

		public long BreakText (ReadOnlySpan<char> text, float maxWidth, out float measuredWidth) =>
			font.BreakText (text, maxWidth, out measuredWidth, this);

		public long BreakText (byte[] text, float maxWidth) =>
			font.BreakText (text, TextEncoding, maxWidth, out _, this);

		public long BreakText (byte[] text, float maxWidth, out float measuredWidth) =>
			font.BreakText (text, TextEncoding, maxWidth, out measuredWidth, this);

		public long BreakText (ReadOnlySpan<byte> text, float maxWidth) =>
			font.BreakText (text, TextEncoding, maxWidth, out _, this);

		public long BreakText (ReadOnlySpan<byte> text, float maxWidth, out float measuredWidth) =>
			font.BreakText (text, TextEncoding, maxWidth, out measuredWidth, this);

		public long BreakText (IntPtr buffer, int length, float maxWidth) =>
			font.BreakText (buffer, length, TextEncoding, maxWidth, out _, this);

		public long BreakText (IntPtr buffer, int length, float maxWidth, out float measuredWidth) =>
			font.BreakText (buffer, length, TextEncoding, maxWidth, out measuredWidth, this);

		public long BreakText (IntPtr buffer, IntPtr length, float maxWidth) =>
			font.BreakText (buffer, (int)length, TextEncoding, maxWidth, out _, this);

		public long BreakText (IntPtr buffer, IntPtr length, float maxWidth, out float measuredWidth) =>
			font.BreakText (buffer, (int)length, TextEncoding, maxWidth, out measuredWidth, this);

		// GetTextPath

		public SKPath GetTextPath (string text, float x, float y) =>
			font.GetTextPath (text, new SKPoint (x, y));

		public SKPath GetTextPath (ReadOnlySpan<char> text, float x, float y) =>
			font.GetTextPath (text, new SKPoint (x, y));

		public SKPath GetTextPath (byte[] text, float x, float y) =>
			font.GetTextPath (text, TextEncoding, new SKPoint (x, y));

		public SKPath GetTextPath (ReadOnlySpan<byte> text, float x, float y) =>
			font.GetTextPath (text, TextEncoding, new SKPoint (x, y));

		public SKPath GetTextPath (IntPtr buffer, int length, float x, float y) =>
			font.GetTextPath (buffer, length, TextEncoding, new SKPoint (x, y));

		public SKPath GetTextPath (IntPtr buffer, IntPtr length, float x, float y) =>
			font.GetTextPath (buffer, (int)length, TextEncoding, new SKPoint (x, y));

		public SKPath GetTextPath (string text, SKPoint[] points) =>
			font.GetTextPath (text, points);

		public SKPath GetTextPath (ReadOnlySpan<char> text, ReadOnlySpan<SKPoint> points) =>
			font.GetTextPath (text, points);

		public SKPath GetTextPath (byte[] text, SKPoint[] points) =>
			font.GetTextPath (text, TextEncoding, points);

		public SKPath GetTextPath (ReadOnlySpan<byte> text, ReadOnlySpan<SKPoint> points) =>
			font.GetTextPath (text, TextEncoding, points);

		public SKPath GetTextPath (IntPtr buffer, int length, SKPoint[] points) =>
			font.GetTextPath (buffer, length, TextEncoding, points);

		public SKPath GetTextPath (IntPtr buffer, int length, ReadOnlySpan<SKPoint> points) =>
			font.GetTextPath (buffer, length, TextEncoding, points);

		public SKPath GetTextPath (IntPtr buffer, IntPtr length, SKPoint[] points) =>
			font.GetTextPath (buffer, (int)length, TextEncoding, points);

		// GetFillPath

		public SKPath GetFillPath (SKPath src)
			=> GetFillPath (src, 1f);

		public SKPath GetFillPath (SKPath src, float resScale)
		{
			var dst = new SKPath ();

			if (GetFillPath (src, dst, resScale)) {
				return dst;
			} else {
				dst.Dispose ();
				return null;
			}
		}

		public SKPath GetFillPath (SKPath src, SKRect cullRect)
			=> GetFillPath (src, cullRect, 1f);

		public SKPath GetFillPath (SKPath src, SKRect cullRect, float resScale)
		{
			var dst = new SKPath ();

			if (GetFillPath (src, dst, cullRect, resScale)) {
				return dst;
			} else {
				dst.Dispose ();
				return null;
			}
		}

		public bool GetFillPath (SKPath src, SKPath dst)
			=> GetFillPath (src, dst, 1f);

		public bool GetFillPath (SKPath src, SKPath dst, float resScale)
		{
			if (src == null)
				throw new ArgumentNullException (nameof (src));
			if (dst == null)
				throw new ArgumentNullException (nameof (dst));

			return SkiaApi.sk_paint_get_fill_path (PaintHandle, src.Handle, dst.Handle, null, resScale);
		}

		public bool GetFillPath (SKPath src, SKPath dst, SKRect cullRect)
			=> GetFillPath (src, dst, cullRect, 1f);

		public bool GetFillPath (SKPath src, SKPath dst, SKRect cullRect, float resScale)
		{
			if (src == null)
				throw new ArgumentNullException (nameof (src));
			if (dst == null)
				throw new ArgumentNullException (nameof (dst));

			return SkiaApi.sk_paint_get_fill_path (PaintHandle, src.Handle, dst.Handle, &cullRect, resScale);
		}

		// CountGlyphs

		public int CountGlyphs (string text) =>
			font.CountGlyphs (text);

		public int CountGlyphs (ReadOnlySpan<char> text) =>
			font.CountGlyphs (text);

		public int CountGlyphs (byte[] text) =>
			font.CountGlyphs (text, TextEncoding);

		public int CountGlyphs (ReadOnlySpan<byte> text) =>
			font.CountGlyphs (text, TextEncoding);

		public int CountGlyphs (IntPtr text, int length) =>
			font.CountGlyphs (text, length, TextEncoding);

		public int CountGlyphs (IntPtr text, IntPtr length) =>
			font.CountGlyphs (text, (int)length, TextEncoding);

		// GetGlyphs

		public ushort[] GetGlyphs (string text) =>
			font.GetGlyphs (text);

		public ushort[] GetGlyphs (ReadOnlySpan<char> text) =>
			font.GetGlyphs (text);

		public ushort[] GetGlyphs (byte[] text) =>
			font.GetGlyphs (text, TextEncoding);

		public ushort[] GetGlyphs (ReadOnlySpan<byte> text) =>
			font.GetGlyphs (text, TextEncoding);

		public ushort[] GetGlyphs (IntPtr text, int length) =>
			font.GetGlyphs (text, length, TextEncoding);

		public ushort[] GetGlyphs (IntPtr text, IntPtr length) =>
			font.GetGlyphs (text, (int)length, TextEncoding);

		// ContainsGlyphs

		public bool ContainsGlyphs (string text) =>
			font.ContainsGlyphs (text);

		public bool ContainsGlyphs (ReadOnlySpan<char> text) =>
			font.ContainsGlyphs (text);

		public bool ContainsGlyphs (byte[] text) =>
			font.ContainsGlyphs (text, TextEncoding);

		public bool ContainsGlyphs (ReadOnlySpan<byte> text) =>
			font.ContainsGlyphs (text, TextEncoding);

		public bool ContainsGlyphs (IntPtr text, int length) =>
			font.ContainsGlyphs (text, length, TextEncoding);

		public bool ContainsGlyphs (IntPtr text, IntPtr length) =>
			font.ContainsGlyphs (text, (int)length, TextEncoding);

		// GetGlyphPositions

		public SKPoint[] GetGlyphPositions (string text, SKPoint origin = default) =>
			font.GetGlyphPositions (text, origin);

		public SKPoint[] GetGlyphPositions (ReadOnlySpan<char> text, SKPoint origin = default) =>
			font.GetGlyphPositions (text, origin);

		public SKPoint[] GetGlyphPositions (ReadOnlySpan<byte> text, SKPoint origin = default) =>
			font.GetGlyphPositions (text, TextEncoding, origin);

		public SKPoint[] GetGlyphPositions (IntPtr text, int length, SKPoint origin = default) =>
			font.GetGlyphPositions (text, length, TextEncoding, origin);

		// GetGlyphOffsets

		public float[] GetGlyphOffsets (string text, float origin = 0f) =>
			font.GetGlyphOffsets (text, origin);

		public float[] GetGlyphOffsets (ReadOnlySpan<char> text, float origin = 0f) =>
			font.GetGlyphOffsets (text, origin);

		public float[] GetGlyphOffsets (ReadOnlySpan<byte> text, float origin = 0f) =>
			font.GetGlyphOffsets (text, TextEncoding, origin);

		public float[] GetGlyphOffsets (IntPtr text, int length, float origin = 0f) =>
			font.GetGlyphOffsets (text, length, TextEncoding, origin);

		// GetGlyphWidths

		public float[] GetGlyphWidths (string text) =>
			font.GetGlyphWidths (text, this);

		public float[] GetGlyphWidths (ReadOnlySpan<char> text) =>
			font.GetGlyphWidths (text, this);

		public float[] GetGlyphWidths (byte[] text) =>
			font.GetGlyphWidths (text, TextEncoding, this);

		public float[] GetGlyphWidths (ReadOnlySpan<byte> text) =>
			font.GetGlyphWidths (text, TextEncoding, this);

		public float[] GetGlyphWidths (IntPtr text, int length) =>
			font.GetGlyphWidths (text, length, TextEncoding, this);

		public float[] GetGlyphWidths (IntPtr text, IntPtr length) =>
			font.GetGlyphWidths (text, (int)length, TextEncoding, this);

		public float[] GetGlyphWidths (string text, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, out bounds, this);

		public float[] GetGlyphWidths (ReadOnlySpan<char> text, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, out bounds, this);

		public float[] GetGlyphWidths (byte[] text, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, TextEncoding, out bounds, this);

		public float[] GetGlyphWidths (ReadOnlySpan<byte> text, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, TextEncoding, out bounds, this);

		public float[] GetGlyphWidths (IntPtr text, int length, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, length, TextEncoding, out bounds, this);

		public float[] GetGlyphWidths (IntPtr text, IntPtr length, out SKRect[] bounds) =>
			font.GetGlyphWidths (text, (int)length, TextEncoding, out bounds, this);

		// GetTextIntercepts

		public float[] GetTextIntercepts (string text, float x, float y, float upperBounds, float lowerBounds) =>
			GetTextIntercepts (text.AsSpan (), x, y, upperBounds, lowerBounds);

		public float[] GetTextIntercepts (ReadOnlySpan<char> text, float x, float y, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.Create (text, font, new SKPoint (x, y));
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetTextIntercepts (byte[] text, float x, float y, float upperBounds, float lowerBounds) =>
			GetTextIntercepts (text.AsSpan (), x, y, upperBounds, lowerBounds);

		public float[] GetTextIntercepts (ReadOnlySpan<byte> text, float x, float y, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.Create (text, TextEncoding, font, new SKPoint (x, y));
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetTextIntercepts (IntPtr text, IntPtr length, float x, float y, float upperBounds, float lowerBounds) =>
			GetTextIntercepts (text, (int)length, x, y, upperBounds, lowerBounds);

		public float[] GetTextIntercepts (IntPtr text, int length, float x, float y, float upperBounds, float lowerBounds)
		{
			if (text == IntPtr.Zero && length != 0)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.Create (text, length, TextEncoding, font, new SKPoint (x, y));
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		// GetTextIntercepts (SKTextBlob)

		public float[] GetTextIntercepts (SKTextBlob text, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			return text.GetIntercepts (upperBounds, lowerBounds, this);
		}

		// GetPositionedTextIntercepts

		public float[] GetPositionedTextIntercepts (string text, SKPoint[] positions, float upperBounds, float lowerBounds) =>
			GetPositionedTextIntercepts (text.AsSpan (), positions, upperBounds, lowerBounds);

		public float[] GetPositionedTextIntercepts (ReadOnlySpan<char> text, ReadOnlySpan<SKPoint> positions, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreatePositioned (text, font, positions);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetPositionedTextIntercepts (byte[] text, SKPoint[] positions, float upperBounds, float lowerBounds) =>
			GetPositionedTextIntercepts (text.AsSpan (), positions, upperBounds, lowerBounds);

		public float[] GetPositionedTextIntercepts (ReadOnlySpan<byte> text, ReadOnlySpan<SKPoint> positions, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreatePositioned (text, TextEncoding, font, positions);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetPositionedTextIntercepts (IntPtr text, int length, SKPoint[] positions, float upperBounds, float lowerBounds) =>
			GetPositionedTextIntercepts (text, (IntPtr)length, positions, upperBounds, lowerBounds);

		public float[] GetPositionedTextIntercepts (IntPtr text, IntPtr length, SKPoint[] positions, float upperBounds, float lowerBounds)
		{
			if (text == IntPtr.Zero && length != IntPtr.Zero)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreatePositioned (text, (int)length, TextEncoding, font, positions);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		// GetHorizontalTextIntercepts

		public float[] GetHorizontalTextIntercepts (string text, float[] xpositions, float y, float upperBounds, float lowerBounds) =>
			GetHorizontalTextIntercepts (text.AsSpan (), xpositions, y, upperBounds, lowerBounds);

		public float[] GetHorizontalTextIntercepts (ReadOnlySpan<char> text, ReadOnlySpan<float> xpositions, float y, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreateHorizontal (text, font, xpositions, y);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetHorizontalTextIntercepts (byte[] text, float[] xpositions, float y, float upperBounds, float lowerBounds) =>
			GetHorizontalTextIntercepts (text.AsSpan (), xpositions, y, upperBounds, lowerBounds);

		public float[] GetHorizontalTextIntercepts (ReadOnlySpan<byte> text, ReadOnlySpan<float> xpositions, float y, float upperBounds, float lowerBounds)
		{
			if (text == null)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreateHorizontal (text, TextEncoding, font, xpositions, y);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		public float[] GetHorizontalTextIntercepts (IntPtr text, int length, float[] xpositions, float y, float upperBounds, float lowerBounds) =>
			GetHorizontalTextIntercepts (text, (IntPtr)length, xpositions, y, upperBounds, lowerBounds);

		public float[] GetHorizontalTextIntercepts (IntPtr text, IntPtr length, float[] xpositions, float y, float upperBounds, float lowerBounds)
		{
			if (text == IntPtr.Zero && length != IntPtr.Zero)
				throw new ArgumentNullException (nameof (text));

			using var blob = SKTextBlob.CreateHorizontal (text, (int)length, TextEncoding, font, xpositions, y);
			return blob.GetIntercepts (upperBounds, lowerBounds, this);
		}

		// Font

		public SKFont ToFont () =>
			SKFont.GetObject (SkiaApi.sk_paint_extra_info_clone_font (Handle));

		internal SKFont GetFont () => font;

		private void UpdateFontEdging (bool antialias)
		{
			var edging = SKFontEdging.Alias;
			if (antialias) {
				edging = lcdRenderText
					? SKFontEdging.SubpixelAntialias
					: SKFontEdging.Antialias;
			}
			font.Edging = edging;
		}

		//

		internal static SKPaint GetObject (IntPtr handle) =>
			handle == IntPtr.Zero ? null : new SKPaint (handle, true);
	}
}
