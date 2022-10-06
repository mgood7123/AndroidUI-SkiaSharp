/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkShader.h"
#include "include/effects/SkBlurMaskFilter.h"
#include "include/effects/SkTableMaskFilter.h"
#include "include/effects/SkShaderMaskFilter.h"

#include "include/c/sk_maskfilter.h"

#include "src/c/sk_types_priv.h"

sk_maskfilter_t* sk_maskfilter_new_table(const uint8_t table[256]) {
    return ToMaskFilter(SkTableMaskFilter::Create(table));
}

sk_maskfilter_t* sk_maskfilter_new_gamma(float gamma) {
    return ToMaskFilter(SkTableMaskFilter::CreateGamma(gamma));
}

sk_maskfilter_t* sk_maskfilter_new_clip(uint8_t min, uint8_t max) {
    return ToMaskFilter(SkTableMaskFilter::CreateClip(min, max));
}

void sk_maskfilter_ref(sk_maskfilter_t* cfilter) {
    SkSafeRef(AsMaskFilter(cfilter));
}

void sk_maskfilter_unref(sk_maskfilter_t* cfilter) {
    SkSafeUnref(AsMaskFilter(cfilter));
}

sk_maskfilter_t* sk_maskfilter_new_blur(sk_blurstyle_t cstyle, float sigma) {
    return ToMaskFilter(SkMaskFilter::MakeBlur((SkBlurStyle)cstyle, sigma).release());
}

sk_maskfilter_t* sk_maskfilter_new_blur_with_flags(sk_blurstyle_t cstyle, float sigma, bool respectCTM) {
    return ToMaskFilter(SkMaskFilter::MakeBlur((SkBlurStyle)cstyle, sigma, respectCTM).release());
}

sk_maskfilter_t* sk_maskfilter_new_shader(sk_shader_t* cshader) {
    return ToMaskFilter(SkShaderMaskFilter::Make(sk_ref_sp(AsShader(cshader))).release());
}

sk_data_t* sk_maskfilter_serialize(const sk_maskfilter_t* maskfilter) {
    return ToData(AsMaskFilter(maskfilter)->serialize().release());
}

sk_maskfilter_t* sk_maskfilter_deserialize(const sk_data_t* data) {
    const SkData* skdata = AsData(data);
    return ToMaskFilter(
        sk_sp<SkMaskFilter>(
            static_cast<SkMaskFilter*>(
                SkFlattenable::Deserialize(
                    SkFlattenable::kSkMaskFilter_Type,
                    skdata->data(),
                    skdata->size()
                ).release()
            )
        ).release()
    );
}
