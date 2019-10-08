// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Reflection;
using System.Runtime.Serialization;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core.Util;

namespace NakedObjects.Meta.Facet {
    [Serializable]
    public sealed class PropertySetterFacetViaModifyMethod : PropertySetterFacetAbstract, IImperativeFacet {
        private readonly MethodInfo method;

        [field: NonSerialized] private Func<object, object[], object> methodDelegate;

        public PropertySetterFacetViaModifyMethod(MethodInfo method, string name, ISpecification holder)
            : base(holder) {
            this.method = method;
            methodDelegate = DelegateUtils.CreateDelegate(method);
            PropertyName = name;
        }

        public override string PropertyName { get; protected set; }

        #region IImperativeFacet Members

        public MethodInfo GetMethod() {
            return method;
        }

        public Func<object, object[], object> GetMethodDelegate() {
            return methodDelegate;
        }

        #endregion

        public override void SetProperty(INakedObjectAdapter inObjectAdapter, INakedObjectAdapter value, ITransactionManager transactionManager, ISession session, ILifecycleManager lifecycleManager) {
            methodDelegate(inObjectAdapter.GetDomainObject(), new[] {value.GetDomainObject()});
        }

        protected override string ToStringValues() {
            return "method=" + method;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) {
            methodDelegate = DelegateUtils.CreateDelegate(method);
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}