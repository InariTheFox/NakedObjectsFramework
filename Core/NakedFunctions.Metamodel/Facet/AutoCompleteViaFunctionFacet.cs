// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core;
using NakedObjects.Core.Util;
using NakedObjects.Core.Util.Query;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;

namespace NakedFunctions.Meta.Facet {
    [Serializable]
    public sealed class AutoCompleteViaFunctionFacet : FacetAbstract, IAutoCompleteFacet, IImperativeFacet {
        private const int DefaultPageSize = 50;
        private readonly MethodInfo method;

        private AutoCompleteViaFunctionFacet(ISpecification holder)
            : base(Type, holder) { }

        public AutoCompleteViaFunctionFacet(MethodInfo autoCompleteMethod, int pageSize, int minLength,
                                            ISpecification holder)
            : this(holder) {
            method = autoCompleteMethod;
            PageSize = pageSize == 0 ? DefaultPageSize : pageSize;
            MinLength = minLength;
        }

        public static Type Type => typeof(IAutoCompleteFacet);

        public int PageSize { get; }

        protected override string ToStringValues() => $"method={method}";

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) { }

        #region IAutoCompleteFacet Members

        public int MinLength { get; }

        public object[] GetCompletions(INakedObjectAdapter inObjectAdapter, string autoCompleteParm, ISession session, IObjectPersistor persistor) {
            try {
                var autoComplete = method.Invoke(null, method.GetParameterValues(inObjectAdapter, autoCompleteParm, session, persistor));

                //returning an IQueryable
                var queryable = autoComplete as IQueryable;
                if (queryable != null) {
                    return queryable.Take(PageSize).ToArray();
                }

                //returning an IEnumerable (of string only)
                var strings = autoComplete as IEnumerable<string>;
                if (strings != null) {
                    return strings.Cast<object>().ToArray();
                }

                //return type is a single object
                if (!CollectionUtils.IsCollection(autoComplete.GetType())) {
                    return new[] {autoComplete};
                }

                throw new NakedObjectDomainException($"Must return IQueryable or a single object from autoComplete method: {method.Name}");
            }
            catch (ArgumentException ae) {
                throw new InvokeException($"autoComplete exception: {method.Name} has mismatched parameter type - must be string", ae);
            }
        }

        #endregion

        #region IImperativeFacet Members

        public MethodInfo GetMethod() => method;

        public Func<object, object[], object> GetMethodDelegate() => null;

        #endregion
    }

    // Copyright (c) Naked Objects Group Ltd.
}