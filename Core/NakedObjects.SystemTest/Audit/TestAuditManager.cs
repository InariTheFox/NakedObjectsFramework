﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using NakedObjects.Architecture.Component;
using NakedObjects.Audit;
using NakedObjects.Core.Util;
using NakedObjects.Meta.Audit;
using NakedObjects.Services;
using NakedObjects.Util;
using NakedObjects.Xat;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace NakedObjects.SystemTest.Audit
{
    [TestFixture]
    public class TestAuditManager : AbstractSystemTest<AuditDbContext>
    {
        #region Run Configuration

        protected override Type[] Types => new[] {
            typeof(MyDefaultAuditor),
            typeof(FooAuditor),
            typeof(QuxAuditor),
            typeof(QueryableList<Foo>)
        };

        protected override Type[] Services => new[] {
            typeof(SimpleRepository<Foo>),
            typeof(SimpleRepository<Bar>),
            typeof(SimpleRepository<Qux>), typeof(FooService),
            typeof(BarService), typeof(QuxService)
        };

        protected override string[] Namespaces => new[] {typeof(Foo).Namespace};

        protected override void RegisterTypes(IServiceCollection services)
        {
            base.RegisterTypes(services);
            var config = new AuditConfiguration<MyDefaultAuditor>();
            config.AddNamespaceAuditor<FooAuditor>(typeof(Foo).FullName);
            config.AddNamespaceAuditor<QuxAuditor>(typeof(Qux).FullName);

            services.AddSingleton<IAuditConfiguration>(config);
            services.AddSingleton<IFacetDecorator, AuditManager>();
        }

        #endregion

        private static void UnexpectedCall(string auditor)
        {
            Assert.Fail("Unexpected call to {0} auditor", auditor);
        }

        public static Action<IPrincipal, object> UnexpectedCallback(string auditor)
        {
            return (p, o) => UnexpectedCall(auditor);
        }

        public static Action<IPrincipal, string, object, bool, object[]> UnexpectedActionCallback(string auditor)
        {
            return (p, a, o, b, pp) => UnexpectedCall(auditor);
        }

        public static Action<IPrincipal, string, string, bool, object[]> UnexpectedServiceActionCallback(string auditor)
        {
            return (p, a, s, b, pp) => UnexpectedCall(auditor);
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorAction()
        {
            MyDefaultAuditor.SetCallbacksExpected();

            ITestObject foo = GetTestService(typeof(SimpleRepository<Foo>)).GetAction("New Instance").InvokeReturnObject();

            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnAction", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().FullName);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                fooCalledCount++;
            };

            foo.GetAction("An Action").InvokeReturnObject();
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorServiceAction()
        {
            ITestService foo = GetTestService("Foo Service");

            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnAction", a);
                Assert.AreEqual("Foo Service", s);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                fooCalledCount++;
            };

            foo.GetAction("An Action").InvokeReturnObject();
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorQueryOnlyAction()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();

            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AQueryOnlyAction", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().FullName);
                Assert.IsTrue(b);
                Assert.AreEqual(0, pp.Length);
                fooCalledCount++;
            };

            foo.GetAction("A Query Only Action").InvokeReturnObject();
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorImplicitQueryOnlyAction()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();

            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnotherQueryOnlyAction", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().FullName);
                Assert.IsTrue(b);
                Assert.AreEqual(0, pp.Length);
                fooCalledCount++;
            };

            foo.GetAction("Another Query Only Action").InvokeReturnCollection();
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorQueryOnlyServiceAction()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestService foo = GetTestService("Foo Service");

            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AQueryOnlyAction", a);
                Assert.AreEqual("Foo Service", s);
                Assert.IsTrue(b);
                Assert.AreEqual(0, pp.Length);
                fooCalledCount++;
            };

            foo.GetAction("A Query Only Action").InvokeReturnObject();
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorActionWithParm()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnActionWithParm", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().FullName);
                Assert.IsFalse(b);
                Assert.AreEqual(1, pp.Length);
                Assert.AreEqual(1, pp[0]);
                fooCalledCount++;
            };

            foo.GetAction("An Action With Parm").InvokeReturnObject(1);
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorServiceActionWithParm()
        {
            ITestService foo = GetTestService("Foo Service");
            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnActionWithParm", a);
                Assert.AreEqual("Foo Service", s);
                Assert.IsFalse(b);
                Assert.AreEqual(1, pp.Length);
                Assert.AreEqual(1, pp[0]);
                fooCalledCount++;
            };

            foo.GetAction("An Action With Parm").InvokeReturnObject(1);
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorActionWithParms()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnActionWithParms", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().FullName);
                Assert.IsFalse(b);
                Assert.AreEqual(2, pp.Length);
                Assert.AreEqual(1, pp[0]);
                Assert.AreSame(foo.NakedObject.Object, pp[1]);
                fooCalledCount++;
            };

            foo.GetAction("An Action With Parms").InvokeReturnObject(1, foo.NakedObject.Object);
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorServiceActionWithParms()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestService foo = GetTestService("Foo Service");
            ITestObject fooObj = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooCalledCount = 0;

            FooAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnActionWithParms", a);
                Assert.AreEqual("Foo Service", s);
                Assert.IsFalse(b);
                Assert.AreEqual(2, pp.Length);
                Assert.AreEqual(1, pp[0]);
                Assert.AreSame(fooObj.NakedObject.Object, pp[1]);
                fooCalledCount++;
            };

            foo.GetAction("An Action With Parms").InvokeReturnObject(1, fooObj);
            Assert.AreEqual(1, fooCalledCount, "expect foo auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorPersist()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int fooPersistedCount = 0;

            string newValue = Guid.NewGuid().ToString();

            FooAuditor.Auditor.objectPersistedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().GetProxiedType().FullName);
                Assert.IsNull(((Foo)o).Prop1);
                fooPersistedCount++;
            };

            foo.Save();

          

            Assert.AreEqual(1, fooPersistedCount, "expect foo auditor to be called for persists");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorUpdate()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            FooAuditor.SetCallbacksExpected();
            ITestObject foo = GetTestService("Foos").GetAction("New Instance").InvokeReturnObject();
            foo.Save();
            MyDefaultAuditor.SetCallbacksUnexpected();
            FooAuditor.SetCallbacksUnexpected();


            int fooUpdatedCount = 0;

            string newValue = Guid.NewGuid().ToString();

            FooAuditor.Auditor.objectUpdatedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Foo", o.GetType().GetProxiedType().FullName);
                Assert.AreEqual(newValue, ((Foo)o).Prop1);
                fooUpdatedCount++;
            };

            NakedObjectsFramework.TransactionManager.StartTransaction();

            foo.GetPropertyByName("Prop1").SetValue(newValue);

            NakedObjectsFramework.TransactionManager.EndTransaction();

            Assert.AreEqual(1, fooUpdatedCount, "expect foo auditor to be called for updates");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorActionQux()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject qux = GetTestService("Quxes").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int quxCalledCound = 0;

            QuxAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnAction", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Qux", o.GetType().FullName);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                quxCalledCound++;
            };

            qux.GetAction("An Action").InvokeReturnObject();
            Assert.AreEqual(1, quxCalledCound, "expect qux auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorServiceActionQux()
        {
            ITestService qux = GetTestService("Qux Service");
            MyDefaultAuditor.SetCallbacksUnexpected();

            int quxCalledCound = 0;

            QuxAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnAction", a);
                Assert.AreEqual("Qux Service", s);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                quxCalledCound++;
            };

            qux.GetAction("An Action").InvokeReturnObject();
            Assert.AreEqual(1, quxCalledCound, "expect qux auditor to be called");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorPersistQux()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject qux = GetTestService("Quxes").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int quxPersistedCount = 0;

            string newValue = Guid.NewGuid().ToString();

            QuxAuditor.Auditor.objectPersistedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Qux", o.GetType().GetProxiedType().FullName);
                Assert.IsNull(((Qux)o).Prop1);
                quxPersistedCount++;
            };

            qux.Save();

            Assert.AreEqual(1, quxPersistedCount, "expect qux auditor to be called for persists");
        }

        [Test]
        public void AuditUsingSpecificTypeAuditorUpdateQux()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            QuxAuditor.SetCallbacksExpected();
            ITestObject qux = GetTestService("Quxes").GetAction("New Instance").InvokeReturnObject();
            qux.Save();
            MyDefaultAuditor.SetCallbacksUnexpected();
            QuxAuditor.SetCallbacksUnexpected();


            int quxUpdatedCount = 0;

            string newValue = Guid.NewGuid().ToString();

          

            QuxAuditor.Auditor.objectUpdatedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Qux", o.GetType().GetProxiedType().FullName);
                Assert.AreEqual(newValue, ((Qux)o).Prop1);
                quxUpdatedCount++;
            };

            NakedObjectsFramework.TransactionManager.StartTransaction();

            qux.GetPropertyByName("Prop1").SetValue(newValue);

            NakedObjectsFramework.TransactionManager.EndTransaction();
            Assert.AreEqual(1, quxUpdatedCount, "expect qux auditor to be called for updates");
        }

        [Test]
        public void DefaultAuditorCalledForNonSpecificTypeAction()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject bar = GetTestService(typeof(SimpleRepository<Bar>)).GetAction("New Instance").InvokeReturnObject();
            bar.Save();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int defaultCalledCount = 0;

            MyDefaultAuditor.Auditor.actionInvokedCallback = (p, a, o, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("AnAction", a);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Bar", o.GetType().GetProxiedType().FullName);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                defaultCalledCount++;
            };

            bar.GetAction("An Action").InvokeReturnObject();
            Assert.AreEqual(1, defaultCalledCount, "expect default auditor to be called");
        }

        [Test]
        public void DefaultAuditorCalledForNonSpecificTypeServiceAction()
        {
            ITestService bar = GetTestService(typeof(SimpleRepository<Bar>));
            MyDefaultAuditor.SetCallbacksUnexpected();

            int defaultCalledCount = 0;

            MyDefaultAuditor.Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.AreEqual("NewInstance", a);
                Assert.AreEqual("Bars", s);
                Assert.IsFalse(b);
                Assert.AreEqual(0, pp.Length);
                defaultCalledCount++;
            };

            bar.GetAction("New Instance").InvokeReturnObject();
            Assert.AreEqual(1, defaultCalledCount, "expect default auditor to be called");
        }

        [Test]
        public void DefaultAuditorCalledForNonSpecificTypePersist()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject bar = GetTestService("Bars").GetAction("New Instance").InvokeReturnObject();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int defaultPersistedCount = 0;

            string newValue = Guid.NewGuid().ToString();

            MyDefaultAuditor.Auditor.objectPersistedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Bar", o.GetType().GetProxiedType().FullName);
                Assert.IsNull(((Bar)o).Prop1);
                defaultPersistedCount++;
            };

            bar.Save();

          
            Assert.AreEqual(1, defaultPersistedCount, "expect default auditor to be called for persists");
        }

        [Test]
        public void DefaultAuditorCalledForNonSpecificTypeUpdate()
        {
            MyDefaultAuditor.SetCallbacksExpected();
            ITestObject bar = GetTestService("Bars").GetAction("New Instance").InvokeReturnObject();
            bar.Save();
            MyDefaultAuditor.SetCallbacksUnexpected();

            int defaultUpdatedCount = 0;

            string newValue = Guid.NewGuid().ToString();

            MyDefaultAuditor.Auditor.objectUpdatedCallback = (p, o) =>
            {
                Assert.AreEqual("sven", p.Identity.Name);
                Assert.IsNotNull(o);
                Assert.AreEqual("NakedObjects.SystemTest.Audit.Bar", o.GetType().GetProxiedType().FullName);
                Assert.AreEqual(newValue, ((Bar)o).Prop1);
                defaultUpdatedCount++;
            };
            NakedObjectsFramework.TransactionManager.StartTransaction();
            bar.GetPropertyByName("Prop1").SetValue(newValue);
            NakedObjectsFramework.TransactionManager.EndTransaction();
            Assert.AreEqual(1, defaultUpdatedCount, "expect default auditor to be called for updates");
        }

        #region Setup/Teardown

        [OneTimeSetUp]
        public  void ClassInitialize()
        {
            AuditDbContext.Delete();
            var context = Activator.CreateInstance<AuditDbContext>();

            context.Database.Create();
            InitializeNakedObjectsFramework(this);
        }

        [OneTimeTearDown]
        public  void ClassCleanup()
        {
            CleanupNakedObjectsFramework(this);
        }

        [SetUp()]
        public void SetUp()
        {
            StartTest();
            SetUser("sven");
        }

        [TearDown()]
        public void TearDown() {
            EndTest();
        }

        #endregion

        #region "Services & Fixtures"

        private static readonly FooAuditor fooAuditor = new FooAuditor();
        protected static MyDefaultAuditor myDefaultAuditor = new MyDefaultAuditor();
        protected static QuxAuditor quxAuditor = new QuxAuditor();

        #endregion
    }

    #region Classes used by tests

    public class AuditDbContext : DbContext {
        private static readonly string Cs = @$"Data Source={Constants.Server};Initial Catalog={DatabaseName};Integrated Security=True;";

        public static void Delete() => System.Data.Entity.Database.Delete(Cs);

        public const string DatabaseName = "TestAudit";
        public AuditDbContext() : base(Cs) { }

        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }
        public DbSet<Qux> Quxes { get; set; }
    }

    public class Auditor
    {
        public Action<IPrincipal, string, object, bool, object[]> actionInvokedCallback;
        public Action<IPrincipal, object> objectPersistedCallback;
        public Action<IPrincipal, object> objectUpdatedCallback;
        public Action<IPrincipal, string, string, bool, object[]> serviceActionInvokedCallback;

        public Auditor(string name)
        {
            serviceActionInvokedCallback = TestAuditManager.UnexpectedServiceActionCallback(name);
            actionInvokedCallback = TestAuditManager.UnexpectedActionCallback(name);
            objectPersistedCallback = TestAuditManager.UnexpectedCallback(name);
            objectUpdatedCallback = TestAuditManager.UnexpectedCallback(name);
        }
    }

    public class MyDefaultAuditor : IAuditor
    {
        public static readonly Auditor Auditor = new Auditor("default");

        public MyDefaultAuditor()
        {
            //Auditor.actionInvokedCallback = (p, a, o, b, pp) => { };
            //Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) => { };
        }

        public string NamespaceToAudit { get; private set; }

        #region IAuditor Members

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, object onObject, bool queryOnly, object[] withParameters)
        {
            Auditor.actionInvokedCallback(byPrincipal, actionName, onObject, queryOnly, withParameters);
        }

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, string serviceName, bool queryOnly, object[] withParameters)
        {
            Auditor.serviceActionInvokedCallback(byPrincipal, actionName, serviceName, queryOnly, withParameters);
        }

        public void ObjectUpdated(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectUpdatedCallback(byPrincipal, updatedObject);
        }

        public void ObjectPersisted(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectPersistedCallback(byPrincipal, updatedObject);
        }

        #endregion

        public static void SetCallbacksExpected()
        {
            Auditor.actionInvokedCallback = (p, a, o, b, pp) => { };
            Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) => { };
            Auditor.objectUpdatedCallback = (p, o) => { };
            Auditor.objectPersistedCallback = (p, o) => { };
        }

        public static void SetCallbacksUnexpected()
        {
            Auditor.actionInvokedCallback = TestAuditManager.UnexpectedActionCallback("default");
            Auditor.serviceActionInvokedCallback = TestAuditManager.UnexpectedServiceActionCallback("default");
            Auditor.objectUpdatedCallback = TestAuditManager.UnexpectedCallback("default");
            Auditor.objectPersistedCallback = TestAuditManager.UnexpectedCallback("default");
        }
    }

    public class FooAuditor : IAuditor
    {
        public static readonly Auditor Auditor = new Auditor("foo");

        public FooAuditor()
        {
            NamespaceToAudit = typeof(Foo).FullName;
        }

        public string NamespaceToAudit { get; private set; }

        #region IAuditor Members

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, object onObject, bool queryOnly, object[] withParameters)
        {
            Auditor.actionInvokedCallback(byPrincipal, actionName, onObject, queryOnly, withParameters);
        }

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, string serviceName, bool queryOnly, object[] withParameters)
        {
            Auditor.serviceActionInvokedCallback(byPrincipal, actionName, serviceName, queryOnly, withParameters);
        }

        public void ObjectUpdated(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectUpdatedCallback(byPrincipal, updatedObject);
        }

        public void ObjectPersisted(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectPersistedCallback(byPrincipal, updatedObject);
        }

        #endregion

        public static void SetCallbacksExpected()
        {
            Auditor.actionInvokedCallback = (p, a, o, b, pp) => { };
            Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) => { };
            Auditor.objectUpdatedCallback = (p, o) => { };
            Auditor.objectPersistedCallback = (p, o) => { };
        }

        public static void SetCallbacksUnexpected()
        {
            Auditor.actionInvokedCallback = TestAuditManager.UnexpectedActionCallback("foo");
            Auditor.serviceActionInvokedCallback = TestAuditManager.UnexpectedServiceActionCallback("foo");
            Auditor.objectUpdatedCallback = TestAuditManager.UnexpectedCallback("foo");
            Auditor.objectPersistedCallback = TestAuditManager.UnexpectedCallback("foo");
        }
    }

    public class QuxAuditor : IAuditor
    {
        public static readonly Auditor Auditor = new Auditor("qux");

        public QuxAuditor()
        {
            NamespaceToAudit = typeof(Qux).FullName;
        }

        public string NamespaceToAudit { get; private set; }

        #region IAuditor Members

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, object onObject, bool queryOnly, object[] withParameters)
        {
            Auditor.actionInvokedCallback(byPrincipal, actionName, onObject, queryOnly, withParameters);
        }

        public void ActionInvoked(IPrincipal byPrincipal, string actionName, string serviceName, bool queryOnly, object[] withParameters)
        {
            Auditor.serviceActionInvokedCallback(byPrincipal, actionName, serviceName, queryOnly, withParameters);
        }

        public void ObjectUpdated(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectUpdatedCallback(byPrincipal, updatedObject);
        }

        public void ObjectPersisted(IPrincipal byPrincipal, object updatedObject)
        {
            Auditor.objectPersistedCallback(byPrincipal, updatedObject);
        }

        #endregion

        public static void SetCallbacksExpected()
        {
            Auditor.actionInvokedCallback = (p, a, o, b, pp) => { };
            Auditor.serviceActionInvokedCallback = (p, a, s, b, pp) => { };
            Auditor.objectUpdatedCallback = (p, o) => { };
            Auditor.objectPersistedCallback = (p, o) => { };
        }

        public static void SetCallbacksUnexpected()
        {
            Auditor.actionInvokedCallback = TestAuditManager.UnexpectedActionCallback("qux");
            Auditor.serviceActionInvokedCallback = TestAuditManager.UnexpectedServiceActionCallback("qux");
            Auditor.objectUpdatedCallback = TestAuditManager.UnexpectedCallback("qux");
            Auditor.objectPersistedCallback = TestAuditManager.UnexpectedCallback("qux");
        }
    }

    public class Foo
    {
        public virtual int Id { get; set; }

        [Optionally]
        public virtual string Prop1 { get; set; }

        public virtual void AnAction() { }
        public virtual void AnActionWithParm(int aParm) { }
        public virtual void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }

        public virtual IQueryable<Foo> AnotherQueryOnlyAction()
        {
            return new QueryableList<Foo>();
        }
    }

    public class Bar
    {
        public virtual int Id { get; set; }

        [Optionally]
        public virtual string Prop1 { get; set; }

        public void AnAction() { }
        public void AnActionWithParm(int aParm) { }
        public void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }
    }

    public class Qux
    {
        public virtual int Id { get; set; }

        [Optionally]
        public virtual string Prop1 { get; set; }

        public void AnAction() { }
        public void AnActionWithParm(int aParm) { }
        public void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }
    }

    public class FooService
    {
        public virtual void AnAction() { }
        public virtual void AnActionWithParm(int aParm) { }
        public virtual void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }
    }

    public class BarService
    {
        public virtual void AnAction() { }
        public virtual void AnActionWithParm(int aParm) { }
        public virtual void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }
    }

    public class QuxService
    {
        public virtual void AnAction() { }
        public virtual void AnActionWithParm(int aParm) { }
        public virtual void AnActionWithParms(int parm1, Foo parm2) { }

        [QueryOnly]
        public virtual void AQueryOnlyAction() { }
    }

    #endregion
}