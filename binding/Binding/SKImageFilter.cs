﻿using System;
using System.ComponentModel;

namespace SkiaSharp
{
	// TODO: `asAColorFilter`
	// TODO: `countInputs`, `getInput`
	// TODO: `cropRectIsSet`, `getCropRect`
	// TODO: `computeFastBounds`, `canComputeFastBounds`

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use SKColorChannel instead.")]
	public enum SKDisplacementMapEffectChannelSelectorType
	{
		Unknown = 0,
		R = 1,
		G = 2,
		B = 3,
		A = 4,
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use CreateDropShadow or CreateDropShadowOnly instead.")]
	public enum SKDropShadowImageFilterShadowMode
	{
		DrawShadowAndForeground = 0,
		DrawShadowOnly = 1,
	}

	[EditorBrowsable (EditorBrowsableState.Never)]
	[Obsolete ("Use SKShaderTileMode instead.")]
	public enum SKMatrixConvolutionTileMode
	{
		Clamp = 0,
		Repeat = 1,
		ClampToBlack = 2,

	}

	public static partial class SkiaExtensions
	{
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use SKColorChannel instead.")]
		public static SKColorChannel ToColorChannel (this SKDisplacementMapEffectChannelSelectorType channelSelectorType) =>
			channelSelectorType switch
			{
				SKDisplacementMapEffectChannelSelectorType.R => SKColorChannel.R,
				SKDisplacementMapEffectChannelSelectorType.G => SKColorChannel.G,
				SKDisplacementMapEffectChannelSelectorType.B => SKColorChannel.B,
				SKDisplacementMapEffectChannelSelectorType.A => SKColorChannel.A,
				_ => SKColorChannel.B,
			};

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use SKShaderTileMode instead.")]
		public static SKShaderTileMode ToShaderTileMode (this SKMatrixConvolutionTileMode tileMode) =>
			tileMode switch
			{
				SKMatrixConvolutionTileMode.Clamp => SKShaderTileMode.Clamp,
				SKMatrixConvolutionTileMode.Repeat => SKShaderTileMode.Repeat,
				_ => SKShaderTileMode.Decal,
			};
	}

	public unsafe class SKImageFilter : SKObject, ISKReferenceCounted
	{
		internal SKImageFilter(IntPtr handle, bool owns)
			: base(handle, owns)
		{
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		// CreateMatrix

		public static SKImageFilter CreateMatrix(SKMatrix matrix, SKSamplingOptions samplingOptions, SKImageFilter input = null)
		{
			return GetObject(SkiaApi.sk_imagefilter_new_matrix(&matrix, &samplingOptions, input == null ? IntPtr.Zero : input.Handle));
		}

		// CreateAlphaThreshold

		public static SKImageFilter CreateAlphaThreshold(SKRectI region, float innerThreshold, float outerThreshold, SKImageFilter input = null)
		{
			var reg = new SKRegion ();
			reg.SetRect (region);
			return CreateAlphaThreshold (reg, innerThreshold, outerThreshold, input);

		}

		public static SKImageFilter CreateAlphaThreshold(SKRegion region, float innerThreshold, float outerThreshold, SKImageFilter input = null)
		{
			if (region == null)
				throw new ArgumentNullException (nameof (region));
			return GetObject(SkiaApi.sk_imagefilter_new_alpha_threshold(region.Handle, innerThreshold, outerThreshold, input == null ? IntPtr.Zero : input.Handle));
		}

		// CreateBlur

		public static SKImageFilter CreateBlur (float sigmaX, float sigmaY, SKImageFilter input = null, SKRect? cropRect = null) =>
			CreateBlur (sigmaX, sigmaY, SKShaderTileMode.Decal, input, cropRect);

		public static SKImageFilter CreateBlur (float sigmaX, float sigmaY, SKShaderTileMode tileMode, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue)
			{
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_blur (sigmaX, sigmaY, tileMode, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateColorFilter

		public static SKImageFilter CreateColorFilter(SKColorFilter cf, SKImageFilter input = null, SKRect? cropRect = null)
		{
			if (cf == null)
				throw new ArgumentNullException(nameof(cf));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_color_filter(cf.Handle, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateCompose

		public static SKImageFilter CreateCompose(SKImageFilter outer, SKImageFilter inner)
		{
			if (outer == null)
				throw new ArgumentNullException(nameof(outer));
			if (inner == null)
				throw new ArgumentNullException(nameof(inner));
			return GetObject(SkiaApi.sk_imagefilter_new_compose(outer.Handle, inner.Handle));
		}

		// CreateDisplacementMapEffect

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateDisplacementMapEffect(SKColorChannel, SKColorChannel, float, SKImageFilter, SKImageFilter, SKRect?) instead.")]
		public static SKImageFilter CreateDisplacementMapEffect (SKDisplacementMapEffectChannelSelectorType xChannelSelector, SKDisplacementMapEffectChannelSelectorType yChannelSelector, float scale, SKImageFilter displacement, SKImageFilter input = null, SKRect? cropRect = null) =>
			CreateDisplacementMapEffect (xChannelSelector.ToColorChannel (), yChannelSelector.ToColorChannel (), scale, displacement, input, cropRect);

		public static SKImageFilter CreateDisplacementMapEffect (SKColorChannel xChannelSelector, SKColorChannel yChannelSelector, float scale, SKImageFilter displacement, SKImageFilter input = null, SKRect? cropRect = null)
		{
			if (displacement == null)
				throw new ArgumentNullException (nameof (displacement));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_displacement_map_effect (xChannelSelector, yChannelSelector, scale, displacement.Handle, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateDropShadow

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateDropShadow or CreateDropShadowOnly instead.")]
		public static SKImageFilter CreateDropShadow (float dx, float dy, float sigmaX, float sigmaY, SKColor color, SKDropShadowImageFilterShadowMode shadowMode, SKImageFilter input = null, SKRect? cropRect = null) =>
			shadowMode == SKDropShadowImageFilterShadowMode.DrawShadowOnly
				? CreateDropShadowOnly (dx, dy, sigmaX, sigmaY, color, input, cropRect)
				: CreateDropShadow (dx, dy, sigmaX, sigmaY, color, input, cropRect);

		public static SKImageFilter CreateDropShadow (float dx, float dy, float sigmaX, float sigmaY, SKColor color, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_drop_shadow (dx, dy, sigmaX, sigmaY, (uint)color, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		public static SKImageFilter CreateDropShadowOnly (float dx, float dy, float sigmaX, float sigmaY, SKColor color, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_drop_shadow_only (dx, dy, sigmaX, sigmaY, (uint)color, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// Create*LitDiffuse

		public static SKImageFilter CreateDistantLitDiffuse(SKPoint3 direction, SKColor lightColor, float surfaceScale, float kd, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_distant_lit_diffuse(&direction, (uint)lightColor, surfaceScale, kd, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		public static SKImageFilter CreatePointLitDiffuse(SKPoint3 location, SKColor lightColor, float surfaceScale, float kd, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_point_lit_diffuse(&location, (uint)lightColor, surfaceScale, kd, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		public static SKImageFilter CreateSpotLitDiffuse(SKPoint3 location, SKPoint3 target, float specularExponent, float cutoffAngle, SKColor lightColor, float surfaceScale, float kd, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_spot_lit_diffuse(&location, &target, specularExponent, cutoffAngle, (uint)lightColor, surfaceScale, kd, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// Create*LitSpecular

		public static SKImageFilter CreateDistantLitSpecular(SKPoint3 direction, SKColor lightColor, float surfaceScale, float ks, float shininess, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_distant_lit_specular(&direction, (uint)lightColor, surfaceScale, ks, shininess, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		public static SKImageFilter CreatePointLitSpecular(SKPoint3 location, SKColor lightColor, float surfaceScale, float ks, float shininess, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_point_lit_specular(&location, (uint)lightColor, surfaceScale, ks, shininess, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		public static SKImageFilter CreateSpotLitSpecular(SKPoint3 location, SKPoint3 target, float specularExponent, float cutoffAngle, SKColor lightColor, float surfaceScale, float ks, float shininess, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_spot_lit_specular(&location, &target, specularExponent, cutoffAngle, (uint)lightColor, surfaceScale, ks, shininess, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateMagnifier

		public static SKImageFilter CreateMagnifier(SKRect src, float inset, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_magnifier(&src, inset, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateMatrixConvolution

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateMatrixConvolution(SKSizeI, float[], float, float, SKPointI, SKShaderTileMode, bool, SKImageFilter, SKRect?) instead.")]
		public static SKImageFilter CreateMatrixConvolution (SKSizeI kernelSize, float[] kernel, float gain, float bias, SKPointI kernelOffset, SKMatrixConvolutionTileMode tileMode, bool convolveAlpha, SKImageFilter input = null, SKRect? cropRect = null) =>
			CreateMatrixConvolution (kernelSize, kernel, gain, bias, kernelOffset, tileMode.ToShaderTileMode (), convolveAlpha, input, cropRect);

		public static SKImageFilter CreateMatrixConvolution (SKSizeI kernelSize, float[] kernel, float gain, float bias, SKPointI kernelOffset, SKShaderTileMode tileMode, bool convolveAlpha, SKImageFilter input = null, SKRect? cropRect = null)
		{
			if (kernel == null)
				throw new ArgumentNullException (nameof (kernel));
			if (kernel.Length != kernelSize.Width * kernelSize.Height)
				throw new ArgumentException ("Kernel length must match the dimensions of the kernel size (Width * Height).", nameof (kernel));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			fixed (float* k = kernel) {
				return GetObject (SkiaApi.sk_imagefilter_new_matrix_convolution (&kernelSize, k, gain, bias, &kernelOffset, tileMode, convolveAlpha, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
			}
		}

		// CreateMerge

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete("Use CreateMerge(SKImageFilter, SKImageFilter, SKRect?) instead.")]
		public static SKImageFilter CreateMerge(SKImageFilter first, SKImageFilter second, SKBlendMode mode, SKRect? cropRect = null)
		{
			return CreateMerge(new [] { first, second }, cropRect);
		}

		public static SKImageFilter CreateMerge(SKImageFilter first, SKImageFilter second, SKRect? cropRect = null)
		{
			return CreateMerge(new [] { first, second }, cropRect);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete("Use CreateMerge(SKImageFilter[], SKRect?) instead.")]
		public static SKImageFilter CreateMerge(SKImageFilter[] filters, SKBlendMode[] modes, SKRect? cropRect = null)
		{
			return CreateMerge (filters, cropRect);
		}

		public static SKImageFilter CreateMerge(SKImageFilter[] filters, SKRect? cropRect = null)
		{
			if (filters == null)
				throw new ArgumentNullException(nameof(filters));
			var handles = new IntPtr[filters.Length];
			for (int i = 0; i < filters.Length; i++)
			{
				handles[i] = filters[i]?.Handle ?? IntPtr.Zero;
			}
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			fixed (IntPtr* h = handles) {
				return GetObject (SkiaApi.sk_imagefilter_new_merge (h, filters.Length, cropRectPtr));
			}
		}

		// CreateDilate

		public static SKImageFilter CreateDilate(int radiusX, int radiusY, SKImageFilter input = null, SKRect? cropRect = null) =>
			CreateDilate ((float)radiusX, (float)radiusY, input, cropRect);

		public static SKImageFilter CreateDilate(float radiusX, float radiusY, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_dilate(radiusX, radiusY, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateErode

		public static SKImageFilter CreateErode(int radiusX, int radiusY, SKImageFilter input = null, SKRect? cropRect = null) =>
			CreateErode ((float)radiusX, (float)radiusY, input, cropRect);

		public static SKImageFilter CreateErode(float radiusX, float radiusY, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_erode(radiusX, radiusY, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreateOffset

		public static SKImageFilter CreateOffset(float dx, float dy, SKImageFilter input = null, SKRect? cropRect = null)
		{
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_offset(dx, dy, input == null ? IntPtr.Zero : input.Handle, cropRectPtr));
		}

		// CreatePicture

		public static SKImageFilter CreatePicture(SKPicture picture)
		{
			if (picture == null)
				throw new ArgumentNullException(nameof(picture));
			return GetObject (SkiaApi.sk_imagefilter_new_picture(picture.Handle));
		}

		public static SKImageFilter CreatePicture(SKPicture picture, SKRect cropRect)
		{
			if (picture == null)
				throw new ArgumentNullException(nameof(picture));
			return GetObject (SkiaApi.sk_imagefilter_new_picture_with_croprect(picture.Handle, &cropRect));
		}

		// CreateTile

		public static SKImageFilter CreateTile(SKRect src, SKRect dst, SKImageFilter input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));
			return GetObject(SkiaApi.sk_imagefilter_new_tile(&src, &dst, input.Handle));
		}

		// CreateBlendMode

		public static SKImageFilter CreateBlendMode(SKBlendMode mode, SKImageFilter background, SKImageFilter foreground = null, SKRect? cropRect = null)
		{
			if (background == null)
				throw new ArgumentNullException(nameof(background));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_blend(mode, background.Handle, foreground == null ? IntPtr.Zero : foreground.Handle, cropRectPtr));
		}

		public static SKImageFilter CreateBlenderMode (SKBlender blender, SKImageFilter background, SKImageFilter foreground = null, SKRect? cropRect = null)
		{
			if (background == null)
				throw new ArgumentNullException (nameof (background));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_blender (blender.Handle, background.Handle, foreground == null ? IntPtr.Zero : foreground.Handle, cropRectPtr));
		}

		// CreateArithmetic

		public static SKImageFilter CreateArithmetic(float k1, float k2, float k3, float k4, bool enforcePMColor, SKImageFilter background, SKImageFilter foreground = null, SKRect? cropRect = null)
		{
			if (background == null)
				throw new ArgumentNullException(nameof(background));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_arithmetic(k1, k2, k3, k4, enforcePMColor, background.Handle, foreground == null ? IntPtr.Zero : foreground.Handle, cropRectPtr));
		}

		// CreateImage

		public static SKImageFilter CreateImage(SKImage image)
		{
			return CreateImage (image, new SKSamplingOptions ());
		}

		public static SKImageFilter CreateImage (SKImage image, SKSamplingOptions samplingOptions)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));
			return GetObject (SkiaApi.sk_imagefilter_new_image_source_sampling_options (image.Handle, &samplingOptions));
		}

		public static SKImageFilter CreateImage(SKImage image, SKRect src, SKRect dst, SKSamplingOptions samplingOptions)
		{
			if (image == null)
				throw new ArgumentNullException(nameof(image));
			return GetObject(SkiaApi.sk_imagefilter_new_image_source(image.Handle, &src, &dst, &samplingOptions));
		}

		public static SKImageFilter CreateRuntimeShader (SKRuntimeShaderBuilder builder, string child_name, SKImageFilter input)
		{
			if (builder == null)
				throw new ArgumentNullException (nameof (builder));
			if (child_name == null)
				throw new ArgumentNullException (nameof (child_name));
			return GetObject (SkiaApi.sk_imagefilter_new_runtime_shader (builder.Handle, child_name, input == null ? IntPtr.Zero : input.Handle));
		}

		public static SKImageFilter CreateRuntimeShader (SKRuntimeShaderBuilder builder, string[] child_names, SKImageFilter[] inputs)
		{
			if (builder == null)
				throw new ArgumentNullException (nameof (builder));
			if (child_names == null)
				throw new ArgumentNullException (nameof (child_names));
			if (inputs == null)
				throw new ArgumentNullException (nameof (inputs));
			if (child_names.Length != inputs.Length)
				throw new ArgumentException ("child length must match input length.");

			var input_handles = new IntPtr[inputs.Length];
			for (int i = 0; i < inputs.Length; i++) {
				input_handles[i] = inputs[i]?.Handle ?? IntPtr.Zero;
			}
			fixed (IntPtr* input_handles_ptr = input_handles)
			{
				return GetObject (SkiaApi.sk_imagefilter_new_runtime_shader_with_multi (builder.Handle, child_names, input_handles_ptr, child_names.Length));
			}
		}

		public static SKImageFilter CreateShader(SKShader shader, bool dither, SKRect? cropRect)
		{
			if (shader == null)
				throw new ArgumentNullException(nameof(shader));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_shader_with_dither(shader.Handle, dither, cropRectPtr));
		}

		// CreatePaint

		public static SKImageFilter CreatePaint(SKPaint paint, SKRect? cropRect = null)
		{
			if (paint == null)
				throw new ArgumentNullException(nameof(paint));
			SKRect tmp;
			SKRect* cropRectPtr = (SKRect*) IntPtr.Zero;
			if (cropRect.HasValue) {
				tmp = cropRect.GetValueOrDefault();
				cropRectPtr = &tmp;
			}
			return GetObject (SkiaApi.sk_imagefilter_new_paint(paint.Handle, cropRectPtr));
		}

		public SKData Serialize ()
		{
			return SKData.GetObject (SkiaApi.sk_imagefilter_serialize (Handle));
		}

		public static SKImageFilter Deserialize (SKData data)
		{
			return GetObject (SkiaApi.sk_imagefilter_deserialize (data.Handle));
		}

		internal static SKImageFilter GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKImageFilter (h, o));
	}
}
