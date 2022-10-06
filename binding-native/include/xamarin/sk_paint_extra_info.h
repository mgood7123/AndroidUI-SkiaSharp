/*
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_paint_extra_info_DEFINED
#define sk_paint_extra_info_DEFINED

#include "sk_xamarin.h"

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

typedef struct sk_paint_extra_info_t sk_paint_extra_info_t;

SK_X_API sk_paint_extra_info_t* sk_paint_extra_info_new(void);
SK_X_API sk_paint_extra_info_t* sk_paint_extra_info_new_with_font(const sk_font_t* font);
SK_X_API sk_paint_extra_info_t* sk_paint_extra_info_new_with_paint(const sk_paint_t* paint);
SK_X_API sk_paint_extra_info_t* sk_paint_extra_info_new_with_font_and_paint(const sk_font_t* font, const sk_paint_t* paint);
SK_X_API void sk_paint_extra_info_delete(sk_paint_extra_info_t* paint);
SK_X_API sk_paint_extra_info_t* sk_paint_extra_info_clone(const sk_paint_extra_info_t* paint);
SK_X_API void sk_paint_extra_info_reset(sk_paint_extra_info_t* paint);
SK_X_API sk_font_t* sk_paint_extra_info_clone_font(const sk_paint_extra_info_t* paint);
SK_X_API sk_paint_t* sk_paint_extra_info_clone_paint(const sk_paint_extra_info_t* paint);
SK_X_API sk_font_t* sk_paint_extra_info_get_font(sk_paint_extra_info_t* paint);
SK_X_API sk_paint_t* sk_paint_extra_info_get_paint(sk_paint_extra_info_t* paint);
SK_X_API void sk_paint_extra_info_set_text_align(sk_paint_extra_info_t* paint, sk_text_align_t align);
SK_X_API sk_text_align_t sk_paint_extra_info_get_text_align(const sk_paint_extra_info_t* paint);
SK_X_API void sk_paint_extra_info_set_text_encoding(sk_paint_extra_info_t* paint, sk_text_encoding_t encoding);
SK_X_API sk_text_encoding_t sk_paint_extra_info_get_text_encoding(const sk_paint_extra_info_t* paint);

SK_C_PLUS_PLUS_END_GUARD

#endif