/*
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/xamarin/SkPaintExtraInfo.h"

#include "include/xamarin/sk_paint_extra_info.h"
#include "src/c/sk_types_priv.h"


static inline const SkPaintExtraInfo* AsPaintExtraInfo(const sk_paint_extra_info_t* c) {
    return reinterpret_cast<const SkPaintExtraInfo*>(c);
}
static inline SkPaintExtraInfo* AsPaintExtraInfo(sk_paint_extra_info_t* c) {
    return reinterpret_cast<SkPaintExtraInfo*>(c);
}
static inline sk_paint_extra_info_t* ToPaintExtraInfo(SkPaintExtraInfo* c) {
    return reinterpret_cast<sk_paint_extra_info_t*>(c);
}


sk_paint_extra_info_t* sk_paint_extra_info_new(void) {
    return ToPaintExtraInfo(new SkPaintExtraInfo());
}

sk_paint_extra_info_t* sk_paint_extra_info_new_with_font(const sk_font_t* font) {
    if (font == nullptr) {
        return ToPaintExtraInfo(new SkPaintExtraInfo());
    }
    return ToPaintExtraInfo(new SkPaintExtraInfo(*AsFont(font)));
}

sk_paint_extra_info_t* sk_paint_extra_info_new_with_paint(const sk_paint_t* paint) {
    if (paint == nullptr) {
        return ToPaintExtraInfo(new SkPaintExtraInfo());
    }
    return ToPaintExtraInfo(new SkPaintExtraInfo(*AsPaint(paint)));
}

sk_paint_extra_info_t* sk_paint_extra_info_new_with_font_and_paint(const sk_font_t* font, const sk_paint_t* paint) {
    if (font == nullptr && paint == nullptr) {
        return ToPaintExtraInfo(new SkPaintExtraInfo());
    }
    if (font == nullptr && paint != nullptr) {
        return ToPaintExtraInfo(new SkPaintExtraInfo(*AsPaint(paint)));
    }
    if (font != nullptr && paint == nullptr) {
        return ToPaintExtraInfo(new SkPaintExtraInfo(*AsFont(font)));
    }
    return ToPaintExtraInfo(new SkPaintExtraInfo(*AsFont(font), *AsPaint(paint)));
}

void sk_paint_extra_info_delete(sk_paint_extra_info_t* paint) {
    delete AsPaintExtraInfo(paint);
}

sk_paint_extra_info_t* sk_paint_extra_info_clone(const sk_paint_extra_info_t* paint) {
    return ToPaintExtraInfo(new SkPaintExtraInfo(*AsPaintExtraInfo(paint)));
}

void sk_paint_extra_info_reset(sk_paint_extra_info_t* paint) {
    AsPaintExtraInfo(paint)->reset();
}

sk_font_t* sk_paint_extra_info_clone_font(const sk_paint_extra_info_t* paint) {
    return ToFont(AsPaintExtraInfo(paint)->cloneFont());
}

sk_paint_t* sk_paint_extra_info_clone_paint(const sk_paint_extra_info_t* paint) {
    return ToPaint(AsPaintExtraInfo(paint)->clonePaint());
}

sk_font_t* sk_paint_extra_info_get_font(sk_paint_extra_info_t* paint) {
    return ToFont(AsPaintExtraInfo(paint)->getFont());
}

sk_paint_t* sk_paint_extra_info_get_paint(sk_paint_extra_info_t* paint) {
    return ToPaint(AsPaintExtraInfo(paint)->getPaint());
}

void sk_paint_extra_info_set_text_align(sk_paint_extra_info_t* paint, sk_text_align_t align) {
    AsPaintExtraInfo(paint)->setTextAlign((SkTextUtils::Align)align);
}

sk_text_align_t sk_paint_extra_info_get_text_align(const sk_paint_extra_info_t* paint) {
    return (sk_text_align_t)AsPaintExtraInfo(paint)->getTextAlign();
}

void sk_paint_extra_info_set_text_encoding(sk_paint_extra_info_t* paint, sk_text_encoding_t encoding) {
    AsPaintExtraInfo(paint)->setTextEncoding((SkTextEncoding)encoding);
}

sk_text_encoding_t sk_paint_extra_info_get_text_encoding(const sk_paint_extra_info_t* paint) {
    return (sk_text_encoding_t)AsPaintExtraInfo(paint)->getTextEncoding();
}