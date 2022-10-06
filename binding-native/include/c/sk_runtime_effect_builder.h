/*
 * Copyright 2020 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_runtime_effect_builder_DEFINED
#define sk_runtime_effect_builder_DEFINED

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

// sk_runtime_effect_builder_t

SK_C_API sk_runtime_shader_builder_t* sk_runtime_shader_builder_new(sk_runtimeeffect_t* effect);
SK_C_API void sk_runtime_shader_builder_delete(sk_runtime_shader_builder_t* builder);
SK_C_API sk_shader_t* sk_runtime_shader_builder_make_shader(sk_runtime_shader_builder_t* builder);
SK_C_API sk_shader_t* sk_runtime_shader_builder_make_shader_with_matrix(sk_runtime_shader_builder_t* builder, const sk_matrix_t* value);
SK_C_API sk_image_t* sk_runtime_shader_builder_make_image(sk_runtime_shader_builder_t* builder, gr_recording_context_t* context, const sk_matrix_t* value, sk_imageinfo_t* resultInfo, bool mipmapped);

SK_C_API sk_runtime_blender_builder_t* sk_runtime_blender_builder_new(sk_runtimeeffect_t* effect);
SK_C_API void sk_runtime_blender_builder_delete(sk_runtime_blender_builder_t* builder);
SK_C_API sk_blender_t* sk_runtime_blender_builder_make_blender(sk_runtime_blender_builder_t* builder);

SK_C_API const sk_runtimeeffect_t* sk_runtime_effect_builder_get_effect(sk_runtime_effect_builder_t* builder);

SK_C_API const sk_runtimeeffect_uniform_t* sk_runtime_effect_builder_get_uniform_by_name(sk_runtime_effect_builder_t* builder, const char* name);
SK_C_API const sk_runtimeeffect_child_t* sk_runtime_effect_builder_get_child_by_name(sk_runtime_effect_builder_t* builder, const char* name);

SK_C_API void sk_runtime_effect_builder_set_uniform_int(sk_runtime_effect_builder_t* builder, const char* name, int value);
SK_C_API void sk_runtime_effect_builder_set_uniform_int2(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2);
SK_C_API void sk_runtime_effect_builder_set_uniform_int3(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2, int v3);
SK_C_API void sk_runtime_effect_builder_set_uniform_int4(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2, int v3, int v4);

SK_C_API void sk_runtime_effect_builder_set_uniform_float(sk_runtime_effect_builder_t* builder, const char* name, float value);
SK_C_API void sk_runtime_effect_builder_set_uniform_float2(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2);
SK_C_API void sk_runtime_effect_builder_set_uniform_float3(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3);
SK_C_API void sk_runtime_effect_builder_set_uniform_float4(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4);

SK_C_API void sk_runtime_effect_builder_set_uniform_float2x2(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4);
SK_C_API void sk_runtime_effect_builder_set_uniform_float3x3(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9);
SK_C_API void sk_runtime_effect_builder_set_uniform_float4x4(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9, float v10, float v11, float v12, float v13, float v14, float v15, float v16);

SK_C_API void sk_runtime_effect_builder_set_uniform_matrix(sk_runtime_effect_builder_t* builder, const char* name, const sk_matrix_t* value);

SK_C_API void sk_runtime_effect_builder_set_child_nullptr(sk_runtime_effect_builder_t* builder, const char* name);
SK_C_API void sk_runtime_effect_builder_set_child_shader(sk_runtime_effect_builder_t* builder, const char* name, const sk_shader_t* value);
SK_C_API void sk_runtime_effect_builder_set_child_color_filter(sk_runtime_effect_builder_t* builder, const char* name, const sk_colorfilter_t* value);
SK_C_API void sk_runtime_effect_builder_set_child_blender(sk_runtime_effect_builder_t* builder, const char* name, const sk_blender_t* value);

SK_C_PLUS_PLUS_END_GUARD

#endif
