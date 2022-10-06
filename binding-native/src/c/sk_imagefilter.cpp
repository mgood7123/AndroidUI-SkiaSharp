/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkColorFilter.h"
#include "include/core/SkImageFilter.h"
#include "include/core/SkPicture.h"
#include "include/core/SkRegion.h"
#include "include/effects/SkImageFilters.h"
#include "include/c/sk_imagefilter.h"

#include "src/c/sk_types_priv.h"


// sk_imagefilter_t

void sk_imagefilter_unref(sk_imagefilter_t* cfilter) {
    SkSafeUnref(AsImageFilter(cfilter));
}

sk_imagefilter_t* sk_imagefilter_new_alpha_threshold(const sk_region_t* region, float innerThreshold, float outerThreshold, sk_imagefilter_t* input) {
    return ToImageFilter(SkImageFilters::AlphaThreshold(*AsRegion(region), innerThreshold, outerThreshold, sk_ref_sp(AsImageFilter(input))).release());
}

sk_imagefilter_t* sk_imagefilter_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePMColor, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Arithmetic(k1, k2, k3, k4, enforcePMColor, sk_ref_sp(AsImageFilter(background)), sk_ref_sp(AsImageFilter(foreground)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_blend(sk_blendmode_t blend, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Blend((SkBlendMode)blend, sk_ref_sp(AsImageFilter(background)), sk_ref_sp(AsImageFilter(foreground)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_blender(const sk_blender_t* blender, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Blend(sk_ref_sp(AsBlender(blender)), sk_ref_sp(AsImageFilter(background)), sk_ref_sp(AsImageFilter(foreground)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_blur(float sigmaX, float sigmaY, sk_shader_tilemode_t tileMode, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Blur(sigmaX, sigmaY, (SkTileMode)tileMode, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_color_filter(sk_colorfilter_t* cf, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::ColorFilter(sk_ref_sp(AsColorFilter(cf)), sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_compose(sk_imagefilter_t* outer, sk_imagefilter_t* inner) {
    return ToImageFilter(SkImageFilters::Compose(sk_ref_sp(AsImageFilter(outer)), sk_ref_sp(AsImageFilter(inner))).release());
}

sk_imagefilter_t* sk_imagefilter_new_displacement_map_effect(sk_color_channel_t xChannelSelector, sk_color_channel_t yChannelSelector, float scale, sk_imagefilter_t* displacement, sk_imagefilter_t* color, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::DisplacementMap((SkColorChannel)xChannelSelector, (SkColorChannel)yChannelSelector, scale, sk_ref_sp(AsImageFilter(displacement)), sk_ref_sp(AsImageFilter(color)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_drop_shadow(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::DropShadow(dx, dy, sigmaX, sigmaY, color, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_drop_shadow_only(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::DropShadow(dx, dy, sigmaX, sigmaY, color, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_image_source(sk_image_t* image, const sk_rect_t* srcRect, const sk_rect_t* dstRect, const sk_sampling_options_t* sampling_options) {
    return ToImageFilter(SkImageFilters::Image(sk_ref_sp(AsImage(image)), *AsRect(srcRect), *AsRect(dstRect), *AsSamplingOptions(sampling_options)).release());
}

sk_imagefilter_t* sk_imagefilter_new_image_source_sampling_options(sk_image_t* image, const sk_sampling_options_t* sampling_options) {
    return ToImageFilter(SkImageFilters::Image(sk_ref_sp(AsImage(image)), *AsSamplingOptions(sampling_options)).release());
}

sk_imagefilter_t* sk_imagefilter_new_magnifier(const sk_rect_t* src, float inset, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Magnifier(*AsRect(src), inset, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_matrix_convolution(const sk_isize_t* kernelSize, const float kernel[], float gain, float bias, const sk_ipoint_t* kernelOffset, sk_shader_tilemode_t ctileMode, bool convolveAlpha, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::MatrixConvolution(*AsISize(kernelSize), kernel, gain, bias, *AsIPoint(kernelOffset), (SkTileMode)ctileMode, convolveAlpha, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_matrix(const sk_matrix_t* cmatrix, const sk_sampling_options_t* sampling_options, sk_imagefilter_t* input) {
    return ToImageFilter(SkImageFilters::MatrixTransform(AsMatrix(cmatrix), *AsSamplingOptions(sampling_options), sk_ref_sp(AsImageFilter(input))).release());
}

sk_imagefilter_t* sk_imagefilter_new_merge(sk_imagefilter_t* cfilters[], int count, const sk_rect_t* cropRect) {
    sk_sp<SkImageFilter> filters[count];
    for (int i = 0; i < count; i++) {
        filters[i] = sk_ref_sp(AsImageFilter(cfilters[i]));
    }
    return ToImageFilter(SkImageFilters::Merge(filters, count, AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_offset(float dx, float dy, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Offset(dx, dy, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_paint(const sk_paint_t* paint, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Paint(*AsPaint(paint), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_picture(sk_picture_t* picture) {
    return ToImageFilter(SkImageFilters::Picture(sk_ref_sp(AsPicture(picture))).release());
}

sk_imagefilter_t* sk_imagefilter_new_picture_with_croprect(sk_picture_t* picture, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Picture(sk_ref_sp(AsPicture(picture)), *AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_runtime_shader(const sk_runtime_shader_builder_t* builder, const char* child_shader_name, sk_imagefilter_t* input) {
#ifdef SK_ENABLE_SKSL
    return ToImageFilter(SkImageFilters::RuntimeShader(*AsRuntimeShaderBuilder(builder), child_shader_name, sk_ref_sp(AsImageFilter(input))).release());
#else
    return nullptr;
#endif
}

sk_imagefilter_t* sk_imagefilter_new_runtime_shader_with_multi(const sk_runtime_shader_builder_t* builder, const char* child_shader_names[], sk_imagefilter_t* inputs[], int count) {
#ifdef SK_ENABLE_SKSL

    std::string_view child_names[count];
    for (int i = 0; i < count; i++) {
        child_names[i] = child_shader_names[i];
    }

    sk_sp<SkImageFilter> filters[count];
    for (int i = 0; i < count; i++) {
        filters[i] = sk_ref_sp(AsImageFilter(inputs[i]));
    }

    return ToImageFilter(SkImageFilters::RuntimeShader(*AsRuntimeShaderBuilder(builder), child_names, filters, count).release());
#else
    return nullptr;
#endif
}

sk_imagefilter_t* sk_imagefilter_new_shader_with_dither(const sk_shader_t* shader, bool dither, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Shader(sk_ref_sp(AsShader(shader)), dither ? SkImageFilters::Dither::kYes : SkImageFilters::Dither::kNo, AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_tile(const sk_rect_t* src, const sk_rect_t* dst, sk_imagefilter_t* input) {
    return ToImageFilter(SkImageFilters::Tile(*AsRect(src), *AsRect(dst), sk_ref_sp(AsImageFilter(input))).release());
}

sk_imagefilter_t* sk_imagefilter_new_dilate(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Dilate(radiusX, radiusY, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_erode(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::Erode(radiusX, radiusY, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}


sk_imagefilter_t* sk_imagefilter_new_distant_lit_diffuse(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::DistantLitDiffuse(*AsPoint3(direction), lightColor, surfaceScale, kd, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_point_lit_diffuse(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::PointLitDiffuse(*AsPoint3(location), lightColor, surfaceScale, kd, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_spot_lit_diffuse(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::SpotLitDiffuse(*AsPoint3(location), *AsPoint3(target), specularExponent, cutoffAngle, lightColor, surfaceScale, kd, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_distant_lit_specular(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::DistantLitSpecular(*AsPoint3(direction), lightColor, surfaceScale, ks, shininess, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_point_lit_specular(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::PointLitSpecular(*AsPoint3(location), lightColor, surfaceScale, ks, shininess, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_imagefilter_t* sk_imagefilter_new_spot_lit_specular(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect) {
    return ToImageFilter(SkImageFilters::SpotLitSpecular(*AsPoint3(location), *AsPoint3(target), specularExponent, cutoffAngle, lightColor, surfaceScale, ks, shininess, sk_ref_sp(AsImageFilter(input)), AsRect(cropRect)).release());
}

sk_data_t* sk_imagefilter_serialize(const sk_imagefilter_t* imagefilter) {
    return ToData(AsImageFilter(imagefilter)->serialize().release());
}

sk_imagefilter_t* sk_imagefilter_deserialize(const sk_data_t* data) {
    const SkData* skdata = AsData(data);
    return ToImageFilter(
        sk_sp<SkImageFilter>(
            static_cast<SkImageFilter*>(
                SkFlattenable::Deserialize(
                    SkFlattenable::kSkImageFilter_Type,
                    skdata->data(),
                    skdata->size()
                ).release()
            )
        ).release()
    );
}
