using System;
using SkiaSharp;

/// <summary>
/// Used to be notified when a gen/unique ID is invalidated, typically to preemptively purge
/// <br></br> associated items from a cache that are no longer reachable. The listener can
/// <br></br> be marked for deregistration if the cached item is remove before the listener is
/// <br></br> triggered. This prevents unbounded listener growth when cache items are routinely
/// <br></br> removed before the gen ID/unique ID is invalidated.
/// </summary>
public unsafe abstract class SKIDChangeListener : SKObject, ISKSkipObjectRegistration
{
	private static readonly SKIDChangeListenerDelegates delegates;
	private readonly IntPtr userData;

	static SKIDChangeListener()
	{
		delegates = new SKIDChangeListenerDelegates
		{
			fChanged = ChangedInternal
		};

		SkiaApi.sk_managed_id_change_listener_set_procs(delegates);
	}

	protected SKIDChangeListener()
		: base(IntPtr.Zero, true)
	{
		userData = DelegateProxies.CreateUserData(this, true);
		Handle = SkiaApi.sk_managed_id_change_listener_new((void*)userData);

		if (Handle == IntPtr.Zero)
			throw new InvalidOperationException("Unable to create a new SKIDChangeListener instance.");
	}

	protected override void DisposeNative()
	{
		DelegateProxies.GetUserData<SKIDChangeListener>(userData, out var gch);

		SkiaApi.sk_managed_id_change_listener_delete(Handle);

		gch.Free();
	}

	/// <summary>
	/// Called when a gen/unique ID is invalidated.
	/// </summary>
	public abstract void Changed();

	/// <summary>
    /// Mark the listener is no longer needed.
    /// <br></br> It should be removed and changed() should not be called.
	/// </summary>
	public void MarkShouldDeregister() {
		SkiaApi.sk_managed_id_change_listener_mark_should_deregister(Handle);
	}

	/// <summary>
	/// Indicates whether markShouldDeregister was called.
	/// </summary>
	public bool ShouldDeregister() {
		return SkiaApi.sk_managed_id_change_listener_should_deregister(Handle);
	}

	// impl

	[MonoPInvokeCallback(typeof(SKIDChangeListenerChangedProxyDelegate))]
	private static void ChangedInternal(IntPtr d, void* context)
	{
		var dump = DelegateProxies.GetUserData<SKIDChangeListener>((IntPtr)context, out _);
		dump.Changed();
	}

	/// <summary>
	/// Manages a list of SkIDChangeListeners.
    /// </summary>
	public unsafe class List : SKObject, ISKSkipObjectRegistration
	{
		private readonly IntPtr userData;

		protected SKIDChangeListener()
			: base(IntPtr.Zero, true)
		{
			userData = DelegateProxies.CreateUserData(this, true);
			Handle = SkiaApi.sk_managed_id_change_listener_list_new((void*)userData);

			if (Handle == IntPtr.Zero)
				throw new InvalidOperationException("Unable to create a new SKIDChangeListener.List instance.");
		}

		protected override void DisposeNative()
		{
			DelegateProxies.GetUserData<List>(userData, out var gch);

			SkiaApi.sk_managed_id_change_listener_list_delete(Handle);

			gch.Free();
		}

		/// <summary>
		/// Add a new listener to the list. It must not already be deregistered. Also clears out
		/// <br></br> previously deregistered listeners.
		/// </summary>
		public void Add(SKIDChangeListener listener, bool singleThreaded = false)
		{
			SkiaApi.sk_managed_id_change_listener_list_add(Handle, listener.Handle, singleThreaded);
		}

		/// <returns>The number of registered listeners (including deregisterd listeners that are yet-to-be removed.</returns>
		public int Count()
		{
			return SkiaApi.sk_managed_id_change_listener_list_count(Handle);
		}

		/// <summary>
		/// Calls changed() on all listeners that haven't been deregistered and resets the list.
		/// </summary>
		public void Changed(bool singleThreaded = false)
		{
			SkiaApi.sk_managed_id_change_listener_list_changed(Handle, singleThreaded);
		}

		/// <summary>
		/// Resets without calling changed() on the listeners.
		/// </summary>
		public void Reset(bool singleThreaded = false)
		{
			SkiaApi.sk_managed_id_change_listener_list_reset(Handle, singleThreaded);
		}
	}
}
