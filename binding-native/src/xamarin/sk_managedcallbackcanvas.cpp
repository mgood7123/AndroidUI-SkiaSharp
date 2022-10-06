/*
 * Copyright 2022 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/xamarin/SkManagedCallbackCanvas.h"

#include "include/xamarin/sk_managedcallbackcanvas.h"
#include "src/c/sk_types_priv.h"
#include "sk_managedcallbackcanvas.h"

static inline SkManagedCallbackCanvas* AsManagedCallbackCanvas(sk_managedcallbackcanvas_t* d) {
    return reinterpret_cast<SkManagedCallbackCanvas*>(d);
}
static inline sk_managedcallbackcanvas_t* ToManagedCallbackCanvas(SkManagedCallbackCanvas* d) {
    return reinterpret_cast<sk_managedcallbackcanvas_t*>(d);
}

static sk_managedcallbackcanvas_procs_t gProcs;

void destroy(SkManagedCallbackCanvas* d, void* context);
void destroy(SkManagedCallbackCanvas* d, void* context) {
    if (gProcs.fDestroy) {
        gProcs.fDestroy(ToManagedCallbackCanvas(d), context);
    }
}

sk_managedcallbackcanvas_t* sk_managedcallbackcanvas_new(void* context, int width, int height) {
    return ToManagedCallbackCanvas(new SkManagedCallbackCanvas(context, width, height));
}

void sk_managedcallbackcanvas_delete(sk_managedcallbackcanvas_t* d) {
    delete AsManagedCallbackCanvas(d);
}




void setMatrix(SkManagedCallbackCanvas* d, void* context, const sk_m44_t* matrix);
void setMatrix(SkManagedCallbackCanvas* d, void* context, const sk_m44_t* matrix) {
    if (gProcs.fSetMatrix) {
        gProcs.fSetMatrix(ToManagedCallbackCanvas(d), context, matrix);
    }
}

void concat(SkManagedCallbackCanvas* d, void* context, const sk_m44_t* matrix);
void concat(SkManagedCallbackCanvas* d, void* context, const sk_m44_t* matrix) {
    if (gProcs.fConcat) {
        gProcs.fConcat(ToManagedCallbackCanvas(d), context, matrix);
    }
}

void scale(SkManagedCallbackCanvas* d, void* context, float x, float y);
void scale(SkManagedCallbackCanvas* d, void* context, float x, float y) {
    if (gProcs.fScale) {
        gProcs.fScale(ToManagedCallbackCanvas(d), context, x, y);
    }
}

void translate(SkManagedCallbackCanvas* d, void* context, float x, float y);
void translate(SkManagedCallbackCanvas* d, void* context, float x, float y) {
    if (gProcs.fTranslate) {
        gProcs.fTranslate(ToManagedCallbackCanvas(d), context, x, y);
    }
}

void save(SkManagedCallbackCanvas* d, void* context);
void save(SkManagedCallbackCanvas* d, void* context) {
    if (gProcs.fSave) {
        gProcs.fSave(ToManagedCallbackCanvas(d), context);
    }
}

void clipRect(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_clipop_t op, bool doAA);
void clipRect(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_clipop_t op, bool doAA) {
    if (gProcs.fClipRect) {
        gProcs.fClipRect(ToManagedCallbackCanvas(d), context, rect, op, doAA);
    }
}

void clipRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect, const sk_clipop_t op, bool doAA);
void clipRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect, const sk_clipop_t op, bool doAA) {
    if (gProcs.fClipRRect) {
        gProcs.fClipRRect(ToManagedCallbackCanvas(d), context, rrect, op, doAA);
    }
}

void clipPath(SkManagedCallbackCanvas* d, void* context, const sk_path_t* path, const sk_clipop_t op, bool doAA);
void clipPath(SkManagedCallbackCanvas* d, void* context, const sk_path_t* path, const sk_clipop_t op, bool doAA) {
    if (gProcs.fClipPath) {
        gProcs.fClipPath(ToManagedCallbackCanvas(d), context, path, op, doAA);
    }
}

void clipShader(SkManagedCallbackCanvas* d, void* context, const sk_shader_t* shader, const sk_clipop_t op);
void clipShader(SkManagedCallbackCanvas* d, void* context, const sk_shader_t* shader, const sk_clipop_t op) {
    if (gProcs.fClipShader) {
        gProcs.fClipShader(ToManagedCallbackCanvas(d), context, shader, op);
    }
}

void clipRegion(SkManagedCallbackCanvas* d, void* context, const sk_region_t* region, const sk_clipop_t op);
void clipRegion(SkManagedCallbackCanvas* d, void* context, const sk_region_t* region, const sk_clipop_t op) {
    if (gProcs.fClipRegion) {
        gProcs.fClipRegion(ToManagedCallbackCanvas(d), context, region, op);
    }
}

void saveLayer(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint);
void saveLayer(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint) {
    if (gProcs.fSaveLayer) {
        gProcs.fSaveLayer(ToManagedCallbackCanvas(d), context, rect, paint);
    }
}

void restore(SkManagedCallbackCanvas* d, void* context);
void restore(SkManagedCallbackCanvas* d, void* context) {
    if (gProcs.fRestore) {
        gProcs.fRestore(ToManagedCallbackCanvas(d), context);
    }
}

void drawPaint(SkManagedCallbackCanvas* d, void* context, const sk_paint_t* paint);
void drawPaint(SkManagedCallbackCanvas* d, void* context, const sk_paint_t* paint) {
    if (gProcs.fDrawPaint) {
        gProcs.fDrawPaint(ToManagedCallbackCanvas(d), context, paint);
    }
}

void drawRect(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint);
void drawRect(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint) {
    if (gProcs.fDrawRect) {
        gProcs.fDrawRect(ToManagedCallbackCanvas(d), context, rect, paint);
    }
}

void drawRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect, const sk_paint_t* paint);
void drawRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect, const sk_paint_t* paint) {
    if (gProcs.fDrawRRect) {
        gProcs.fDrawRRect(ToManagedCallbackCanvas(d), context, rrect, paint);
    }
}

void drawDRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect1, const sk_rrect_t* rrect2, const sk_paint_t* paint);
void drawDRRect(SkManagedCallbackCanvas* d, void* context, const sk_rrect_t* rrect1, const sk_rrect_t* rrect2, const sk_paint_t* paint) {
    if (gProcs.fDrawDRRect) {
        gProcs.fDrawDRRect(ToManagedCallbackCanvas(d), context, rrect1, rrect2, paint);
    }
}

void drawOval(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint);
void drawOval(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const sk_paint_t* paint) {
    if (gProcs.fDrawOval) {
        gProcs.fDrawOval(ToManagedCallbackCanvas(d), context, rect, paint);
    }
}

void drawArc(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, float a, float b, bool c, const sk_paint_t* paint);
void drawArc(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, float a, float b, bool c, const sk_paint_t* paint) {
    if (gProcs.fDrawArc) {
        gProcs.fDrawArc(ToManagedCallbackCanvas(d), context, rect, a, b, c, paint);
    }
}

void drawPoints(SkManagedCallbackCanvas* d, void* context, sk_point_mode_t pointMode, size_t count, const sk_point_t points[], const sk_paint_t* paint);
void drawPoints(SkManagedCallbackCanvas* d, void* context, sk_point_mode_t pointMode, size_t count, const sk_point_t points[], const sk_paint_t* paint) {
    if (gProcs.fDrawPoints) {
        gProcs.fDrawPoints(ToManagedCallbackCanvas(d), context, pointMode, count, points, paint);
    }
}

void drawPath(SkManagedCallbackCanvas* d, void* context, const sk_path_t* path, const sk_paint_t* paint);
void drawPath(SkManagedCallbackCanvas* d, void* context, const sk_path_t* path, const sk_paint_t* paint) {
    if (gProcs.fDrawPath) {
        gProcs.fDrawPath(ToManagedCallbackCanvas(d), context, path, paint);
    }
}

void flush(SkManagedCallbackCanvas* d, void* context);
void flush(SkManagedCallbackCanvas* d, void* context) {
    if (gProcs.fFlush) {
        gProcs.fFlush(ToManagedCallbackCanvas(d), context);
    }
}

void drawRegion(SkManagedCallbackCanvas* d, void* context, const sk_region_t* region, const sk_paint_t* paint);
void drawRegion(SkManagedCallbackCanvas* d, void* context, const sk_region_t* region, const sk_paint_t* paint) {
    if (gProcs.fDrawRegion) {
        gProcs.fDrawRegion(ToManagedCallbackCanvas(d), context, region, paint);
    }
}

void drawImage(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, float x, float y, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint);
void drawImage(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, float x, float y, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint) {
    if (gProcs.fDrawImage) {
        gProcs.fDrawImage(ToManagedCallbackCanvas(d), context, image, x, y, sampling_options, paint);
    }
}

void drawImageRect(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, const sk_rect_t* src, const sk_rect_t* dest, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint, sk_src_rect_constraint_t constraint);
void drawImageRect(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, const sk_rect_t* src, const sk_rect_t* dest, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint, sk_src_rect_constraint_t constraint) {
    if (gProcs.fDrawImageRect) {
        gProcs.fDrawImageRect(ToManagedCallbackCanvas(d), context, image, src, dest, sampling_options, paint, constraint);
    }
}

void drawImageLattice(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dest, const sk_filter_mode_t filter, const sk_paint_t* paint);
void drawImageLattice(SkManagedCallbackCanvas* d, void* context, const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dest, const sk_filter_mode_t filter, const sk_paint_t* paint) {
    if (gProcs.fDrawImageLattice) {
        gProcs.fDrawImageLattice(ToManagedCallbackCanvas(d), context, image, lattice, dest, filter, paint);
    }
}

void drawSlug(SkManagedCallbackCanvas* d, void* context, const sk_slug_t* slug);
void drawSlug(SkManagedCallbackCanvas* d, void* context, const sk_slug_t* slug) {
    if (gProcs.fDrawSlug) {
        gProcs.fDrawSlug(ToManagedCallbackCanvas(d), context, slug);
    }
}

void drawTextBlob(SkManagedCallbackCanvas* d, void* context, const sk_textblob_t* blob, float x, float y, const sk_paint_t* paint);
void drawTextBlob(SkManagedCallbackCanvas* d, void* context, const sk_textblob_t* blob, float x, float y, const sk_paint_t* paint) {
    if (gProcs.fDrawTextBlob) {
        gProcs.fDrawTextBlob(ToManagedCallbackCanvas(d), context, blob, x, y, paint);
    }
}

void drawDrawable(SkManagedCallbackCanvas* d, void* context, const sk_drawable_t* drawable, sk_matrix_t matrix);
void drawDrawable(SkManagedCallbackCanvas* d, void* context, const sk_drawable_t* drawable, sk_matrix_t matrix) {
    if (gProcs.fDrawDrawable) {
        gProcs.fDrawDrawable(ToManagedCallbackCanvas(d), context, drawable, matrix);
    }
}

void drawVertices(SkManagedCallbackCanvas* d, void* context, const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint);
void drawVertices(SkManagedCallbackCanvas* d, void* context, const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint) {
    if (gProcs.fDrawVertices) {
        gProcs.fDrawVertices(ToManagedCallbackCanvas(d), context, vertices, mode, paint);
    }
}

void drawAtlas(SkManagedCallbackCanvas* d, void* context, const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_sampling_options_t* sampling_options, const sk_rect_t* cullRect, const sk_paint_t* paint);
void drawAtlas(SkManagedCallbackCanvas* d, void* context, const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_sampling_options_t* sampling_options, const sk_rect_t* cullRect, const sk_paint_t* paint) {
    if (gProcs.fDrawAtlas) {
        gProcs.fDrawAtlas(ToManagedCallbackCanvas(d), context, atlas, xform, tex, colors, count, mode, sampling_options, cullRect, paint);
    }
}

void drawAnnotation(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const char* key, sk_data_t* value);
void drawAnnotation(SkManagedCallbackCanvas* d, void* context, const sk_rect_t* rect, const char* key, sk_data_t* value) {
    if (gProcs.fDrawAnnotation) {
        gProcs.fDrawAnnotation(ToManagedCallbackCanvas(d), context, rect, key, value);
    }
}

void drawPatch(SkManagedCallbackCanvas* d, void* context, const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint);
void drawPatch(SkManagedCallbackCanvas* d, void* context, const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint) {
    if (gProcs.fDrawPatch) {
        gProcs.fDrawPatch(ToManagedCallbackCanvas(d), context, cubics, colors, texCoords, mode, paint);
    }
}

void sk_managedcallbackcanvas_set_procs(sk_managedcallbackcanvas_procs_t procs) {
    gProcs = procs;

    SkManagedCallbackCanvas::Procs p;
    p.fConcat = concat;
    p.fClipRect = clipRect;
    p.fClipRRect = clipRRect;
    p.fClipPath = clipPath;
    p.fClipShader = clipShader;
    p.fClipRegion = clipRegion;
    p.fDrawAnnotation = drawAnnotation;
    p.fDrawAtlas = drawAtlas;
    p.fDrawPaint = drawPaint;
    p.fDrawRect = drawRect;
    p.fDrawOval = drawOval;
    p.fDrawPoints = drawPoints;
    p.fDrawRegion = drawRegion;
    p.fDrawArc = drawArc;
    p.fDrawPatch = drawPatch;
    p.fDrawPath = drawPath;
    p.fDrawImage = drawImage;
    p.fDrawImageLattice = drawImageLattice;
    p.fDrawImageRect = drawImageRect;
    p.fDrawSlug = drawSlug;
    p.fDrawTextBlob = drawTextBlob;
    p.fDrawDrawable = drawDrawable;
    p.fDrawVertices = drawVertices;
    p.fDestroy = destroy;
    p.fFlush = flush;
    p.fRestore = restore;
    p.fSave = save;
    p.fSaveLayer = saveLayer;
    p.fScale = scale;
    p.fSetMatrix = setMatrix;
    p.fTranslate = translate;

    SkManagedCallbackCanvas::setProcs(p);
}