using System;
using System.Runtime.InteropServices;

namespace SkiaSharp
{
	public unsafe class SK_UTF_CString : SKObject
	{
		int length;
		System.Text.Encoding encoding;
		SKTextEncoding sktextencoding;

		public int Length => length;
		public System.Text.Encoding Encoding => encoding;
		public SKTextEncoding SKTextEncoding => sktextencoding;

		internal SK_UTF_CString (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SK_UTF_CString (string input, SKTextEncoding encoding = SKTextEncoding.Utf8) : this (IntPtr.Zero, false)
		{
			this.encoding = encoding switch {
				SKTextEncoding.Utf8 => System.Text.Encoding.UTF8,
				SKTextEncoding.Utf16 => System.Text.Encoding.Unicode,
				SKTextEncoding.Utf32 => System.Text.Encoding.UTF32,
				_ => throw new ArgumentOutOfRangeException (nameof (encoding), $"Encoding {encoding} is not supported."),
			};
			sktextencoding = encoding;

			length = this.encoding.GetByteCount (input);

			var buffer = (byte*)Marshal.AllocHGlobal (length + 1);

			fixed (char* pInput = input) {
				this.encoding.GetBytes (pInput, input.Length, buffer, length);
			}

			buffer[length] = 0;

			Handle = (IntPtr)buffer;
			OwnsHandle = true;
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			Marshal.FreeHGlobal(Handle);

		internal static SK_UTF_CString GetObject (IntPtr handle) =>
			handle == IntPtr.Zero ? null : new SK_UTF_CString (handle, true);
	}

	internal unsafe class SKString : SKObject, ISKSkipObjectRegistration
	{
		internal SKString (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKString ()
			: base (SkiaApi.sk_string_new_empty (), true)
		{
			if (Handle == IntPtr.Zero) {
				throw new InvalidOperationException ("Unable to create a new SKString instance.");
			}
		}
		
		public SKString (byte [] src, long length)
			: base (CreateCopy (src, length), true)
		{
			if (Handle == IntPtr.Zero) {
				throw new InvalidOperationException ("Unable to copy the SKString instance.");
			}
		}
		
		private static IntPtr CreateCopy (byte [] src, long length)
		{
			fixed (byte* s = src) {
				return SkiaApi.sk_string_new_with_copy (s, (IntPtr)length);
			}
		}

		public SKString (byte [] src)
			: this (src, src.Length)
		{
		}
		
		public SKString (string str)
			: this (StringUtilities.GetEncodedText (str, SKTextEncoding.Utf8))
		{
		}
		
		public override string ToString ()
		{
			var cstr = SkiaApi.sk_string_get_c_str (Handle);
			var clen = SkiaApi.sk_string_get_size (Handle);
			return StringUtilities.GetString ((IntPtr)cstr, (int)clen, SKTextEncoding.Utf8);
		}

		public static explicit operator string (SKString skString)
		{
			return skString.ToString ();
		}
		
		internal static SKString Create (string str)
		{
			if (str == null) {
				return null;
			}
			return new SKString (str);
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			SkiaApi.sk_string_destructor (Handle);

		internal static SKString GetObject (IntPtr handle) =>
			handle == IntPtr.Zero ? null : new SKString (handle, true);
	}
}

