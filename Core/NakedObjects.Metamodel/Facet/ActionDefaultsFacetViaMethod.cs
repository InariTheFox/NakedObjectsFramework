// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core.Util;

[assembly: InternalsVisibleTo("NakedObjects.ParallelReflector.Test")]
[assembly: InternalsVisibleTo("NakedObjects.Reflector.Test")]

namespace NakedObjects.Meta.Facet {
    [Serializable]
    public sealed class ActionDefaultsFacetViaMethod : ActionDefaultsFacetAbstract, IImperativeFacet {
        private readonly ILogger<ActionDefaultsFacetViaMethod> logger;
        private readonly MethodInfo method;

        public ActionDefaultsFacetViaMethod(MethodInfo method, ISpecification holder, ILogger<ActionDefaultsFacetViaMethod> logger)
            : base(holder) {
            this.method = method;
            this.logger = logger;
            MethodDelegate = LogNull(DelegateUtils.CreateDelegate(method), logger);
        }

        // for testing only 
        [field: NonSerialized]
        internal Func<object, object[], object> MethodDelegate { get; private set; }

        #region IImperativeFacet Members

        public MethodInfo GetMethod() => method;

        public Func<object, object[], object> GetMethodDelegate() => MethodDelegate;

        #endregion

        public override (object, TypeOfDefaultValue) GetDefault(INakedObjectAdapter nakedObjectAdapter) {
            // type safety is given by the reflector only identifying methods that match the 
            // parameter type
            var defaultValue = MethodDelegate(nakedObjectAdapter.GetDomainObject(), new object[] { });
            return (defaultValue, TypeOfDefaultValue.Explicit);
        }

        protected override string ToStringValues() => $"method={method}";

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => MethodDelegate = LogNull(DelegateUtils.CreateDelegate(method), logger);
    }

    // Copyright (c) Naked Objects Group Ltd.
}