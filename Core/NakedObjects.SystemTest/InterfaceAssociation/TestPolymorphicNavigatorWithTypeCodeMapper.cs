﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Linq;
using NakedObjects.Persistor.Entity.Configuration;
using NakedObjects.Services;
using NakedObjects.SystemTest.PolymorphicAssociations;
using NUnit.Framework;

namespace NakedObjects.SystemTest.PolymorphicNavigator {
    [TestFixture]
    public class TestPolymorphicNavigatorWithTypeCodeMapper : TestPolymorphicNavigatorAbstract {
        //private static bool fixturesRun;

        [SetUp]
        public void SetUp() {
            StartTest();
        }

        [TearDown]
        public void TearDown() {
            base.EndTest();
        }

        protected override string[] Namespaces {
            get { return new[] {typeof(PolymorphicPayment).Namespace}; }
        }

        private const string databaseName = "TestPolymorphicNavigatorWithTypeCodeMapper";

        protected override EntityObjectStoreConfiguration Persistor {
            get {
                var config = new EntityObjectStoreConfiguration {EnforceProxies = false};
                config.UsingCodeFirstContext(() => new PolymorphicNavigationContext(databaseName));
                return config;
            }
        }

        //protected override void RegisterTypes(IServiceCollection services)
        //{
        //    base.RegisterTypes(services);
        //    var config = new EntityObjectStoreConfiguration { EnforceProxies = false };
        //    config.UsingCodeFirstContext(() => new PolymorphicNavigationContext(databaseName));
        //    services.AddSingleton<IEntityObjectStoreConfiguration>(config);
        //    services.AddSingleton<IMenuFactory, NullMenuFactory>();
        //}

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            //PolymorphicNavigationContext.Delete(databaseName);
            InitializeNakedObjectsFramework(this);
            RunFixtures();
        }

        [OneTimeTearDown]
        public void DeleteDatabase() {
            CleanupNakedObjectsFramework(this);
        }

        protected override object[] Fixtures {
            get { return new object[] {new FixtureEntities(), new FixtureLinksUsingTypeCode()}; }
        }

        protected override Type[] Services => base.Services.Union(new[] {typeof(Services.PolymorphicNavigator), typeof(SimpleTypeCodeMapper)}).ToArray();

        [Test]
        public override void AttemptSetPolymorphicPropertyWithATransientAssociatedObject() {
            base.AttemptSetPolymorphicPropertyWithATransientAssociatedObject();
        }

        [Test]
        //[Ignore("investigate")]
        public override void AttemptToAddSameItemTwice() {
            base.AttemptToAddSameItemTwice();
        }

        [Test]
        public override void AttemptToRemoveNonExistentItem() {
            base.AttemptToRemoveNonExistentItem();
        }

        [Test]
        public void ChangePolymorphicPropertyOnPersistentObject() {
            ChangePolymorphicPropertyOnPersistentObject("CUS", "SUP");
        }

        [Test]
        //[Ignore("investigate")]
        public override void ClearPolymorphicProperty() {
            base.ClearPolymorphicProperty();
        }

        [Test]
        //[Ignore("investigate")]
        public override void FindOwnersForObject() {
            base.FindOwnersForObject();
        }

        [Test]
        public void PolymorphicCollectionAddDifferentItems() {
            base.PolymorphicCollectionAddDifferentItems("INV", "EXP");
        }

        [Test]
        public void PolymorphicCollectionAddMutlipleItemsOfOneType() {
            base.PolymorphicCollectionAddMutlipleItemsOfOneType("INV");
        }

        [Test]
        //[Ignore("investigate")]
        public override void RemoveItem() {
            base.RemoveItem();
        }

        [Test]
        public void SetPolymorphicPropertyOnPersistentObject() {
            base.SetPolymorphicPropertyOnPersistentObject("CUS");
        }

        [Test]
        public void SetPolymorphicPropertyOnTransientObject() {
            base.SetPolymorphicPropertyOnTransientObject("CUS");
        }
    }

    public class SimpleTypeCodeMapper : ITypeCodeMapper {
        #region ITypeCodeMapper Members

        public Type TypeFromCode(string code) {
            if (code == "CUS") { return typeof(CustomerAsPayee); }

            if (code == "SUP") { return typeof(SupplierAsPayee); }

            if (code == "INV") { return typeof(InvoiceAsPayableItem); }

            if (code == "EXP") { return typeof(ExpenseClaimAsPayableItem); }

            throw new DomainException("Code not recognised: " + code);
        }

        public string CodeFromType(Type type) {
            if (type == typeof(CustomerAsPayee)) { return "CUS"; }

            if (type == typeof(SupplierAsPayee)) { return "SUP"; }

            if (type == typeof(InvoiceAsPayableItem)) { return "INV"; }

            if (type == typeof(ExpenseClaimAsPayableItem)) { return "EXP"; }

            throw new DomainException("Type not recognised: " + type);
        }

        #endregion
    }
}