/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkMatrix.h"
#include "include/core/SkM44.h"
#include "include/utils/SkCamera.h"

#include "include/c/sk_matrix.h"

#include "src/c/sk_types_priv.h"

bool sk_matrix_try_invert(sk_matrix_t *matrix, sk_matrix_t *result) {
    SkMatrix m = AsMatrix(matrix);
    if (!result)
        return m.invert(nullptr);

    SkMatrix inverse;
    bool invertible = m.invert(&inverse);
    *result = ToMatrix(&inverse);
    return invertible;
}

void sk_matrix_concat(sk_matrix_t *matrix, sk_matrix_t *first, sk_matrix_t *second) {
    SkMatrix m = AsMatrix(matrix);
    m.setConcat(AsMatrix(first), AsMatrix(second));
    *matrix = ToMatrix(&m);
}

void sk_matrix_pre_concat(sk_matrix_t *target, sk_matrix_t *matrix) {
    SkMatrix m = AsMatrix(target);
    m.preConcat(AsMatrix(matrix));
    *target = ToMatrix(&m);
}

void sk_matrix_post_concat(sk_matrix_t *target, sk_matrix_t *matrix) {
    SkMatrix m = AsMatrix(target);
    m.postConcat(AsMatrix(matrix));
    *target = ToMatrix(&m);
}

void sk_matrix_map_rect(sk_matrix_t *matrix, sk_rect_t *dest, sk_rect_t *source) {
    AsMatrix(matrix).mapRect(AsRect(dest), *AsRect(source));
}

void sk_matrix_map_points(sk_matrix_t *matrix, sk_point_t *dst, sk_point_t *src, int count) {
    AsMatrix(matrix).mapPoints(AsPoint(dst), AsPoint(src), count);
}

void sk_matrix_map_vectors(sk_matrix_t *matrix, sk_point_t *dst, sk_point_t *src, int count) {
    AsMatrix(matrix).mapVectors(AsPoint(dst), AsPoint(src), count);
}

void sk_matrix_map_xy(sk_matrix_t *matrix, float x, float y, sk_point_t* cresult) {
    SkPoint result;
    AsMatrix(matrix).mapXY(x, y, &result);
    *cresult = *ToPoint(&result);
}

void sk_matrix_map_vector(sk_matrix_t *matrix, float x, float y, sk_point_t* cresult) {
    SkPoint result;
    AsMatrix(matrix).mapVector(x, y, &result);
    *cresult = *ToPoint(&result);
}

float sk_matrix_map_radius(sk_matrix_t *matrix, float radius) {
    return AsMatrix(matrix).mapRadius(radius);
}

// additional
sk_matrix_t sk_matrix_scale(float sx, float sy) {
    return ToMatrix(SkMatrix::Scale(sx, sy));
}
sk_matrix_t sk_matrix_translate(float dx, float dy) {
    return ToMatrix(SkMatrix::Translate(dx, dy));
}
sk_matrix_t sk_matrix_translate_point(sk_point_t t) {
    return ToMatrix(SkMatrix::Translate(AsPoint(t)));
}
sk_matrix_t sk_matrix_translate_ipoint(sk_ipoint_t t) {
    return ToMatrix(SkMatrix::Translate(AsIPoint(t)));
}
sk_matrix_t sk_matrix_rotate_deg(float deg) {
    return ToMatrix(SkMatrix::RotateDeg(deg));
}
sk_matrix_t sk_matrix_rotate_deg_point(float deg, sk_point_t t) {
    return ToMatrix(SkMatrix::RotateDeg(deg, AsPoint(t)));
}
sk_matrix_t sk_matrix_rotate_rad(float rad) {
    return ToMatrix(SkMatrix::RotateRad(rad));
}
sk_matrix_type_mask_t sk_matrix_get_type(sk_matrix_t* matrix) {
    return (sk_matrix_type_mask_t)AsMatrix(matrix).getType(); // calls ComputeTypeMask
}
bool sk_matrix_is_identity(sk_matrix_t* matrix) {
    return AsMatrix(matrix).isIdentity(); // calls getType()
}
bool sk_matrix_is_scale_translate(sk_matrix_t* matrix) {
    return AsMatrix(matrix).isScaleTranslate(); // calls getType()
}
bool sk_matrix_is_translate(sk_matrix_t* matrix) {
    return AsMatrix(matrix).isTranslate(); // calls getType()
}
bool sk_matrix_rect_stays_rect(sk_matrix_t* matrix) {
    return AsMatrix(matrix).rectStaysRect(); // calls getType()
}
bool sk_matrix_preserves_axis_alignment(sk_matrix_t* matrix) {
    return AsMatrix(matrix).preservesAxisAlignment();
}
bool sk_matrix_has_perspective(sk_matrix_t* matrix) {
    return AsMatrix(matrix).hasPerspective(); // calls getType()
}
bool sk_matrix_is_similarity(sk_matrix_t* matrix, float tol) {
    return AsMatrix(matrix).isSimilarity(tol);
}
bool sk_matrix_preserves_right_angles(sk_matrix_t* matrix, float tol) {
    return AsMatrix(matrix).preservesRightAngles(tol);
}
float sk_matrix_get(sk_matrix_t* matrix, sk_matrix_row_major_mask_t mask) {
    return AsMatrix(matrix).get(mask);
}
float sk_matrix_rc(sk_matrix_t* matrix, int r, int c) {
    return AsMatrix(matrix).rc(r, c);
}
void sk_matrix_get9(sk_matrix_t* matrix, float* buffer) {
    return AsMatrix(matrix).get9(buffer);
}
void sk_matrix_set9(sk_matrix_t* matrix, float* buffer, sk_matrix_t* result) {
    SkMatrix m = AsMatrix(matrix);
    m.set9(buffer);
    *result = ToMatrix(&m);
}
void sk_matrix_reset(sk_matrix_t* matrix, sk_matrix_t* result) {
    SkMatrix m = AsMatrix(matrix);
    m.reset();
    *result = ToMatrix(&m);
}
void sk_matrix_set_identity(sk_matrix_t* matrix, sk_matrix_t* result) {
    SkMatrix m = AsMatrix(matrix);
    m.setIdentity();
    *result = ToMatrix(&m);
}
void sk_matrix_normalize_perspective(sk_matrix_t* matrix, sk_matrix_t* result) {
    SkMatrix m = AsMatrix(matrix);
    m.normalizePerspective();
    *result = ToMatrix(&m);
}
void sk_matrix_map_homogeneous_points3(sk_matrix_t* matrix, sk_point3_t* dst, sk_point3_t* src, int count) {
    AsMatrix(matrix).mapHomogeneousPoints(AsPoint3(dst), AsPoint3(src), count);
}
void sk_matrix_map_homogeneous_points(sk_matrix_t* matrix, sk_point3_t* dst, sk_point_t* src, int count) {
    AsMatrix(matrix).mapHomogeneousPoints(AsPoint3(dst), AsPoint(src), count);
}
bool sk_matrix_is_finite(sk_matrix_t* matrix) {
    return AsMatrix(matrix).isFinite();
}
void sk_matrix_pre_scale(sk_matrix_t* result, sk_matrix_t* matrix, float sx, float sy) {
    SkMatrix m = AsMatrix(matrix);
    m.preScale(sx, sy);
    *result = ToMatrix(&m);
}
void sk_matrix_pre_scale_with_pivot(sk_matrix_t* result, sk_matrix_t* matrix, float sx, float sy, float px, float py) {
    SkMatrix m = AsMatrix(matrix);
    m.preScale(sx, sy, px, py);
    *result = ToMatrix(&m);
}
void sk_matrix_post_scale(sk_matrix_t* result, sk_matrix_t* matrix, float sx, float sy) {
    SkMatrix m = AsMatrix(matrix);
    m.postScale(sx, sy);
    *result = ToMatrix(&m);
}
void sk_matrix_post_scale_with_pivot(sk_matrix_t* result, sk_matrix_t* matrix, float sx, float sy, float px, float py) {
    SkMatrix m = AsMatrix(matrix);
    m.postScale(sx, sy, px, py);
    *result = ToMatrix(&m);
}
void sk_matrix_pre_translate(sk_matrix_t* result, sk_matrix_t* matrix, float dx, float dy) {
    SkMatrix m = AsMatrix(matrix);
    m.preTranslate(dx, dy);
    *result = ToMatrix(&m);
}
void sk_matrix_post_translate(sk_matrix_t* result, sk_matrix_t* matrix, float dx, float dy) {
    SkMatrix m = AsMatrix(matrix);
    m.postTranslate(dx, dy);
    *result = ToMatrix(&m);
}
bool sk_matrix_set_rect_to_rect(sk_matrix_t* matrix, sk_matrix_t* result, sk_rect_t* dest, sk_rect_t* source, sk_matrix_scale_to_fit_t stf) {
    SkMatrix m = AsMatrix(matrix);
    bool r = m.setRectToRect(*AsRect(source), *AsRect(dest), (SkMatrix::ScaleToFit)stf);
    *result = ToMatrix(&m);
    return r;
}



// 3d view

sk_3dview_t* sk_3dview_new(void) {
    return To3DView(new Sk3DView());
}

void sk_3dview_destroy(sk_3dview_t* cview) {
    delete As3DView(cview);
}

void sk_3dview_save(sk_3dview_t* cview) {
    As3DView(cview)->save();
}

void sk_3dview_restore(sk_3dview_t* cview) {
    As3DView(cview)->restore();
}

void sk_3dview_translate(sk_3dview_t* cview, float x, float y, float z) {
    As3DView(cview)->translate(x, y, z);
}

void sk_3dview_rotate_x_degrees(sk_3dview_t* cview, float degrees) {
    As3DView(cview)->rotateX(degrees);
}

void sk_3dview_rotate_y_degrees(sk_3dview_t* cview, float degrees) {
    As3DView(cview)->rotateY(degrees);
}

void sk_3dview_rotate_z_degrees(sk_3dview_t* cview, float degrees) {
    As3DView(cview)->rotateZ(degrees);
}

void sk_3dview_rotate_x_radians(sk_3dview_t* cview, float radians) {
    As3DView(cview)->rotateX(SkRadiansToDegrees(radians));
}

void sk_3dview_rotate_y_radians(sk_3dview_t* cview, float radians) {
    As3DView(cview)->rotateY(SkRadiansToDegrees(radians));
}

void sk_3dview_rotate_z_radians(sk_3dview_t* cview, float radians) {
    As3DView(cview)->rotateZ(SkRadiansToDegrees(radians));
}

void sk_3dview_get_matrix(sk_3dview_t* cview, sk_matrix_t* cmatrix) {
    SkMatrix matrix;
    As3DView(cview)->getMatrix(&matrix);
    *cmatrix = ToMatrix(&matrix);
}

void sk_3dview_apply_to_canvas(sk_3dview_t* cview, sk_canvas_t* ccanvas) {
    As3DView(cview)->applyToCanvas(AsCanvas(ccanvas));
}

float sk_3dview_dot_with_normal(sk_3dview_t* cview, float dx, float dy, float dz) {
    return As3DView(cview)->dotWithNormal(dx, dy, dz);
}

// matrix 4x4, wraps old SkMatrix44 api around newer M44 api

#define SMAT4(column, row) m44->setRC(row, column
#define GMAT4(column, row) m44->rc(row, column)

#define MAT4TX GMAT4(3, 0)
#define MAT4TY GMAT4(3, 1)
#define MAT4TZ GMAT4(3, 2)

#define MAT4SX GMAT4(0, 0)
#define MAT4SY GMAT4(1, 1)
#define MAT4SZ GMAT4(2, 2)

#define MAT4PX GMAT4(0, 3)
#define MAT4PY GMAT4(1, 3)
#define MAT4PZ GMAT4(2, 3)

#define SWAPMAT4(c1, r1, c2, r2) \
{ \
    SkScalar tmp = GMAT4(c1, r1); \
    SMAT4(c1, r1), GMAT4(c2, r2)); \
    SMAT4(c2, r2), tmp); \
}

void sk_m44_destroy(sk_m44_t* matrix) {
    delete AsM44(matrix);
}

sk_m44_t* sk_m44_new(void) {
    return ToM44(new SkM44(SkM44::Uninitialized_Constructor::kUninitialized_Constructor));
}

sk_m44_t* sk_m44_new_identity(void) {
    return ToM44(new SkM44());
}

sk_m44_t* sk_m44_new_copy(const sk_m44_t* src) {
    return ToM44(new SkM44(AsM44(*src)));
}

sk_m44_t* sk_m44_new_concat(const sk_m44_t* a, const sk_m44_t* b) {
    return ToM44(new SkM44(AsM44(*a), AsM44(*b)));
}

sk_m44_t* sk_m44_new_matrix(const sk_matrix_t* src) {
    return ToM44(new SkM44(AsMatrix(src)));
}

bool sk_m44_equals(sk_m44_t* matrix, const sk_m44_t* other) {
    return *AsM44(matrix) == *AsM44(other);
}

void sk_m44_to_matrix(sk_m44_t* matrix, sk_matrix_t* dst) {
    SkMatrix m = AsM44(matrix)->asM33();
    *dst = ToMatrix(&m);
}

void sk_m44_set_identity(sk_m44_t* matrix) {
    AsM44(matrix)->setIdentity();
}

float sk_m44_get(sk_m44_t* matrix, int row, int col) {
    return AsM44(matrix)->rc(row, col);
}

void sk_m44_set(sk_m44_t* matrix, int row, int col, float value) {
    AsM44(matrix)->setRC(row, col, value);
}

void sk_m44_as_col_major(sk_m44_t* matrix, float* dst) {
    AsM44(matrix)->getColMajor(dst);
}

void sk_m44_as_row_major(sk_m44_t* matrix, float* dst) {
    AsM44(matrix)->getRowMajor(dst);
}

void sk_m44_set_col_major(sk_m44_t* matrix, float* src) {
    *AsM44(matrix) = SkM44::ColMajor(src);
}

void sk_m44_set_row_major(sk_m44_t* matrix, float* src) {
    *AsM44(matrix) = SkM44::RowMajor(src);
}

void sk_m44_set_3x3_row_major(sk_m44_t* matrix, float* src) {
    //void SkMatrix44::set3x3RowMajorf(const float src[]) {
    //    fMat[0][0] = src[0]; fMat[0][1] = src[3]; fMat[0][2] = src[6]; fMat[0][3] = 0;
    //    fMat[1][0] = src[1]; fMat[1][1] = src[4]; fMat[1][2] = src[7]; fMat[1][3] = 0;
    //    fMat[2][0] = src[2]; fMat[2][1] = src[5]; fMat[2][2] = src[8]; fMat[2][3] = 0;
    //    fMat[3][0] = 0;      fMat[3][1] = 0;      fMat[3][2] = 0;      fMat[3][3] = 1;
    //    this->recomputeTypeMask();
    //}

    SkM44* m44 = AsM44(matrix);
    SMAT4(0, 0), src[0]); SMAT4(0, 1), src[3]); SMAT4(0, 2), src[6]); SMAT4(0, 3), 0);
    SMAT4(1, 0), src[1]); SMAT4(1, 1), src[4]); SMAT4(1, 2), src[7]); SMAT4(1, 3), 0);
    SMAT4(2, 0), src[2]); SMAT4(2, 1), src[5]); SMAT4(2, 2), src[8]); SMAT4(2, 3), 0);
    SMAT4(3, 0), 0);      SMAT4(3, 1), 0);      SMAT4(3, 2), 0);      SMAT4(3, 3), 1);
}

void sk_m44_set_translate(sk_m44_t* matrix, float dx, float dy, float dz) {
    AsM44(matrix)->setTranslate(dx, dy, dz);
}

void sk_m44_pre_translate(sk_m44_t* matrix, float dx, float dy, float dz) {
    AsM44(matrix)->preTranslate(dx, dy, dz);
}

void sk_m44_post_translate(sk_m44_t* matrix, float dx, float dy, float dz) {
    AsM44(matrix)->postTranslate(dx, dy, dz);
}

void sk_m44_set_scale(sk_m44_t* matrix, float sx, float sy, float sz) {
    AsM44(matrix)->setScale(sx, sy, sz);
}

void sk_m44_pre_scale(sk_m44_t* matrix, float sx, float sy, float sz) {
    AsM44(matrix)->preScale(sx, sy, sz);
}

void sk_m44_post_scale(sk_m44_t* matrix, float sx, float sy, float sz) {
    SkM44* m44 = AsM44(matrix);
    m44->postConcat(SkM44::Scale(sx, sy, sz));
}

void sk_m44_set_rotate_about_degrees(sk_m44_t* matrix, float x, float y, float z, float degrees) {
    sk_m44_set_rotate_about_radians(matrix, x, y, z, degrees * SK_ScalarPI / 180);
}

//void sk_m44_set_rotate_about_radians(sk_m44_t* matrix, float x, float y, float z, float radians) {
//    SkM44* m44 = AsM44(matrix);
//    m44->setRotate(SkV3{ x, y, z }, radians);
//}
//
//void sk_m44_set_rotate_about_radians_unit(sk_m44_t* matrix, float x, float y, float z, float radians) {
//    SkM44* m44 = AsM44(matrix);
//    m44->setRotateUnit(SkV3{ x, y, z }, radians);
//}

void sk_m44_set_rotate_about_radians(sk_m44_t* matrix, float x, float y, float z, float radians) {
    double len2 = (double)x * x + (double)y * y + (double)z * z;
    if (1 != len2) {
        if (0 == len2) {
            AsM44(matrix)->setIdentity();
            return;
        }
        double scale = 1 / sqrt(len2);
        x = SkScalar(x * scale);
        y = SkScalar(y * scale);
        z = SkScalar(z * scale);
    }
    sk_m44_set_rotate_about_radians_unit(matrix, x, y, z, radians);
}

// prototype
void set3x3(SkM44* m44, SkScalar m_00, SkScalar m_10, SkScalar m_20,
    SkScalar m_01, SkScalar m_11, SkScalar m_21,
    SkScalar m_02, SkScalar m_12, SkScalar m_22);

void set3x3(SkM44* m44, SkScalar m_00, SkScalar m_10, SkScalar m_20,
    SkScalar m_01, SkScalar m_11, SkScalar m_21,
    SkScalar m_02, SkScalar m_12, SkScalar m_22) {
    SMAT4(0, 0), m_00); SMAT4(0, 1), m_10); SMAT4(0, 2), m_20); SMAT4(0, 3), 0);
    SMAT4(1, 0), m_01); SMAT4(1, 1), m_11); SMAT4(1, 2), m_21); SMAT4(1, 3), 0);
    SMAT4(2, 0), m_02); SMAT4(2, 1), m_12); SMAT4(2, 2), m_22); SMAT4(2, 3), 0);
    SMAT4(3, 0), 0);    SMAT4(3, 1), 0);    SMAT4(3, 2), 0);    SMAT4(3, 3), 1);
}

void sk_m44_set_rotate_about_radians_unit(sk_m44_t* matrix, float x, float y, float z, float radians) {
    double c = cos(radians);
    double s = sin(radians);
    double C = 1 - c;
    double xs = x * s;
    double ys = y * s;
    double zs = z * s;
    double xC = x * C;
    double yC = y * C;
    double zC = z * C;
    double xyC = x * yC;
    double yzC = y * zC;
    double zxC = z * xC;

    // if you're looking at wikipedia, remember that we're column major.
    SkM44* m44 = AsM44(matrix);
    set3x3(m44,  SkScalar(x * xC + c),     // scale x
                 SkScalar(xyC + zs),       // skew x
                 SkScalar(zxC - ys),       // trans x

                 SkScalar(xyC - zs),       // skew y
                 SkScalar(y * yC + c),     // scale y
                 SkScalar(yzC + xs),       // trans y

                 SkScalar(zxC + ys),       // persp x
                 SkScalar(yzC - xs),       // persp y
                 SkScalar(z * zC + c));    // persp 2
}

void sk_m44_set_concat(sk_m44_t* matrix, const sk_m44_t* a, const sk_m44_t* b) {
    AsM44(matrix)->setConcat(AsM44(*a), AsM44(*b));
}

void sk_m44_pre_concat(sk_m44_t* matrix, const sk_m44_t* m) {
    AsM44(matrix)->preConcat(AsM44(*m));
}

void sk_m44_post_concat(sk_m44_t* matrix, const sk_m44_t* m) {
    AsM44(matrix)->postConcat(AsM44(*m));
}

bool sk_m44_invert(sk_m44_t* matrix, sk_m44_t* inverse) {
    return AsM44(matrix)->invert(AsM44(inverse));
}

void sk_m44_transpose(sk_m44_t* matrix) {
    SkM44* m44 = AsM44(matrix);
    SkM44 identity = SkM44();
    if (!(*m44 == identity)) {
        SWAPMAT4(0, 1, 1, 0);
        SWAPMAT4(0, 2, 2, 0);
        SWAPMAT4(0, 3, 3, 0);
        SWAPMAT4(1, 2, 2, 1);
        SWAPMAT4(1, 3, 3, 1);
        SWAPMAT4(2, 3, 3, 2);
    }
}

void sk_m44_map_scalars(sk_m44_t* matrix, const float* src, float* dst) {
    SkM44* m44 = AsM44(matrix);

    SkScalar storage[4];
    SkScalar* result = (src == dst) ? storage : dst;

    for (int i = 0; i < 4; i++) {
        SkScalar value = 0;
        for (int j = 0; j < 4; j++) {
            value += GMAT4(j, i) * src[j];
        }
        result[i] = value;
    }

    if (storage == result) {
        memcpy(dst, storage, sizeof(storage));
    }
}

bool sk_m44_preserves_2d_axis_alignment(sk_m44_t* matrix, float epsilon) {
    SkM44* m44 = AsM44(matrix);

    // Can't check (mask & kPerspective_Mask) because Z isn't relevant here.
    if (0 != MAT4PX || 0 != MAT4PY) return false;

    // A matrix with two non-zeroish values in any of the upper right
    // rows or columns will skew.  If only one value in each row or
    // column is non-zeroish, we get a scale plus perhaps a 90-degree
    // rotation.
    int col0 = 0;
    int col1 = 0;
    int row0 = 0;
    int row1 = 0;

    // Must test against epsilon, not 0, because we can get values
    // around 6e-17 in the matrix that "should" be 0.

    if (SkScalarAbs(GMAT4(0, 0)) > epsilon) {
        col0++;
        row0++;
    }
    if (SkScalarAbs(GMAT4(0, 1)) > epsilon) {
        col1++;
        row0++;
    }
    if (SkScalarAbs(GMAT4(1, 0)) > epsilon) {
        col0++;
        row1++;
    }
    if (SkScalarAbs(GMAT4(1, 1)) > epsilon) {
        col1++;
        row1++;
    }
    if (col0 > 1 || col1 > 1 || row0 > 1 || row1 > 1) {
        return false;
    }

    return true;
}

// beyond this point we use the emulate a large portion SkMatrix44 api

// avoid collisions with SkMatrix enum

using Matrix44_TypeMask = uint8_t;
enum : Matrix44_TypeMask {
    Matrix44_kIdentity_Mask = 0,
    Matrix44_kTranslate_Mask = 1 << 0,    //!< set if the matrix has translation
    Matrix44_kScale_Mask = 1 << 1,        //!< set if the matrix has any scale != 1
    Matrix44_kAffine_Mask = 1 << 2,       //!< set if the matrix skews or rotates
    Matrix44_kPerspective_Mask = 1 << 3,  //!< set if the matrix is in perspective
};

// prototype
void recomputeTypeMask(SkM44* m44, Matrix44_TypeMask& Matrix44_fTypeMask);

void recomputeTypeMask(SkM44 * m44, Matrix44_TypeMask & Matrix44_fTypeMask) {
    if (0 != MAT4PX || 0 != MAT4PY || 0 != MAT4PZ || 1 != GMAT4(3, 3)) {
        Matrix44_fTypeMask = Matrix44_kTranslate_Mask | Matrix44_kScale_Mask | Matrix44_kAffine_Mask | Matrix44_kPerspective_Mask;
        return;
    }

    Matrix44_TypeMask mask = Matrix44_kIdentity_Mask;
    if (0 != MAT4TX || 0 != MAT4TX || 0 != MAT4TZ) {
        mask |= Matrix44_kTranslate_Mask;
    }

    if (1 != MAT4SX || 1 != MAT4SY || 1 != MAT4SZ) {
        mask |= Matrix44_kScale_Mask;
    }

    if (0 != GMAT4(1, 0) || 0 != GMAT4(0, 1) || 0 != GMAT4(0, 2) ||
        0 != GMAT4(2, 0) || 0 != GMAT4(1, 2) || 0 != GMAT4(2, 1)) {
        mask |= Matrix44_kAffine_Mask;
    }
    Matrix44_fTypeMask = mask;
}

double sk_m44_determinant(sk_m44_t* matrix) {
    SkM44* m44 = AsM44(matrix);
    SkM44 identity = SkM44();

    if (*m44 == identity) {
        return 1;
    }

    Matrix44_TypeMask mask;
    recomputeTypeMask(m44, mask);

    if (!(mask & ~(Matrix44_kScale_Mask | Matrix44_kTranslate_Mask))) {
        return GMAT4(0, 0) * GMAT4(1, 1) * GMAT4(2, 2) * GMAT4(3, 3);
    }

    double a00 = GMAT4(0, 0);
    double a01 = GMAT4(0, 1);
    double a02 = GMAT4(0, 2);
    double a03 = GMAT4(0, 3);
    double a10 = GMAT4(1, 0);
    double a11 = GMAT4(1, 1);
    double a12 = GMAT4(1, 2);
    double a13 = GMAT4(1, 3);
    double a20 = GMAT4(2, 0);
    double a21 = GMAT4(2, 1);
    double a22 = GMAT4(2, 2);
    double a23 = GMAT4(2, 3);
    double a30 = GMAT4(3, 0);
    double a31 = GMAT4(3, 1);
    double a32 = GMAT4(3, 2);
    double a33 = GMAT4(3, 3);

    double b00 = a00 * a11 - a01 * a10;
    double b01 = a00 * a12 - a02 * a10;
    double b02 = a00 * a13 - a03 * a10;
    double b03 = a01 * a12 - a02 * a11;
    double b04 = a01 * a13 - a03 * a11;
    double b05 = a02 * a13 - a03 * a12;
    double b06 = a20 * a31 - a21 * a30;
    double b07 = a20 * a32 - a22 * a30;
    double b08 = a20 * a33 - a23 * a30;
    double b09 = a21 * a32 - a22 * a31;
    double b10 = a21 * a33 - a23 * a31;
    double b11 = a22 * a33 - a23 * a32;

    // Calculate the determinant
    return b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;
}

typedef void (*Map2Procf)(SkM44* m44, const float src2[], int count, float dst4[]);
typedef void (*Map2Procd)(SkM44* m44, const double src2[], int count, double dst4[]);

static void map2_if(SkM44 * m44, const float* SK_RESTRICT src2,
    int count, float* SK_RESTRICT dst4) {
    for (int i = 0; i < count; ++i) {
        dst4[0] = src2[0];
        dst4[1] = src2[1];
        dst4[2] = 0;
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_id(SkM44 * m44, const double* SK_RESTRICT src2,
    int count, double* SK_RESTRICT dst4) {
    for (int i = 0; i < count; ++i) {
        dst4[0] = src2[0];
        dst4[1] = src2[1];
        dst4[2] = 0;
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_tf(SkM44 * m44, const float* SK_RESTRICT src2,
    int count, float* SK_RESTRICT dst4) {
    const float mat30 = float(GMAT4(3, 0));
    const float mat31 = float(GMAT4(3, 1));
    const float mat32 = float(GMAT4(3, 2));
    for (int n = 0; n < count; ++n) {
        dst4[0] = src2[0] + mat30;
        dst4[1] = src2[1] + mat31;
        dst4[2] = mat32;
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_td(SkM44 * m44, const double* SK_RESTRICT src2,
    int count, double* SK_RESTRICT dst4) {
    for (int n = 0; n < count; ++n) {
        dst4[0] = src2[0] + GMAT4(3, 0);
        dst4[1] = src2[1] + GMAT4(3, 1);
        dst4[2] = GMAT4(3, 2);
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_sf(SkM44 * m44, const float* SK_RESTRICT src2,
    int count, float* SK_RESTRICT dst4) {
    const float mat32 = float(GMAT4(3, 2));
    for (int n = 0; n < count; ++n) {
        dst4[0] = float(GMAT4(0, 0) * src2[0] + GMAT4(3, 0));
        dst4[1] = float(GMAT4(1, 1) * src2[1] + GMAT4(3, 1));
        dst4[2] = mat32;
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_sd(SkM44 * m44, const double* SK_RESTRICT src2,
    int count, double* SK_RESTRICT dst4) {
    for (int n = 0; n < count; ++n) {
        dst4[0] = GMAT4(0, 0) * src2[0] + GMAT4(3, 0);
        dst4[1] = GMAT4(1, 1) * src2[1] + GMAT4(3, 1);
        dst4[2] = GMAT4(3, 2);
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_af(SkM44 * m44, const float* SK_RESTRICT src2,
    int count, float* SK_RESTRICT dst4) {
    SkScalar r;
    for (int n = 0; n < count; ++n) {
        SkScalar sx = src2[0];
        SkScalar sy = src2[1];
        r = GMAT4(0, 0) * sx + GMAT4(1, 0) * sy + GMAT4(3, 0);
        dst4[0] = float(r);
        r = GMAT4(0, 1) * sx + GMAT4(1, 1) * sy + GMAT4(3, 1);
        dst4[1] = float(r);
        r = GMAT4(0, 2) * sx + GMAT4(1, 2) * sy + GMAT4(3, 2);
        dst4[2] = float(r);
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_ad(SkM44 * m44, const double* SK_RESTRICT src2,
    int count, double* SK_RESTRICT dst4) {
    for (int n = 0; n < count; ++n) {
        double sx = src2[0];
        double sy = src2[1];
        dst4[0] = GMAT4(0, 0) * sx + GMAT4(1, 0) * sy + GMAT4(3, 0);
        dst4[1] = GMAT4(0, 1) * sx + GMAT4(1, 1) * sy + GMAT4(3, 1);
        dst4[2] = GMAT4(0, 2) * sx + GMAT4(1, 2) * sy + GMAT4(3, 2);
        dst4[3] = 1;
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_pf(SkM44 * m44, const float* SK_RESTRICT src2,
    int count, float* SK_RESTRICT dst4) {
    SkScalar r;
    for (int n = 0; n < count; ++n) {
        SkScalar sx = src2[0];
        SkScalar sy = src2[1];
        for (int i = 0; i < 4; i++) {
            r = GMAT4(0, i) * sx + GMAT4(1, i) * sy + GMAT4(3, i);
            dst4[i] = float(r);
        }
        src2 += 2;
        dst4 += 4;
    }
}

static void map2_pd(SkM44 * m44, const double* SK_RESTRICT src2,
    int count, double* SK_RESTRICT dst4) {
    for (int n = 0; n < count; ++n) {
        double sx = src2[0];
        double sy = src2[1];
        for (int i = 0; i < 4; i++) {
            dst4[i] = GMAT4(0, i) * sx + GMAT4(1, i) * sy + GMAT4(3, i);
        }
        src2 += 2;
        dst4 += 4;
    }
}

static void map2(SkM44* m44, const float src2[], int count, float dst4[]) {
    static const Map2Procf gProc[] = {
        map2_if, map2_tf, map2_sf, map2_sf, map2_af, map2_af, map2_af, map2_af
    };

    Matrix44_TypeMask mask;
    recomputeTypeMask(m44, mask);
    Map2Procf proc = (mask & Matrix44_kPerspective_Mask) ? map2_pf : gProc[mask];
    proc(m44, src2, count, dst4);
}

static void map2(SkM44 * m44, const double src2[], int count, double dst4[]) {
    static const Map2Procd gProc[] = {
        map2_id, map2_td, map2_sd, map2_sd, map2_ad, map2_ad, map2_ad, map2_ad
    };

    Matrix44_TypeMask mask;
    recomputeTypeMask(m44, mask);
    Map2Procd proc = (mask & Matrix44_kPerspective_Mask) ? map2_pd : gProc[mask];
    proc(m44, src2, count, dst4);
}

void sk_m44_map2(sk_m44_t* matrix, const float* src2, int count, float* dst4) {
    map2(AsM44(matrix), src2, count, dst4);
}
