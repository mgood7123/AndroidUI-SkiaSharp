/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkFont.h"
#include "include/core/SkTypeface.h"
#include "include/utils/SkTextUtils.h"

#include "include/c/sk_font.h"

#include "src/c/sk_types_priv.h"

#include "src/core/SkStrikeCache.h"
#include "src/core/SkStrikeSpec.h"
#include "src/utils/SkUTF.h"

// sk_font_t

sk_font_t* sk_font_new(void) {
    return ToFont(new SkFont());
}

sk_font_t* sk_font_new_with_values(sk_typeface_t* typeface, float size, float scaleX, float skewX) {
    return ToFont(new SkFont(sk_ref_sp(AsTypeface(typeface)), size, scaleX, skewX));
}

void sk_font_delete(sk_font_t* font) {
    delete AsFont(font);
}

bool sk_font_is_force_auto_hinting(const sk_font_t* font) {
    return AsFont(font)->isForceAutoHinting();
}

void sk_font_set_force_auto_hinting(sk_font_t* font, bool value) {
    AsFont(font)->setForceAutoHinting(value);
}

bool sk_font_is_embedded_bitmaps(const sk_font_t* font) {
    return AsFont(font)->isEmbeddedBitmaps();
}

void sk_font_set_embedded_bitmaps(sk_font_t* font, bool value) {
    AsFont(font)->setEmbeddedBitmaps(value);
}

bool sk_font_is_subpixel(const sk_font_t* font) {
    return AsFont(font)->isSubpixel();
}

void sk_font_set_subpixel(sk_font_t* font, bool value) {
    AsFont(font)->setSubpixel(value);
}

bool sk_font_is_linear_metrics(const sk_font_t* font) {
    return AsFont(font)->isLinearMetrics();
}

void sk_font_set_linear_metrics(sk_font_t* font, bool value) {
    AsFont(font)->setLinearMetrics(value);
}

bool sk_font_is_embolden(const sk_font_t* font) {
    return AsFont(font)->isEmbolden();
}

void sk_font_set_embolden(sk_font_t* font, bool value) {
    AsFont(font)->setEmbolden(value);
}

bool sk_font_is_baseline_snap(const sk_font_t* font) {
    return AsFont(font)->isBaselineSnap();
}

void sk_font_set_baseline_snap(sk_font_t* font, bool value) {
    AsFont(font)->setBaselineSnap(value);
}

sk_font_edging_t sk_font_get_edging(const sk_font_t* font) {
    return (sk_font_edging_t)AsFont(font)->getEdging();
}

void sk_font_set_edging(sk_font_t* font, sk_font_edging_t value) {
    AsFont(font)->setEdging((SkFont::Edging)value);
}

sk_font_hinting_t sk_font_get_hinting(const sk_font_t* font) {
    return (sk_font_hinting_t)AsFont(font)->getHinting();
}

void sk_font_set_hinting(sk_font_t* font, sk_font_hinting_t value) {
    AsFont(font)->setHinting((SkFontHinting)value);
}

sk_typeface_t* sk_font_get_typeface(const sk_font_t* font) {
    return ToTypeface(AsFont(font)->refTypeface().release());
}

void sk_font_set_typeface(sk_font_t* font, sk_typeface_t* value) {
    AsFont(font)->setTypeface(sk_ref_sp(AsTypeface(value)));
}

float sk_font_get_size(const sk_font_t* font) {
    return AsFont(font)->getSize();
}

void sk_font_set_size(sk_font_t* font, float value) {
    AsFont(font)->setSize(value);
}

float sk_font_get_scale_x(const sk_font_t* font) {
    return AsFont(font)->getScaleX();
}

void sk_font_set_scale_x(sk_font_t* font, float value) {
    AsFont(font)->setScaleX(value);
}

float sk_font_get_skew_x(const sk_font_t* font) {
    return AsFont(font)->getSkewX();
}

void sk_font_set_skew_x(sk_font_t* font, float value) {
    AsFont(font)->setSkewX(value);
}

int sk_font_text_to_glyphs(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, uint16_t glyphs[], int maxGlyphCount) {
    return AsFont(font)->textToGlyphs(text, byteLength, (SkTextEncoding)encoding, glyphs, maxGlyphCount);
}

uint16_t sk_font_unichar_to_glyph(const sk_font_t* font, int32_t uni) {
    return AsFont(font)->unicharToGlyph(uni);
}

void sk_font_unichars_to_glyphs(const sk_font_t* font, const int32_t uni[], int count, uint16_t glyphs[]) {
    AsFont(font)->unicharsToGlyphs(uni, count, glyphs);
}

float sk_font_measure_text(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, sk_rect_t* bounds, const sk_paint_t* paint) {
    return AsFont(font)->measureText(text, byteLength, (SkTextEncoding)encoding, AsRect(bounds), AsPaint(paint));
}

void sk_font_measure_text_no_return(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, sk_rect_t* bounds, const sk_paint_t* paint, float* measuredWidth) {
    *measuredWidth = sk_font_measure_text(font, text, byteLength, encoding, bounds, paint);
}

static inline SkUnichar SkUTFN_Next(SkTextEncoding enc, const void** ptr, const void* stop) {
    switch (enc) {
    case SkTextEncoding::kUTF8:
        return SkUTF::NextUTF8((const char**)ptr, (const char*)stop);
    case SkTextEncoding::kUTF16:
        return SkUTF::NextUTF16((const uint16_t**)ptr, (const uint16_t*)stop);
    case SkTextEncoding::kUTF32:
        return SkUTF::NextUTF32((const int32_t**)ptr, (const int32_t*)stop);
    default: SkDEBUGFAIL("unknown text encoding"); return -1;
    }
}

size_t sk_font_break_text(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, float maxWidth, float* measuredWidth, const sk_paint_t* paint) {
    const SkFont* skfont = AsFont(font);
    SkTextEncoding skencoding = (SkTextEncoding)encoding;
    const SkPaint* skpaint = AsPaint(paint);

    if (0 == byteLength || 0 >= maxWidth) {
        if (measuredWidth) {
            *measuredWidth = 0;
        }
        return 0;
    }
    if (0 == skfont->getSize()) {
        if (measuredWidth) {
            *measuredWidth = 0;
        }
        return byteLength;
    }

    SkASSERT(text != nullptr);

    auto [strikeSpec, scale] = SkStrikeSpec::MakeCanonicalized(*skfont, skpaint);

    SkBulkGlyphMetrics metrics{ strikeSpec };

    // adjust max instead of each glyph
    if (scale) {
        maxWidth /= scale;
    }

    SkScalar width = 0;

    const char* start = (const char*)text;
    const char* stop = start + byteLength;
    while (start < stop) {
        const char* curr = start;

        // read the glyph and move the pointer
        SkGlyphID glyphID;
        if (skencoding == SkTextEncoding::kGlyphID) {
            auto glyphs = (const uint16_t*)start;
            glyphID = *glyphs;
            glyphs++;
            start = (const char*)glyphs;
        }
        else {
            auto t = (const void*)start;
            auto unichar = SkUTFN_Next(skencoding, &t, stop);
            start = (const char*)t;
            glyphID = skfont->getTypefaceOrDefault()->unicharToGlyph(unichar);
        }

        auto glyph = metrics.glyph(glyphID);

        SkScalar x = glyph->advanceX();
        if ((width += x) > maxWidth) {
            width -= x;
            start = curr;
            break;
        }
    }

    if (measuredWidth) {
        if (scale) {
            width *= scale;
        }
        *measuredWidth = width;
    }

    // return the number of bytes measured
    return start - stop + byteLength;
}

void sk_font_get_widths_bounds(const sk_font_t* font, const uint16_t glyphs[], int count, float widths[], sk_rect_t bounds[], const sk_paint_t* paint) {
    AsFont(font)->getWidthsBounds(glyphs, count, widths, AsRect(bounds), AsPaint(paint));
}

void sk_font_get_pos(const sk_font_t* font, const uint16_t glyphs[], int count, sk_point_t pos[], sk_point_t* origin) {
    AsFont(font)->getPos(glyphs, count, AsPoint(pos), *AsPoint(origin));
}

void sk_font_get_xpos(const sk_font_t* font, const uint16_t glyphs[], int count, float xpos[], float origin) {
    AsFont(font)->getXPos(glyphs, count, xpos, origin);
}

bool sk_font_get_path(const sk_font_t* font, uint16_t glyph, sk_path_t* path) {
    return AsFont(font)->getPath(glyph, AsPath(path));
}

void sk_font_get_paths(const sk_font_t* font, uint16_t glyphs[], int count, const sk_glyph_path_proc glyphPathProc, void* context) {
    struct Pair {
        void* fContext;
        const sk_glyph_path_proc fProc;
    } pair = { context, glyphPathProc };

    auto proc = [](const SkPath* p, const SkMatrix& m, void* c) {
        Pair* pair = static_cast<Pair*>(c);
        if (pair->fProc) {
            sk_matrix_t cm = ToMatrix(m);
            pair->fProc(ToPath(p), &cm, pair->fContext);
        }
    };

    AsFont(font)->getPaths(glyphs, count, proc, &pair);
}

float sk_font_get_metrics(const sk_font_t* font, sk_fontmetrics_t* metrics) {
    return AsFont(font)->getMetrics(AsFontMetrics(metrics));
}

// sk_text_utils

void sk_text_utils_get_path(const void* text, size_t length, sk_text_encoding_t encoding, float x, float y, const sk_font_t* font, sk_path_t* path) {
    SkTextUtils::GetPath(text, length, (SkTextEncoding)encoding, x, y, *AsFont(font), AsPath(path));
}

// prototype
void GetPosPath(const void* text, size_t length, SkTextEncoding encoding, const SkPoint pos[], const SkFont& font, SkPath* path);

void GetPosPath(const void* text, size_t length, SkTextEncoding encoding,
    const SkPoint pos[], const SkFont& font, SkPath* path) {
    SkAutoToGlyphs ag(font, text, length, encoding);

    struct Rec {
        SkPath* fDst;
        const SkPoint* fPos;
    } rec = { path, pos };

    path->reset();
    font.getPaths(ag.glyphs(), ag.count(), [](const SkPath* src, const SkMatrix& mx, void* ctx) {
        Rec* rec = (Rec*)ctx;
        if (src) {
            SkMatrix m(mx);
            m.postTranslate(rec->fPos->fX, rec->fPos->fY);
            rec->fDst->addPath(*src, m);
        }
        rec->fPos += 1;
        }, &rec);
}

void sk_text_utils_get_pos_path(const void* text, size_t length, sk_text_encoding_t encoding, const sk_point_t pos[], const sk_font_t* font, sk_path_t* path) {
    GetPosPath(text, length, (SkTextEncoding)encoding, AsPoint(pos), *AsFont(font), AsPath(path));
}
