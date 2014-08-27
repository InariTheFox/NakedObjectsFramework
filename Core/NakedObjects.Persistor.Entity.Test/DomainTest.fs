﻿// Copyright © Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 
module NakedObjects.DomainTest
open NUnit.Framework
open DomainTestCode
open TestTypes
open NakedObjects.EntityObjectStore
open TestCode
open System.Data.Entity.Core.Objects
open NakedObjects.Core.Context
open NakedObjects.Core.Security
open System.Security.Principal

let persistor =
    let c = new EntityObjectStoreConfiguration()
    let s = new SimpleSession(new GenericPrincipal(new GenericIdentity(""), [||]))
    let u = new SimpleUpdateNotifier()
    c.ContextConfiguration <- [|(box PocoConfig :?> EntityContextConfiguration)|]
    let p = new EntityObjectStore(s, u, c, new EntityOidGenerator(NakedObjectsContext.Reflector), NakedObjectsContext.Reflector)
    setupPersistorForTesting p

let overwritePersistor =
    let config = 
        let pc = new NakedObjects.EntityObjectStore.PocoEntityContextConfiguration()
        pc.ContextName <- "AdventureWorksEntities"  
        pc.DefaultMergeOption <- MergeOption.OverwriteChanges
        pc
    let c = new EntityObjectStoreConfiguration()
    let s = new SimpleSession(new GenericPrincipal(new GenericIdentity(""), [||]))
    let u = new SimpleUpdateNotifier()
    c.ContextConfiguration <- [|(box config :?> EntityContextConfiguration)|]
    let p = new EntityObjectStore(s, u, c, new EntityOidGenerator(NakedObjectsContext.Reflector), NakedObjectsContext.Reflector)
    setupPersistorForTesting p

[<TestFixture>]
type DomainTests() = class              
    [<TestFixtureSetUp>] 
    member x.Setup() = 
        DomainSetup()
        //let sink = setupPersistorForTesting persistor
        ()
    [<TestFixtureTearDown>] member x.TearDown() = persistor.Reset()
    [<Test>] member x.TestCreateEntityPersistor() = CanCreateEntityPersistor persistor    
    [<Test>] member x.TestGetInstancesGeneric() = CanGetInstancesGeneric persistor
    [<Test>] member x.TestGetInstancesByType() = CanGetInstancesByType persistor
    [<Test>] member x.TestGetInstancesIsProxy() = CanGetInstancesIsProxy persistor
    [<Test>] member x.TestGetManyToOneReference() = CanGetManyToOneReference persistor
    [<Test>] member x.TestGetObjectBySingleKey() = CanGetObjectBySingleKey persistor
    [<Test>] member x.TestGetObjectByMultiKey() = CanGetObjectByMultiKey persistor
    [<Test>] member x.TestGetObjectByStringKey() = CanGetObjectByStringKey persistor
    [<Test>] member x.TestGetObjectByDateKey() = CanGetObjectByDateKey persistor   
    [<Test>] member x.TestCreateTransientObject() = DomainTestCode.CanCreateTransientObject persistor
    [<Test>] member x.TestSaveTransientObjectWithScalarProperties() = CanSaveTransientObjectWithScalarProperties persistor
    [<Test>] member x.TestSaveTransientObjectWithPersistentReferenceProperty() = CanSaveTransientObjectWithPersistentReferenceProperty persistor 
    [<Test>] member x.TestCanSaveTransientObjectWithPersistentReferencePropertyInSeperateTransaction() = CanSaveTransientObjectWithPersistentReferencePropertyInSeperateTransaction persistor 
    [<Test>] member x.TestSaveTransientObjectWithTransientReferenceProperty() = CanSaveTransientObjectWithTransientReferenceProperty persistor           
    [<Test>] member x.TestSaveTransientObjectWithTransientReferencePropertyAndConfirmProxies() = CanSaveTransientObjectWithTransientReferencePropertyAndConfirmProxies persistor
    [<Test>] member x.TestUpdatePersistentObjectWithScalarProperties() = CanUpdatePersistentObjectWithScalarProperties persistor
    [<Test>] member x.TestUpdatePersistentObjectWithReferenceProperties() = CanUpdatePersistentObjectWithReferenceProperties persistor
    [<Test>] member x.TestUpdatePersistentObjectWithReferencePropertiesDoFixup() = CanUpdatePersistentObjectWithReferencePropertiesDoFixup persistor            
    [<Test>] member x.TestUpdatePersistentObjectWithCollectionProperties() = CanUpdatePersistentObjectWithCollectionProperties persistor      
    [<Test>] member x.TestUpdatePersistentObjectWithCollectionPropertiesDoFixup() = CanUpdatePersistentObjectWithCollectionPropertiesDoFixup persistor
    [<Test>] member x.TestNavigateReferences() = CanNavigateReferences persistor
    [<Test>] member x.TestUpdatePersistentObjectWithScalarPropertiesErrorAndReattempt() = CanUpdatePersistentObjectWithScalarPropertiesErrorAndReattempt persistor 
    [<Test>] member x.TestUpdatePersistentObjectWithScalarPropertiesIgnore() = CanUpdatePersistentObjectWithScalarPropertiesIgnore persistor    
    [<Test>] member x.TestSaveTransientObjectWithScalarPropertiesErrorAndReattempt() = CanSaveTransientObjectWithScalarPropertiesErrorAndReattempt persistor
    [<Test>] member x.TestSaveTransientObjectWithScalarPropertiesErrorAndIgnore() = CanSaveTransientObjectWithScalarPropertiesErrorAndIgnore persistor
    [<Test>] member x.TestPersistingPersistedCalledForCreateInstance() = CanPersistingPersistedCalledForCreateInstance persistor
    [<Test>] member x.TestPersistingPersistedCalledForCreateInstanceWithReference() = CanPersistingPersistedCalledForCreateInstanceWithReference persistor
    [<Test>] member x.TestUpdatingUpdatedCalledForChange() = CanUpdatingUpdatedCalledForChange persistor
    [<Test>] member x.TestGetKeyForType() = CanGetKeyForType persistor
    [<Test>] member x.TestGetKeysForType() = CanGetKeysForType persistor
    [<Test>] member x.TestContainerInjectionCalledForNewInstance() = CanContainerInjectionCalledForNewInstance persistor
    [<Test>] member x.TestContainerInjectionCalledForGetInstance() = CanContainerInjectionCalledForGetInstance (resetPersistor persistor)
    [<Test>] member x.TestCreateManyToMany() = CanCreateManyToMany persistor 
    [<Test>] member x.TestCanUpdatePersistentObjectWithScalarPropertiesAbort() = CanUpdatePersistentObjectWithScalarPropertiesAbort persistor
    [<Test>] member x.TestUpdatePersistentObjectWithReferencePropertiesAbort() = CanUpdatePersistentObjectWithReferencePropertiesAbort persistor
    [<Test>] member x.TestUpdatePersistentObjectWithCollectionPropertiesAbort() = CanUpdatePersistentObjectWithCollectionPropertiesAbort persistor
    [<Test>] member x.TestRemoteResolve() = CanRemoteResolve (resetPersistor persistor)
    [<Test>] member x.TestCanGetContextForCollection() = DomainCanGetContextForCollection  persistor
    [<Test>] member x.TestCanGetContextForNonGenericCollection() = DomainCanGetContextForNonGenericCollection  persistor
    [<Test>] member x.TestCanGetContextForArray() = DomainCanGetContextForArray  persistor
    [<Test>] member x.TestCanGetContextForType() = DomainCanGetContextForType  persistor
    [<Test>] member x.TestCanDetectConcurrency() = CanDetectConcurrency persistor
    [<Test>] member x.DataUpdateNoCustomOnPersistingError() = DataUpdateNoCustomOnPersistingError persistor  
    [<Test>] member x.DataUpdateNoCustomOnUpdatingError() = DataUpdateNoCustomOnUpdatingError persistor 
    [<Test>] member x.ConcurrencyNoCustomOnUpdatingError() = ConcurrencyNoCustomOnUpdatingError persistor  
    [<Test>] member x.OverWriteChangesOptionRefreshesObject() = OverWriteChangesOptionRefreshesObject overwritePersistor
    [<Test>] member x.AppendOnlyOptionDoesNotRefreshObject() = AppendOnlyOptionDoesNotRefreshObject persistor
    [<Test>] member x.OverWriteChangesOptionRefreshesObjectNonGenericGet() = OverWriteChangesOptionRefreshesObjectNonGenericGet overwritePersistor
    [<Test>] member x.AppendOnlyOptionDoesNotRefreshObjectNonGenericGet() = AppendOnlyOptionDoesNotRefreshObjectNonGenericGet persistor 
    [<Test>] member x.ExplicitOverwriteChangesRefreshesObject() = ExplicitOverwriteChangesRefreshesObject persistor 
    [<Test>] member x.GetKeysReturnsKey() = GetKeysReturnsKey persistor
end