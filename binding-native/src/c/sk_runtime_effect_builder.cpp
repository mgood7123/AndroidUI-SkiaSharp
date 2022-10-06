/*
 * Copyright 2020 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkTypes.h"
#include "include/core/SkColorFilter.h"
#include "include/core/SkShader.h"
#include "include/effects/SkRuntimeEffect.h"

#include "include/c/sk_types.h"
#include "include/c/sk_runtime_effect_builder.h"

#include "src/c/sk_types_priv.h"

sk_runtime_shader_builder_t* sk_runtime_shader_builder_new(sk_runtimeeffect_t* effect) {
    return ToRuntimeShaderBuilder(new SkRuntimeShaderBuilder(sk_ref_sp<SkRuntimeEffect>(AsRuntimeEffect(effect))));
}

void sk_runtime_shader_builder_delete(sk_runtime_shader_builder_t* builder) {
    delete AsRuntimeShaderBuilder(builder);
}

sk_shader_t* sk_runtime_shader_builder_make_shader(sk_runtime_shader_builder_t* builder) {
    return ToShader(AsRuntimeShaderBuilder(builder)->makeShader().release());
}

sk_shader_t* sk_runtime_shader_builder_make_shader_with_matrix(sk_runtime_shader_builder_t* builder, const sk_matrix_t* value) {
    const SkMatrix localMatrix = AsMatrix(value);
    return ToShader(AsRuntimeShaderBuilder(builder)->makeShader(&localMatrix).release());
}

sk_image_t* sk_runtime_shader_builder_make_image(sk_runtime_shader_builder_t* builder, gr_recording_context_t* context, const sk_matrix_t* value, sk_imageinfo_t* resultInfo, bool mipmapped) {
    const SkMatrix localMatrix = AsMatrix(value);
    return ToImage(AsRuntimeShaderBuilder(builder)->makeImage(AsGrRecordingContext(context), &localMatrix, AsImageInfo(resultInfo), mipmapped).release());
}

sk_runtime_blender_builder_t* sk_runtime_blender_builder_new(sk_runtimeeffect_t* effect) {
    return ToRuntimeBlenderBuilder(new SkRuntimeBlendBuilder(sk_ref_sp<SkRuntimeEffect>(AsRuntimeEffect(effect))));
}

void sk_runtime_blender_builder_delete(sk_runtime_blender_builder_t* builder) {
    delete AsRuntimeBlenderBuilder(builder);
}

sk_blender_t* sk_runtime_blender_builder_make_blender(sk_runtime_blender_builder_t* builder) {
    return ToBlender(AsRuntimeBlenderBuilder(builder)->makeBlender().release());
}

const sk_runtimeeffect_t* sk_runtime_effect_builder_get_effect(sk_runtime_effect_builder_t* builder) {
    return ToRuntimeEffect(AsRuntimeEffectBuilder(builder)->effect());
}

const sk_runtimeeffect_uniform_t* sk_runtime_effect_builder_get_uniform_by_name(sk_runtime_effect_builder_t* builder, const char* name) {
    return ToRuntimeEffectUniform(AsRuntimeEffectBuilder(builder)->uniform(name).fVar);
}

const sk_runtimeeffect_child_t* sk_runtime_effect_builder_get_child_by_name(sk_runtime_effect_builder_t* builder, const char* name) {
    return ToRuntimeEffectChild(AsRuntimeEffectBuilder(builder)->child(name).fChild);
}

void sk_runtime_effect_builder_set_uniform_int(sk_runtime_effect_builder_t* builder, const char* name, int value) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = value;
}
void sk_runtime_effect_builder_set_uniform_int2(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<int, 2> { v1, v2 };
}
void sk_runtime_effect_builder_set_uniform_int3(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2, int v3) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<int, 3> { v1, v2, v3 };
}
void sk_runtime_effect_builder_set_uniform_int4(sk_runtime_effect_builder_t* builder, const char* name, int v1, int v2, int v3, int v4) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<int, 4> { v1, v2, v3, v4 };
}


void sk_runtime_effect_builder_set_uniform_float(sk_runtime_effect_builder_t* builder, const char* name, float value) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = value;
}
void sk_runtime_effect_builder_set_uniform_float2(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 2> { v1, v2 };
}
void sk_runtime_effect_builder_set_uniform_float3(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 3> { v1, v2, v3 };
}
void sk_runtime_effect_builder_set_uniform_float4(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 4> { v1, v2, v3, v4 };
}


void sk_runtime_effect_builder_set_uniform_float2x2(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 4> {
        v1, v2,
        v3, v4
    };
}
void sk_runtime_effect_builder_set_uniform_float3x3(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 9> { 
        v1, v2, v3,
        v4, v5, v6,
        v7, v8, v9
    };
}
void sk_runtime_effect_builder_set_uniform_float4x4(sk_runtime_effect_builder_t* builder, const char* name, float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9, float v10, float v11, float v12, float v13, float v14, float v15, float v16) {
    AsRuntimeEffectBuilder(builder)->uniform(name) = std::array<float, 16> {
        v1,  v2,  v3,  v4,
        v5,  v6,  v7,  v8,
        v9,  v10, v11, v12,
        v13, v14, v15, v16
    };
}


void sk_runtime_effect_builder_set_uniform_matrix(sk_runtime_effect_builder_t* builder, const char* name, const sk_matrix_t* value) {

    // SkMatrix AsMatrix(const sk_matrix_t* matrix)

    AsRuntimeEffectBuilder(builder)->uniform(name) = AsMatrix(value);
}


// should we use
//   sk_sp<T>(const T*)
// or
//   sk_ref_sp<T>(const T*)
// ?

void sk_runtime_effect_builder_set_child_nullptr(sk_runtime_effect_builder_t* builder, const char* name) {
    AsRuntimeEffectBuilder(builder)->child(name) = nullptr;
}
void sk_runtime_effect_builder_set_child_shader(sk_runtime_effect_builder_t* builder, const char* name, const sk_shader_t* value) {
    AsRuntimeEffectBuilder(builder)->child(name) = sk_ref_sp<SkShader>(AsShader(value));
}
void sk_runtime_effect_builder_set_child_color_filter(sk_runtime_effect_builder_t* builder, const char* name, const sk_colorfilter_t* value) {
    AsRuntimeEffectBuilder(builder)->child(name) = sk_ref_sp<SkColorFilter>(AsColorFilter(value));
}
void sk_runtime_effect_builder_set_child_blender(sk_runtime_effect_builder_t* builder, const char* name, const sk_blender_t* value) {
    AsRuntimeEffectBuilder(builder)->child(name) = sk_ref_sp<SkBlender>(AsBlender(value));
}
