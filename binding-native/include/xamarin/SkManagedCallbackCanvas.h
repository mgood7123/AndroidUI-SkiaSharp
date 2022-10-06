/*
 * Copyright 2022 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef SkManagedCallbackCanvas_h
#define SkManagedCallbackCanvas_h

#include "include/core/SkCanvasVirtualEnforcer.h"
#include "include/private/SkTDArray.h"
#include "include/utils/SkNoDrawCanvas.h"
#include "include/core/SkTypes.h"
#include "include/c/sk_types.h"

class SK_API SkManagedCallbackCanvas;

// delegate declarations

class SkManagedCallbackCanvas : public SkCanvasVirtualEnforcer<SkNoDrawCanvas> {
public:
    SkManagedCallbackCanvas(void* context, int width, int height);

    ~SkManagedCallbackCanvas() override;

public:
#define ANDROIDUI_PROC(name) typedef void (*name) (SkManagedCallbackCanvas* d, void* context
    ANDROIDUI_PROC(empty_proc));
    ANDROIDUI_PROC(matrix_proc), const sk_m44_t* matrix);
    ANDROIDUI_PROC(paint_proc), const sk_paint_t* paint);
    ANDROIDUI_PROC(rect_clip_bool_proc), const sk_rect_t* rect, const sk_clipop_t op, bool doAA);
    ANDROIDUI_PROC(rrect_clip_bool_proc), const sk_rrect_t* rrect, const sk_clipop_t op, bool doAA);
    ANDROIDUI_PROC(path_clip_bool_proc), const sk_path_t* path, const sk_clipop_t op, bool doAA);
    ANDROIDUI_PROC(shader_clip_proc), const sk_shader_t* shader, const sk_clipop_t op);
    ANDROIDUI_PROC(region_clip_proc), const sk_region_t* region, const sk_clipop_t op);
    ANDROIDUI_PROC(float_float_proc), float value1, float value2);
    ANDROIDUI_PROC(rect_paint_proc), const sk_rect_t* rect, const sk_paint_t* paint);
    ANDROIDUI_PROC(rrect_paint_proc), const sk_rrect_t* rrect, const sk_paint_t* paint);
    ANDROIDUI_PROC(rrect_rrect_paint_proc), const sk_rrect_t* rrect1, const sk_rrect_t* rrect2, const sk_paint_t* paint);
    ANDROIDUI_PROC(rect_float_float_bool_paint_proc), const sk_rect_t* rect, float a, float b, bool c, const sk_paint_t* paint);
    ANDROIDUI_PROC(pointmode_sizet_pointarray_paint_proc), sk_point_mode_t pointMode, size_t count, const sk_point_t points[], const sk_paint_t* paint);
    ANDROIDUI_PROC(region_paint_proc), const sk_region_t* region, const sk_paint_t* paint);
    ANDROIDUI_PROC(path_paint_proc), const sk_path_t* path, const sk_paint_t* paint);
    ANDROIDUI_PROC(image_float_float_samplingoptions_paint_proc), const sk_image_t* image, float x, float y, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint);
    ANDROIDUI_PROC(image_rect_rect_samplingoptions_constraint_proc), const sk_image_t* image, const sk_rect_t* src, const sk_rect_t* dest, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint, sk_src_rect_constraint_t constraint);
    ANDROIDUI_PROC(image_lattice_rect_filtermode_paint_proc), const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dest, const sk_filter_mode_t filter, const sk_paint_t* paint);
    ANDROIDUI_PROC(text_blob_float_float_paint_proc), const sk_textblob_t* blob, float x, float y, const sk_paint_t* paint);
    ANDROIDUI_PROC(slug_proc), const sk_slug_t* slug);
    ANDROIDUI_PROC(drawable_matrix_proc), const sk_drawable_t* drawable, sk_matrix_t matrix);
    ANDROIDUI_PROC(point_color_point_blendmode_paint_proc), const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint);
    ANDROIDUI_PROC(vertices_blendmode_paint_proc), const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint);
    ANDROIDUI_PROC(atlas_rsxform_rect_color_int_blendmode_samplingoptions_rect_paint_proc), const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_sampling_options_t* sampling_options, const sk_rect_t* cullRect, const sk_paint_t* paint);
    ANDROIDUI_PROC(rect_string_data_proc), const sk_rect_t* rect, const char* key, sk_data_t* value);
#undef ANDROIDUI_PROC

    struct Procs {
        matrix_proc fConcat;
        rect_clip_bool_proc fClipRect;
        rrect_clip_bool_proc fClipRRect;
        path_clip_bool_proc fClipPath;
        shader_clip_proc fClipShader;
        region_clip_proc fClipRegion;
        matrix_proc fSetMatrix;
        paint_proc fDrawPaint;
        rect_paint_proc fDrawRect;
        rrect_paint_proc fDrawRRect;
        rrect_rrect_paint_proc fDrawDRRect;
        rect_paint_proc fDrawOval;
        region_paint_proc fDrawRegion;
        pointmode_sizet_pointarray_paint_proc fDrawPoints;
        rect_float_float_bool_paint_proc fDrawArc;
        image_float_float_samplingoptions_paint_proc fDrawImage;
        image_rect_rect_samplingoptions_constraint_proc fDrawImageRect;
        image_lattice_rect_filtermode_paint_proc fDrawImageLattice;
        text_blob_float_float_paint_proc fDrawTextBlob;
        slug_proc fDrawSlug;
        drawable_matrix_proc fDrawDrawable;
        vertices_blendmode_paint_proc fDrawVertices;
        atlas_rsxform_rect_color_int_blendmode_samplingoptions_rect_paint_proc fDrawAtlas;
        rect_string_data_proc fDrawAnnotation;
        point_color_point_blendmode_paint_proc fDrawPatch;
        path_paint_proc fDrawPath;
        empty_proc fDestroy;
        empty_proc fFlush;
        empty_proc fRestore;
        empty_proc fSave;
        rect_paint_proc fSaveLayer;
        float_float_proc fScale;
        float_float_proc fTranslate;
    };

    static void setProcs(Procs procs);

protected:
    void willSave() override;
    SaveLayerStrategy getSaveLayerStrategy(const SaveLayerRec&) override;
    bool onDoSaveBehind(const SkRect*) override;
    void willRestore() override;

    void didConcat44(const SkM44&) override;
    void didSetM44(const SkM44&) override;
    void didScale(SkScalar, SkScalar) override;
    void didTranslate(SkScalar, SkScalar) override;

    void onDrawDRRect(const SkRRect&, const SkRRect&, const SkPaint&) override;
#if SK_SUPPORT_GPU
    void onDrawSlug(const sktext::gpu::Slug* slug) override;
#endif
    void onDrawGlyphRunList(
        const sktext::GlyphRunList& glyphRunList, const SkPaint& paint) override;
    void onDrawTextBlob(const SkTextBlob* blob, SkScalar x, SkScalar y,
        const SkPaint& paint) override;
    void onDrawPatch(const SkPoint cubics[12], const SkColor colors[4],
        const SkPoint texCoords[4], SkBlendMode, const SkPaint& paint) override;

    void onDrawPaint(const SkPaint&) override;
    void onDrawBehind(const SkPaint&) override;
    void onDrawPoints(PointMode, size_t count, const SkPoint pts[], const SkPaint&) override;
    void onDrawRect(const SkRect&, const SkPaint&) override;
    void onDrawRegion(const SkRegion&, const SkPaint&) override;
    void onDrawOval(const SkRect&, const SkPaint&) override;
    void onDrawArc(const SkRect&, SkScalar, SkScalar, bool, const SkPaint&) override;
    void onDrawRRect(const SkRRect&, const SkPaint&) override;
    void onDrawPath(const SkPath&, const SkPaint&) override;

    void onDrawImage2(const SkImage*, SkScalar, SkScalar, const SkSamplingOptions&, const SkPaint*) override;
    void onDrawImageRect2(const SkImage*, const SkRect&, const SkRect&, const SkSamplingOptions&, const SkPaint*, SrcRectConstraint) override;
    void onDrawImageLattice2(const SkImage*, const Lattice&, const SkRect&, SkFilterMode, const SkPaint*) override;
    void onDrawAtlas2(const SkImage*, const SkRSXform[], const SkRect[], const SkColor[], int, SkBlendMode, const SkSamplingOptions&, const SkRect*, const SkPaint*) override;

    void onDrawVerticesObject(const SkVertices*, SkBlendMode, const SkPaint&) override;
    void onDrawShadowRec(const SkPath&, const SkDrawShadowRec&) override;

    void onClipRect(const SkRect&, SkClipOp, ClipEdgeStyle) override;
    void onClipRRect(const SkRRect&, SkClipOp, ClipEdgeStyle) override;
    void onClipPath(const SkPath&, SkClipOp, ClipEdgeStyle) override;
    void onClipShader(sk_sp<SkShader>, SkClipOp) override;
    void onClipRegion(const SkRegion&, SkClipOp) override;

    void onDrawPicture(const SkPicture*, const SkMatrix*, const SkPaint*) override;
    void onDrawDrawable(SkDrawable*, const SkMatrix*) override;
    void onDrawAnnotation(const SkRect&, const char[], SkData*) override;

    void onDrawEdgeAAQuad(const SkRect&, const SkPoint[4], QuadAAFlags, const SkColor4f&,
        SkBlendMode) override;
    void onDrawEdgeAAImageSet2(const ImageSetEntry[], int count, const SkPoint[], const SkMatrix[],
        const SkSamplingOptions&, const SkPaint*, SrcRectConstraint) override;

    void onFlush() override;

    sk_sp<SkSurface> onNewSurface(const SkImageInfo&, const SkSurfaceProps&) override;

private:
    void* fContext;
    static Procs fProcs;

private:
    typedef SkCanvasVirtualEnforcer<SkNoDrawCanvas> INHERITED;
};


#endif
