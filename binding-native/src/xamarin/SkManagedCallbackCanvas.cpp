/*
 * Copyright 2022 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/xamarin/SkManagedCallbackCanvas.h"


#include "include/core/SkShader.h"
#include "include/utils/SkNWayCanvas.h"
#include "include/core/SkPicture.h" // to draw the skpicture onto this callback canvas
#include "src/core/SkCanvasPriv.h"
#include "src/text/GlyphRun.h"

#include "src/c/sk_types_priv.h"
#include <cstdio>

#if false
#define ANDROIDUI_PRINT_FUNCTION_NAME printf("%s\n", __PRETTY_FUNCTION__)
#else
#define ANDROIDUI_PRINT_FUNCTION_NAME
#endif

// based on SKNWayCanvas

SkManagedCallbackCanvas::Procs SkManagedCallbackCanvas::fProcs;

void SkManagedCallbackCanvas::setProcs(SkManagedCallbackCanvas::Procs procs) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    fProcs = procs;
}

SkManagedCallbackCanvas::SkManagedCallbackCanvas(void* context, int width, int height) : INHERITED(width, height) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    fContext = context;
}

SkManagedCallbackCanvas::~SkManagedCallbackCanvas() {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDestroy) {
        fProcs.fDestroy(this, fContext);
    }
}

void SkManagedCallbackCanvas::willSave() {
    ANDROIDUI_PRINT_FUNCTION_NAME;

    if (fProcs.fSave) {
        fProcs.fSave(this, fContext);
    }

    this->INHERITED::willSave();
}

SkCanvas::SaveLayerStrategy SkManagedCallbackCanvas::getSaveLayerStrategy(const SaveLayerRec& rec) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fSaveLayer) {
        fProcs.fSaveLayer(this, fContext, ToRect(rec.fBounds), ToPaint(rec.fPaint));
    }
    this->INHERITED::getSaveLayerStrategy(rec);
    // No need for a layer.
    return kNoLayer_SaveLayerStrategy;
}

void SkManagedCallbackCanvas::willRestore() {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fRestore) {
        fProcs.fRestore(this, fContext);
    }
    this->INHERITED::willRestore();
}


void SkManagedCallbackCanvas::didConcat44(const SkM44& m) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fConcat) {
        fProcs.fConcat(this, fContext, ToM44(&m));
    }
}

void SkManagedCallbackCanvas::didSetM44(const SkM44& m) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fSetMatrix) {
        fProcs.fSetMatrix(this, fContext, ToM44(&m));
    }
}

void SkManagedCallbackCanvas::didTranslate(SkScalar x, SkScalar y) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fTranslate) {
        fProcs.fTranslate(this, fContext, x, y);
    }
}

void SkManagedCallbackCanvas::didScale(SkScalar x, SkScalar y) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fScale) {
        fProcs.fScale(this, fContext, x, y);
    }
}

void SkManagedCallbackCanvas::onClipRect(const SkRect& rect, SkClipOp op, ClipEdgeStyle edgeStyle) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fClipRect) {
        fProcs.fClipRect(this, fContext, ToRect(&rect), (sk_clipop_t)op, kSoft_ClipEdgeStyle == edgeStyle);
    }
    this->INHERITED::onClipRect(rect, op, edgeStyle);
}

void SkManagedCallbackCanvas::onClipRRect(const SkRRect& rrect, SkClipOp op, ClipEdgeStyle edgeStyle) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fClipRRect) {
        fProcs.fClipRRect(this, fContext, ToRRect(&rrect), (sk_clipop_t)op, kSoft_ClipEdgeStyle == edgeStyle);
    }
    this->INHERITED::onClipRRect(rrect, op, edgeStyle);
}

void SkManagedCallbackCanvas::onClipPath(const SkPath& path, SkClipOp op, ClipEdgeStyle edgeStyle) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fClipPath) {
        fProcs.fClipPath(this, fContext, ToPath(&path), (sk_clipop_t)op, kSoft_ClipEdgeStyle == edgeStyle);
    }
    this->INHERITED::onClipPath(path, op, edgeStyle);
}

void SkManagedCallbackCanvas::onClipShader(sk_sp<SkShader> sh, SkClipOp op) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fClipShader) {
        fProcs.fClipShader(this, fContext, ToShader(sh.get()), (sk_clipop_t)op);
    }
    this->INHERITED::onClipShader(std::move(sh), op);
}

void SkManagedCallbackCanvas::onClipRegion(const SkRegion& deviceRgn, SkClipOp op) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fClipRegion) {
        fProcs.fClipRegion(this, fContext, ToRegion(&deviceRgn), (sk_clipop_t)op);
    }
    this->INHERITED::onClipRegion(deviceRgn, op);
}

void SkManagedCallbackCanvas::onDrawPaint(const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawPaint) {
        fProcs.fDrawPaint(this, fContext, ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawPoints(PointMode mode, size_t count, const SkPoint pts[], const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawPoints) {
        fProcs.fDrawPoints(this, fContext, (sk_point_mode_t) mode, count, ToPoint(pts), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawRect(const SkRect& rect, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawRect) {
        fProcs.fDrawRect(this, fContext, ToRect(&rect), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawRegion(const SkRegion& region, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawRegion) {
        fProcs.fDrawRegion(this, fContext, ToRegion(&region), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawOval(const SkRect& rect, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawOval) {
        fProcs.fDrawOval(this, fContext, ToRect(&rect), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawArc(const SkRect& rect, SkScalar startAngle, SkScalar sweepAngle, bool useCenter, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawArc) {
        fProcs.fDrawArc(this, fContext, ToRect(&rect), startAngle, sweepAngle, useCenter, ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawRRect(const SkRRect& rrect, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawRRect) {
        fProcs.fDrawRRect(this, fContext, ToRRect(&rrect), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawDRRect(const SkRRect& outer, const SkRRect& inner, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawDRRect) {
        fProcs.fDrawDRRect(this, fContext, ToRRect(&outer), ToRRect(&inner), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawPath(const SkPath& path, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawPath) {
        fProcs.fDrawPath(this, fContext, ToPath(&path), ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawImage2(const SkImage* image, SkScalar left, SkScalar top, const SkSamplingOptions& sampling_options, const SkPaint* paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawImage) {
        fProcs.fDrawImage(this, fContext, ToImage(image), left, top, ToSamplingOptions(&sampling_options), ToPaint(paint));
    }
}

void SkManagedCallbackCanvas::onDrawImageLattice2(const SkImage* image, const Lattice& lattice, const SkRect& dst, SkFilterMode filter, const SkPaint* paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawImageLattice) {
        fProcs.fDrawImageLattice(this, fContext, ToImage(image), ToLattice(&lattice), ToRect(&dst), (sk_filter_mode_t)filter, ToPaint(paint));
    }
}

void SkManagedCallbackCanvas::onDrawImageRect2(const SkImage* image, const SkRect& src, const SkRect& dst, const SkSamplingOptions& sampling_options, const SkPaint* paint, SrcRectConstraint constraint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawImageRect) {
        fProcs.fDrawImageRect(this, fContext, ToImage(image), ToRect(&src), ToRect(&dst), ToSamplingOptions(&sampling_options), ToPaint(paint), (sk_src_rect_constraint_t)constraint);
    }
}

void SkManagedCallbackCanvas::onDrawSlug(const sktext::gpu::Slug* slug) {
    printf("%s\n", __PRETTY_FUNCTION__);
    if (fProcs.fDrawSlug) {
        fProcs.fDrawSlug(this, fContext, ToSlug(slug));
    }
}

void SkManagedCallbackCanvas::onDrawGlyphRunList(const sktext::GlyphRunList& glyphRunList, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    sk_sp<SkTextBlob> blob = sk_ref_sp(glyphRunList.blob());
    if (glyphRunList.blob() == nullptr) {
        blob = glyphRunList.makeBlob();
    }

    this->onDrawTextBlob(blob.get(), glyphRunList.origin().x(), glyphRunList.origin().y(), paint);
}

void SkManagedCallbackCanvas::onDrawTextBlob(const SkTextBlob* blob, SkScalar x, SkScalar y, const SkPaint &paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawTextBlob) {
        fProcs.fDrawTextBlob(this, fContext, ToTextBlob(blob), x, y, ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawPicture(const SkPicture* picture, const SkMatrix* matrix, const SkPaint* paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;

    if (!paint || paint->canComputeFastBounds()) {
        SkRect bounds = picture->cullRect();
        if (paint) {
            paint->computeFastBounds(bounds, &bounds);
        }
        if (matrix) {
            matrix->mapRect(&bounds);
        }
        if (this->quickReject(bounds)) {
            return;
        }
    }

    SkAutoCanvasMatrixPaint acmp(this, matrix, paint, picture->cullRect());
    picture->playback(this);
}

void SkManagedCallbackCanvas::onDrawDrawable(SkDrawable* drawable, const SkMatrix* matrix) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawDrawable) {
        fProcs.fDrawDrawable(this, fContext, ToDrawable(drawable), ToMatrix(matrix));
    }
}

void SkManagedCallbackCanvas::onDrawVerticesObject(const SkVertices* vertices, SkBlendMode bmode, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawVertices) {
        fProcs.fDrawVertices(this, fContext, ToVertices(vertices), (sk_blendmode_t)bmode, ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawPatch(const SkPoint cubics[12], const SkColor colors[4], const SkPoint texCoords[4], SkBlendMode bmode, const SkPaint& paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawPatch) {
        fProcs.fDrawPatch(this, fContext, ToPoint(cubics), colors, ToPoint(texCoords), (sk_blendmode_t)bmode, ToPaint(&paint));
    }
}

void SkManagedCallbackCanvas::onDrawAtlas2(const SkImage* image, const SkRSXform xform[], const SkRect tex[], const SkColor colors[], int count, SkBlendMode bmode, const SkSamplingOptions& sampling_options, const SkRect* cull, const SkPaint* paint) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawAtlas) {
        fProcs.fDrawAtlas(this, fContext, ToImage(image), ToRSXform(xform), ToRect(tex), colors, count, (sk_blendmode_t)bmode, ToSamplingOptions(&sampling_options), ToRect(cull), ToPaint(paint));
    }
}

void SkManagedCallbackCanvas::onDrawAnnotation(const SkRect& rect, const char key[], SkData* data) {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fDrawAnnotation) {
        fProcs.fDrawAnnotation(this, fContext, ToRect(&rect), key, ToData(data));
    }
}

void SkManagedCallbackCanvas::onFlush() {
    ANDROIDUI_PRINT_FUNCTION_NAME;
    if (fProcs.fFlush) {
        fProcs.fFlush(this, fContext);
    }
}

bool SkManagedCallbackCanvas::onDoSaveBehind(const SkRect* bounds) {
    printf("%s\n", __PRETTY_FUNCTION__);
    //Iter iter(fList);
    //while (iter.next()) {
    //    SkCanvasPriv::SaveBehind(iter.get(), bounds);
    //}
    this->INHERITED::onDoSaveBehind(bounds);
    return false;
}

void SkManagedCallbackCanvas::onDrawBehind(const SkPaint& paint) {
    printf("%s\n", __PRETTY_FUNCTION__);
    //Iter iter(fList);
    //while (iter.next()) {
    //    SkCanvasPriv::DrawBehind(iter.get(), paint);
    //}
}

void SkManagedCallbackCanvas::onDrawShadowRec(const SkPath& path, const SkDrawShadowRec& rec) {
    printf("%s\n", __PRETTY_FUNCTION__);
    //Iter iter(fList);
    //while (iter.next()) {
    //    iter->private_draw_shadow_rec(path, rec);
    //}
}

void SkManagedCallbackCanvas::onDrawEdgeAAQuad(const SkRect& rect, const SkPoint clip[4], QuadAAFlags aa, const SkColor4f& color, SkBlendMode mode) {
    printf("%s\n", __PRETTY_FUNCTION__);
    //Iter iter(fList);
    //while (iter.next()) {
    //    iter->experimental_DrawEdgeAAQuad(rect, clip, aa, color, mode);
    //}
}

void SkManagedCallbackCanvas::onDrawEdgeAAImageSet2(const ImageSetEntry set[], int count, const SkPoint dstClips[], const SkMatrix preViewMatrices[], const SkSamplingOptions& sampling_options, const SkPaint* paint, SrcRectConstraint constraint) {
    printf("%s\n", __PRETTY_FUNCTION__);
    //Iter iter(fList);
    //while (iter.next()) {
    //    iter->experimental_DrawEdgeAAImageSet2(
    //            set, count, dstClips, preViewMatrices, sampling_options, paint, constraint);
    //}
}


sk_sp<SkSurface> SkManagedCallbackCanvas::onNewSurface(const SkImageInfo&, const SkSurfaceProps&) {
    return nullptr;
}
