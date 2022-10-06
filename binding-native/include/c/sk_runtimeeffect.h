/*
 * Copyright 2020 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_runtimeeffect_DEFINED
#define sk_runtimeeffect_DEFINED

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

// sk_runtimeeffect_t

SK_C_API sk_runtimeeffect_t* sk_runtimeeffect_make_for_shader(sk_string_t* sksl, sk_string_t* error);
SK_C_API sk_runtimeeffect_t* sk_runtimeeffect_make_for_color_filter(sk_string_t* sksl, sk_string_t* error);
SK_C_API sk_runtimeeffect_t* sk_runtimeeffect_make_for_blender(sk_string_t* sksl, sk_string_t* error);
SK_C_API sk_runtimeeffect_child_type_t sk_runtimeeffect_get_type(const sk_runtimeeffect_t* effect);
SK_C_API void sk_runtimeeffect_unref(sk_runtimeeffect_t* effect);
SK_C_API sk_shader_t* sk_runtimeeffect_make_shader              (sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_shader_t** children, size_t childCount, const sk_matrix_t* localMatrix);
SK_C_API sk_colorfilter_t* sk_runtimeeffect_make_color_filter   (sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_colorfilter_t** children, size_t childCount);
SK_C_API sk_blender_t* sk_runtimeeffect_make_blender            (sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_blender_t** children, size_t childCount);
SK_C_API size_t sk_runtimeeffect_get_uniform_size(const sk_runtimeeffect_t* effect);
SK_C_API size_t sk_runtimeeffect_get_uniform_count(const sk_runtimeeffect_t* effect);
SK_C_API const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_name(const sk_runtimeeffect_t* effect, const char* name);
SK_C_API const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_index(const sk_runtimeeffect_t* effect, int index);
SK_C_API const sk_runtimeeffect_child_t* sk_runtimeeffect_get_child_from_name(const sk_runtimeeffect_t* effect, const char* name);
SK_C_API const sk_runtimeeffect_child_t* sk_runtimeeffect_get_child_from_index(const sk_runtimeeffect_t* effect, int index);
SK_C_API size_t sk_runtimeeffect_get_children_count(const sk_runtimeeffect_t* effect);

// sk_runtimeeffect_uniform_t

SK_C_API void sk_runtimeeffect_uniform_get_name(const sk_runtimeeffect_uniform_t* child, sk_string_t* name);
SK_C_API int sk_runtimeeffect_uniform_get_offset(const sk_runtimeeffect_uniform_t* child);
SK_C_API sk_runtimeeffect_uniform_type_t sk_runtimeeffect_uniform_get_type(const sk_runtimeeffect_uniform_t* child);
SK_C_API size_t sk_runtimeeffect_uniform_get_count(const sk_runtimeeffect_uniform_t* child);
SK_C_API size_t sk_runtimeeffect_uniform_get_size_in_bytes(const sk_runtimeeffect_uniform_t* child);

// sk_runtimeeffect_child_t

SK_C_API void sk_runtimeeffect_child_get_name(const sk_runtimeeffect_child_t* child, sk_string_t* name);
SK_C_API sk_runtimeeffect_child_type_t sk_runtimeeffect_child_get_type(const sk_runtimeeffect_child_t* child);
SK_C_API int sk_runtimeeffect_child_get_index(const sk_runtimeeffect_child_t* child);

SK_C_PLUS_PLUS_END_GUARD

#endif
