/*
 * Copyright 2014 Google Inc.
 * Copyright 2015 Xamarin Inc.
 * Copyright 2017 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/core/SkSerialProcs.h"
#include "include/core/SkTextBlob.h"

#include "include/c/sk_textblob.h"

#include "src/c/sk_types_priv.h"

// serialization
#include "src/core/SkPictureData.h"

#include "include/core/SkImageGenerator.h"
#include "include/core/SkTypeface.h"
#include "include/private/SkTo.h"
#include "src/core/SkAutoMalloc.h"
#include "src/core/SkPicturePriv.h"
#include "src/core/SkPictureRecord.h"
#include "src/core/SkReadBuffer.h"
#include "src/core/SkTextBlobPriv.h"
#include "src/core/SkVerticesPriv.h"
#include "src/core/SkWriteBuffer.h"

void sk_textblob_ref(const sk_textblob_t* blob) {
    SkSafeRef(AsTextBlob(blob));
}

void sk_textblob_unref(const sk_textblob_t* blob) {
    SkSafeUnref(AsTextBlob(blob));
}

uint32_t sk_textblob_get_unique_id(const sk_textblob_t* blob) {
    return AsTextBlob(blob)->uniqueID();
}

void sk_textblob_get_bounds(const sk_textblob_t* blob, sk_rect_t* bounds) {
    *bounds = ToRect(AsTextBlob(blob)->bounds());
}

int sk_textblob_get_intercepts(const sk_textblob_t* blob, const float bounds[2], float intervals[], const sk_paint_t* paint) {
    return AsTextBlob(blob)->getIntercepts(bounds, intervals, AsPaint(paint));
}

// serialization and deserialization requires SkSerialProcs
// skia uses empty proc by default

sk_data_t* sk_textblob_serialize(const sk_textblob_t* blob) {
    SkSerialProcs procs;

    // We serialize all typefaces into the typeface section of the top-level picture.
    SkRefCntSet localTypefaceSet;

    SkFactorySet factSet;  // buffer refs factSet, so factSet must come first.
    SkBinaryWriteBuffer buffer;
    buffer.setFactoryRecorder(sk_ref_sp(&factSet));
    buffer.setSerialProcs(procs);
    buffer.setTypefaceRecorder(sk_ref_sp(&localTypefaceSet));

    SkTextBlobPriv::Flatten(*AsTextBlob(blob), buffer);

    // at this point our typefaceSet contains data, write it to stream

    SkDynamicMemoryWStream stream;
    // deserialization seems to require the buffer set a version
    // give it skia's current skp version
    stream.write32(SkToU32(SkPicturePriv::kCurrent_Version));

    int count = localTypefaceSet.count();

    SkASSERT(count == 1);

    SkAutoSTMalloc<16, SkTypeface*> storage(count);
    SkTypeface** array = (SkTypeface**)storage.get();
    localTypefaceSet.copyToArray((SkRefCnt**)array);

    array[0]->serialize(&stream);

    stream.write32(SkToU32(buffer.bytesWritten()));
    buffer.writeToStream(&stream);

    return ToData(stream.detachAsData().release());
}

sk_textblob_t* sk_textblob_deserialize(const sk_data_t* data) {
    if (data == nullptr) return nullptr;
    SkDeserialProcs procs;
    const SkData* skdata = AsData(data);
    SkMemoryStream stream(skdata->data(), skdata->size());

    uint32_t size;

    SkTypefacePlayback                 fTFPlayback;

    uint32_t version;
    if (!stream.readU32(&version)) { // the skia skp version this was serialized with
        return nullptr;
    }

    // deserialize typeface
    size = 1;
    fTFPlayback.setCount(size);
    for (uint32_t i = 0; i < size; ++i) {
        sk_sp<SkTypeface> tf;
        tf = SkTypeface::MakeDeserialize(&stream);
        if (!tf) {    // failed to deserialize
            // fTFPlayback asserts it never has a null, so we plop in
            // the default here.
            tf = SkTypeface::MakeDefault();
        }
        fTFPlayback[i] = std::move(tf);
    }

    // deserialize buffer
    if (!stream.readU32(&size)) { // read buffer length
        return nullptr;
    }
    SkAutoMalloc storage(size);
    if (stream.read(storage.get(), size) != size) {
        return nullptr;
    }

    // buffer has been deserialized, it contains 1 text blob

    SkReadBuffer buffer(storage.get(), size);

    buffer.setVersion(version);
    buffer.setDeserialProcs(procs);
    fTFPlayback.setupBuffer(buffer);

    return ToTextBlob(const_cast<SkTextBlob*>(SkTextBlobPriv::MakeFromBuffer(buffer).release()));
}


sk_textblob_builder_t* sk_textblob_builder_new(void) {
    return ToTextBlobBuilder(new SkTextBlobBuilder());
}

void sk_textblob_builder_delete(sk_textblob_builder_t* builder) {
    delete AsTextBlobBuilder(builder);
}

sk_textblob_t* sk_textblob_builder_make(sk_textblob_builder_t* builder) {
    return ToTextBlob(AsTextBlobBuilder(builder)->make().release());
}

void sk_textblob_builder_alloc_run(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float x, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer) {
    *runbuffer = ToTextBlobBuilderRunBuffer(AsTextBlobBuilder(builder)->allocRun(AsFont(*font), count, x, y, AsRect(bounds)));
}

void sk_textblob_builder_alloc_run_pos_h(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer) {
    *runbuffer = ToTextBlobBuilderRunBuffer(AsTextBlobBuilder(builder)->allocRunPosH(AsFont(*font), count, y, AsRect(bounds)));
}

void sk_textblob_builder_alloc_run_pos(sk_textblob_builder_t* builder, const sk_font_t* font, int count, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer) {
    *runbuffer = ToTextBlobBuilderRunBuffer(AsTextBlobBuilder(builder)->allocRunPos(AsFont(*font), count, AsRect(bounds)));
}

void sk_textblob_builder_alloc_run_rsxform(sk_textblob_builder_t* builder, const sk_font_t* font, int count, sk_textblob_builder_runbuffer_t* runbuffer) {
    *runbuffer = ToTextBlobBuilderRunBuffer(AsTextBlobBuilder(builder)->allocRunRSXform(AsFont(*font), count));
}
