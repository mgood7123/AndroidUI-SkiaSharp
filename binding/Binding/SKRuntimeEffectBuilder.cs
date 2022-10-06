using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SkiaSharp
{
	public unsafe class SKRuntimeEffectBuilder : SKObject
	{
		internal SKRuntimeEffectBuilder (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		// not allowed to instantiate directly
		private SKRuntimeEffectBuilder () : base(IntPtr.Zero, false)
		{
		}

		internal static SKRuntimeEffectBuilder GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKRuntimeEffectBuilder (h, o));

		internal static SKRuntimeEffectBuilder GetObject (IntPtr handle, bool owns) =>
			GetOrAddObject (handle, owns, (h, o) => new SKRuntimeEffectBuilder (h, o));

		// Properties

		public SKRuntimeEffect Effect =>
			SKRuntimeEffect.GetObject(SkiaApi.sk_runtime_effect_builder_get_effect (Handle), false);

		public SKRuntimeEffect.Uniform GetUniform (string name) =>
			SKRuntimeEffect.Uniform.GetObject (SkiaApi.sk_runtime_effect_builder_get_uniform_by_name (Handle, name));

		public SKRuntimeEffect.Child GetChild (string name) =>
			SKRuntimeEffect.Child.GetObject (SkiaApi.sk_runtime_effect_builder_get_child_by_name (Handle, name));

		public void SetUniformValue(string name, int value)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_int (Handle, name, value);
		}

		public void SetUniformValue (string name, int v1, int v2)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_int2 (Handle, name, v1, v2);
		}

		public void SetUniformValue (string name, int v1, int v2, int v3)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_int3 (Handle, name, v1, v2, v3);
		}

		public void SetUniformValue (string name, int v1, int v2, int v3, int v4)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_int4 (Handle, name, v1, v2, v3, v4);
		}

		public void SetUniformValue (string name, float value)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float (Handle, name, value);
		}

		public void SetUniformValue (string name, float v1, float v2)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float2 (Handle, name, v1, v2);
		}

		public void SetUniformValue (string name, float v1, float v2, float v3)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float3 (Handle, name, v1, v2, v3);
		}

		public void SetUniformValue (string name, float v1, float v2, float v3, float v4)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float4 (Handle, name, v1, v2, v3, v4);
		}

		public void SetUniformValue_2x2 (string name, float v1, float v2, float v3, float v4)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float2x2 (
				Handle,
				name,
				v1, v2,
				v3, v4
			);
		}

		public void SetUniformValue_3x3 (string name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float3x3 (
				Handle,
				name,
				v1, v2, v3,
				v4, v5, v6,
				v7, v8, v9
			);
		}

		public void SetUniformValue_4x4 (string name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9, float v10, float v11, float v12, float v13, float v14, float v15, float v16)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_float4x4 (
				Handle,
				name,
				v1,  v2,  v3,  v4,
				v5,  v6,  v7,  v8,
				v9,  v10, v11, v12,
				v13, v14, v15, v16
			);
		}

		public void SetUniformValue (string name, SKMatrix matrix)
		{
			SkiaApi.sk_runtime_effect_builder_set_uniform_matrix (Handle, name, &matrix);
		}

		public void SetChildValueNull (string name)
		{
			SkiaApi.sk_runtime_effect_builder_set_child_nullptr (Handle, name);
		}

		public void SetChildValue (string name, SKShader shader)
		{
			SkiaApi.sk_runtime_effect_builder_set_child_shader (Handle, name, shader.Handle);
		}

		public void SetChildValue (string name, SKColorFilter colorFilter)
		{
			SkiaApi.sk_runtime_effect_builder_set_child_color_filter (Handle, name, colorFilter.Handle);
		}

		public void SetChildValue (string name, SKBlender blender)
		{
			SkiaApi.sk_runtime_effect_builder_set_child_blender (Handle, name, blender.Handle);
		}
	}

	public unsafe class SKRuntimeShaderBuilder : SKRuntimeEffectBuilder
	{

		internal SKRuntimeShaderBuilder (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKRuntimeShaderBuilder (SKRuntimeEffect effect)
		: base(SkiaApi.sk_runtime_shader_builder_new (effect.Handle), true)
		{
		}

		internal static SKRuntimeShaderBuilder GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKRuntimeShaderBuilder (h, o));

		internal static SKRuntimeShaderBuilder GetObject (IntPtr handle, bool owns) =>
			GetOrAddObject (handle, owns, (h, o) => new SKRuntimeShaderBuilder (h, o));

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			SkiaApi.sk_runtime_shader_builder_delete (Handle);

		public SKShader ToShader () =>
			SKShader.GetObject (SkiaApi.sk_runtime_shader_builder_make_shader (Handle));

		public SKShader ToShader (SKMatrix matrix) =>
			SKShader.GetObject (SkiaApi.sk_runtime_shader_builder_make_shader_with_matrix (Handle, &matrix));

		public SKImage ToImage (GRRecordingContext recordingContext, SKMatrix matrix, SKImageInfo resultInfo, bool mipmapped)
		{
			if (recordingContext == null)
				throw new ArgumentNullException (nameof (recordingContext));

			var nInfo = SKImageInfoNative.FromManaged (ref resultInfo);
			return SKImage.GetObject (SkiaApi.sk_runtime_shader_builder_make_image (Handle, recordingContext.Handle, &matrix, &nInfo, mipmapped));
		}
	}

	public unsafe class SKRuntimeBlenderBuilder : SKRuntimeEffectBuilder
	{
		internal SKRuntimeBlenderBuilder (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKRuntimeBlenderBuilder (SKRuntimeEffect effect)
		: base (SkiaApi.sk_runtime_blender_builder_new (effect.Handle), true)
		{
		}

		internal static SKRuntimeBlenderBuilder GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKRuntimeBlenderBuilder (h, o));

		internal static SKRuntimeBlenderBuilder GetObject (IntPtr handle, bool owns) =>
			GetOrAddObject (handle, owns, (h, o) => new SKRuntimeBlenderBuilder (h, o));

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		protected override void DisposeNative () =>
			SkiaApi.sk_runtime_blender_builder_delete (Handle);

		public SKBlender ToBlender () =>
			SKBlender.GetObject (SkiaApi.sk_runtime_blender_builder_make_blender (Handle));
	}
}
