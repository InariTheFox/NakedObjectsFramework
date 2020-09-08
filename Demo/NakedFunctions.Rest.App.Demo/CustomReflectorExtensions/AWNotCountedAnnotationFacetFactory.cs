﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Immutable;
using System.Reflection;
using AdventureWorksModel;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;
using NakedObjects.Reflect.FacetFactory;

namespace NakedObjects.Rest.App.Demo.AWCustom {
    public sealed class AWNotCountedAnnotationFacetFactory : AnnotationBasedFacetFactoryAbstract {
        public AWNotCountedAnnotationFacetFactory(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.Collections) { }

        private static void Process(MemberInfo member, ISpecification holder) {
            var attribute = member.GetCustomAttribute<AWNotCountedAttribute>();
            FacetUtils.AddFacet(Create(attribute, holder));
        }

        public override void Process(IReflector reflector, PropertyInfo property, IMethodRemover methodRemover, ISpecificationBuilder specification) {
            Process(property, specification);
        }

        private static INotCountedFacet Create(AWNotCountedAttribute attribute, ISpecification holder) => attribute == null ? null : new NotCountedFacet(holder);
    }

    public sealed class AWNotCountedAnnotationFacetFactoryParallel : ParallelReflect.FacetFactory.AnnotationBasedFacetFactoryAbstract {
        public AWNotCountedAnnotationFacetFactoryParallel(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.Collections) { }

        private static void Process(MemberInfo member, ISpecification holder) {
            var attribute = member.GetCustomAttribute<AWNotCountedAttribute>();
            FacetUtils.AddFacet(Create(attribute, holder));
        }

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, PropertyInfo property, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            Process(property, specification);
            return metamodel;
        }

        private static INotCountedFacet Create(AWNotCountedAttribute attribute, ISpecification holder) => attribute == null ? null : new NotCountedFacet(holder);
    }
}