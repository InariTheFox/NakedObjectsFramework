// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Core.Util;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;
using NakedObjects.Util;

namespace NakedObjects.ParallelReflect.FacetFactory {
    public sealed class RegExAnnotationFacetFactory : AnnotationBasedFacetFactoryAbstract {
        private readonly ILogger<RegExAnnotationFacetFactory> logger;

        public RegExAnnotationFacetFactory(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.ObjectsInterfacesPropertiesAndActionParameters) =>
            logger = loggerFactory.CreateLogger<RegExAnnotationFacetFactory>();

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, Type type, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            var attribute = type.GetCustomAttribute<RegularExpressionAttribute>() ?? (Attribute) type.GetCustomAttribute<RegExAttribute>();
            FacetUtils.AddFacet(Create(attribute, specification));
            return metamodel;
        }

        private void Process(MemberInfo member, ISpecification holder) {
            var attribute = member.GetCustomAttribute<RegularExpressionAttribute>() ?? (Attribute) member.GetCustomAttribute<RegExAttribute>();
            FacetUtils.AddFacet(Create(attribute, holder));
        }

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, MethodInfo method, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            if (TypeUtils.IsString(method.ReturnType)) {
                Process(method, specification);
            }

            return metamodel;
        }

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, PropertyInfo property, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            if (property.GetGetMethod() != null && TypeUtils.IsString(property.PropertyType)) {
                Process(property, specification);
            }

            return metamodel;
        }

        public override IImmutableDictionary<string, ITypeSpecBuilder> ProcessParams(IReflector reflector, MethodInfo method, int paramNum, ISpecificationBuilder holder, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            var parameter = method.GetParameters()[paramNum];
            if (TypeUtils.IsString(parameter.ParameterType)) {
                var attribute = parameter.GetCustomAttribute<RegularExpressionAttribute>() ?? (Attribute) parameter.GetCustomAttribute<RegExAttribute>();
                FacetUtils.AddFacet(Create(attribute, holder));
            }

            return metamodel;
        }

        private IRegExFacet Create(Attribute attribute, ISpecification holder) =>
            attribute switch {
                null => null,
                RegularExpressionAttribute expressionAttribute => Create(expressionAttribute, holder),
                RegExAttribute exAttribute => Create(exAttribute, holder),
                _ => throw new ArgumentException(logger.LogAndReturn($"Unexpected attribute type: {attribute.GetType()}"))
            };

        private static IRegExFacet Create(RegExAttribute attribute, ISpecification holder) => new RegExFacet(attribute.Validation, attribute.Format, attribute.CaseSensitive, attribute.Message, holder);

        private static IRegExFacet Create(RegularExpressionAttribute attribute, ISpecification holder) => new RegExFacet(attribute.Pattern, string.Empty, true, attribute.ErrorMessage, holder);
    }
}