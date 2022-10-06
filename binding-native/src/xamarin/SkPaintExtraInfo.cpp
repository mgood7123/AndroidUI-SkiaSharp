/*
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkPaint.h"
#include "include/core/SkFont.h"
#include "include/utils/SkTextUtils.h"
#include "include/xamarin/SkPaintExtraInfo.h"

SkPaintExtraInfo::SkPaintExtraInfo()
    : fFont(SkFont())
    , fPaint(SkPaint())
    , fTextAlign(SkTextUtils::Align::kLeft_Align)
    , fTextEncoding(SkTextEncoding::kUTF8)
{
    fFont.setLinearMetrics(true);
    fFont.setEdging(SkFont::Edging::kAlias);
}

SkPaintExtraInfo::SkPaintExtraInfo(const SkFont& font)
    : fFont(font)
    , fPaint(SkPaint())
    , fTextAlign(SkTextUtils::Align::kLeft_Align)
    , fTextEncoding(SkTextEncoding::kUTF8)
{
}

SkPaintExtraInfo::SkPaintExtraInfo(const SkPaint& paint)
    : fFont(SkFont())
    , fPaint(paint)
    , fTextAlign(SkTextUtils::Align::kLeft_Align)
    , fTextEncoding(SkTextEncoding::kUTF8)
{
    fFont.setLinearMetrics(true);
    fFont.setEdging(SkFont::Edging::kAlias);
}

SkPaintExtraInfo::SkPaintExtraInfo(const SkFont& font, const SkPaint& paint)
    : fFont(font)
    , fPaint(paint)
    , fTextAlign(SkTextUtils::Align::kLeft_Align)
    , fTextEncoding(SkTextEncoding::kUTF8)
{
}

SkPaintExtraInfo::SkPaintExtraInfo(const SkPaintExtraInfo& paint) = default;

SkPaintExtraInfo::~SkPaintExtraInfo() = default;

void SkPaintExtraInfo::reset() {
    *this = SkPaintExtraInfo();
}

SkFont* SkPaintExtraInfo::cloneFont() const {
    return new SkFont(fFont);
}

SkPaint* SkPaintExtraInfo::clonePaint() const {
    return new SkPaint(fPaint);
}

SkFont* SkPaintExtraInfo::getFont() {
    return &fFont;
}

SkPaint* SkPaintExtraInfo::getPaint() {
    return &fPaint;
}

void SkPaintExtraInfo::setTextAlign(SkTextUtils::Align textAlign) {
    fTextAlign = textAlign;
}

SkTextUtils::Align SkPaintExtraInfo::getTextAlign() const {
    return fTextAlign;
}

void SkPaintExtraInfo::setTextEncoding(SkTextEncoding encoding) {
    fTextEncoding = encoding;
}

SkTextEncoding SkPaintExtraInfo::getTextEncoding() const {
    return fTextEncoding;
}