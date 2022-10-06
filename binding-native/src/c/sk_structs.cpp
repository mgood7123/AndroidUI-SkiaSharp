/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "src/c/sk_types_priv.h"

#include "include/codec/SkCodec.h"
#include "include/core/SkCanvas.h"
#include "include/core/SkDocument.h"
#include "include/core/SkImageInfo.h"
#include "include/core/SkPaint.h"
#include "include/core/SkPoint.h"
#include "include/core/SkPoint3.h"
#include "include/core/SkRect.h"
#include "include/core/SkRSXform.h"
#include "include/core/SkSamplingOptions.h"
#include "include/core/SkSize.h"
#include "include/core/SkSurfaceProps.h"
#include "include/core/SkTextBlob.h"
#include "include/core/SkTime.h"
#include "include/effects/SkHighContrastFilter.h"
#include "src/core/SkMask.h"

#if SK_SUPPORT_GPU
#include "include/gpu/GrTypes.h"
#include "include/gpu/GrContextOptions.h"
#include "include/gpu/gl/GrGLTypes.h"

#if SK_VULKAN
#include "include/gpu/vk/GrVkTypes.h"
#endif

#endif

#if __cplusplus >= 199711L

#define STRINGIFY(x) #x
#define TOSTRING(x) STRINGIFY(x)
#define ASSERT_MSG(SK, C) "ABI changed, you must update the C structure for " TOSTRING(#C) " in sk_types.h based on the current C++ structure of " TOSTRING(#SK) "."

// custom mappings:
//  - sk_matrix_t
//  - sk_document_pdf_metadata_t

#define CHECK(SK, C) static_assert (sizeof (SK) == sizeof (C), ASSERT_MSG(SK, C))

CHECK(SkIPoint, sk_ipoint_t);
CHECK(SkPoint, sk_point_t);
CHECK(SkIRect, sk_irect_t);
CHECK(SkRect, sk_rect_t);
CHECK(SkISize, sk_isize_t);
CHECK(SkSize, sk_size_t);
CHECK(SkPoint3, sk_point3_t);
CHECK(SkImageInfo, sk_imageinfo_t);
CHECK(SkFontMetrics, sk_fontmetrics_t);
CHECK(SkCodec::Options, sk_codec_options_t);
CHECK(SkMask, sk_mask_t);
CHECK(SkCanvas::Lattice, sk_lattice_t);
CHECK(SkTime::DateTime, sk_time_datetime_t);
CHECK(SkCodec::FrameInfo, sk_codec_frameinfo_t);
CHECK(skcms_TransferFunction, sk_colorspace_transfer_fn_t);
CHECK(SkColorSpacePrimaries, sk_colorspace_primaries_t);
CHECK(SkHighContrastConfig, sk_highcontrastconfig_t);
CHECK(SkPngEncoder::Options, sk_pngencoder_options_t);
CHECK(SkJpegEncoder::Options, sk_jpegencoder_options_t);
CHECK(SkWebpEncoder::Options, sk_webpencoder_options_t);
CHECK(SkTextBlobBuilder::RunBuffer, sk_textblob_builder_runbuffer_t);
CHECK(SkRSXform, sk_rsxform_t);
CHECK(SkColor4f, sk_color4f_t);
CHECK(skcms_Matrix3x3, sk_colorspace_xyz_t);
CHECK(SkCubicResampler, sk_cubic_resampler_t);
CHECK(SkSamplingOptions, sk_sampling_options_t);
CHECK(SkAndroidCodec::AndroidOptions, sk_android_codec_options_t);

#if SK_SUPPORT_GPU
static_assert (sizeof (gr_gl_framebufferinfo_t) == sizeof (GrGLFramebufferInfo), ASSERT_MSG(GrGLFramebufferInfo, gr_gl_framebufferinfo_t));
static_assert (sizeof (gr_gl_textureinfo_t) == sizeof (GrGLTextureInfo), ASSERT_MSG(GrGLTextureInfo, gr_gl_textureinfo_t));

#if SK_VULKAN
static_assert (sizeof (gr_vk_alloc_t) == sizeof (GrVkAlloc), ASSERT_MSG(GrVkAlloc, gr_vk_alloc_t));
static_assert (sizeof (gr_vk_imageinfo_t) == sizeof (GrVkImageInfo), ASSERT_MSG(GrVkImageInfo, gr_vk_imageinfo_t));
static_assert (sizeof (gr_vk_ycbcrconversioninfo_t) == sizeof (GrVkYcbcrConversionInfo), ASSERT_MSG(GrVkYcbcrConversionInfo, gr_vk_ycbcrconversioninfo_t));
#endif
#endif

#endif
