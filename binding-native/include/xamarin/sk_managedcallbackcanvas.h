/*
 * Copyright 2022 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_managedcallbackcanvas_DEFINED
#define sk_managedcallbackcanvas_DEFINED

#include "sk_xamarin.h"

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

typedef struct sk_managedcallbackcanvas_t sk_managedcallbackcanvas_t;

#define ANDROIDUI_PROC(name) typedef void (*sk_managedcallbackcanvas_##name) (sk_managedcallbackcanvas_t* d, void* context
ANDROIDUI_PROC(concat_proc), const sk_m44_t* matrix);
ANDROIDUI_PROC(clip_rect_proc), const sk_rect_t* rect, const sk_clipop_t op, bool doAA);
ANDROIDUI_PROC(clip_rrect_proc), const sk_rrect_t* rrect, const sk_clipop_t op, bool doAA);
ANDROIDUI_PROC(clip_path_proc), const sk_path_t* path, const sk_clipop_t op, bool doAA);
ANDROIDUI_PROC(clip_shader_proc), const sk_shader_t* shader, const sk_clipop_t op);
ANDROIDUI_PROC(clip_region_proc), const sk_region_t* region, const sk_clipop_t op);
ANDROIDUI_PROC(draw_paint_proc), const sk_paint_t* paint);
ANDROIDUI_PROC(draw_rect_proc), const sk_rect_t* rect, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_rrect_proc), const sk_rrect_t* rrect, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_drrect_proc), const sk_rrect_t* rrect1, const sk_rrect_t* rrect2, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_oval_proc), const sk_rect_t* rect, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_points_proc), sk_point_mode_t pointMode, size_t count, const sk_point_t points[], const sk_paint_t* paint);
ANDROIDUI_PROC(draw_region_proc), const sk_region_t* region, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_arc_proc), const sk_rect_t* rect, float a, float b, bool c, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_path_proc), const sk_path_t* path, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_image_proc), const sk_image_t* image, float x, float y, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_image_rect_proc), const sk_image_t* image, const sk_rect_t* src, const sk_rect_t* dest, const sk_sampling_options_t* sampling_options, const sk_paint_t* paint, sk_src_rect_constraint_t constraint);
ANDROIDUI_PROC(draw_image_lattice_proc), const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dest, const sk_filter_mode_t filter, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_text_blob_proc), const sk_textblob_t* blob, float x, float y, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_slug_proc), const sk_slug_t* slug);
ANDROIDUI_PROC(draw_drawable_proc), const sk_drawable_t* drawable, sk_matrix_t matrix);
ANDROIDUI_PROC(draw_vertices_proc), const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_atlas_proc), const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_sampling_options_t* sampling_options, const sk_rect_t* cullRect, const sk_paint_t* paint);
ANDROIDUI_PROC(draw_annotation_proc), const sk_rect_t* rect, const char* key, sk_data_t* value);
ANDROIDUI_PROC(draw_patch_proc), const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint);
ANDROIDUI_PROC(destroy_proc));
ANDROIDUI_PROC(flush_proc));
ANDROIDUI_PROC(restore_proc));
ANDROIDUI_PROC(save_proc));
ANDROIDUI_PROC(save_layer_proc), const sk_rect_t* rect, const sk_paint_t* paint);
ANDROIDUI_PROC(set_matrix_proc), const sk_m44_t* matrix);
ANDROIDUI_PROC(scale_proc), float x, float y);
ANDROIDUI_PROC(translate_proc), float x, float y);
#undef ANDROIDUI_PROC

typedef struct {
    sk_managedcallbackcanvas_concat_proc fConcat;
    sk_managedcallbackcanvas_clip_rect_proc fClipRect;
    sk_managedcallbackcanvas_clip_rrect_proc fClipRRect;
    sk_managedcallbackcanvas_clip_path_proc fClipPath;
    sk_managedcallbackcanvas_clip_shader_proc fClipShader;
    sk_managedcallbackcanvas_clip_region_proc fClipRegion;
    sk_managedcallbackcanvas_draw_annotation_proc fDrawAnnotation;
    sk_managedcallbackcanvas_draw_arc_proc fDrawArc;
    sk_managedcallbackcanvas_draw_atlas_proc fDrawAtlas;
    sk_managedcallbackcanvas_draw_drawable_proc fDrawDrawable;
    sk_managedcallbackcanvas_draw_drrect_proc fDrawDRRect;
    sk_managedcallbackcanvas_draw_image_rect_proc fDrawImageRect;
    sk_managedcallbackcanvas_draw_image_lattice_proc fDrawImageLattice;
    sk_managedcallbackcanvas_draw_image_proc fDrawImage;
    sk_managedcallbackcanvas_draw_oval_proc fDrawOval;
    sk_managedcallbackcanvas_draw_paint_proc fDrawPaint;
    sk_managedcallbackcanvas_draw_patch_proc fDrawPatch;
    sk_managedcallbackcanvas_draw_path_proc fDrawPath;
    sk_managedcallbackcanvas_draw_points_proc fDrawPoints;
    sk_managedcallbackcanvas_draw_rect_proc fDrawRect;
    sk_managedcallbackcanvas_draw_region_proc fDrawRegion;
    sk_managedcallbackcanvas_draw_rrect_proc fDrawRRect;
    sk_managedcallbackcanvas_draw_slug_proc fDrawSlug;
    sk_managedcallbackcanvas_draw_text_blob_proc fDrawTextBlob;
    sk_managedcallbackcanvas_draw_vertices_proc fDrawVertices;
    sk_managedcallbackcanvas_destroy_proc fDestroy;
    sk_managedcallbackcanvas_flush_proc fFlush;
    sk_managedcallbackcanvas_save_proc fSave;
    sk_managedcallbackcanvas_save_layer_proc fSaveLayer;
    sk_managedcallbackcanvas_set_matrix_proc fSetMatrix;
    sk_managedcallbackcanvas_restore_proc fRestore;
    sk_managedcallbackcanvas_translate_proc fScale;
    sk_managedcallbackcanvas_scale_proc fTranslate;
} sk_managedcallbackcanvas_procs_t;

SK_X_API sk_managedcallbackcanvas_t* sk_managedcallbackcanvas_new(void* context, int width, int height);
SK_X_API void sk_managedcallbackcanvas_delete(sk_managedcallbackcanvas_t*);
SK_X_API void sk_managedcallbackcanvas_set_procs(sk_managedcallbackcanvas_procs_t procs);

SK_C_PLUS_PLUS_END_GUARD

#endif
