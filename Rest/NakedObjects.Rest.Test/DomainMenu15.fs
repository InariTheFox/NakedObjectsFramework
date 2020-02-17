﻿//// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
//// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
//// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
//// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
//// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//// See the License for the specific language governing permissions and limitations under the License.
module DomainMenu15

//open NUnit.Framework
//open NakedObjects.Rest
//open System.Net
//open Microsoft.Net.Http.Headers
//open Newtonsoft.Json.Linq
//open NakedObjects.Rest.Snapshot.Constants
//open System.Web.Http
//open System.Linq
//open RestTestFunctions
//open Microsoft.AspNetCore.Http.Headers
//open Microsoft.Extensions.Primitives

//let GetMenu(api : RestfulObjectsControllerBase) =
//    let sName =  ttc "RestfulObjects.Test.Data.RestDataRepository" 
//    let mName =  "RestDataRepository"
//    let url = sprintf "http://localhost/menus/%s" mName
//    let args = CreateReservedArgs ""
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(mName, args)
//    let jsonResult = readSnapshotToJson result
//    let parsedResult = JObject.Parse(jsonResult)
//    let mst = ttc "RestfulObjects.Test.Data.MostSimple"
//    let mstp = ttc "RestfulObjects.Test.Data.MostSimplePersist"
    
//    let makeParm pmid pid fid rt = 
     
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeValueParm pmid pid fid rt = 
     
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let p1 = makeParm "withAction" "AzContributedAction" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p2 = makeParm "withAction" "AzContributedActionOnBaseClass" "With Action" (ttc "RestfulObjects.Test.Data.WithAction")
//    let p3 = makeParm "withAction" "AzContributedActionWithRefParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p4 = makeParm "withOtherAction" "AzContributedActionWithRefParm" "With Other Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p5 = makeParm "withAction" "AzContributedActionWithValueParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p6 = makeValueParm "parm" "AzContributedActionWithValueParm" "Parm" (ttc "string")
    
//    let expected = 
//        [ TProperty(JsonPropertyNames.MenuId, TObjectVal(mName))
//          TProperty(JsonPropertyNames.Title, TObjectVal("Rest Data Repository"))
//          TProperty(JsonPropertyNames.Links, 
//                    TArray([ TObjectJson(makeGetLinkProp RelValues.Self (sprintf "menus/%s" mName) RepresentationTypes.Menu "")   ]))
          
//          TProperty
//              (JsonPropertyNames.Members, 
               
//               TObjectJson
//                   ([ TProperty("AzContributedAction", TObjectJson(makeServiceActionMember "AzContributedAction" sName mst [ p1 ]))
//                      TProperty("AzContributedActionOnBaseClass", TObjectJson(makeServiceActionMember "AzContributedActionOnBaseClass" sName mst [ p2 ]))
//                      TProperty("AzContributedActionWithRefParm", TObjectJson(makeServiceActionMember "AzContributedActionWithRefParm" sName mst [ p3; p4 ]))
                      
//                      TProperty
//                          ("AzContributedActionWithValueParm", TObjectJson(makeServiceActionMember "AzContributedActionWithValueParm" sName mst [ p5; p6 ]))
//                      TProperty("CreateTransientMostSimple", TObjectJson(makeServiceActionMemberNoParms "CreateTransientMostSimple" sName mstp))
                      
//                      TProperty
//                          ("CreateTransientWithValue", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithValue" sName (ttc "RestfulObjects.Test.Data.WithValuePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithReference", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithReference" sName (ttc "RestfulObjects.Test.Data.WithReferencePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithCollection", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithCollection" sName (ttc "RestfulObjects.Test.Data.WithCollectionPersist"))) ]))
//          TProperty(JsonPropertyNames.Extensions, 
//                    TObjectJson([  ])) ]
//    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, jsonResult)
//    Assert.AreEqual(new typeType(RepresentationTypes.Menu, ""), result.Content.Headers.ContentType)
//    assertNonExpiringCache result
//    Assert.IsNull(result.Headers.ETag)
//    compareObject expected parsedResult

//let GetContributorMenu(api : RestfulObjectsControllerBase) = 
//    let sName =  ttc "RestfulObjects.Test.Data.ContributorService" 
//    let mName =  "ContributorService"
//    let url = sprintf "http://localhost/menus/%s" mName
//    let args = CreateReservedArgs ""
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(mName, args)
//    let jsonResult = readSnapshotToJson result
//    let parsedResult = JObject.Parse(jsonResult)
//    let mst = ttc "RestfulObjects.Test.Data.MostSimple"
    
//    let makeCollectionParm  contribName pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([ ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal("list"))
//                                                  TProperty(JsonPropertyNames.ElementType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.PluralName, TObjectVal("Most Simples"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)

//    let makeValueParm  contribName pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal("number"))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)

//    let contribName = ttc "RestfulObjects.Test.Data.ContributorService"

//    let p1 = makeCollectionParm contribName "ms" "ACollectionContributedActionNoParms" "Ms" mst
//    let p2 = makeCollectionParm contribName "ms" "ACollectionContributedActionParm" "Ms" mst
//    let p3 = makeValueParm contribName "id" "ACollectionContributedActionParm" "Id" mst

//    let membersProp = 
//        TProperty(JsonPropertyNames.Members, TObjectJson([ TProperty("ANonContributedAction", TObjectJson(makeServiceActionMemberNoParms "ANonContributedAction" sName mst))
//                                                           TProperty("ACollectionContributedActionNoParms", 
//                                                                     TObjectJson(makeServiceActionMember "ACollectionContributedActionNoParms" contribName mst [ p1 ]))
//                                                           TProperty("ACollectionContributedActionParm", 
//                                                                     TObjectJson(makeServiceActionMember "ACollectionContributedActionParm" contribName mst [ p2; p3 ]))]))


//    let expected = 
//        [ TProperty(JsonPropertyNames.MenuId, TObjectVal(mName))
//          TProperty(JsonPropertyNames.Title, TObjectVal("Contributor Service"))
//          TProperty(JsonPropertyNames.Links, 
//                    TArray([ TObjectJson(makeGetLinkProp RelValues.Self (sprintf "menus/%s" mName) RepresentationTypes.Menu "")
//                              ]))
          
//          membersProp

//          TProperty(JsonPropertyNames.Extensions, 
//                    TObjectJson([  ])) ]
//    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, jsonResult)
//    Assert.AreEqual(new typeType(RepresentationTypes.Menu, ""), result.Content.Headers.ContentType)
//    assertNonExpiringCache result
//    Assert.IsNull(result.Headers.ETag)
//    compareObject expected parsedResult



//let GetMenuSimpleOnly(api : RestfulObjectsControllerBase) = 
//    let sName =  ttc "RestfulObjects.Test.Data.RestDataRepository" 
//    let mName =  "RestDataRepository"
//    let argS = "x-ro-domain-model=simple"
//    let url = sprintf "http://localhost/menus/%s?%s" mName argS
//    let args = CreateReservedArgs argS
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(mName, args)
//    let jsonResult = readSnapshotToJson result
//    let parsedResult = JObject.Parse(jsonResult)
//    let mst = ttc "RestfulObjects.Test.Data.MostSimple"
//    let mstp = ttc "RestfulObjects.Test.Data.MostSimplePersist"

    
//    let makeParm pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, TArray([]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeValueParm pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, TArray([]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let p1 = makeParm "withAction" "AzContributedAction" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p2 = makeParm "withAction" "AzContributedActionOnBaseClass" "With Action" (ttc "RestfulObjects.Test.Data.WithAction")
//    let p3 = makeParm "withAction" "AzContributedActionWithRefParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p4 = makeParm "withOtherAction" "AzContributedActionWithRefParm" "With Other Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p5 = makeParm "withAction" "AzContributedActionWithValueParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p6 = makeValueParm "parm" "AzContributedActionWithValueParm" "Parm" (ttc "string")
    
//    let expected = 
//        [ TProperty(JsonPropertyNames.MenuId, TObjectVal(mName))
//          TProperty(JsonPropertyNames.Title, TObjectVal("Rest Data Repository"))
          
//          TProperty
//              (JsonPropertyNames.Links, TArray([ TObjectJson(makeGetLinkProp RelValues.Self (sprintf "menus/%s" mName) RepresentationTypes.Menu "") ]))
          
//          TProperty
//              (JsonPropertyNames.Members, 
               
//               TObjectJson
//                   ([ TProperty("AzContributedAction", TObjectJson(makeServiceActionMemberSimple "AzContributedAction" sName mst [ p1 ]))
//                      TProperty("AzContributedActionOnBaseClass", TObjectJson(makeServiceActionMemberSimple "AzContributedActionOnBaseClass" sName mst [ p2 ]))
                      
//                      TProperty
//                          ("AzContributedActionWithRefParm", TObjectJson(makeServiceActionMemberSimple "AzContributedActionWithRefParm" sName mst [ p3; p4 ]))
                      
//                      TProperty
//                          ("AzContributedActionWithValueParm", 
//                           TObjectJson(makeServiceActionMemberSimple "AzContributedActionWithValueParm" sName mst [ p5; p6 ]))
//                      TProperty("CreateTransientMostSimple", TObjectJson(makeServiceActionMemberNoParmsSimple "CreateTransientMostSimple" sName mstp))
                      
//                      TProperty
//                          ("CreateTransientWithValue", 
//                           TObjectJson(makeServiceActionMemberNoParmsSimple "CreateTransientWithValue" sName (ttc "RestfulObjects.Test.Data.WithValuePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithReference", 
//                           TObjectJson(makeServiceActionMemberNoParmsSimple "CreateTransientWithReference" sName (ttc "RestfulObjects.Test.Data.WithReferencePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithCollection", 
                           
//                           TObjectJson
//                               (makeServiceActionMemberNoParmsSimple "CreateTransientWithCollection" sName (ttc "RestfulObjects.Test.Data.WithCollectionPersist"))) ]))
//          TProperty(JsonPropertyNames.Extensions, 
//                    TObjectJson([  ])) ]
//    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, jsonResult)
//    Assert.AreEqual(new typeType(RepresentationTypes.Menu, ""), result.Content.Headers.ContentType)
//    assertNonExpiringCache result
//    Assert.IsTrue(result.Headers.ETag = null)
//    compareObject expected parsedResult

//let GetMenuWithMediaType(api : RestfulObjectsControllerBase) = 
//    let sName =  ttc "RestfulObjects.Test.Data.RestDataRepository" 
//    let mName =  "RestDataRepository"
//    let url = sprintf "http://localhost/menus/%s" mName
//    let msg = jsonGetMsg (url)
//    let headers = new RequestHeaders(msg.Headers)
//    let name = new StringSegment("profile")
//    headers.Accept.Single().Parameters.Add(new NameValueHeaderValue(name, (makeProfile RepresentationTypes.Menu)))
//    let args = CreateReservedArgs ""
//    api.Request <- msg
//    let result = api.GetMenu(mName, args)
//    let jsonResult = readSnapshotToJson result
//    let parsedResult = JObject.Parse(jsonResult)
//    let mst = ttc "RestfulObjects.Test.Data.MostSimple"
//    let mstp = ttc "RestfulObjects.Test.Data.MostSimplePersist"

    
//    let makeParm pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeValueParm pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let p1 = makeParm "withAction" "AzContributedAction" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p2 = makeParm "withAction" "AzContributedActionOnBaseClass" "With Action" (ttc "RestfulObjects.Test.Data.WithAction")
//    let p3 = makeParm "withAction" "AzContributedActionWithRefParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p4 = makeParm "withOtherAction" "AzContributedActionWithRefParm" "With Other Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p5 = makeParm "withAction" "AzContributedActionWithValueParm" "With Action" (ttc "RestfulObjects.Test.Data.WithActionObject")
//    let p6 = makeValueParm "parm" "AzContributedActionWithValueParm" "Parm" (ttc "string")
    
//    let expected = 
//        [ TProperty(JsonPropertyNames.MenuId, TObjectVal(mName))
//          TProperty(JsonPropertyNames.Title, TObjectVal("Rest Data Repository"))
//          TProperty(JsonPropertyNames.Links, 
//                    TArray([ TObjectJson(makeGetLinkProp RelValues.Self (sprintf "menus/%s" mName) RepresentationTypes.Menu "")
//                              ]))
          
//          TProperty
//              (JsonPropertyNames.Members, 
               
//               TObjectJson
//                   ([ TProperty("AzContributedAction", TObjectJson(makeServiceActionMember "AzContributedAction" sName mst [ p1 ]))
//                      TProperty("AzContributedActionOnBaseClass", TObjectJson(makeServiceActionMember "AzContributedActionOnBaseClass" sName mst [ p2 ]))
//                      TProperty("AzContributedActionWithRefParm", TObjectJson(makeServiceActionMember "AzContributedActionWithRefParm" sName mst [ p3; p4 ]))
                      
//                      TProperty
//                          ("AzContributedActionWithValueParm", TObjectJson(makeServiceActionMember "AzContributedActionWithValueParm" sName mst [ p5; p6 ]))
//                      TProperty("CreateTransientMostSimple", TObjectJson(makeServiceActionMemberNoParms "CreateTransientMostSimple" sName mstp))
                      
//                      TProperty
//                          ("CreateTransientWithValue", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithValue" sName (ttc "RestfulObjects.Test.Data.WithValuePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithReference", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithReference" sName (ttc "RestfulObjects.Test.Data.WithReferencePersist")))
                      
//                      TProperty
//                          ("CreateTransientWithCollection", 
//                           TObjectJson(makeServiceActionMemberNoParms "CreateTransientWithCollection" sName (ttc "RestfulObjects.Test.Data.WithCollectionPersist"))) ]))
//          TProperty(JsonPropertyNames.Extensions, 
//                    TObjectJson([  ])) ]
//    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, jsonResult)
//    Assert.AreEqual(new typeType(RepresentationTypes.Menu, ""), result.Content.Headers.ContentType)
//    assertNonExpiringCache result
//    compareObject expected parsedResult

//let GetWithActionMenu(api : RestfulObjectsControllerBase) = 
//    let sName =  ttc "RestfulObjects.Test.Data.WithActionService" 
//    let mName =  "WithActionService"

//    let url = sprintf "http://localhost/menus/%s" mName
//    let args = CreateReservedArgs ""
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(mName, args)
//    let jsonResult = readSnapshotToJson result
//    let parsedResult = JObject.Parse(jsonResult)
//    let mst = ttc "RestfulObjects.Test.Data.MostSimple"
//    let mp r n = sprintf ";%s=\"%s\"" r n
    
//    let makeParm pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithAC pmid pid fid rt = 
    
//        let autoRel = RelValues.Prompt + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let acurl = sprintf "services/%s/actions/%s/params/%s/prompt" sName pid pmid
//        let argP = 
//            TProperty
//                (JsonPropertyNames.Arguments, 
//                 TObjectJson([ TProperty(JsonPropertyNames.XRoSearchTerm, TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null)) ])) ]))
//        let extP = TProperty(JsonPropertyNames.Extensions, TObjectJson([ TProperty(JsonPropertyNames.MinLength, TObjectVal(3)) ]))
//        let ac = TObjectJson(argP :: extP :: makeLinkPropWithMethodAndTypes "GET" autoRel acurl RepresentationTypes.Prompt "" "" true)
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, 
//                                    TArray([ 
//                                             ac ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithChoicesAndDefault pmid pid fid rt = 
      
//        let choiceRel = RelValues.Choice + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let defaultRel = RelValues.Default + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let choice1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
//        let choice2 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("2")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "2")) RepresentationTypes.Object mst
//        let obj1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp defaultRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Default, TObjectJson(obj1))
//                          TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectJson(choice1)
//                                             TObjectJson(choice2) ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithChoices pmid pid fid rt = 
        
//        let choiceRel = RelValues.Choice + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let choice1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
//        let choice2 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("2")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "2")) RepresentationTypes.Object mst
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectJson(choice1)
//                                             TObjectJson(choice2) ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithDefault pmid pid fid rt = 
      
//        let defaultRel = RelValues.Default + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let obj1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp defaultRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Default, TObjectJson(obj1))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeStringParmWithDefaults pmid pid fid rt et = 
      
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectVal("string1")
//                                             TObjectVal("string2")
//                                             TObjectVal("string3") ]))
//                          TProperty(JsonPropertyNames.Default, 
//                                    TArray([ TObjectVal("string2")
//                                             TObjectVal("string3") ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.ElementType, TObjectVal(et))
//                                                  TProperty(JsonPropertyNames.CustomChoices, 
//                                                            TObjectJson([ TProperty("string1", TObjectVal("string1"))
//                                                                          TProperty("string2", TObjectVal("string2"))
//                                                                          TProperty("string3", TObjectVal("string3")) ]))
//                                                  TProperty(JsonPropertyNames.PluralName, TObjectVal("Strings"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithDefaults pmid pid fid rt et = 
      
//        let defaultRel = RelValues.Default + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let choiceRel = RelValues.Choice + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let c1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
//        let c2 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("2")) 
//            :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "2")) RepresentationTypes.Object mst
//        //let c3 =  TProperty(JsonPropertyNames.Title, TObjectVal("3")) :: makeGetLinkProp choiceRel (sprintf "objects/%s/%s" mst (ktc "3"))  RepresentationTypes.Object mst
//        let d1 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("1")) 
//            :: makeGetLinkProp defaultRel (sprintf "objects/%s/%s" mst (ktc "1")) RepresentationTypes.Object mst
//        let d2 = 
//            TProperty(JsonPropertyNames.Title, TObjectVal("2")) 
//            :: makeGetLinkProp defaultRel (sprintf "objects/%s/%s" mst (ktc "2")) RepresentationTypes.Object mst
        
//        //let d3 =  TProperty(JsonPropertyNames.Title, TObjectVal("3")) :: makeGetLinkProp defaultRel (sprintf "objects/%s/%s" mst (ktc "3"))  RepresentationTypes.Object mst
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectJson(c1)
//                                             TObjectJson(c2) ]))
//                          TProperty(JsonPropertyNames.Default, 
//                                    TArray([ TObjectJson(d1)
//                                             TObjectJson(d2) ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.ElementType, TObjectVal(et))
//                                                  TProperty(JsonPropertyNames.PluralName, TObjectVal("Most Simples"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeValueParm pmid pid fid rt = 
     
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParm pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParmWithRange pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.CustomRange, TObjectJson([TProperty("min", TObjectVal(1)); TProperty("max", TObjectVal(500))]))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)

//    let makeIntParmWithHint pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.PresentationHint, TObjectVal("class9 class10"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParmWithChoicesAndDefault pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Default, TObjectVal(4))
//                          TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectVal(1)
//                                             TObjectVal(2)
//                                             TObjectVal(3) ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.CustomChoices, 
//                                                            TObjectJson([ TProperty("1", TObjectVal(1))
//                                                                          TProperty("2", TObjectVal(2))
//                                                                          TProperty("3", TObjectVal(3)) ]))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParmWithChoices pmid pid fid rt = 
      
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Choices, 
//                                    TArray([ TObjectVal(1)
//                                             TObjectVal(2)
//                                             TObjectVal(3) ]))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([ ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.CustomChoices, 
//                                                            TObjectJson([ TProperty("1", TObjectVal(1))
//                                                                          TProperty("2", TObjectVal(2))
//                                                                          TProperty("3", TObjectVal(3)) ]))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParmWithDefault pmid pid fid rt = 
       
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Default, TObjectVal(4))
                          
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeOptParm pmid pid fid rt d ml p = 
      
        
//        let p = 
//            TObjectJson([ TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([ ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(d))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(ml))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(p))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(true)) ])) ])
//        TProperty(pmid, p)
    
//    let makeDTParm pmid pid = 
       
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Default, TObjectVal("2016-02-16"))
//                          TProperty
//                              (JsonPropertyNames.Links, 
//                               TArray([  ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal("Parm"))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("date"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.CustomMask, TObjectVal("d"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeParmWithConditionalChoices pmid pid fid rt = 
       
//        let autoRel = RelValues.Prompt + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let acurl = sprintf "services/%s/actions/%s/params/%s/prompt" sName pid pmid
    
        
//        let argP = 
//            TProperty(JsonPropertyNames.Arguments, 
//                      TObjectJson([ TProperty("parm4", 
//                                              TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null))
//                                                            TProperty(JsonPropertyNames.Links, TArray([  ])) ])) ]))
        
//        let ac = TObjectJson(argP :: makeLinkPropWithMethodAndTypes "GET" autoRel acurl RepresentationTypes.Prompt "" "" true)
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, 
//                                    TArray([ 
//                                             ac ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeIntParmWithConditionalChoices pmid pid fid rt = 
      
//        let autoRel = RelValues.Prompt + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let acurl = sprintf "services/%s/actions/%s/params/%s/prompt" sName pid pmid
     
        
//        let argP = 
//            TProperty(JsonPropertyNames.Arguments, 
//                      TObjectJson([ TProperty("parm3", 
//                                              TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null))
//                                                            TProperty(JsonPropertyNames.Links, TArray([  ])) ]))
//                                    TProperty("parm4", 
//                                              TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null))
//                                                            TProperty(JsonPropertyNames.Links, TArray([  ])) ])) ]))
        
//        let ac = TObjectJson(argP :: makeLinkPropWithMethodAndTypes "GET" autoRel acurl RepresentationTypes.Prompt "" "" true)
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, 
//                                    TArray([ 
//                                             ac ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("int"))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let makeStringParmWithConditionalChoices pmid pid fid rt = 
       
//        let autoRel = RelValues.Prompt + mp RelParamValues.Action pid + mp RelParamValues.Param pmid
//        let acurl = sprintf "services/%s/actions/%s/params/%s/prompt" sName pid pmid
      
//        let argP = 
//            TProperty(JsonPropertyNames.Arguments, 
//                      TObjectJson([ TProperty("parm3", 
//                                              TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null))
//                                                            TProperty(JsonPropertyNames.Links, TArray([  ])) ]))
//                                    TProperty("parm4", 
//                                              TObjectJson([ TProperty(JsonPropertyNames.Value, TObjectVal(null))
//                                                            TProperty(JsonPropertyNames.Links, TArray([  ])) ])) ]))
        
//        let ac = TObjectJson(argP :: makeLinkPropWithMethodAndTypes "GET" autoRel acurl RepresentationTypes.Prompt "" "" true)
        
//        let p = 
//            TObjectJson([ TProperty(JsonPropertyNames.Links, 
//                                    TArray([ 
//                                             ac ]))
//                          TProperty(JsonPropertyNames.Extensions, 
//                                    TObjectJson([ TProperty(JsonPropertyNames.FriendlyName, TObjectVal(fid))
//                                                  TProperty(JsonPropertyNames.Description, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.ReturnType, TObjectVal(rt))
//                                                  TProperty(JsonPropertyNames.Format, TObjectVal("string"))
//                                                  TProperty(JsonPropertyNames.MaxLength, TObjectVal(0))
//                                                  TProperty(JsonPropertyNames.Pattern, TObjectVal(""))
//                                                  TProperty(JsonPropertyNames.Optional, TObjectVal(false)) ])) ])
//        TProperty(pmid, p)
    
//    let p1 = makeIntParm "parm1" "AnActionReturnsObjectWithParameterAnnotatedQueryOnly" "Parm1" (ttc "number")
//    let p2 = makeIntParm "parm1" "AnActionReturnsObjectWithParameters" "Parm1" (ttc "number")
//    let p3 = makeParm "parm2" "AnActionReturnsObjectWithParameters" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p4 = makeIntParm "parm1" "AnActionReturnsObjectWithParametersAnnotatedIdempotent" "Parm1" (ttc "number")
//    let p5 = makeParm "parm2" "AnActionReturnsObjectWithParametersAnnotatedIdempotent" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p6 = makeIntParm "parm1" "AnActionReturnsObjectWithParametersAnnotatedQueryOnly" "Parm1" (ttc "number")
//    let p7 = makeParm "parm2" "AnActionReturnsObjectWithParametersAnnotatedQueryOnly" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p8 = makeOptParm "parm" "AnActionWithOptionalParm" "Optional Parm" (ttc "string") "an optional parm" 101 "[A-Z]"
//    let p9 = makeOptParm "parm" "AnActionWithOptionalParmQueryOnly" "Parm" (ttc "string") "" 0 ""
//    let p10 = makeIntParm "parm1" "AnActionWithParametersWithChoicesWithDefaults" "Parm1" (ttc "number")
//    let p11 = makeIntParmWithChoicesAndDefault "parm7" "AnActionWithParametersWithChoicesWithDefaults" "Parm7" (ttc "number")
//    let p12 = makeParm "parm2" "AnActionWithParametersWithChoicesWithDefaults" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p13 = makeParmWithChoicesAndDefault "parm8" "AnActionWithParametersWithChoicesWithDefaults" "Parm8" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p14 = makeParm "parm2" "AnActionWithReferenceParameter" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p15 = makeParmWithChoices "parm4" "AnActionWithReferenceParameterWithChoices" "Parm4" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p16 = makeParmWithDefault "parm6" "AnActionWithReferenceParameterWithDefault" "Parm6" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p17 = makeParmWithAC "parm0" "AnActionWithReferenceParametersWithAutoComplete" "Parm0" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p18 = makeParmWithAC "parm1" "AnActionWithReferenceParametersWithAutoComplete" "Parm1" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p19 = makeValueParm "parm" "AnOverloadedAction1" "Parm" (ttc "string")
//    let p20 = makeIntParm "parm1" "AnActionWithValueParameter" "Parm1" (ttc "number")
//    let p21 = makeIntParmWithChoices "parm3" "AnActionWithValueParameterWithChoices" "Parm3" (ttc "number")
//    let p22 = makeIntParmWithDefault "parm5" "AnActionWithValueParameterWithDefault" "Parm5" (ttc "number")
//    let p25 = makeIntParm "parm1" "AnActionReturnsCollectionWithParameters" "Parm1" (ttc "number")
//    let p26 = makeParm "parm2" "AnActionReturnsCollectionWithParameters" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p27 = makeIntParmWithHint "parm1" "AnActionReturnsCollectionWithScalarParameters" "Parm1" (ttc "number")
//    let p28 = makeValueParm "parm2" "AnActionReturnsCollectionWithScalarParameters" "Parm2" (ttc "string")
//    let p29 = makeIntParm "parm1" "AnActionReturnsQueryableWithParameters" "Parm1" (ttc "number")
//    let p30 = makeParm "parm2" "AnActionReturnsQueryableWithParameters" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p31 = makeIntParm "parm1" "AnActionReturnsQueryableWithScalarParameters" "Parm1" (ttc "number")
//    let p32 = makeValueParm "parm2" "AnActionReturnsQueryableWithScalarParameters" "Parm2" (ttc "string")
//    let p33 = makeIntParm "parm1" "AnActionReturnsScalarWithParameters" "Parm1" (ttc "number")
//    let p34 = makeParm "parm2" "AnActionReturnsScalarWithParameters" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p35 = makeIntParm "parm1" "AnActionReturnsVoidWithParameters" "Parm1" (ttc "number")
//    let p36 = makeParm "parm2" "AnActionReturnsVoidWithParameters" "Parm2" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p37 = makeIntParm "parm1" "AnActionValidateParameters" "Parm1" (ttc "number")
//    let p38 = makeIntParm "parm2" "AnActionValidateParameters" "Parm2" (ttc "number")
//    let p39 = makeDTParm "parm" "AnActionWithDateTimeParm"
//    let p40 = makeParmWithConditionalChoices "parm4" "AnActionWithReferenceParameterWithConditionalChoices" "Parm4" (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p41 = makeIntParmWithConditionalChoices "parm3" "AnActionWithValueParametersWithConditionalChoices" "Parm3" (ttc "number")
//    let p42 = makeStringParmWithConditionalChoices "parm4" "AnActionWithValueParametersWithConditionalChoices" "Parm4" (ttc "string")
//    let p43 = makeStringParmWithDefaults "parm" "AnActionWithCollectionParameter" "Parm" (ttc "list") (ttc "string")
//    let p44 = makeParmWithDefaults "parm" "AnActionWithCollectionParameterRef" "Parm" (ttc "list") (ttc "RestfulObjects.Test.Data.MostSimple")
//    let p45 = makeIntParmWithRange "parm1" "AnActionWithValueParameterWithRange" "Parm1" (ttc "number")

//    let expected = 
//        [ TProperty(JsonPropertyNames.MenuId, TObjectVal(mName))
//          TProperty(JsonPropertyNames.Title, TObjectVal("With Action Service"))
//          TProperty(JsonPropertyNames.Links, 
//                    TArray([ TObjectJson(makeGetLinkProp RelValues.Self (sprintf "menus/%s" mName) RepresentationTypes.Menu "")
//                              ]))
//          TProperty(JsonPropertyNames.Members, 
//                    TObjectJson([ 
////                                 TProperty
////                                      ("ADisabledAction", 
////                                       
////                                       TObjectJson
////                                           (TProperty(JsonPropertyNames.DisabledReason, TObjectVal("Always disabled")) 
////                                            :: makeServiceActionMemberNoParms "ADisabledAction" sName mst))
////                                  
////                                  TProperty
////                                      ("ADisabledCollectionAction", 
////                                       
////                                       TObjectJson
////                                           (TProperty(JsonPropertyNames.DisabledReason, TObjectVal("Always disabled")) 
////                                            :: makeServiceActionCollectionMemberNoParms "ADisabledCollectionAction" sName mst))
////                                  
////                                  TProperty
////                                      ("ADisabledQueryAction", 
////                                       
////                                       TObjectJson
////                                           (TProperty(JsonPropertyNames.DisabledReason, TObjectVal("Always disabled")) 
////                                            :: makeServiceActionCollectionMemberNoParms "ADisabledQueryAction" sName mst))
//                                  TProperty("AnAction", TObjectJson(makeServiceActionMemberNoParms "AnAction" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsViewModel", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionReturnsViewModel" sName (ttc "RestfulObjects.Test.Data.MostSimpleViewModel")))
                                  
//                                  TProperty
//                                      ("AnActionReturnsRedirectedObject", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionReturnsRedirectedObject" sName 
//                                                (ttc "RestfulObjects.Test.Data.RedirectedObject")))
                                  
//                                  TProperty
//                                      ("AnActionReturnsWithDateTimeKeyQueryOnly", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionReturnsWithDateTimeKeyQueryOnly" sName (ttc "RestfulObjects.Test.Data.WithDateTimeKey")))
//                                  TProperty("AnActionAnnotatedIdempotent", TObjectJson(makeServiceActionMemberNoParms "AnActionAnnotatedIdempotent" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionAnnotatedIdempotentReturnsViewModel", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionAnnotatedIdempotentReturnsViewModel" sName 
//                                                (ttc "RestfulObjects.Test.Data.MostSimpleViewModel")))
                                  
//                                  TProperty
//                                      ("AnActionAnnotatedIdempotentReturnsNull", 
//                                       TObjectJson(makeServiceActionMemberNoParms "AnActionAnnotatedIdempotentReturnsNull" sName mst))
//                                  TProperty("AnActionAnnotatedQueryOnly", TObjectJson(makeServiceActionMemberNoParms "AnActionAnnotatedQueryOnly" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionAnnotatedQueryOnlyReturnsViewModel", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionAnnotatedQueryOnlyReturnsViewModel" sName 
//                                                (ttc "RestfulObjects.Test.Data.MostSimpleViewModel")))
                                  
//                                  TProperty
//                                      ("AnActionAnnotatedQueryOnlyReturnsNull", 
//                                       TObjectJson(makeServiceActionMemberNoParms "AnActionAnnotatedQueryOnlyReturnsNull" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsCollection", TObjectJson(makeServiceActionCollectionMemberNoParms "AnActionReturnsCollection" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsCollectionEmpty", 
//                                       TObjectJson(makeServiceActionCollectionMemberNoParms "AnActionReturnsCollectionEmpty" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsCollectionNull", 
//                                       TObjectJson(makeServiceActionCollectionMemberNoParms "AnActionReturnsCollectionNull" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsCollectionWithParameters", 
//                                       TObjectJson(makeServiceActionCollectionMember "AnActionReturnsCollectionWithParameters" sName mst [ p25; p26 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsCollectionWithScalarParameters", 
//                                       TObjectJson(makeServiceActionCollectionMember "AnActionReturnsCollectionWithScalarParameters" sName mst [ p27; p28 ]))
//                                  TProperty("AnActionReturnsNull", TObjectJson(makeServiceActionMemberNoParms "AnActionReturnsNull" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsNullViewModel", 
                                       
//                                       TObjectJson
//                                           (makeServiceActionMemberNoParms "AnActionReturnsNullViewModel" sName 
//                                                (ttc "RestfulObjects.Test.Data.MostSimpleViewModel")))
                                  
//                                  TProperty
//                                      ("AnActionReturnsObjectWithParameterAnnotatedQueryOnly", 
//                                       TObjectJson(makeServiceActionMember "AnActionReturnsObjectWithParameterAnnotatedQueryOnly" sName mst [ p1 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsObjectWithParameters", 
//                                       TObjectJson(makeServiceActionMember "AnActionReturnsObjectWithParameters" sName mst [ p2; p3 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsObjectWithParametersAnnotatedIdempotent", 
//                                       TObjectJson(makeServiceActionMember "AnActionReturnsObjectWithParametersAnnotatedIdempotent" sName mst [ p4; p5 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsObjectWithParametersAnnotatedQueryOnly", 
//                                       TObjectJson(makeServiceActionMember "AnActionReturnsObjectWithParametersAnnotatedQueryOnly" sName mst [ p6; p7 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsQueryable", TObjectJson(makeServiceActionCollectionMemberNoParms "AnActionReturnsQueryable" sName mst))
                                  
//                                  TProperty
//                                      ("AnActionReturnsQueryableWithParameters", 
//                                       TObjectJson(makeServiceActionCollectionMember "AnActionReturnsQueryableWithParameters" sName mst [ p29; p30 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsQueryableWithScalarParameters", 
//                                       TObjectJson(makeServiceActionCollectionMember "AnActionReturnsQueryableWithScalarParameters" sName mst [ p31; p32 ]))
                                  
//                                  TProperty
//                                      ("AnActionReturnsScalar", 
//                                       TObjectJson(makeActionMemberNumber "services" "AnActionReturnsScalar" sName "An Action Returns Scalar" "" "int" []))
                                  
//                                  TProperty
//                                      ("AnActionReturnsScalarEmpty", 
                                       
//                                       TObjectJson
//                                           (makeActionMemberString "services" "AnActionReturnsScalarEmpty" sName "An Action Returns Scalar Empty" "" "string" []))
                                  
//                                  TProperty
//                                      ("AnActionReturnsScalarNull", 
                                       
//                                       TObjectJson
//                                           (makeActionMemberString "services" "AnActionReturnsScalarNull" sName "An Action Returns Scalar Null" "" "string" []))
                                  
//                                  TProperty
//                                      ("AnActionReturnsScalarWithParameters", 
                                       
//                                       TObjectJson
//                                           (makeActionMemberNumber "services" "AnActionReturnsScalarWithParameters" sName 
//                                                "An Action Returns Scalar With Parameters" "" "int" [ p33; p34 ]))
//                                  TProperty("AnActionReturnsVoid", TObjectJson(makeServiceActionVoidMember "AnActionReturnsVoid" sName))
                                  
//                                  TProperty
//                                      ("AnActionReturnsVoidWithParameters", 
                                       
//                                       TObjectJson
//                                           (makeVoidActionMember "services" "AnActionReturnsVoidWithParameters" sName "An Action Returns Void With Parameters" 
//                                                "an action for testing" [ p35; p36 ]))
                                  
//                                  TProperty
//                                      ("AnActionValidateParameters", 
                                       
//                                       TObjectJson
//                                           (makeActionMemberNumber "services" "AnActionValidateParameters" sName "An Action Validate Parameters" "" "int" 
//                                                [ p37; p38 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithCollectionParameter", 
                                       
//                                       TObjectJson
//                                           (makeVoidActionMember "services" "AnActionWithCollectionParameter" sName "An Action With Collection Parameter" "" 
//                                                [ p43 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithCollectionParameterRef", 
                                       
//                                       TObjectJson
//                                           (makeVoidActionMember "services" "AnActionWithCollectionParameterRef" sName "An Action With Collection Parameter Ref" 
//                                                "" [ p44 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithDateTimeParm", 
//                                       TObjectJson(makeVoidActionMember "services" "AnActionWithDateTimeParm" sName "An Action With Date Time Parm" "" [ p39 ]))
//                                  TProperty("AnActionWithOptionalParm", TObjectJson(makeServiceActionMember "AnActionWithOptionalParm" sName mst [ p8 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithOptionalParmQueryOnly", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithOptionalParmQueryOnly" sName mst [ p9 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithParametersWithChoicesWithDefaults", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithParametersWithChoicesWithDefaults" sName mst [ p10; p11; p12; p13 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithReferenceParameter", TObjectJson(makeServiceActionMember "AnActionWithReferenceParameter" sName mst [ p14 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithReferenceParameterWithChoices", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithReferenceParameterWithChoices" sName mst [ p15 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithReferenceParameterWithConditionalChoices", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithReferenceParameterWithConditionalChoices" sName mst [ p40 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithReferenceParameterWithDefault", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithReferenceParameterWithDefault" sName mst [ p16 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithReferenceParametersWithAutoComplete", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithReferenceParametersWithAutoComplete" sName mst [ p17; p18 ]))
                                  
//                                  TProperty
//                                      ("AnOverloadedAction", 
//                                       TObjectJson(makeActionMember "services" "AnOverloadedAction" sName "An Overloaded Action" "" mst []))
                                  
////                                  TProperty
////                                      ("AnOverloadedAction1", 
////                                       TObjectJson(makeActionMember "services" "AnOverloadedAction1" sName "An Overloaded Action" "" mst [ p19 ]))
//                                  TProperty("AnActionWithValueParameter", TObjectJson(makeServiceActionMember "AnActionWithValueParameter" sName mst [ p20 ]))
                                  
//                                  TProperty("AnActionWithValueParameterWithRange", TObjectJson(makeServiceActionMember "AnActionWithValueParameterWithRange" sName mst [ p45 ]))

//                                  TProperty
//                                      ("AnActionWithValueParametersWithConditionalChoices", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithValueParametersWithConditionalChoices" sName mst [ p41; p42 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithValueParameterWithChoices", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithValueParameterWithChoices" sName mst [ p21 ]))
                                  
//                                  TProperty
//                                      ("AnActionWithValueParameterWithDefault", 
//                                       TObjectJson(makeServiceActionMember "AnActionWithValueParameterWithDefault" sName mst [ p22 ]))
//                                  TProperty("AnError", TObjectJson(makeActionMemberNumber "services" "AnError" sName "An Error" "" "int" []))
//                                  TProperty("AnErrorCollection", TObjectJson(makeServiceActionCollectionMemberNoParms "AnErrorCollection" sName mst))
//                                  TProperty("AnErrorQuery", TObjectJson(makeServiceActionCollectionMemberNoParms "AnErrorQuery" sName mst)) ]))
//          TProperty(JsonPropertyNames.Extensions, 
//                    TObjectJson([  ])) ]
//    Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, jsonResult)
//    Assert.AreEqual(new typeType(RepresentationTypes.Menu, ""), result.Content.Headers.ContentType)
//    assertNonExpiringCache result
//    Assert.IsNull(result.Headers.ETag)
//    compareObject expected parsedResult

//let InvalidGetMenu(api : RestfulObjectsControllerBase) = 
//    let sName = ""
//    let url = sprintf "http://localhost/menus/%s" sName
//    let args = CreateReservedArgs ""
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(sName, args)
//    let jsonResult = readSnapshotToJson result
//    Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, jsonResult)
//    Assert.AreEqual("199 RestfulObjects \"Exception of type 'NakedObjects.Facade.BadRequestNOSException' was thrown.\"", result.Headers.Warning.ToString())
//    Assert.AreEqual("", jsonResult)

//let NotFoundGetMenu(api : RestfulObjectsControllerBase) = 
//    let sName =  "NoSuchMenu"
//    let url = sprintf "http://localhost/menus/%s" sName
//    let args = CreateReservedArgs ""
//    api.Request <- jsonGetMsg (url)
//    let result = api.GetMenu(sName, args)
//    let jsonResult = readSnapshotToJson result
//    Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, jsonResult)
//    Assert.AreEqual(sprintf "199 RestfulObjects \"No such menu %s\"" sName, result.Headers.Warning.ToString())
//    Assert.AreEqual("", jsonResult)

//let NotAcceptableGetMenuWrongMediaType(api : RestfulObjectsControllerBase) = 
//    let sName =  "RestDataRepository"
//    let url = sprintf "http://localhost/menus/%s" sName
//    let args = CreateReservedArgs ""
//    try 
//        let msg = jsonGetMsg (url)
//        let headers = new RequestHeaders(msg.Headers)
//        headers.Accept.Single().Parameters.Add(new NameValueHeaderValue("profile", (makeProfile RepresentationTypes.ObjectCollection)))
//        api.Request <- msg
//        api.GetMenu(sName, args) |> ignore
//        Assert.Fail("expect exception")
//    with :? HttpResponseException as ex -> Assert.AreEqual(HttpStatusCode.NotAcceptable, ex.Response.StatusCode)

