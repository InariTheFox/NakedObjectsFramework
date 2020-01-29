// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Meta.Facet;
using NakedObjects.ParallelReflect.FacetFactory;

namespace NakedObjects.ParallelReflect.Test.FacetFactory {
    [TestClass]
    public class EagerlyAnnotationFacetFactoryTest : AbstractFacetFactoryTest {
        private EagerlyAnnotationFacetFactory annotationFacetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof(IEagerlyFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return annotationFacetFactory; }
        }

        #region Setup/Teardown

        [TestInitialize]
        public override void SetUp() {
            base.SetUp();
            annotationFacetFactory = new EagerlyAnnotationFacetFactory(0);
        }

        [TestCleanup]
        public override void TearDown() {
            annotationFacetFactory = null;
            base.TearDown();
        }

        #endregion

        private class Customer1 {
            [Eagerly(EagerlyAttribute.Do.Rendering)]
// ReSharper disable UnusedMember.Local
            public int Prop { get; set; }

            [Eagerly(EagerlyAttribute.Do.Rendering)]
            public IList<Customer1> Coll { get; set; }

            [Eagerly(EagerlyAttribute.Do.Rendering)]
            public IList<Customer1> Act() {
                return new List<Customer1>();
            }
        }

        [Eagerly(EagerlyAttribute.Do.Rendering)]
        private class Customer2 {
            public int Prop { get; set; }
            public IList<Customer1> Coll { get; set; }

            public IList<Customer1> Act() {
                return new List<Customer1>();
            }
        }

        [TestMethod]
        public void TestEagerlyAnnotationPickedUpOnClass() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            metamodel = annotationFacetFactory.Process(Reflector, typeof(Customer2), MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is EagerlyFacet);
            var propertyDefaultFacetAnnotation = (EagerlyFacet) facet;
            Assert.AreEqual(EagerlyAttribute.Do.Rendering, propertyDefaultFacetAnnotation.What);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyAnnotationPickedUpOnCollection() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer1), "Coll");
            metamodel = annotationFacetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is EagerlyFacet);
            var propertyDefaultFacetAnnotation = (EagerlyFacet) facet;
            Assert.AreEqual(EagerlyAttribute.Do.Rendering, propertyDefaultFacetAnnotation.What);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyAnnotationPickedUpOnMethod() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            MethodInfo method = FindMethod(typeof(Customer1), "Act");
            metamodel = annotationFacetFactory.Process(Reflector, method, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is EagerlyFacet);
            var propertyDefaultFacetAnnotation = (EagerlyFacet) facet;
            Assert.AreEqual(EagerlyAttribute.Do.Rendering, propertyDefaultFacetAnnotation.What);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyAnnotationPickedUpOnProperty() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer1), "Prop");
            metamodel = annotationFacetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is EagerlyFacet);
            var propertyDefaultFacetAnnotation = (EagerlyFacet) facet;
            Assert.AreEqual(EagerlyAttribute.Do.Rendering, propertyDefaultFacetAnnotation.What);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyNotPickedUpOnClass() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            metamodel = annotationFacetFactory.Process(Reflector, typeof(Customer1), MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNull(facet);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyNotPickedUpOnCollection() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer2), "Coll");
            metamodel = annotationFacetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNull(facet);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyNotPickedUpOnMethod() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            MethodInfo method = FindMethod(typeof(Customer2), "Act");
            metamodel = annotationFacetFactory.Process(Reflector, method, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNull(facet);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestEagerlyNotPickedUpOnProperty() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer2), "Prop");
            metamodel = annotationFacetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IEagerlyFacet));
            Assert.IsNull(facet);
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public override void TestFeatureTypes() {
            FeatureType featureTypes = annotationFacetFactory.FeatureTypes;
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Objects));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Properties));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Collections));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Actions));
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.ActionParameters));
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
    // ReSharper restore UnusedMember.Local
}