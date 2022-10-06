/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/effects/SkBlenders.h"

#include "include/c/sk_blender.h"

#include "src/c/sk_types_priv.h"

void sk_blender_unref(sk_blender_t* filter) {
    SkSafeUnref(AsBlender(filter));
}

sk_blender_t* sk_blender_new_mode(sk_blendmode_t cmode) {
    return ToBlender(SkBlender::Mode((SkBlendMode)cmode).release());
}

sk_blender_t* sk_blender_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePremul) {
    return ToBlender(SkBlenders::Arithmetic(k1, k2, k3, k4, enforcePremul).release());
}

sk_data_t* sk_blender_serialize(const sk_blender_t* blender) {
    return ToData(AsBlender(blender)->serialize().release());
}

sk_blender_t* sk_blender_deserialize(const sk_data_t* data) {
    const SkData* skdata = AsData(data);
    return ToBlender(
        sk_sp<SkBlender>(
            static_cast<SkBlender*>(
                SkFlattenable::Deserialize(
                    SkFlattenable::kSkBlender_Type,
                    skdata->data(),
                    skdata->size()
                ).release()
                )
            ).release()
    );
}
