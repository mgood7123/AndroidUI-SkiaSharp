/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_imagefilter_DEFINED
#define sk_imagefilter_DEFINED

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

// sk_imagefilter_t

SK_C_API void sk_imagefilter_unref(sk_imagefilter_t*);

SK_C_API sk_imagefilter_t* sk_imagefilter_new_alpha_threshold(const sk_region_t* region, float innerThreshold, float outerThreshold, sk_imagefilter_t* input);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePMColor, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_blend(sk_blendmode_t blend, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_blender(const sk_blender_t* blender, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_blur(float sigmaX, float sigmaY, sk_shader_tilemode_t tileMode, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_color_filter(sk_colorfilter_t* cf, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_compose(sk_imagefilter_t* outer, sk_imagefilter_t* inner);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_displacement_map_effect(sk_color_channel_t xChannelSelector, sk_color_channel_t yChannelSelector, float scale, sk_imagefilter_t* displacement, sk_imagefilter_t* color, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_drop_shadow(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_drop_shadow_only(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_image_source(sk_image_t* image, const sk_rect_t* srcRect, const sk_rect_t* dstRect, const sk_sampling_options_t* sampling_options);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_image_source_sampling_options(sk_image_t* image, const sk_sampling_options_t* sampling_options);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_magnifier(const sk_rect_t* src, float inset, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_matrix_convolution(const sk_isize_t* kernelSize, const float kernel[], float gain, float bias, const sk_ipoint_t* kernelOffset, sk_shader_tilemode_t tileMode, bool convolveAlpha, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_matrix(const sk_matrix_t* matrix, const sk_sampling_options_t* sampling_options, sk_imagefilter_t* input);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_merge(sk_imagefilter_t* filters[], int count, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_offset(float dx, float dy, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_paint(const sk_paint_t* paint, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_picture(sk_picture_t* picture);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_picture_with_croprect(sk_picture_t* picture, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_runtime_shader(const sk_runtime_shader_builder_t* builder, const char* child_shader_name, sk_imagefilter_t* input);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_runtime_shader_with_multi(const sk_runtime_shader_builder_t* builder, const char* child_shader_names[], sk_imagefilter_t* inputs[], int count);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_shader_with_dither(const sk_shader_t* shader, bool dither, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_tile(const sk_rect_t* src, const sk_rect_t* dst, sk_imagefilter_t* input);

SK_C_API sk_imagefilter_t* sk_imagefilter_new_dilate(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_erode(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_rect_t* cropRect);

SK_C_API sk_imagefilter_t* sk_imagefilter_new_distant_lit_diffuse(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_point_lit_diffuse(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_spot_lit_diffuse(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_distant_lit_specular(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_point_lit_specular(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect);
SK_C_API sk_imagefilter_t* sk_imagefilter_new_spot_lit_specular(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_rect_t* cropRect);

SK_C_API sk_data_t* sk_imagefilter_serialize(const sk_imagefilter_t* imagefilter);
SK_C_API sk_imagefilter_t* sk_imagefilter_deserialize(const sk_data_t* data);


SK_C_PLUS_PLUS_END_GUARD

#endif
