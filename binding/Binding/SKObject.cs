﻿using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace SkiaSharp
{
	public abstract class SKObject : SKNativeObject
	{
		private readonly object locker = new object ();

		private ConcurrentDictionary<IntPtr, SKObject> ownedObjects;
		private ConcurrentDictionary<IntPtr, SKObject> keepAliveObjects;

		internal ConcurrentDictionary<IntPtr, SKObject> OwnedObjects {
			get {
				if (ownedObjects == null) {
					lock (locker) {
						ownedObjects ??= new ConcurrentDictionary<IntPtr, SKObject> ();
					}
				}
				return ownedObjects;
			}
		}

		internal ConcurrentDictionary<IntPtr, SKObject> KeepAliveObjects {
			get {
				if (keepAliveObjects == null) {
					lock (locker) {
						keepAliveObjects ??= new ConcurrentDictionary<IntPtr, SKObject> ();
					}
				}
				return keepAliveObjects;
			}
		}

		static SKObject ()
		{
			SkiaSharpVersion.CheckNativeLibraryCompatible (true);

			SKColorSpace.EnsureStaticInstanceAreInitialized ();
			SKData.EnsureStaticInstanceAreInitialized ();
			SKFontManager.EnsureStaticInstanceAreInitialized ();
			SKTypeface.EnsureStaticInstanceAreInitialized ();
		}

		internal SKObject (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		public override IntPtr Handle {
			get => base.Handle;
			protected set {
				if (value == IntPtr.Zero) {
					DeregisterHandle (Handle, this);
					base.Handle = value;
				} else {
					base.Handle = value;
					RegisterHandle (Handle, this);
				}
			}
		}

		protected override void DisposeUnownedManaged ()
		{
			if (ownedObjects != null) {
				foreach (var child in ownedObjects) {
					if (child.Value is SKObject c && !c.OwnsHandle)
						c.DisposeInternal ();
				}
			}
		}

		protected override void DisposeManaged ()
		{
			if (ownedObjects != null) {
				foreach (var child in ownedObjects) {
					if (child.Value is SKObject c && c.OwnsHandle)
						c.DisposeInternal ();
				}
				ownedObjects.Clear ();
			}
			keepAliveObjects?.Clear ();
		}

		protected override void DisposeNative ()
		{
			if (this is ISKReferenceCounted refcnt)
				refcnt.SafeUnRef ();
		}

		internal static TSkiaObject GetOrAddObject<TSkiaObject> (IntPtr handle, Func<IntPtr, bool, TSkiaObject> objectFactory)
			where TSkiaObject : SKObject
		{
			if (handle == IntPtr.Zero)
				return null;

			return HandleDictionary.GetOrAddObject (handle, true, true, objectFactory);
		}

		internal static TSkiaObject GetOrAddObject<TSkiaObject> (IntPtr handle, bool owns, Func<IntPtr, bool, TSkiaObject> objectFactory)
			where TSkiaObject : SKObject
		{
			if (handle == IntPtr.Zero)
				return null;

			return HandleDictionary.GetOrAddObject (handle, owns, true, objectFactory);
		}

		internal static TSkiaObject GetOrAddObject<TSkiaObject> (IntPtr handle, bool owns, bool unrefExisting, Func<IntPtr, bool, TSkiaObject> objectFactory)
			where TSkiaObject : SKObject
		{
			if (handle == IntPtr.Zero)
				return null;

			return HandleDictionary.GetOrAddObject (handle, owns, unrefExisting, objectFactory);
		}

		internal static void RegisterHandle (IntPtr handle, SKObject instance)
		{
			if (handle == IntPtr.Zero || instance == null)
				return;

			HandleDictionary.RegisterHandle (handle, instance);
		}

		internal static void DeregisterHandle (IntPtr handle, SKObject instance)
		{
			if (handle == IntPtr.Zero)
				return;

			HandleDictionary.DeregisterHandle (handle, instance);
		}

		internal static bool GetInstance<TSkiaObject> (IntPtr handle, out TSkiaObject instance)
			where TSkiaObject : SKObject
		{
			if (handle == IntPtr.Zero) {
				instance = null;
				return false;
			}

			return HandleDictionary.GetInstance<TSkiaObject> (handle, out instance);
		}

		// indicate that the user cannot dispose the object
		internal void PreventPublicDisposal ()
		{
			IgnorePublicDispose = true;
		}

		// indicate that the ownership of this object is now in the hands of
		// the native object
		internal void RevokeOwnership (SKObject newOwner)
		{
			OwnsHandle = false;
			IgnorePublicDispose = true;

			if (newOwner == null)
				DisposeInternal ();
			else
				newOwner.OwnedObjects[Handle] = this;
		}

		// indicate that the child is controlled by the native code and
		// the managed wrapper should be disposed when the owner is
		internal static T OwnedBy<T> (T child, SKObject owner)
			where T : SKObject
		{
			if (child != null) {
				owner.OwnedObjects[child.Handle] = child;
			}

			return child;
		}

		// indicate that the child was created by the managed code and
		// should be disposed when the owner is
		internal static T Owned<T> (T owner, SKObject child)
			where T : SKObject
		{
			if (child != null) {
				if (owner != null)
					owner.OwnedObjects[child.Handle] = child;
				else
					child.Dispose ();
			}

			return owner;
		}

		// indicate that the chile should not be garbage collected while
		// the owner still lives
		internal static T Referenced<T> (T owner, SKObject child)
			where T : SKObject
		{
			if (child != null && owner != null)
				owner.KeepAliveObjects[child.Handle] = child;

			return owner;
		}

		internal static T[] PtrToStructureArray<T> (IntPtr intPtr, int count)
		{
			var items = new T[count];
			var size = Marshal.SizeOf<T> ();
			for (var i = 0; i < count; i++) {
				var newPtr = new IntPtr (intPtr.ToInt64 () + (i * size));
				items[i] = Marshal.PtrToStructure<T> (newPtr);
			}
			return items;
		}

		internal static T PtrToStructure<T> (IntPtr intPtr, int index)
		{
			var size = Marshal.SizeOf<T> ();
			var newPtr = new IntPtr (intPtr.ToInt64 () + (index * size));
			return Marshal.PtrToStructure<T> (newPtr);
		}
	}

	public abstract class SKNativeObject : IDisposable
	{
		internal bool fromFinalizer = false;

		// stack trace
		private string s;

		public static bool LOG_ALLOCATIONS = false;
		private static Action<string> c1;
		private static Action<string> c2;
		private static Action<string> c3;
		private static Action<string> c4;

		public static Action<string> LOG_ALLOCATION_CONSTRUCTOR_ENTER_CALLBACK {
			get { return c1; }
			set { c1 = value; }
		}
		public static Action<string> LOG_ALLOCATION_CONSTRUCTOR_EXIT_CALLBACK {
			get { return c2; }
			set { c2 = value; }
		}
		public static Action<string> LOG_ALLOCATION_DESTRUCTOR_ENTER_CALLBACK {
			get { return c3; }
			set { c3 = value; }
		}
		public static Action<string> LOG_ALLOCATION_DESTRUCTOR_EXIT_CALLBACK {
			get { return c4; }
			set { c4 = value; }
		}

		private int isDisposed = 0;

		internal SKNativeObject (IntPtr handle)
			: this (handle, true)
		{
		}

		internal SKNativeObject (IntPtr handle, bool ownsHandle)
		{
			if (LOG_ALLOCATIONS) {
				string tmp;
#if NETCOREAPP2_0_OR_GREATER
				tmp = new System.Diagnostics.StackTrace(true).ToString ();
#elif NET20_OR_GREATER
				tmp = new System.Diagnostics.StackTrace(true).ToString ();
#elif NETSTANDARD2_0_OR_GREATER
				tmp = new System.Diagnostics.StackTrace(true).ToString ();
#elif NET20_OR_GREATER
				tmp = new System.Diagnostics.StackTrace(true).ToString ();
#else
				try {
					throw new Exception ();
				} catch (Exception thrown) {
					tmp = thrown.StackTrace?.ToString();
				}
#endif
				if (tmp == null || tmp.Length == 0) {
					s = "No Stack Info";
				} else {
					s = tmp;
				}
				if (LOG_ALLOCATION_CONSTRUCTOR_ENTER_CALLBACK != null) {
					LOG_ALLOCATION_CONSTRUCTOR_ENTER_CALLBACK.Invoke (s);
				}
				Handle = handle;
				OwnsHandle = ownsHandle;
				if (LOG_ALLOCATION_CONSTRUCTOR_EXIT_CALLBACK != null) {
					LOG_ALLOCATION_CONSTRUCTOR_EXIT_CALLBACK.Invoke (s);
				}
			} else {
				Handle = handle;
				OwnsHandle = ownsHandle;
			}
		}

		~SKNativeObject ()
		{
			if (LOG_ALLOCATIONS) {
				if (LOG_ALLOCATION_DESTRUCTOR_ENTER_CALLBACK != null) {
					LOG_ALLOCATION_DESTRUCTOR_ENTER_CALLBACK.Invoke (s);
				}
				fromFinalizer = true;

				if (Interlocked.CompareExchange (ref isDisposed, 1, 0) == 0)
					Dispose (false);

				if (LOG_ALLOCATION_DESTRUCTOR_EXIT_CALLBACK != null) {
					LOG_ALLOCATION_DESTRUCTOR_EXIT_CALLBACK.Invoke (s);
				}
			} else {
				fromFinalizer = true;

				if (Interlocked.CompareExchange (ref isDisposed, 1, 0) == 0)
					Dispose (false);
			}
		}

		public virtual IntPtr Handle { get; protected set; }

		protected internal virtual bool OwnsHandle { get; protected set; }

		protected internal bool IgnorePublicDispose { get; set; }

		protected internal bool IsDisposed => isDisposed == 1;

		protected virtual void DisposeUnownedManaged ()
		{
			// dispose of any managed resources that are not actually owned
		}

		protected virtual void DisposeManaged ()
		{
			// dispose of any managed resources
		}

		protected virtual void DisposeNative ()
		{
			// dispose of any unmanaged resources
		}

		protected virtual void Dispose (bool disposing)
		{
			// dispose any objects that are owned/created by native code
			if (disposing)
				DisposeUnownedManaged ();

			// destroy the native object
			if (Handle != IntPtr.Zero && OwnsHandle)
				DisposeNative ();

			// dispose any remaining managed-created objects
			if (disposing)
				DisposeManaged ();

			Handle = IntPtr.Zero;
		}

		public void Dispose ()
		{
			if (IgnorePublicDispose)
				return;

			DisposeInternal ();
		}

		protected internal void DisposeInternal ()
		{
			if (Interlocked.CompareExchange (ref isDisposed, 1, 0) == 0)
				Dispose (true);
			GC.SuppressFinalize (this);
		}
	}

	internal static class SKObjectExtensions
	{
		public static bool IsUnique (this IntPtr handle, bool isVirtual)
		{
			if (isVirtual)
				return SkiaApi.sk_refcnt_unique (handle);
			else
				return SkiaApi.sk_nvrefcnt_unique (handle);
		}

		public static void SafeRef (this ISKReferenceCounted obj)
		{
			if (obj is ISKNonVirtualReferenceCounted nvrefcnt)
				nvrefcnt.ReferenceNative ();
			else
				SkiaApi.sk_refcnt_safe_unref (obj.Handle);
		}

		public static void SafeUnRef (this ISKReferenceCounted obj)
		{
			if (obj is ISKNonVirtualReferenceCounted nvrefcnt)
				nvrefcnt.UnreferenceNative ();
			else
				SkiaApi.sk_refcnt_safe_unref (obj.Handle);
		}
	}

	internal interface ISKReferenceCounted
	{
		IntPtr Handle { get; }
	}

	internal interface ISKNonVirtualReferenceCounted : ISKReferenceCounted
	{
		void ReferenceNative ();

		void UnreferenceNative ();
	}

	internal interface ISKSkipObjectRegistration
	{
	}
}
