/*
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef SkPaintExtraInfo_h
#define SkPaintExtraInfo_h

#include "include/core/SkPaint.h"
#include "include/core/SkFont.h"
#include "include/utils/SkTextUtils.h"

class SkPaintExtraInfo {
public:
    SkPaintExtraInfo();
    SkPaintExtraInfo(const SkFont& font);
    SkPaintExtraInfo(const SkPaint& paint);
    SkPaintExtraInfo(const SkFont& font, const SkPaint& paint);
    SkPaintExtraInfo(const SkPaintExtraInfo& paint);
    ~SkPaintExtraInfo();

public:
    void reset();

    SkFont* cloneFont() const;
    SkPaint* clonePaint() const;

    SkPaint* getPaint();
    SkFont* getFont();

    void setTextAlign(SkTextUtils::Align textAlign);
    SkTextUtils::Align getTextAlign() const;

    void setTextEncoding(SkTextEncoding encoding);
    SkTextEncoding getTextEncoding() const;

private:
    SkPaint fPaint;
    SkFont fFont;
    SkTextUtils::Align fTextAlign;
    SkTextEncoding fTextEncoding;
};

#endif