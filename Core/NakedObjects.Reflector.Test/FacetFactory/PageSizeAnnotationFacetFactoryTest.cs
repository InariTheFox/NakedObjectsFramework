// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Meta.Adapter;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.SpecImmutable;
using NakedObjects.Reflect.FacetFactory;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace NakedObjects.Reflect.Test.FacetFactory {
    [TestClass]
    public class PageSizeAnnotationFacetFactoryTest : AbstractFacetFactoryTest {
        private PageSizeAnnotationFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof(IPageSizeFacet)}; }
        }

        protected override IFacetFactory FacetFactory => facetFactory;

        [TestMethod]
        public void TestDefaultPageSizePickedUp() {
            var actionMethod = FindMethod(typeof(Customer1), "SomeAction");
            var identifier = new IdentifierImpl("Customer1", "SomeAction");
            var actionPeer = ImmutableSpecFactory.CreateActionSpecImmutable(identifier, null, null);
            new FallbackFacetFactory(0, null).Process(Reflector, actionMethod, MethodRemover, actionPeer);
            var facet = actionPeer.GetFacet(typeof(IPageSizeFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is PageSizeFacetDefault);
            var pageSizeFacet = (IPageSizeFacet) facet;
            Assert.AreEqual(20, pageSizeFacet.Value);
            AssertNoMethodsRemoved();
        }

        [TestMethod]
        public override void TestFeatureTypes() {
            var featureTypes = facetFactory.FeatureTypes;
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.Objects));
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.Properties));
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.Collections));
            Assert.IsTrue(featureTypes.HasFlag(FeatureType.Actions));
            Assert.IsFalse(featureTypes.HasFlag(FeatureType.ActionParameters));
        }

        [TestMethod]
        public void TestPageSizeAnnotationPickedUp() {
            var actionMethod = FindMethod(typeof(Customer), "SomeAction");
            facetFactory.Process(Reflector, actionMethod, MethodRemover, Specification);
            var facet = Specification.GetFacet(typeof(IPageSizeFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is PageSizeFacetAnnotation);
            var pageSizeFacet = (IPageSizeFacet) facet;
            Assert.AreEqual(7, pageSizeFacet.Value);
            AssertNoMethodsRemoved();
        }

        #region Nested type: Customer

        private class Customer {
            [PageSize(7)]
// ReSharper disable once UnusedMember.Local
            public IQueryable<Customer> SomeAction() => null;
        }

        #endregion

        #region Nested type: Customer1

        private class Customer1 {
// ReSharper disable once UnusedMember.Local
            public IQueryable<Customer1> SomeAction() => null;
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new PageSizeAnnotationFacetFactory(0, LoggerFactory);
        }

        [TestCleanup]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion
    }

    // Copyright (c) Naked Objects Group Ltd.
}