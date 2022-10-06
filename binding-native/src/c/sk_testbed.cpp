/*
 * Copyright 2020 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkAnnotation.h"
#include "include/core/SkCanvas.h"
#include "include/core/SkOverdrawCanvas.h"
#include "include/utils/SkNoDrawCanvas.h"
#include "include/utils/SkNWayCanvas.h"

#include "include/c/sk_testbed.h"

#include "src/c/sk_types_priv.h"

void sk_testbed(sk_canvas_t* ccanvas, int test_number) {
    if (ccanvas == nullptr) return;
    auto* canvas = AsCanvas(ccanvas);
    if (test_number == 0) {
        {
            SkRect r;
            r.setLTRB(0, 0, 800, 450);
            SkPaint p;
            p.setColor(0xff000000);
            canvas->drawRect(r, p);
        }
        {
            SkRect r;
            r.setLTRB(0, 0, 800, 450);
            SkPaint p;
            p.setColor(0xffffffff);
            canvas->saveLayer(r, &p);
        }
        {
            SkRect r;
            r.setLTRB(0, 23, 800, 450);
            SkPaint p;
            p.setColor(0xffffffff);
            canvas->saveLayer(r, &p);
        }
        {
            SkM44 m;
            m.setRC(0, 0, 1);
            m.setRC(0, 1, 0);
            m.setRC(0, 2, 0);
            m.setRC(0, 3, 0);
            m.setRC(1, 0, 0);
            m.setRC(1, 1, 1);
            m.setRC(1, 2, 0);
            m.setRC(1, 3, 23);
            m.setRC(2, 0, 0);
            m.setRC(2, 1, 0);
            m.setRC(2, 2, 1);
            m.setRC(2, 3, 0);
            m.setRC(3, 0, 0);
            m.setRC(3, 1, 0);
            m.setRC(3, 2, 0);
            m.setRC(3, 3, 1);
            canvas->concat(m);
        }
        {
            canvas->restore();
        }
        {
            SkRect r;
            r.setLTRB(0, 0, 800, 23);
            SkPaint p;
            p.setColor(0xffffffff);
            canvas->saveLayer(r, &p);
        }
        {
            SkRect r;
            r.setLTRB(0, 0, 800, 23);
            SkPaint p;
            p.setColor(0xff888888);
            canvas->drawRect(r, p);
        }
        {
            canvas->restore();
        }
        {
            canvas->restore();
        }
    }
}
