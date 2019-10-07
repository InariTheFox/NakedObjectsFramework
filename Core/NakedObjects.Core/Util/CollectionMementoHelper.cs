﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core.Adapter;

namespace NakedObjects.Core.Util {
    public static class CollectionMementoHelper {
        public static bool IsPaged(this INakedObjectAdapter nakedObjectAdapter) {
            var oid = nakedObjectAdapter.Oid as ICollectionMemento;
            return oid != null && oid.IsPaged;
        }

        public static bool IsNotQueryable(this INakedObjectAdapter nakedObjectAdapter) {
            var oid = nakedObjectAdapter.Oid as ICollectionMemento;
            return oid != null && oid.IsNotQueryable;
        }

        public static void SetNotQueryable(this INakedObjectAdapter nakedObjectAdapter, bool isNotQueryable) {
            var oid = nakedObjectAdapter.Oid as ICollectionMemento;
            if (oid != null) {
                oid.IsNotQueryable = isNotQueryable;
            }
        }

        // for test purposes only 
        public static ICollectionMemento TestMemento(ILifecycleManager lifecycleManager, INakedObjectManager nakedObjectManager, IMetamodelManager metamodelManager, INakedObjectAdapter target, IActionSpec actionSpec, INakedObjectAdapter[] parameters) {
            return new CollectionMemento(lifecycleManager, nakedObjectManager, metamodelManager, target, actionSpec, parameters);
        }

        // for test purposes only 
        public static ICollectionMemento TestMemento(ILifecycleManager lifecycleManager, INakedObjectManager nakedObjectManager, IMetamodelManager metamodelManager, string[] strings) {
            return new CollectionMemento(lifecycleManager, nakedObjectManager, metamodelManager, strings);
        }
    }
}