using System;
using System.ComponentModel;
using System.IO;

namespace SkiaSharp
{
	// TODO: missing the `QueryYuv8` and `GetYuv8Planes` members

	public unsafe class SKAndroidCodec : SKObject, ISKSkipObjectRegistration
	{
		internal SKAndroidCodec(IntPtr handle, bool owns)
			: base(handle, owns)
		{
		}

		protected override void Dispose(bool disposing) =>
			base.Dispose(disposing);

		protected override void DisposeNative() =>
			SkiaApi.sk_android_codec_destroy(Handle);

		public SKImageInfo Info
		{
			get
			{
				SKImageInfoNative cinfo;
				SkiaApi.sk_android_codec_get_info(Handle, &cinfo);
				return SKImageInfoNative.ToManaged(ref cinfo);
			}
		}

		public SKCodec Codec =>
			SKCodec.GetObject(SkiaApi.sk_android_codec_get_codec(Handle));

		public SKColorSpaceIccProfile ICCProfile =>
			SKColorSpaceIccProfile.GetObject(SkiaApi.sk_android_codec_get_icc_profile(Handle));

		public SKEncodedImageFormat EncodedFormat =>
			SkiaApi.sk_android_codec_get_encoded_format(Handle);

		public SKColorType ComputeOutputColorType(SKColorType requestedColorType)
		{
			return (SKColorType)SkiaApi.sk_android_codec_compute_output_color_type(Handle, requestedColorType);
		}

		public SKAlphaType ComputeOutputAlphaType(bool requestedUnpremul)
		{
			return (SKAlphaType)SkiaApi.sk_android_codec_compute_output_alpha_type(Handle, requestedUnpremul);
		}

		public SKColorSpace ComputeOutputColorSpace(SKColorType outputColorType, SKColorSpace preferredColorSpace)
		{
			return (SKColorSpace)SkiaApi.sk_android_codec_compute_output_color_space(Handle, requested, outputColorType, preferredColorSpace);
		}

		public SKColorType ComputeOutputColorType(SKColorType requested)
		{
			return (SKColorType)SkiaApi.sk_android_codec_compute_output_color_type(Handle, requested);
		}

		public SKSizeI GetSampledDimensions(int sampleSize)
		{
			SKSizeI dimensions;
			SkiaApi.sk_android_codec_get_sampled_dimensions(Handle, sampleSize, &dimensions);
			return dimensions;
		}

		public bool GetSupportedSubset(ref SKRectI desiredSubset)
		{
			fixed (SKRectI* ds = &desiredSubset)
			{
				return SkiaApi.sk_android_codec_get_supported_subset(Handle, ds);
			}
		}

		public SKSizeI GetSampledSubsetDimensions(int sampleSize, ref SKRectI desiredSubset)
		{
			SKSizeI dimensions;
			fixed (SKRectI* ds = &desiredSubset)
			{
				SkiaApi.sk_android_codec_get_sampled_subset_dimensions(Handle, sampleSize, &dimensions, ds);
			}
			return dimensions;
		}

		// android pixels


		public SKAndroidCodecResult GetAndroidPixels(out byte[] pixels, SKAndroidCodecOptions options) =>
			GetAndroidPixels(Info, out pixels, options);

		public SKAndroidCodecResult GetAndroidPixels(SKImageInfo info, out byte[] pixels, SKAndroidCodecOptions options)
		{
			pixels = new byte[info.BytesSize];
			return GetAndroidPixels(info, pixels, options);
		}

		public SKAndroidCodecResult GetAndroidPixels(SKImageInfo info, byte[] pixels, SKAndroidCodecOptions options)
		{
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));

			fixed (byte* p = pixels)
			{
				return GetAndroidPixels(info, (IntPtr)p, info.RowBytes, options);
			}
		}

		public SKAndroidCodecResult GetAndroidPixels(SKImageInfo info, IntPtr pixels, SKAndroidCodecOptions options) =>
			GetAndroidPixels(info, pixels, info.RowBytes, options);


		public SKAndroidCodecResult GetAndroidPixels(SKImageInfo info, IntPtr pixels, int rowBytes, SKAndroidCodecOptions options)
		{
			if (pixels == IntPtr.Zero)
				throw new ArgumentNullException(nameof(pixels));

			var nInfo = SKImageInfoNative.FromManaged(ref info);
			var nOptions = new SKAndroidCodecOptionsInternal
			{
				fZeroInitialized = options.ZeroInitialized,
				fSubset = null,
				fSampleSize = options.SampleSize,
			};
			var subset = default(SKRectI);
			if (options.HasSubset)
			{
				subset = options.Subset.Value;
				nOptions.fSubset = &subset;
			}
			return SkiaApi.sk_android_codec_get_android_pixels(Handle, &nInfo, (void*)pixels, (IntPtr)rowBytes, &nOptions);
		}


		public byte[] AndroidPixelsSimplified
		{
			get
			{
				var result = GetAndroidPixelsSimplified(out var pixels);
				if (result != SKAndroidCodecResult.Success && result != SKAndroidCodecResult.IncompleteInput)
				{
					throw new Exception(result.ToString());
				}
				return pixels;
			}
		}

		public SKAndroidCodecResult GetAndroidPixelsSimplified(out byte[] pixels) =>
			GetAndroidPixelsSimplified(Info, out pixels);

		public SKAndroidCodecResult GetAndroidPixelsSimplified(SKImageInfo info, out byte[] pixels)
		{
			pixels = new byte[info.BytesSize];
			return GetAndroidPixelsSimplified(info, pixels);
		}

		public SKAndroidCodecResult GetAndroidPixelsSimplified(SKImageInfo info, byte[] pixels)
		{
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));

			fixed (byte* p = pixels)
			{
				return GetAndroidPixelsSimplified(info, (IntPtr)p, info.RowBytes);
			}
		}

		public SKAndroidCodecResult GetAndroidPixelsSimplified(SKImageInfo info, IntPtr pixels) =>
			GetAndroidPixelsSimplified(info, pixels, info.RowBytes);

		public SKAndroidCodecResult GetAndroidPixelsSimplified(SKImageInfo info, IntPtr pixels) =>
			GetAndroidPixelsSimplified(info, pixels, info.RowBytes);


		public SKAndroidCodecResult GetAndroidPixelsSimplified(SKImageInfo info, IntPtr pixels, int rowBytes)
		{
			if (pixels == IntPtr.Zero)
				throw new ArgumentNullException(nameof(pixels));

			var nInfo = SKImageInfoNative.FromManaged(ref info);
			return SkiaApi.sk_android_codec_get_android_pixels_simplified(Handle, &nInfo, (void*)pixels, (IntPtr)rowBytes);
		}

		// pixels

		public byte[] Pixels
		{
			get
			{
				var result = GetPixels(out var pixels);
				if (result != SKAndroidCodecResult.Success && result != SKAndroidCodecResult.IncompleteInput)
				{
					throw new Exception(result.ToString());
				}
				return pixels;
			}
		}

		public SKAndroidCodecResult GetPixels(out byte[] pixels) =>
			GetPixels(Info, out pixels);

		public SKAndroidCodecResult GetPixels(SKImageInfo info, out byte[] pixels)
		{
			pixels = new byte[info.BytesSize];
			return GetPixels(info, pixels);
		}

		public SKAndroidCodecResult GetPixels(SKImageInfo info, byte[] pixels)
		{
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));

			fixed (byte* p = pixels)
			{
				return GetPixels(info, (IntPtr)p, info.RowBytes, SKAndroidCodecOptions.Default);
			}
		}

		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels) =>
			GetPixels(info, pixels, info.RowBytes, SKAndroidCodecOptions.Default);

		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, SKAndroidCodecOptions options) =>
			GetPixels(info, pixels, info.RowBytes, options);

		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, int rowBytes, SKAndroidCodecOptions options)
		{
			if (pixels == IntPtr.Zero)
				throw new ArgumentNullException(nameof(pixels));

			var nInfo = SKImageInfoNative.FromManaged(ref info);
			var nOptions = new SKAndroidCodecOptionsInternal
			{
				fZeroInitialized = options.ZeroInitialized,
				fSubset = null,
				fFrameIndex = options.FrameIndex,
				fPriorFrame = options.PriorFrame,
			};
			var subset = default(SKRectI);
			if (options.HasSubset)
			{
				subset = options.Subset.Value;
				nOptions.fSubset = &subset;
			}
			return SkiaApi.sk_android_codec_get_pixels(Handle, &nInfo, (void*)pixels, (IntPtr)rowBytes, &nOptions);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr, int, SKAndroidCodecOptions) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, int rowBytes, SKAndroidCodecOptions options, IntPtr colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, rowBytes, options);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr, SKAndroidCodecOptions) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, SKAndroidCodecOptions options, IntPtr colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, info.RowBytes, options);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, IntPtr colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, info.RowBytes, SKAndroidCodecOptions.Default);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr, int, SKAndroidCodecOptions) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, int rowBytes, SKAndroidCodecOptions options, SKColorTable colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, rowBytes, options);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr, SKAndroidCodecOptions) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, SKAndroidCodecOptions options, SKColorTable colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, info.RowBytes, options);

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The Index8 color type and color table is no longer supported. Use GetPixels(SKImageInfo, IntPtr) instead.")]
		public SKAndroidCodecResult GetPixels(SKImageInfo info, IntPtr pixels, SKColorTable colorTable, ref int colorTableCount) =>
			GetPixels(info, pixels, info.RowBytes, SKAndroidCodecOptions.Default);

		// create (Codec)

		public static SKAndroidCodec Create(SKCodec codec)
		{
			return Create(codec, null);
		}

		public static SKAndroidCodec Create(SKCodec codec, SKPngChunkReader chunkReader)
		{
			if (codec == null)
				throw new ArgumentNullException(nameof(codec));

			return GetObject(SkiaApi.sk_android_codec_new_from_codec(codec.Handle, chunkReader == null ? IntPtr.Zero : chunkReader.Handle));
		}

		// create (streams)

		public static SKAndroidCodec Create(string filename) =>
			Create(filename, out var result);

		public static SKAndroidCodec Create(string filename, SKPngChunkReader chunkReader) =>
			Create(stream, out result, chunkReader, SelectionPolicy.preferStillImage);

		public static SKAndroidCodec Create(string filename, SelectionPolicy selectionPolicy) =>
			Create(stream, out result, null, selectionPolicy);

		public static SKAndroidCodec Create(string filename, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy) =>
			Create(stream, out result, chunkReader, selectionPolicy);



		public static SKAndroidCodec Create(string filename, out SKAndroidCodecResult result) =>
			Create(stream, out result, null, SelectionPolicy.preferStillImage);

		public static SKAndroidCodec Create(string filename, out SKAndroidCodecResult result, SelectionPolicy selectionPolicy) =>
			Create(stream, out result, chunkReader, SelectionPolicy.preferStillImage);

		public static SKAndroidCodec Create(string filename, out SKAndroidCodecResult result, SelectionPolicy selectionPolicy) =>
			Create(stream, out result, null, selectionPolicy);

		public static SKAndroidCodec Create(string filename, out SKAndroidCodecResult result, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy)
		{
			var stream = SKFileStream.OpenStream(filename);
			if (stream == null)
			{
				result = SKAndroidCodecResult.InternalError;
				return null;
			}

			return Create(stream, out result, chunkReader, selectionPolicy);
		}



		public static SKAndroidCodec Create(Stream stream) =>
			Create(stream, out var result);

		public static SKAndroidCodec Create(Stream stream, SKPngChunkReader chunkReader) =>
			Create(stream, out var result, chunkReader);

		public static SKAndroidCodec Create(Stream stream, SelectionPolicy selectionPolicy) =>
			Create(stream, out var result, selectionPolicy);

		public static SKAndroidCodec Create(Stream stream, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy) =>
			Create(stream, out var result, chunk_reader, selectionPolicy);



		public static SKAndroidCodec Create(Stream stream, out SKAndroidCodecResult result) =>
			Create(WrapManagedStream(stream), out result);

		public static SKAndroidCodec Create(Stream stream, out SKAndroidCodecResult result, SKPngChunkReader chunkReader) =>
			Create(WrapManagedStream(stream), out result, chunkReader);

		public static SKAndroidCodec Create(Stream stream, out SKAndroidCodecResult result, SelectionPolicy selectionPolicy) =>
			Create(WrapManagedStream(stream), out result, selectionPolicy);

		public static SKAndroidCodec Create(Stream stream, out SKAndroidCodecResult result, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy) =>
			Create(WrapManagedStream(stream), out result, chunkReader, selectionPolicy);



		public static SKAndroidCodec Create(SKStream stream) =>
			Create(stream, out var result);

		public static SKAndroidCodec Create(SKStream stream, SKPngChunkReader chunkReader) =>
			Create(stream, out var result, chunkReader);

		public static SKAndroidCodec Create(SKStream stream, SelectionPolicy selectionPolicy) =>
			Create(stream, out var result, selectionPolicy);

		public static SKAndroidCodec Create(SKStream stream, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy) =>
			Create(stream, out var result, chunkReader, selectionPolicy);



		public static SKAndroidCodec Create(SKStream stream, out SKAndroidCodecResult result) =>
			Create(stream, out result, null);

		public static SKAndroidCodec Create(SKStream stream, out SKAndroidCodecResult result, SKPngChunkReader chunkReader) =>
			Create(stream, out result, chunkReader, SelectionPolicy.preferStillImage);

		public static SKAndroidCodec Create(SKStream stream, out SKAndroidCodecResult result, SelectionPolicy selectionPolicy) =>
			Create(stream, out result, null, selectionPolicy);

		public static SKAndroidCodec Create(SKStream stream, out SKAndroidCodecResult result, SKPngChunkReader chunkReader, SelectionPolicy selectionPolicy)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (stream is SKFileStream filestream && !filestream.IsValid)
				throw new ArgumentException("File stream was not valid.", nameof(stream));

			fixed (SKAndroidCodecResult* r = &result)
			{
				var codec = GetObject(SkiaApi.sk_android_codec_new_from_stream(stream.Handle, r, chunkReader == null ? IntPtr.Zero : chunkReader.Handle, selectionPolicy));
				stream.RevokeOwnership(codec);
				return codec;
			}
		}


		// create (data)

		public static SKAndroidCodec Create(SKData data)
		{
			return Create(data, null);
		}

		public static SKAndroidCodec Create(SKData data, SKPngChunkReader chunkReader)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			return GetObject(SkiaApi.sk_android_codec_new_from_data(data.Handle, chunkReader == null ? IntPtr.Zero : chunkReader.Handle));
		}

		// utils

		internal static SKStream WrapManagedStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			// we will need a seekable stream, so buffer it if need be
			if (stream.CanSeek)
			{
				return new SKManagedStream(stream, true);
			}
			else
			{
				return new SKFrontBufferedManagedStream(stream, MinBufferedBytesNeeded, true);
			}
		}

		internal static SKAndroidCodec GetObject(IntPtr handle) =>
			handle == IntPtr.Zero ? null : new SKAndroidCodec(handle, true);
	}
}
