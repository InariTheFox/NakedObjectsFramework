// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
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
    public class MemberOrderAnnotationFacetFactoryTest : AbstractFacetFactoryTest {
        private MemberOrderAnnotationFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof(IMemberOrderFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        #region Setup/Teardown

        [TestInitialize]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new MemberOrderAnnotationFacetFactory(0);
        }

        [TestCleanup]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion

        private class Customer {
            [MemberOrder(Sequence = "1")]
// ReSharper disable UnusedMember.Local
            public string FirstName {
                get { return null; }
            }
        }

        private class Customer1 {
            [MemberOrder(Sequence = "2")]
            public IList Orders {
                get { return null; }
            }

// ReSharper disable once UnusedParameter.Local
            public void AddToOrders(Order o) { }
        }

        private class Customer2 {
            [MemberOrder(Sequence = "3")]
            public void SomeAction() { }
        }

// ReSharper disable once ClassNeverInstantiated.Local
        private class Order { }

        [TestMethod]
        public override void TestFeatureTypes() {
            FeatureType featureTypes = facetFactory.FeatureTypes;
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.Objects));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Properties));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Collections));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Actions));
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.ActionParameters));
        }

        [TestMethod]
        public void TestMemberOrderAnnotationPickedUpOnAction() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            MethodInfo method = FindMethod(typeof(Customer2), "SomeAction");
            metamodel = facetFactory.Process(Reflector, method, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IMemberOrderFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MemberOrderFacet);
            var memberOrderFacetAnnotation = (MemberOrderFacet) facet;
            Assert.AreEqual("3", memberOrderFacetAnnotation.Sequence);
            AssertNoMethodsRemoved();
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestMemberOrderAnnotationPickedUpOnCollection() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer1), "Orders");
            metamodel = facetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IMemberOrderFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MemberOrderFacet);
            var memberOrderFacetAnnotation = (MemberOrderFacet) facet;
            Assert.AreEqual("2", memberOrderFacetAnnotation.Sequence);
            AssertNoMethodsRemoved();
            Assert.IsNotNull(metamodel);
        }

        [TestMethod]
        public void TestMemberOrderAnnotationPickedUpOnProperty() {
            IImmutableDictionary<string, ITypeSpecBuilder> metamodel = new Dictionary<string, ITypeSpecBuilder>().ToImmutableDictionary();

            PropertyInfo property = FindProperty(typeof(Customer), "FirstName");
            metamodel = facetFactory.Process(Reflector, property, MethodRemover, Specification, metamodel);
            IFacet facet = Specification.GetFacet(typeof(IMemberOrderFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MemberOrderFacet);
            var memberOrderFacetAnnotation = (MemberOrderFacet) facet;
            Assert.AreEqual("1", memberOrderFacetAnnotation.Sequence);
            AssertNoMethodsRemoved();
            Assert.IsNotNull(metamodel);
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
    // ReSharper restore UnusedMember.Local
}