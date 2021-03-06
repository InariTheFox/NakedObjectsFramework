// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Reflection;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;

namespace NakedObjects.Reflect.FacetFactory {
    /// <summary>
    ///     Creates an <see cref="IFinderActionFacet" /> based on the presence of an
    ///     <see cref="FinderActionAttribute" /> annotation
    /// </summary>
    public sealed class FinderActionFacetFactory : AnnotationBasedFacetFactoryAbstract {
        public FinderActionFacetFactory(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.Actions) { }

        private static void Process(MethodInfo member, ISpecification holder) {
            var attribute = member.GetCustomAttribute<FinderActionAttribute>();
            if (attribute != null) {
                FacetUtils.AddFacet(Create(attribute, holder));
            }
        }

        public override void Process(IReflector reflector, MethodInfo method, IMethodRemover methodRemover, ISpecificationBuilder specification) => Process(method, specification);

        private static IFacet Create(FinderActionAttribute attribute, ISpecification holder) => attribute == null ? null : new FinderActionFacet(holder);
    }
}