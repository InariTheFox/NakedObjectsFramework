﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Data.Entity;
using SystemTest.ContributedActions;
using NakedObjects;
using NakedObjects.Menu;
using NakedObjects.Services;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace NakedObjects.SystemTest.Menus
{
    //This class is not testing menus, nor TestMenus, but simply backwards compatibility
    //of GetAction, including with specified subMenu.
    [TestFixture]
    public class TestAccessingMenuActionsViaGetAction : AbstractSystemTest<CADbContext>
    {
        [Test]
        public void ContributedActionToObjectWithDefaultMenu()
        {
            var foo = NewTestObject<Foo>();
            Assert.IsNotNull(foo.GetAction("Action1"));
        }

        [Test]
        public void ContributedActionToSubMenuObjectWithDefaultMenu()
        {
            var foo = NewTestObject<Foo>();
            Assert.IsNotNull(foo.GetAction("Action2", "Sub"));
            Assert.IsNotNull(foo.GetAction("Action2")); //Note that you can also access the action directly
            try
            {
                foo.GetAction("Action1", "Sub");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Assert.IsNotNull failed. No menu item with name: Action1", e.Message);
            }
        }

        [Test]
        public void ContributedActionToObjectWithExplicitMenu()
        {
            var bar = NewTestObject<Bar>();
            Assert.IsNotNull(bar.GetAction("Action3"));
            Assert.IsNotNull(bar.GetAction("Action4"));
            Assert.IsNotNull(bar.GetAction("Action5"));
            Assert.IsNotNull(bar.GetAction("Action4", "Sub1"));
            Assert.IsNotNull(bar.GetAction("Action5", "Sub2"));
        }

        #region Setup/Teardown 

        [OneTimeSetUp]
        public  void ClassInitialize()
        {
            Database.Delete(CADbContext.DatabaseName);
            var context = Activator.CreateInstance<CADbContext>();

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
        }

        #endregion

        #region Configuration

        protected override string[] Namespaces
        {
            get { return new[] { typeof(Foo).Namespace }; }
        }

        protected override Type[] Services
        {
            get
            {
                return new[] {
                    typeof (SimpleRepository<Foo>),
                    typeof (SimpleRepository<Foo2>),
                    typeof (SimpleRepository<Bar>),
                    typeof (ContributingService)
                };
            }
        }

        #endregion
    }
}

namespace SystemTest.ContributedActions
{
    public class CADbContext : DbContext
    {
        public const string DatabaseName = "Tests";
        public CADbContext() : base(DatabaseName) { }

        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }
    }

    public class Foo
    {
        [NakedObjectsIgnore]
        public virtual int Id { get; set; }

        public void NativeAction() { }
    }

    public class Foo2 : Foo { }

    public class Bar
    {
        [NakedObjectsIgnore]
        public virtual int Id { get; set; }

        public static void Menu(IMenu menu)
        {
            menu.CreateSubMenu("Sub1");
            menu.AddContributedActions();
        }
    }

    public class ContributingService
    {
        public void Action1(string str, [ContributedAction] Foo foo) { }

        public void Action2(string str, [ContributedAction("Sub")] Foo foo) { }

        public void Action3([ContributedAction] Bar bar) { }

        public void Action4([ContributedAction("Sub1")] Bar bar) { }

        public void Action5([ContributedAction("Sub2")] Bar bar) { }
    }
}