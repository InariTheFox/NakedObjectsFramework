﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;

namespace NakedObjects.ParallelReflect.FacetFactory {
    public sealed class RemoveDynamicProxyMethodsFacetFactory : FacetFactoryAbstract {
        private static readonly string[] MethodsToRemove = {"GetBasePropertyValue", "SetBasePropertyValue", "SetChangeTracker"};

        public RemoveDynamicProxyMethodsFacetFactory(int numericOrder, ILoggerFactory loggerFactory)
            : base(numericOrder, loggerFactory, FeatureType.ObjectsInterfacesAndProperties) { }

        private static bool IsDynamicProxyType(Type type) => type.FullName?.StartsWith("System.Data.Entity.DynamicProxies", StringComparison.Ordinal) == true;

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, Type type, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            if (IsDynamicProxyType(type)) {
                foreach (var method in type.GetMethods().Join(MethodsToRemove, mi => mi.Name, s => s, (mi, s) => mi)) {
                    if (methodRemover != null && method != null) {
                        methodRemover.RemoveMethod(method);
                    }
                }
            }

            return metamodel;
        }

        public override IImmutableDictionary<string, ITypeSpecBuilder> Process(IReflector reflector, PropertyInfo property, IMethodRemover methodRemover, ISpecificationBuilder specification, IImmutableDictionary<string, ITypeSpecBuilder> metamodel) {
            if (IsDynamicProxyType(property.DeclaringType) && property.Name.Equals("RelationshipManager", StringComparison.Ordinal)) {
                FacetUtils.AddFacet(new HiddenFacet(WhenTo.Always, specification));
            }

            return metamodel;
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}