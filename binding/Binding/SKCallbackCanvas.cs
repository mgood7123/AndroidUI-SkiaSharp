using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace SkiaSharp
{
	public unsafe abstract class SKCallbackCanvas : SKNoDrawCanvas
	{
		private static readonly SKManagedCallbackCanvasDelegates delegates;
		private readonly IntPtr userData;
		private int fromNative;

		static SKCallbackCanvas ()
		{
			delegates = new SKManagedCallbackCanvasDelegates {
				fConcat = ConcatInternal,
				fClipRect = ClipRectInternal,
				fClipRRect = ClipRRectInternal,
				fClipPath = ClipPathInternal,
				fClipShader = ClipShaderInternal,
				fClipRegion = ClipRegionInternal,
				fDrawAnnotation = DrawAnnotationInternal,
				fDrawAtlas = DrawAtlasInternal,
				fDrawPaint = DrawPaintInternal,
				fDrawRect = DrawRectInternal,
				fDrawOval = DrawOvalInternal,
				fDrawPoints = DrawPointsInternal,
				fDrawRegion = DrawRegionInternal,
				fDrawArc = DrawArcInternal,
				fDrawPatch = DrawPatchInternal,
				fDrawPath = DrawPathInternal,
				fDrawImage = DrawImageInternal,
				fDrawImageLattice = DrawImageLatticeInternal,
				fDrawImageRect = DrawImageRectInternal,
				fDrawTextBlob = DrawTextBlobInternal,
				fDrawDrawable = DrawDrawableInternal,
				fDrawVertices = DrawVerticesInternal,
				fDrawSlug = DrawSlugInternal,
				fDestroy = DestroyInternal,
				fFlush = FlushInternal,
				fRestore = RestoreInternal,
				fSave = SaveInternal,
				fSaveLayer = SaveLayerInternal,
				fScale = ScaleInternal,
				fSetMatrix = SetMatrixInternal,
				fTranslate = TranslateInternal,
			};

			SkiaApi.sk_managedcallbackcanvas_set_procs (delegates);
		}

		protected SKCallbackCanvas (int width, int height)
			: base (IntPtr.Zero, true)
		{
			userData = DelegateProxies.CreateUserData (this, true);
			Handle = SkiaApi.sk_managedcallbackcanvas_new ((void*)userData, width, height);

			if (Handle == IntPtr.Zero)
				throw new InvalidOperationException ("Unable to create a new SKCallbackCanvas instance.");
		}

		protected override void DisposeNative ()
		{
			if (Interlocked.CompareExchange (ref fromNative, 0, 0) == 0) {
				SkiaApi.sk_managedcallbackcanvas_delete (Handle);
			}
		}

		protected abstract void OnConcat (SKMatrix44 matrix);
		protected abstract void OnClipRect (SKRect rect, SKClipOperation operation, bool antiAlias);
		protected abstract void OnClipRoundRect (SKRoundRect rrect, SKClipOperation operation, bool antiAlias);
		protected abstract void OnClipPath (SKPath path, SKClipOperation operation, bool antiAlias);
		protected abstract void OnClipShader (SKShader shader, SKClipOperation operation);
		protected abstract void OnClipRegion (SKRegion region, SKClipOperation operation);
		protected abstract void OnDrawAnnotation (SKRect rect, string key, SKData value);
		protected abstract void OnDrawArc (SKRect rect, float startAngle, float sweepAngle, bool useCenter, SKPaint paint);
		protected abstract unsafe void OnDrawAtlas (SKImage atlas, SKRect[] sprites, SKRotationScaleMatrix[] transforms, SKColor[] colors, SKBlendMode mode, SKSamplingOptions samplingOptions, SKRect* cullRect, SKPaint paint);
		protected abstract void OnDrawDrawable (SKDrawable drawable, SKMatrix matrix);
		protected abstract void OnDrawImageRect (SKImage image, SKRect dest, SKSamplingOptions samplingOptions, SKPaint paint, SKSrcRectConstraint constraint);
		protected abstract void OnDrawImageRect (SKImage image, SKRect src, SKRect dest, SKSamplingOptions samplingOptions, SKPaint paint, SKSrcRectConstraint constraint);
		protected abstract void OnDrawImageLattice (SKImage image, SKLattice lattice, SKRect dest, SKFilterMode filter, SKPaint paint);
		protected abstract void OnDrawImage (SKImage image, float x, float y, SKSamplingOptions samplingOptions, SKPaint paint);
		protected abstract void OnDrawOval (SKRect rect, SKPaint paint);
		protected abstract void OnDrawPaint (SKPaint paint);
		protected abstract void OnDrawPath (SKPath path, SKPaint paint);
		protected abstract void OnDrawPatch (SKPoint[] cubics, SKColor[] colors, SKPoint[] texCoords, SKBlendMode mode, SKPaint paint);
		protected abstract void OnDrawPoints (SKPointMode pointmode, SKPoint[] points, SKPaint paint);
		protected abstract void OnDrawRect (SKRect rect, SKPaint paint);
		protected abstract void OnDrawRegion (SKRegion region, SKPaint paint);
		protected abstract void OnDrawRoundRectDifference (SKRoundRect outer, SKRoundRect inner, SKPaint paint);
		protected abstract void OnDrawRoundRect (SKRoundRect rect, SKPaint paint);
		protected abstract void OnDrawSlug (IntPtr slug);
		protected abstract void OnDrawText (SKTextBlob blob, float x, float y, SKPaint paint);
		protected abstract void OnDrawVertices (SKVertices vertices, SKBlendMode mode, SKPaint paint);
		protected abstract void OnFlush ();
		protected abstract void OnRestore ();
		protected abstract void OnSave ();
		protected abstract void OnSaveLayer (SKPaint paint);
		protected abstract void OnSaveLayer (SKRect limit, SKPaint paint);
		protected abstract void OnScale (float sx, float sy);
		protected abstract void OnSetMatrix (SKMatrix44 matrix);
		protected abstract void OnTranslate (float dx, float dy);

		// impl

		class DisposeIfNotOwns<T> : IDisposable where T : SKObject
		{
			private bool disposed;
			private T obj;
			public DisposeIfNotOwns (T value)
			{
				obj = value;
			}

			public static implicit operator T (DisposeIfNotOwns<T> disposeIfNotOwns) { return disposeIfNotOwns.obj; }
			public static implicit operator DisposeIfNotOwns<T> (T value) { return new DisposeIfNotOwns<T> (value); }

			protected virtual void Dispose (bool disposing)
			{
				if (disposed)
				{
					// already disposed
					return;
				}
				else
				{
					disposed = true;
				}

				if (obj == null) {
					// no object to dispose
					// this may happen if we receive a null object to store
					return;
				}

				if (!obj.OwnsHandle)
				{
					// we receive an instance that we created
					if (disposing)
					{
						obj.Dispose();
					}
					obj = null;
				}
				else
				{
					// we receive an instance that is not ours, do not dispose it
					// this can happen when the HandleDictionary matches a given handle with an existing instance
					obj = null;
				}
			}

			public void Dispose ()
			{
				Dispose (true);
				GC.SuppressFinalize (this);
			}

			~DisposeIfNotOwns ()
			{
				Dispose (false);
			}
		}

		static T GetManagedObjectFromNative<T> (IntPtr handle, Func<IntPtr, T> creator) where T : SKObject
		{
			if (handle == IntPtr.Zero) {
				return null;
			}

			bool is_ISKNonVirtualReferenceCounted = typeof (ISKNonVirtualReferenceCounted).IsAssignableFrom (typeof (T));
			bool is_ISKReferenceCounted = typeof (ISKReferenceCounted).IsAssignableFrom (typeof (T));

			// can we use this to simplify?
			bool is_ISKSkipObjectRegistration = typeof (ISKSkipObjectRegistration).IsAssignableFrom (typeof (T));

			if (is_ISKReferenceCounted && !is_ISKNonVirtualReferenceCounted) {
				return GetOrAddObject (handle, false, false, (h, o) => creator.Invoke (h));
			} else {
				return creator.Invoke (handle);
			}
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasConcatProxyDelegate))]
		private static void ConcatInternal (IntPtr d, void* context, IntPtr matrix)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKMatrix44> m44 = GetManagedObjectFromNative<SKMatrix44> (matrix, h => new SKMatrix44 (h, false));
			dump.OnConcat (m44);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasScaleProxyDelegate))]
		private static void ScaleInternal (IntPtr d, void* context, float x, float y)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnScale (x, y);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasTranslateProxyDelegate))]
		private static void TranslateInternal (IntPtr d, void* context, float x, float y)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnTranslate (x, y);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasSetMatrixProxyDelegate))]
		private static void SetMatrixInternal (IntPtr d, void* context, IntPtr matrix)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKMatrix44> m44 = GetManagedObjectFromNative<SKMatrix44> (matrix, h => new SKMatrix44 (h, false));
			dump.OnSetMatrix (m44);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawPaintProxyDelegate))]
		private static void DrawPaintInternal (IntPtr d, void* context, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawPaint (managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawRectProxyDelegate))]
		private static void DrawRectInternal (IntPtr d, void* context, SKRect* rect, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			SKRect managedRect = rect == default ? SKRect.Empty : *rect;
			dump.OnDrawRect (managedRect, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawRRectProxyDelegate))]
		private static void DrawRRectInternal (IntPtr d, void* context, IntPtr rrect, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			using DisposeIfNotOwns<SKRoundRect> managedRRect = GetManagedObjectFromNative<SKRoundRect>(rrect, h => new SKRoundRect(h, false));
			dump.OnDrawRoundRect (managedRRect, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawDRRectProxyDelegate))]
		private static void DrawDRRectInternal (IntPtr d, void* context, IntPtr rrect1, IntPtr rrect2, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			using DisposeIfNotOwns<SKRoundRect> managedRRect1 = GetManagedObjectFromNative<SKRoundRect>(rrect1, h => new SKRoundRect(h, false));
			using DisposeIfNotOwns<SKRoundRect> managedRRect2 = GetManagedObjectFromNative<SKRoundRect>(rrect2, h => new SKRoundRect(h, false));
			dump.OnDrawRoundRectDifference (managedRRect1, managedRRect2, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawArcProxyDelegate))]
		private static void DrawArcInternal (IntPtr d, void* context, SKRect* rect, float a, float b, bool c, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			SKRect managedRect = rect == default ? SKRect.Empty : *rect;
			dump.OnDrawArc (managedRect, a, b, c, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawOvalProxyDelegate))]
		private static void DrawOvalInternal (IntPtr d, void* context, SKRect* rect, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			SKRect managedRect = rect == default ? SKRect.Empty : *rect;
			dump.OnDrawOval (managedRect, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawRegionProxyDelegate))]
		private static void DrawRegionInternal (IntPtr d, void* context, IntPtr region, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKRegion> managedRegion = GetManagedObjectFromNative<SKRegion>(region, h => new SKRegion(h, false));
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawRegion (managedRegion, managedPaint);
		}

		private static unsafe T[] GetArray<T> (IntPtr count, T* ptr) where T : unmanaged
		{
			return GetArray<T> ((int)count, ptr);
		}

		private static unsafe T[] GetArray<T> (int count, T* ptr) where T : unmanaged
		{
			if (count == default || ptr == default) {
				return null;
			}

			T[] array = new T[count];
			for (int i = 0; i < count; i++) {
				array[i] = ptr[i];
			}
			return array;
		}

		private static unsafe SKColor[] GetColorArray (IntPtr count, uint* ptr)
		{
			return GetColorArray ((int)count, ptr);
		}

		private static unsafe SKColor[] GetColorArray (int count, uint* ptr)
		{
			if (count == default || ptr == default) {
				return null;
			}

			SKColor[] array = new SKColor[count];
			for (int i = 0; i < count; i++) {
				array[i] = ptr[i];
			}
			return array;
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawPointsProxyDelegate))]
		private static void DrawPointsInternal (IntPtr d, void* context, SKPointMode pointmode, IntPtr count, SKPoint* points, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			SKPoint[] managedPoints = GetArray<SKPoint> (count, points);
			dump.OnDrawPoints (pointmode, managedPoints, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawPathProxyDelegate))]
		private static void DrawPathInternal (IntPtr d, void* context, IntPtr path, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPath> managedPath = GetManagedObjectFromNative<SKPath>(path, h => new SKPath(h, false));
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawPath (managedPath, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawPatchProxyDelegate))]
		private static void DrawPatchInternal (IntPtr d, void* context, SKPoint* cubics, uint* colors, SKPoint* texCoords, SKBlendMode mode, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			SKPoint[] managedCubics = GetArray<SKPoint> (12, cubics);
			SKColor[] managedColors = GetColorArray(4, colors);
			SKPoint[] managedTexCoords = GetArray<SKPoint> (4, texCoords);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawPatch (managedCubics, managedColors, managedTexCoords, mode, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawImageProxyDelegate))]
		private static void DrawImageInternal (IntPtr d, void* context, IntPtr image, float x, float y, SKSamplingOptions* sampling_options, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKImage> managedImage = GetManagedObjectFromNative<SKImage>(image, h => new SKImage(h, false));
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawImage (managedImage, x, y, sampling_options == default ? new SKSamplingOptions () : *sampling_options, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawImageLatticeProxyDelegate))]
		private static void DrawImageLatticeInternal (IntPtr d, void* context, IntPtr image, SKLatticeInternal* lattice, SKRect* dest, SKFilterMode filter, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKImage> managedImage = GetManagedObjectFromNative<SKImage>(image, h => new SKImage(h, false));
			SKLattice managedLattice = new SKLattice ();
			if (lattice != default) {
				//public struct SKLattice : IEquatable<SKLattice>
				//{
				managedLattice = new SKLattice ();
				//	public int[] XDivs { readonly get; set; }
				managedLattice.XDivs = GetArray<int>(lattice->fXCount, lattice->fXDivs);
				//	public int[] YDivs { readonly get; set; }
				managedLattice.YDivs = GetArray<int> (lattice->fYCount, lattice->fYDivs);
				int count = lattice->fXCount * lattice->fYCount;
				//	public SKRectI? Bounds { readonly get; set; }
				managedLattice.Bounds = lattice->fBounds == default ? null : lattice->fBounds[0];
				//	public SKLatticeRectType[] RectTypes { readonly get; set; }
				managedLattice.RectTypes = GetArray<SKLatticeRectType> (count, lattice->fRectTypes);
				//	public SKColor[] Colors { readonly get; set; }
				managedLattice.Colors = GetColorArray(count, lattice->fColors);
			}
			SKRect managedDest = dest == default ? SKRect.Empty : *dest;
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawImageLattice (managedImage, managedLattice, managedDest, filter, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawImageRectProxyDelegate))]
		private static void DrawImageRectInternal (IntPtr d, void* context, IntPtr image, SKRect* src, SKRect* dest, SKSamplingOptions* sampling_options, IntPtr paint, SKSrcRectConstraint constraint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKImage> managedImage = GetManagedObjectFromNative<SKImage> (image, h => new SKImage (h, false));
			SKRect managedDest = dest == default ? SKRect.Empty : *dest;
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint> (paint, h => new SKPaint(IntPtr.Zero, h));
			if (src == default)
			{
				dump.OnDrawImageRect (managedImage, managedDest, sampling_options == default ? new SKSamplingOptions () : *sampling_options, managedPaint, constraint);
			}
			else
			{
				dump.OnDrawImageRect (managedImage, *src, managedDest, sampling_options == default ? new SKSamplingOptions () : *sampling_options, managedPaint, constraint);
			}
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawTextBlobProxyDelegate))]
		private static void DrawTextBlobInternal (IntPtr d, void* context, IntPtr blob, float x, float y, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKTextBlob> managedBlob = GetManagedObjectFromNative<SKTextBlob>(blob, h => new SKTextBlob(h, false));
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawText (managedBlob, x, y, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawDrawableProxyDelegate))]
		private static void DrawDrawableInternal (IntPtr d, void* context, IntPtr drawable, SKMatrix matrix)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKDrawable> managedDrawable = GetManagedObjectFromNative<SKDrawable> (drawable, h => new SKDrawable (h, false));
			dump.OnDrawDrawable (managedDrawable, matrix);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawVerticesProxyDelegate))]
		private static void DrawVerticesInternal (IntPtr d, void* context, IntPtr vertices, SKBlendMode mode, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKVertices> managedVertices = GetManagedObjectFromNative<SKVertices> (vertices, h => new SKVertices (h, false));
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawVertices (managedVertices, mode, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawSlugProxyDelegate))]
		private static void DrawSlugInternal (IntPtr d, void* context, IntPtr slug)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnDrawSlug (slug);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawAtlasProxyDelegate))]
		private static void DrawAtlasInternal (IntPtr d, void* context, IntPtr atlas, SKRotationScaleMatrix* form, SKRect* tex, uint* colors, int count, SKBlendMode mode, SKSamplingOptions* sampling_options, SKRect* cullRect, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKImage> managedImage = GetManagedObjectFromNative<SKImage>(atlas, h => new SKImage(h, false));
			SKRotationScaleMatrix[] managedForm = GetArray<SKRotationScaleMatrix> (count, form);
			SKRect[] managedTex = GetArray<SKRect> (count, tex);
			SKColor[] managedColors = GetColorArray (count, colors);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			dump.OnDrawAtlas (managedImage, managedTex, managedForm, managedColors, mode, sampling_options == default ? new SKSamplingOptions() : *sampling_options, cullRect, managedPaint);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDrawAnnotationProxyDelegate))]
		private static void DrawAnnotationInternal (IntPtr d, void* context, SKRect* rect, void* key, IntPtr data)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			SKRect managedRect = rect == default ? SKRect.Empty : *rect;
			using DisposeIfNotOwns<SKData> managedData = GetManagedObjectFromNative<SKData>(data, h => new SKData(h, false));
			string managedKey = Marshal.PtrToStringAnsi ((IntPtr)key);
			dump.OnDrawAnnotation (managedRect, managedKey, managedData);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasClipRectProxyDelegate))]
		private static void ClipRectInternal (IntPtr d, void* context, SKRect* rect, SKClipOperation operation, bool antialias)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			SKRect managedRect = rect == default ? SKRect.Empty : *rect;
			dump.OnClipRect (managedRect, operation, antialias);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasClipRRectProxyDelegate))]
		private static void ClipRRectInternal (IntPtr d, void* context, IntPtr rrect, SKClipOperation operation, bool antialias)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKRoundRect> managedRRect = GetManagedObjectFromNative<SKRoundRect>(rrect, h => new SKRoundRect(h, false));
			dump.OnClipRoundRect (managedRRect, operation, antialias);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasClipPathProxyDelegate))]
		private static void ClipPathInternal (IntPtr d, void* context, IntPtr path, SKClipOperation operation, bool antialias)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPath> managedPath = GetManagedObjectFromNative<SKPath>(path, h => new SKPath(h, false));
			dump.OnClipPath (managedPath, operation, antialias);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasClipShaderProxyDelegate))]
		private static void ClipShaderInternal (IntPtr d, void* context, IntPtr shader, SKClipOperation operation)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKShader> managedShader = GetManagedObjectFromNative<SKShader>(shader, h => new SKShader(h, false));
			dump.OnClipShader (managedShader, operation);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasClipRegionProxyDelegate))]
		private static void ClipRegionInternal (IntPtr d, void* context, IntPtr region, SKClipOperation operation)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKRegion> managedRegion = GetManagedObjectFromNative<SKRegion>(region, h => new SKRegion(h, false));
			dump.OnClipRegion (managedRegion, operation);
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasSaveLayerProxyDelegate))]
		private static void SaveLayerInternal (IntPtr d, void* context, SKRect* rect, IntPtr paint)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			using DisposeIfNotOwns<SKPaint> managedPaint = GetManagedObjectFromNative<SKPaint>(paint, h => new SKPaint(IntPtr.Zero, h));
			if (rect == default) {
				dump.OnSaveLayer (managedPaint);
			} else {
				dump.OnSaveLayer (*rect, managedPaint);
			}
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasSaveProxyDelegate))]
		private static void SaveInternal (IntPtr d, void* context)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnSave ();
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasRestoreProxyDelegate))]
		private static void RestoreInternal (IntPtr d, void* context)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnRestore ();
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasFlushProxyDelegate))]
		private static void FlushInternal (IntPtr d, void* context)
		{
			var dump = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out _);
			dump.OnFlush ();
		}

		[MonoPInvokeCallback (typeof (SKManagedCallbackCanvasDestroyProxyDelegate))]
		private static void DestroyInternal (IntPtr s, void* context)
		{
			var id = DelegateProxies.GetUserData<SKCallbackCanvas> ((IntPtr)context, out var gch);
			if (id != null) {
				Interlocked.Exchange (ref id.fromNative, 1);
				id.Dispose ();
			}
			gch.Free ();
		}
	}
}
