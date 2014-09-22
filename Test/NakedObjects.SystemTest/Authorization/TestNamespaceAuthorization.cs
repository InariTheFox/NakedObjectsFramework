﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NakedObjects.Persistor.Objectstore.Inmemory;
using NakedObjects.Core.NakedObjectsSystem;
using NakedObjects.Boot;
using NakedObjects.Services;
using NakedObjects.Reflector.Security;
using NakedObjects.Security;
using System.Security.Principal;
using NakedObjects.Xat;
using NakedObjects;
using MyApp.MyCluster1;
using MyApp.MyCluster2;
using NotMyApp.MyCluster2;

namespace NakedObjects.SystemTest.Authorization.NamespaceAuthorization
{
    [TestClass, Ignore]
    public class TestCustomAuthorizationManager : AbstractSystemTest
    {
        #region Setup/Teardown

        [TestInitialize()]
        public void SetupTest()
        {
            //InitializeNakedObjectsFramework();
            SetUser("sven");
        }

        [TestCleanup()]
        public void TearDownTest()
        {
            ///CleanupNakedObjectsFramework();
            MemoryObjectStore.DiscardObjects();
        }

        #endregion

        #region "Services & Fixtures"
        protected override IServicesInstaller MenuServices
        {
            get
            {
                return new ServicesInstaller(
                    new SimpleRepository<Foo1>(),
                    new SimpleRepository<Bar1>(),
                    new SimpleRepository<Foo2>(),
                    new SimpleRepository<Bar2>());
            }
        }

        //protected override IAuthorizerInstaller Authorizer
        //{
        //    get
        //    {
        //        return new CustomAuthorizerInstaller(
        //            new MyDefaultAuthorizer(),
        //            new MyAppAuthorizer(),
        //            new MyCluster1Authorizer(),
        //            new MyBar1Authorizer()
        //            );
        //    }
        //}
        #endregion

        [TestMethod]
        public void AuthorizerWithMostSpecificNamespaceIsInvokedForVisibility()
        {
            //Bar1
            var bar1 = GetTestService("Bar1s").GetAction("New Instance").InvokeReturnObject();
            try
            {
                bar1.GetPropertyByName("Prop1").AssertIsVisible();
                Assert.Fail("Should not get to here");
            }
            catch (Exception e)
            {
                Assert.AreEqual("MyBar1Authorizer#IsVisible, user: sven, target: Bar1, memberName: Prop1", e.InnerException.Message);
            }

            //Foo1
            var foo1 = GetTestService("Foo1s").GetAction("New Instance").InvokeReturnObject();
            try
            {
                foo1.GetPropertyByName("Prop1").AssertIsVisible();
                Assert.Fail("Should not get to here");
            }
            catch (Exception e)
            {
                Assert.AreEqual("MyCluster1Authorizer#IsVisible, user: sven, target: Foo1, memberName: Prop1", e.InnerException.Message);
            }

            //Foo2
            var foo2 = GetTestService("Foo2s").GetAction("New Instance").InvokeReturnObject();
            try
            {
                foo2.GetPropertyByName("Prop1").AssertIsVisible();
                Assert.Fail("Should not get to here");
            }
            catch (Exception e)
            {
                Assert.AreEqual("MyAppAuthorizer#IsVisible, user: sven, target: Foo2, memberName: Prop1", e.InnerException.Message);
            }

            //Bar2
            var bar2 = GetTestService("Bar2s").GetAction("New Instance").InvokeReturnObject();
            bar2.GetPropertyByName("Prop1").AssertIsVisible();
        }
    }

    public class MyDefaultAuthorizer : ITypeAuthorizer<object>
    {
        //bool initialized = false;

        public void Init()
        {
            //initialized = false;
            throw new NotImplementedException();
        }

        public bool IsEditable(IPrincipal principal, object target, string memberName)
        {
            return true;
        }

        public bool IsVisible(IPrincipal principal, object target, string memberName)
        {
            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }

    public class MyAppAuthorizer : INamespaceAuthorizer
    {

        public string NamespaceToAuthorize
        {
            get { return "MyApp"; }
        }

        public void Init()
        {
        }

        public bool IsEditable(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyAppAuthorizer#IsEditable, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public bool IsVisible(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyAppAuthorizer#IsVisible, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }

    public class MyCluster1Authorizer : INamespaceAuthorizer
    {

        public string NamespaceToAuthorize
        {
            get { return "MyApp.MyCluster1"; }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsEditable(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyCluster1Authorizer#IsEditable, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public bool IsVisible(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyCluster1Authorizer#IsVisible, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }

    public class MyBar1Authorizer : INamespaceAuthorizer
    {

        public string NamespaceToAuthorize
        {
            get { return "MyApp.MyCluster1.Bar1"; }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsEditable(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyBar1Authorizer#IsEditable, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public bool IsVisible(IPrincipal principal, object target, string memberName)
        {
            throw new Exception(String.Format("MyBar1Authorizer#IsVisible, user: {0}, target: {1}, memberName: {2}", principal.Identity.Name, target.ToString(), memberName));
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}

namespace MyApp.MyCluster1
{
    public class Foo1
    {

        public override string ToString()
        {
            return "Foo1";
        }
        [Optionally]
        public virtual string Prop1 { get; set; }
        public void Act1() { }
    }

    public class Bar1
    {

        public override string ToString()
        {
            return "Bar1";
        }
        [Optionally]
        public virtual string Prop1 { get; set; }
        public void Act1() { }
    }

}

namespace MyApp.MyCluster2
{
    public class Foo2
    {

        public override string ToString()
        {
            return "Foo2";
        }
        [Optionally]
        public virtual string Prop1 { get; set; }
        public void Act1() { }
    }
}

namespace NotMyApp.MyCluster2
{
    public class Bar2
    {

        public override string ToString()
        {
            return "Bar2";
        }
        [Optionally]
        public virtual string Prop1 { get; set; }
        public void Act1() { }
    }
}