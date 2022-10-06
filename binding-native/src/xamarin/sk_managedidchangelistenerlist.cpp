/*
 * Copyright 2022 Microsoft Corporation. All rights reserved.
 *
 * Use of this source code is governed by a BSD-style license that can be
 * found in the LICENSE file.
 */

#include "include/xamarin/sk_managedidchangelistener.h"
#include "include/xamarin/SkManagedIDChangeListener.h"

#include "include/xamarin/sk_managedidchangelistenerlist.h"
#include "include/xamarin/SkManagedIDChangeListenerList.h"

#include "src/c/sk_types_priv.h"

static inline SkManagedIDChangeListener* AsSkManagedIDChangeListener(sk_idchangelistener_t* d) {
    return reinterpret_cast<SkManagedIDChangeListener*>(d);
}
static inline SkManagedIDChangeListenerList* AsSkManagedIDChangeListenerList(sk_idchangelistenerlist_t* d) {
    return reinterpret_cast<SkManagedIDChangeListenerList*>(d);
}
static inline sk_idchangelistenerlist_t* ToSkManagedIDChangeListenerList(SkManagedIDChangeListenerList* d) {
    return reinterpret_cast<sk_idchangelistenerlist_t*>(d);
}

static sk_idchangelistenerlist_procs_t gProcs;

// prototype
void destroy_List(SkManagedIDChangeListenerList* d, void* context);
void destroy_List(SkManagedIDChangeListenerList* d, void* context) {
    if (gProcs.fDestroy) {
        gProcs.fDestroy(ToSkManagedIDChangeListenerList(d), context);
    }
}

sk_idchangelistenerlist_t* sk_managedidchangelistenerlist_new(void* context) {
    return ToSkManagedIDChangeListenerList(new SkManagedIDChangeListenerList(context));
}

void sk_managedidchangelistenerlist_delete(sk_idchangelistenerlist_t* d) {
    delete AsSkManagedIDChangeListenerList(d);
}

void sk_managedidchangelistenerlist_add(sk_idchangelistenerlist_t* d, sk_idchangelistener_t* listener) {
    AsSkManagedIDChangeListenerList(d)->add(sk_sp<SkManagedIDChangeListener>(AsSkManagedIDChangeListener(listener)));
}

int32_t sk_managedidchangelistenerlist_count(sk_idchangelistenerlist_t* d) {
    return AsSkManagedIDChangeListenerList(d)->count();
}

void sk_managedidchangelistenerlist_changed(sk_idchangelistenerlist_t* d) {
    AsSkManagedIDChangeListenerList(d)->changed();
}

void sk_managedidchangelistenerlist_reset(sk_idchangelistenerlist_t* d) {
    AsSkManagedIDChangeListenerList(d)->reset();
}

void sk_managedidchangelistenerlist_set_procs(sk_idchangelistenerlist_procs_t procs) {
    gProcs = procs;

    SkManagedIDChangeListenerList::Procs p;
    p.fDestroy = destroy_List;

    SkManagedIDChangeListenerList::setProcs(p);
}
