using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SkiaSharp
{
	public unsafe class SKRuntimeEffect : SKObject, ISKReferenceCounted
	{
		public unsafe class Uniform : SKObject
		{
			internal Uniform (IntPtr handle, bool owns)
				: base (handle, owns)
			{
			}

			internal static Uniform GetObject (IntPtr handle) =>
				GetOrAddObject (handle, false, (h, o) => new Uniform (h, o));

			// Properties

			public string Name {
				get {
					using var str = new SKString ();
					SkiaApi.sk_runtimeeffect_uniform_get_name (Handle, str.Handle);
					return str.ToString ();
				}
			}

			public int Offset =>
				(int)SkiaApi.sk_runtimeeffect_uniform_get_offset (Handle);

			public SKRuntimeEffectUniformType Type =>
				SkiaApi.sk_runtimeeffect_uniform_get_type (Handle);

			public int Count =>
				(int)SkiaApi.sk_runtimeeffect_uniform_get_count (Handle);

			public int SizeInBytes =>
				(int)SkiaApi.sk_runtimeeffect_uniform_get_count (Handle);
		}

		public unsafe class Child : SKObject
		{
			internal Child (IntPtr handle, bool owns)
				: base (handle, owns)
			{
			}

			internal static Child GetObject (IntPtr handle) =>
				GetOrAddObject (handle, false, (h, o) => new Child (h, o));

			// Properties

			public string Name {
				get {
					using var str = new SKString ();
					SkiaApi.sk_runtimeeffect_child_get_name (Handle, str.Handle);
					return str.ToString ();
				}
			}

			public SKRuntimeEffectChildType Type =>
				SkiaApi.sk_runtimeeffect_child_get_type (Handle);

			public int Index =>
				(int)SkiaApi.sk_runtimeeffect_child_get_index (Handle);
		}

		internal SKRuntimeEffect (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		internal static SKRuntimeEffect GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKRuntimeEffect (h, o));

		internal static SKRuntimeEffect GetObject (IntPtr handle, bool owns) =>
			GetOrAddObject (handle, owns, (h, o) => new SKRuntimeEffect (h, o));

		// Create

		public static SKRuntimeEffect CreateForShader (string sksl, out string errors)
		{
			using var s = new SKString (sksl);
			using var errorString = new SKString ();
			var effect = GetObject (SkiaApi.sk_runtimeeffect_make_for_shader (s.Handle, errorString.Handle));
			errors = errorString?.ToString ();
			if (errors?.Length == 0)
				errors = null;
			return effect;
		}

		public static SKRuntimeEffect CreateForColorFilter (string sksl, out string errors)
		{
			using var s = new SKString (sksl);
			using var errorString = new SKString ();
			var effect = GetObject (SkiaApi.sk_runtimeeffect_make_for_color_filter (s.Handle, errorString.Handle));
			errors = errorString?.ToString ();
			if (errors?.Length == 0)
				errors = null;
			return effect;
		}

		public static SKRuntimeEffect CreateForBlender (string sksl, out string errors)
		{
			using var s = new SKString (sksl);
			using var errorString = new SKString ();
			var effect = GetObject (SkiaApi.sk_runtimeeffect_make_for_blender (s.Handle, errorString.Handle));
			errors = errorString?.ToString ();
			if (errors?.Length == 0)
				errors = null;
			return effect;
		}

		// Properties

		public SKRuntimeEffectChildType EffectType =>
			SkiaApi.sk_runtimeeffect_get_type (Handle);

		public int UniformSize =>
			(int)SkiaApi.sk_runtimeeffect_get_uniform_size (Handle);

		public int ChildrenCount =>
			(int)SkiaApi.sk_runtimeeffect_get_children_count (Handle);

		public int UniformCount =>
			(int)SkiaApi.sk_runtimeeffect_get_uniform_count (Handle);

		public Uniform GetUniform (int i)
		{
			if (i < 0 || i >= ChildrenCount)
				return null;

			return Uniform.GetObject(SkiaApi.sk_runtimeeffect_get_uniform_from_index (Handle, i));
		}

		public Uniform GetUniform (string name)
		{
			return Uniform.GetObject (SkiaApi.sk_runtimeeffect_get_uniform_from_name (Handle, name));
		}

		public Child GetChild (int i)
		{
			if (i < 0 || i >= ChildrenCount)
				return null;

			return Child.GetObject (SkiaApi.sk_runtimeeffect_get_child_from_index (Handle, i));
		}

		public Child GetChild (string name)
		{
			return Child.GetObject (SkiaApi.sk_runtimeeffect_get_child_from_name (Handle, name));
		}
	}
}
