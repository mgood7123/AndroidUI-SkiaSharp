/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#ifndef sk_blender_DEFINED
#define sk_blender_DEFINED

#include "include/c/sk_types.h"

SK_C_PLUS_PLUS_BEGIN_GUARD

SK_C_API void sk_blender_unref(sk_blender_t* blender);
SK_C_API sk_blender_t* sk_blender_new_mode(sk_blendmode_t mode);
SK_C_API sk_blender_t* sk_blender_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePremul);
SK_C_API sk_data_t* sk_blender_serialize(const sk_blender_t* blender);
SK_C_API sk_blender_t* sk_blender_deserialize(const sk_data_t* data);

SK_C_PLUS_PLUS_END_GUARD

#endif
