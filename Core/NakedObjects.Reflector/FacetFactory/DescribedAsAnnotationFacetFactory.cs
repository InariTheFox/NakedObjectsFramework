// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core.Util;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;

namespace NakedObjects.Reflect.FacetFactory {
    public sealed class DescribedAsAnnotationFacetFactory : AnnotationBasedFacetFactoryAbstract {
        private readonly ILogger<DescribedAsAnnotationFacetFactory> logger;

        public DescribedAsAnnotationFacetFactory(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.Everything) =>
            logger = loggerFactory.CreateLogger<DescribedAsAnnotationFacetFactory>();

        public override void Process(IReflector reflector, Type type, IMethodRemover methodRemover, ISpecificationBuilder specification) {
            var attribute = type.GetCustomAttribute<DescriptionAttribute>() ?? (Attribute) type.GetCustomAttribute<DescribedAsAttribute>();
            FacetUtils.AddFacet(Create(attribute, specification));
        }

        private void Process(MemberInfo member, ISpecification holder) {
            var attribute = member.GetCustomAttribute<DescriptionAttribute>() ?? (Attribute) member.GetCustomAttribute<DescribedAsAttribute>();
            FacetUtils.AddFacet(Create(attribute, holder));
        }

        public override void Process(IReflector reflector, MethodInfo method, IMethodRemover methodRemover, ISpecificationBuilder specification) {
            Process(method, specification);
        }

        public override void Process(IReflector reflector, PropertyInfo property, IMethodRemover methodRemover, ISpecificationBuilder specification) {
            Process(property, specification);
        }

        public override void ProcessParams(IReflector reflector, MethodInfo method, int paramNum, ISpecificationBuilder holder) {
            var parameter = method.GetParameters()[paramNum];
            var attribute = parameter.GetCustomAttribute<DescriptionAttribute>() ?? (Attribute) parameter.GetCustomAttribute<DescribedAsAttribute>();
            FacetUtils.AddFacet(Create(attribute, holder));
        }

        private IDescribedAsFacet Create(Attribute attribute, ISpecification holder) =>
            attribute switch {
                null => null,
                DescribedAsAttribute asAttribute => Create(asAttribute, holder),
                DescriptionAttribute descriptionAttribute => Create(descriptionAttribute, holder),
                _ => throw new ArgumentException(logger.LogAndReturn($"Unexpected attribute type: {attribute.GetType()}"))
            };

        private static IDescribedAsFacet Create(DescribedAsAttribute attribute, ISpecification holder) => new DescribedAsFacetAnnotation(attribute.Value, holder);

        private static IDescribedAsFacet Create(DescriptionAttribute attribute, ISpecification holder) => new DescribedAsFacetAnnotation(attribute.Description, holder);
    }
}