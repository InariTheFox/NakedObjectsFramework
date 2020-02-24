﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Common.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Net.Http.Headers;
using NakedObjects.Facade;
using NakedObjects.Facade.Contexts;
using NakedObjects.Rest.API;
using NakedObjects.Rest.Model;
using NakedObjects.Rest.Snapshot.Constants;
using NakedObjects.Rest.Snapshot.Representations;
using NakedObjects.Rest.Snapshot.Utility;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace NakedObjects.Rest {
    public class RestfulObjectsControllerBase : ControllerBase {
        #region static and routes

        private static readonly ILog Logger = LogManager.GetLogger<RestfulObjectsControllerBase>();

        static RestfulObjectsControllerBase() {
            // defaults 
            CacheSettings = (0, 0, 0);
            DefaultPageSize = 20;
            InlineDetailsInActionMemberRepresentations = true;
            InlineDetailsInCollectionMemberRepresentations = true;
            InlineDetailsInPropertyMemberRepresentations = true;
            AllowMutatingActionOnImmutableObject = false;
        }

        protected RestfulObjectsControllerBase(IFrameworkFacade frameworkFacade) {
            FrameworkFacade = frameworkFacade;
            OidStrategy = frameworkFacade.OidStrategy;
        }

        public static bool IsReadOnly { get; set; }

        public static bool InlineDetailsInActionMemberRepresentations { get; set; }

        public static bool InlineDetailsInCollectionMemberRepresentations { get; set; }

        public static bool InlineDetailsInPropertyMemberRepresentations { get; set; }

        // cache settings in seconds, 0 = no cache, "no, short, long")   
        public static (int, int, int) CacheSettings { get; set; }

        public static int DefaultPageSize {
            get => RestControlFlags.ConfiguredPageSize;
            set => RestControlFlags.ConfiguredPageSize = value;
        }

        public static bool AcceptHeaderStrict {
            get => RestSnapshot.AcceptHeaderStrict;
            set => RestSnapshot.AcceptHeaderStrict = value;
        }

        protected IFrameworkFacade FrameworkFacade { get; set; }
        public IOidStrategy OidStrategy { get; set; }
        public static bool AllowMutatingActionOnImmutableObject { get; set; }

        private static string PrefixRoute(string segment, string prefix)
            => string.IsNullOrWhiteSpace(prefix) ? segment : EnsureTrailingSlash(prefix) + segment;

        private static string EnsureTrailingSlash(string path)
            => path.EndsWith("/") ? path : path + "/";

        public static void AddRestRoutes(IRouteBuilder routes, string routePrefix = "") {
            if (!string.IsNullOrWhiteSpace(routePrefix)) {
                UriMtHelper.GetApplicationPath = (req) => {
                    var appPath = req.PathBase.ToString() ?? "";
                    return EnsureTrailingSlash(appPath) + EnsureTrailingSlash(routePrefix);
                };
            }

            var domainTypes = PrefixRoute(SegmentValues.DomainTypes, routePrefix);
            var services = PrefixRoute(SegmentValues.Services, routePrefix);
            var objects = PrefixRoute(SegmentValues.Objects, routePrefix);
            var images = PrefixRoute(SegmentValues.Images, routePrefix);
            var user = PrefixRoute(SegmentValues.User, routePrefix);
            var version = PrefixRoute(SegmentValues.Version, routePrefix);
            var home = PrefixRoute(SegmentValues.HomePage, routePrefix);

            // custom extension 
            var menus = PrefixRoute(SegmentValues.Menus, routePrefix);

            // ReSharper disable RedundantArgumentName
            routes.MapRoute("GetInvokeIsTypeOf",
                template: domainTypes + "/{typeName}/" + SegmentValues.TypeActions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "GetInvokeTypeActions"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidInvokeIsTypeOf",
                template: domainTypes + "/{typeName}/" + SegmentValues.TypeActions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetInvokeOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "GetInvokeOnService"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("PutInvokeOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "PutInvokeOnService"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("PUT")}
            );

            routes.MapRoute("PostInvokeOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "PostInvokeOnService"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("POST")}
            );

            routes.MapRoute("InvalidInvokeOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetInvoke",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "GetInvoke"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("PutInvoke",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "PutInvoke"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("PUT")}
            );

            routes.MapRoute("PostInvoke",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "PostInvoke"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("POST")}
            );

            routes.MapRoute("InvalidInvoke",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Invoke,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("InvalidActionParameterType",
                template: domainTypes + "/{typeName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Params + "/{parmName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("InvalidActionType",
                template: domainTypes + "/{typeName}/" + SegmentValues.Actions + "/{actionName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetAction",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}",
                defaults: new {controller = "RestfulObjects", action = "GetAction"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidAction",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("InvalidCollectionType",
                template: domainTypes + "/{typeName}/" + SegmentValues.Collections + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("DeleteCollection",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Collections + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "DeleteCollection"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("DELETE")}
            );

            routes.MapRoute("PostCollection",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Collections + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "PostCollection"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("POST")}
            );

            routes.MapRoute("GetCollection",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Collections + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "GetCollection"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidCollection",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Collections + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("InvalidPropertyType",
                template: domainTypes + "/{typeName}/" + SegmentValues.Properties + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("DeleteProperty",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "DeleteProperty"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("DELETE")}
            );

            routes.MapRoute("PutProperty",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "PutProperty"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("PUT")}
            );

            routes.MapRoute("GetProperty",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "GetProperty"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidProperty",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("PutObject",
                template: objects + "/{domainType}/{instanceId}",
                defaults: new {controller = "RestfulObjects", action = "PutObject"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("PUT")}
            );

            routes.MapRoute("GetObject",
                template: objects + "/{domainType}/{instanceId}",
                defaults: new {controller = "RestfulObjects", action = "GetObject"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidObject",
                template: objects + "/{domainType}/{instanceId}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("PutPersistPropertyPrompt",
                template: objects + "/{domainType}/" + SegmentValues.Properties + "/{propertyName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "PutPersistPropertyPrompt"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("PUT")}
            );

            routes.MapRoute("InvalidPersistPropertyPrompt",
                template: objects + "/{domainType}/" + SegmentValues.Properties + "/{propertyName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetPropertyPrompt",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "GetPropertyPrompt"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidPropertyPrompt",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetParameterPrompt",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Params + "/{parmName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "GetParameterPrompt"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidParameterPrompt",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Params + "/{parmName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetParameterPromptOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Params + "/{parmName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "GetParameterPromptOnService"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidParameterPromptOnService",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}/" + SegmentValues.Params + "/{parmName}/" + SegmentValues.Prompt,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("GetCollectionValue",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Collections + "/{propertyName}/" + SegmentValues.CollectionValue,
                defaults: new {controller = "RestfulObjects", action = "GetCollectionValue"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidCollectionValue",
                template: objects + "/{domainType}/{instanceId}/" + SegmentValues.Properties + "/{propertyName}/" + SegmentValues.CollectionValue,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Persist",
                template: objects + "/{domainType}",
                defaults: new {controller = "RestfulObjects", action = "PostPersist"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("POST")}
            );

            routes.MapRoute("InvalidPersist",
                template: objects + "/{domainType}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Image",
                template: images + "/{imageId}",
                defaults: new {controller = "RestfulObjects", action = "GetImage"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidImage",
                template: images + "/{imageId}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("ServiceAction",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}",
                defaults: new {controller = "RestfulObjects", action = "GetServiceAction"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidServiceAction",
                template: services + "/{serviceName}/" + SegmentValues.Actions + "/{actionName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Service",
                template: services + "/{serviceName}",
                defaults: new {controller = "RestfulObjects", action = "GetService"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidService",
                template: services + "/{serviceName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Menu",
                template: menus + "/{menuName}",
                defaults: new {controller = "RestfulObjects", action = "GetMenu"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidMenu",
                template: menus + "/{menuName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("InvalidDomainType",
                template: domainTypes + "/{typeName}",
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("DomainTypes",
                template: domainTypes,
                defaults: new {controller = "RestfulObjects", action = "GetDomainTypes"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidDomainTypes",
                template: domainTypes,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Version",
                template: version,
                defaults: new {controller = "RestfulObjects", action = "GetVersion"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidVersion",
                template: version,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Services",
                template: services,
                defaults: new {controller = "RestfulObjects", action = "GetServices"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidServices",
                template: services,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Menus",
                template: menus,
                defaults: new {controller = "RestfulObjects", action = "GetMenus"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidMenus",
                template: menus,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("User",
                template: user,
                defaults: new {controller = "RestfulObjects", action = "GetUser"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidUser",
                template: user,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});

            routes.MapRoute("Home",
                template: home,
                defaults: new {controller = "RestfulObjects", action = "GetHome"},
                constraints: new {httpMethod = new HttpMethodRouteConstraint("GET")}
            );

            routes.MapRoute("InvalidHome",
                template: home,
                defaults: new {controller = "RestfulObjects", action = "InvalidMethod"});
            // ReSharper restore RedundantArgumentName
        }

        #endregion

        #region api

        [FromQuery(Name = RestControlFlags.ValidateOnlyReserved)]
        public bool ValidateOnly { get; set; }

        [FromQuery(Name = RestControlFlags.DomainTypeReserved)]
        public string DomainType { get; set; }

        [FromQuery(Name = RestControlFlags.ElementTypeReserved)]
        public string ElementType { get; set; }

        [FromQuery(Name = RestControlFlags.DomainModelReserved)]
        public string DomainModel { get; set; }

        [FromQuery(Name = RestControlFlags.FollowLinksReserved)]
        public bool? FollowLinks { get; set; }

        [FromQuery(Name = RestControlFlags.SortByReserved)]
        public bool SortBy { get; set; }

        [FromQuery(Name = RestControlFlags.SearchTermReserved)]
        public string SearchTerm { get; set; }

        [FromQuery(Name = RestControlFlags.PageReserved)]
        public int Page { get; set; }

        [FromQuery(Name = RestControlFlags.PageSizeReserved)]
        public int PageSize { get; set; }

        [FromQuery(Name = RestControlFlags.InlinePropertyDetailsReserved)]
        public bool? InlinePropertyDetails { get; set; }

        [FromQuery(Name = RestControlFlags.InlineCollectionItemsReserved)]
        public bool? InlineCollectionItems { get; set; }

        public virtual ActionResult GetHome() =>
            InitAndHandleErrors(SnapshotFactory.HomeSnapshot(OidStrategy, Request, GetFlags()));

        public virtual ActionResult GetUser() =>
            InitAndHandleErrors(SnapshotFactory.UserSnapshot(OidStrategy, FrameworkFacade.GetUser, Request, GetFlags()));

        public virtual ActionResult GetServices() =>
            InitAndHandleErrors(SnapshotFactory.ServicesSnapshot(OidStrategy, FrameworkFacade.GetServices, Request, GetFlags()));

        public virtual ActionResult GetMenus() =>
            InitAndHandleErrors(SnapshotFactory.MenusSnapshot(OidStrategy, FrameworkFacade.GetMainMenus, Request, GetFlags()));

        public virtual ActionResult GetVersion() =>
            InitAndHandleErrors(SnapshotFactory.VersionSnapshot(OidStrategy, GetOptionalCapabilities, Request, GetFlags()));

        public virtual ActionResult GetService(string serviceName) =>
            InitAndHandleErrors(SnapshotFactory.ObjectSnapshot(OidStrategy, () => FrameworkFacade.GetServiceByName(serviceName), Request, GetFlags()));

        public virtual ActionResult GetMenu(string menuName) =>
            InitAndHandleErrors(SnapshotFactory.MenuSnapshot(OidStrategy, () => FrameworkFacade.GetMenuByName(menuName), Request, GetFlags()));

        public virtual ActionResult GetServiceAction(string serviceName, string actionName) =>
            InitAndHandleErrors(SnapshotFactory.ActionSnapshot(OidStrategy, () => FrameworkFacade.GetServiceActionByName(serviceName, actionName), Request, GetFlags()));

        public virtual ActionResult GetImage(string imageId) =>
            InitAndHandleErrors(SnapshotFactory.ObjectSnapshot(OidStrategy, () => FrameworkFacade.GetImage(imageId), Request, GetFlags()));

        public virtual ActionResult GetObject(string domainType, string instanceId) =>
            InitAndHandleErrors(SnapshotFactory.ObjectSnapshot(OidStrategy, () => FrameworkFacade.GetObjectByName(domainType, instanceId), Request, GetFlags()));

        public virtual ActionResult GetPropertyPrompt(string domainType, string instanceId, string propertyName, ArgumentMap arguments) {
            Func<RestSnapshot> PromptSnapshot() {
                var (argsContext, flags) = ProcessArgumentMap(arguments, false, true);
                PropertyContextFacade PropertyContext() => FrameworkFacade.GetPropertyByName(domainType, instanceId, propertyName, argsContext);
                return SnapshotFactory.PromptSnaphot(OidStrategy, PropertyContext, Request, flags);
            }

            return InitAndHandleErrors(PromptSnapshot());
        }

        public virtual ActionResult PutPersistPropertyPrompt(string domainType, string propertyName, PromptArgumentMap promptArguments) {
            Func<RestSnapshot> PromptSnapshot() {
                var persistArgs = ProcessPromptArguments(promptArguments);
                var (promptArgs, flags) = ProcessArgumentMap(promptArguments, false, false);
                PropertyContextFacade PropertyContext() => FrameworkFacade.GetTransientPropertyByName(domainType, propertyName, persistArgs, promptArgs);
                return SnapshotFactory.PromptSnaphot(OidStrategy, PropertyContext, Request, flags);
            }

            return InitAndHandleErrors(PromptSnapshot());
        }

        public virtual ActionResult GetParameterPrompt(string domainType, string instanceId, string actionName, string parmName, ArgumentMap arguments) {
            Func<RestSnapshot> PromptSnapshot() {
                var (argsContext, flags) = ProcessArgumentMap(arguments, false, true);
                ParameterContextFacade ParameterContext() => FrameworkFacade.GetObjectParameterByName(domainType, instanceId, actionName, parmName, argsContext);
                return SnapshotFactory.PromptSnaphot(OidStrategy, ParameterContext, Request, flags);
            }

            return InitAndHandleErrors(PromptSnapshot());
        }

        public virtual ActionResult GetParameterPromptOnService(string serviceName, string actionName, string parmName, ArgumentMap arguments) {
            Func<RestSnapshot> PromptSnapshot() {
                var (argsContext, flags) = ProcessArgumentMap(arguments, false, true);
                ParameterContextFacade ParameterContext() => FrameworkFacade.GetServiceParameterByName(serviceName, actionName, parmName, argsContext);
                return SnapshotFactory.PromptSnaphot(OidStrategy, ParameterContext, Request, flags);
            }

            return InitAndHandleErrors(PromptSnapshot());
        }

        public virtual ActionResult PutObject(string domainType, string instanceId, ArgumentMap arguments) {
            (Func<RestSnapshot>, bool) PutObject() {
                RejectRequestIfReadOnly();
                var (argsContext, flags) = ProcessArgumentMap(arguments, true, false);
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                ObjectContextFacade context = FrameworkFacade.PutObjectAndValidate(domainType, instanceId, argsContext);
                return (SnapshotFactory.ObjectSnapshot(OidStrategy, () => context, Request, flags), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = PutObject();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult PostPersist(string domainType, PersistArgumentMap arguments) {
            (Func<RestSnapshot>, bool) PersistObject() {
                RejectRequestIfReadOnly();
                var (argsContext, flags) = ProcessPersistArguments(arguments);
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                ObjectContextFacade context = FrameworkFacade.PersistObjectAndValidate(domainType, argsContext, flags);
                return (SnapshotFactory.ObjectSnapshot(OidStrategy, () => context, Request, flags, HttpStatusCode.Created), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = PersistObject();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult GetProperty(string domainType, string instanceId, string propertyName) =>
            InitAndHandleErrors(SnapshotFactory.PropertySnapshot(OidStrategy, () => FrameworkFacade.GetPropertyByName(domainType, instanceId, propertyName), Request, GetFlags()));

        public virtual ActionResult GetCollection(string domainType, string instanceId, string propertyName) =>
            InitAndHandleErrors(SnapshotFactory.PropertySnapshot(OidStrategy, () => FrameworkFacade.GetCollectionPropertyByName(domainType, instanceId, propertyName), Request, GetFlags()));

        public virtual ActionResult GetCollectionValue(string domainType, string instanceId, string propertyName) =>
            InitAndHandleErrors(SnapshotFactory.CollectionValueSnapshot(OidStrategy, () => FrameworkFacade.GetCollectionPropertyByName(domainType, instanceId, propertyName), Request, GetFlags()));

        public virtual ActionResult GetAction(string domainType, string instanceId, string actionName) =>
            InitAndHandleErrors(SnapshotFactory.ActionSnapshot(OidStrategy, () => FrameworkFacade.GetObjectActionByName(domainType, instanceId, actionName), Request, GetFlags()));

        public virtual ActionResult PutProperty(string domainType, string instanceId, string propertyName, SingleValueArgument argument) {
            (Func<RestSnapshot>, bool) PutProperty() {
                RejectRequestIfReadOnly();
                var (argContext, flags) = ProcessArgument(argument);
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                var context = FrameworkFacade.PutPropertyAndValidate(domainType, instanceId, propertyName, argContext);
                return (SnapshotFactory.PropertySnapshot(OidStrategy, () => context, Request, flags), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = PutProperty();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult DeleteProperty(string domainType, string instanceId, string propertyName) {
            (Func<RestSnapshot>, bool) DeleteProperty() {
                RejectRequestIfReadOnly();
                var (argContext, flags) = ProcessDeleteArgument();
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                var context = FrameworkFacade.DeletePropertyAndValidate(domainType, instanceId, propertyName, argContext);
                return (SnapshotFactory.PropertySnapshot(OidStrategy, () => context, Request, flags), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = DeleteProperty();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult PostCollection(string domainType, string instanceId, string propertyName, SingleValueArgument argument)
            => StatusCode((int) HttpStatusCode.Forbidden);

        public virtual ActionResult DeleteCollection(string domainType, string instanceId, string propertyName, SingleValueArgument argument)
            => StatusCode((int) HttpStatusCode.Forbidden);

        private ActionResult Invoke(string domainType, string instanceId, string actionName, ArgumentMap arguments, bool queryOnly) {
            (Func<RestSnapshot>, bool) Execute() {
                if (!queryOnly) {
                    RejectRequestIfReadOnly();
                }

                var (argsContext, flags) = ProcessArgumentMap(arguments, false, queryOnly);
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                var context = FrameworkFacade.ExecuteActionAndValidate(domainType, instanceId, actionName, argsContext);
                return (SnapshotFactory.ActionResultSnapshot(OidStrategy, () => context, Request, flags), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = Execute();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult GetInvoke(string domainType, string instanceId, string actionName, ArgumentMap arguments) =>
            Invoke(domainType, instanceId, actionName, arguments, true);

        public virtual ActionResult PutInvoke(string domainType, string instanceId, string actionName, ArgumentMap arguments) =>
            Invoke(domainType, instanceId, actionName, arguments, false);

        public virtual ActionResult PostInvoke(string domainType, string instanceId, string actionName, ArgumentMap arguments) =>
            Invoke(domainType, instanceId, actionName, arguments, false);

        private ActionResult InvokeOnService(string serviceName, string actionName, ArgumentMap arguments, bool queryOnly) {
            (Func<RestSnapshot>, bool) Execute() {
                if (!queryOnly) {
                    RejectRequestIfReadOnly();
                }

                // ignore concurrency always true here
                var (argsContext, flags) = ProcessArgumentMap(arguments, false, true);
                // seems strange to call and then wrap in lambda but need to validate here not when snapshot created
                var context = FrameworkFacade.ExecuteServiceActionAndValidate(serviceName, actionName, argsContext);
                return (SnapshotFactory.ActionResultSnapshot(OidStrategy, () => context, Request, flags), flags.ValidateOnly);
            }

            return InitAndHandleErrors(() => {
                var (snapshotFunc, validateOnly) = Execute();
                return SnapshotOrNoContent(snapshotFunc, validateOnly);
            });
        }

        public virtual ActionResult GetInvokeOnService(string serviceName, string actionName, ArgumentMap arguments) =>
            InvokeOnService(serviceName, actionName, arguments, true);

        public virtual ActionResult PutInvokeOnService(string serviceName, string actionName, ArgumentMap arguments) =>
            InvokeOnService(serviceName, actionName, arguments, false);

        public virtual ActionResult PostInvokeOnService(string serviceName, string actionName, ArgumentMap arguments) =>
            InvokeOnService(serviceName, actionName, arguments, true);

        public virtual ActionResult GetInvokeTypeActions(string typeName, string actionName, ArgumentMap arguments) {

            Func<RestSnapshot> GetTypeAction() => SnapshotFactory.TypeActionSnapshot(OidStrategy, () => GetIsTypeOf(actionName, typeName, arguments), Request, GetFlags());

            Func<RestSnapshot> TypeAction() =>
                actionName switch {
                    WellKnownIds.IsSubtypeOf => GetTypeAction(),
                    WellKnownIds.IsSupertypeOf => GetTypeAction(),
                    _ => throw new TypeActionResourceNotFoundException(actionName, typeName)
                };

            return InitAndHandleErrors(TypeAction());
        }

        public virtual ActionResult InvalidMethod() => StatusCode((int) HttpStatusCode.MethodNotAllowed);

        #endregion

        #region helpers

        private RestControlFlags GetFlags() {
            return RestControlFlags.FlagsFromArguments(ValidateOnly,
                Page,
                PageSize,
                DomainModel,
                InlineDetailsInActionMemberRepresentations,
                InlineDetailsInCollectionMemberRepresentations,
                InlinePropertyDetails.HasValue ? InlinePropertyDetails.Value : InlineDetailsInPropertyMemberRepresentations,
                InlineCollectionItems.HasValue && InlineCollectionItems.Value,
                AllowMutatingActionOnImmutableObject);
        }

        private RestControlFlags GetFlags(ArgumentMap arguments) {
            if (arguments.IsMalformed || arguments.ReservedArguments == null) {
                var errorMsg = arguments.IsMalformed ? arguments.MalformedReason : "Reserved args = null";
                var msg = $"Malformed arguments{(RestSnapshot.DebugWarnings ? " : " + errorMsg : "")}";

                throw new BadRequestNOSException(msg); // todo i18n
            }

            return GetFlagsFromArguments(arguments.ReservedArguments);
        }

        private RestControlFlags GetFlags(SingleValueArgument arguments) {
            if (arguments.IsMalformed || arguments.ReservedArguments == null) {
                var errorMsg = arguments.IsMalformed ? arguments.MalformedReason : "Reserved args = null";
                var msg = $"Malformed arguments{(RestSnapshot.DebugWarnings ? " : " + errorMsg : "")}";

                throw new BadRequestNOSException(msg); // todo i18n
            }

            return GetFlagsFromArguments(arguments.ReservedArguments);
        }

        private static RestControlFlags GetFlagsFromArguments(ReservedArguments reservedArguments) {
            return RestControlFlags.FlagsFromArguments(reservedArguments.ValidateOnly,
                reservedArguments.Page,
                reservedArguments.PageSize,
                reservedArguments.DomainModel,
                InlineDetailsInActionMemberRepresentations,
                InlineDetailsInCollectionMemberRepresentations,
                reservedArguments.InlinePropertyDetails.HasValue ? reservedArguments.InlinePropertyDetails.Value : InlineDetailsInPropertyMemberRepresentations,
                reservedArguments.InlineCollectionItems.HasValue && reservedArguments.InlineCollectionItems.Value,
                AllowMutatingActionOnImmutableObject);
        }

        private string GetIfMatchTag() {
            var headers = Request.GetTypedHeaders();

            if (headers.IfMatch.Any()) {
                string quotedTag = headers.IfMatch.First().Tag.ToString();
                return quotedTag.Replace("\"", "");
            }

            return null;
        }

        private RestSnapshot SnapshotOrNoContent(Func<RestSnapshot> ss, bool validateOnly)
            => validateOnly ? throw new NoContentNOSException() : ss();

        private MethodType GetExpectedMethodType(HttpMethod method) {
            if (method == HttpMethod.Get) {
                return MethodType.QueryOnly;
            }

            if (method == HttpMethod.Put) {
                return MethodType.Idempotent;
            }

            return MethodType.NonIdempotent;
        }

        private void SetCaching(ResponseHeaders m, RestSnapshot ss, (int, int, int) cacheSettings) {
            int cacheTime = 0;

            switch (ss.Representation.GetCaching()) {
                case CacheType.Transactional:
                    cacheTime = cacheSettings.Item1;
                    break;
                case CacheType.UserInfo:
                    cacheTime = cacheSettings.Item2;
                    break;
                case CacheType.NonExpiring:
                    cacheTime = cacheSettings.Item3;
                    break;
            }

            if (cacheTime == 0) {
                m.CacheControl = new CacheControlHeaderValue {NoCache = true};
                m.Append(HeaderNames.Pragma, "no-cache");
            }
            else {
                m.CacheControl = new CacheControlHeaderValue {MaxAge = new TimeSpan(0, 0, 0, cacheTime)};
            }

            DateTime now = DateTime.UtcNow;

            m.Date = new DateTimeOffset(now);
            m.Expires = new DateTimeOffset(now).Add(new TimeSpan(0, 0, 0, cacheTime));
        }

        private void AppendWarningHeader(ResponseHeaders responseHeaders, string warning) {
            if (!string.IsNullOrWhiteSpace(warning)) {
                responseHeaders.Append(HeaderNames.Warning, warning);
            }
        }

        private void SetHeaders(RestSnapshot ss) {
            var msg = ControllerContext.HttpContext.Response;
            var responseHeaders = GetResponseHeaders();

            foreach (WarningHeaderValue w in ss.WarningHeaders) {
                AppendWarningHeader(responseHeaders, w.ToString());
            }

            foreach (string allowHeader in ss.AllowHeaders) {
                responseHeaders.Append(HeaderNames.Allow, allowHeader);
            }

            if (ss.Location != null) {
                responseHeaders.Location = ss.Location;
            }

            if (ss.Etag != null) {
                responseHeaders.ETag = ss.Etag;
            }

            MediaTypeHeaderValue ct = ss.Representation.GetContentType();

            if (ct != null) {
                //formatter.SupportedMediaTypes.Add(ct);
            }

            responseHeaders.ContentType = ct;

            SetCaching(responseHeaders, ss, CacheSettings);

            ss.ValidateOutgoingMediaType(ss.Representation is AttachmentRepresentation);
            msg.StatusCode = (int) ss.HttpStatusCode;
        }

        private void ValidateDomainModel() {
            if (DomainModel != null && DomainModel != RestControlFlags.DomainModelType.Simple.ToString().ToLower() && DomainModel != RestControlFlags.DomainModelType.Formal.ToString().ToLower()) {

                var msg = $"Invalid domainModel: {DomainModel}";

                throw new ValidationException((int) HttpStatusCode.BadRequest, msg);
            }
        }

        private void ValidateBinding() {
            if (ModelState.ErrorCount > 0) {
                throw new BadRequestNOSException("Malformed arguments");
            }
        }

        private void Validate() {
            ValidateBinding();
            ValidateDomainModel();
        }

        private ActionResult InitAndHandleErrors(Func<RestSnapshot> f) {
            bool success = false;
            Exception endTransactionError = null;
            RestSnapshot ss;
            try {
                Validate();
                FrameworkFacade.Start();
                ss = f();
                success = true;
            }
            catch (ValidationException validationException) {
                var warning = RestUtils.ToWarningHeaderValue(199, validationException.Message);
                AppendWarningHeader(GetResponseHeaders(), warning.ToString());
                return StatusCode(validationException.StatusCode);
            }
            catch (RedirectionException redirectionException) {
                var responseHeaders = ControllerContext.HttpContext.Response.GetTypedHeaders();
                responseHeaders.Location = redirectionException.RedirectAddress;
                return StatusCode(redirectionException.StatusCode);
            }
            catch (NakedObjectsFacadeException e) {
                return ErrorResult(e);
            }
            catch (Exception e) {
                Logger.ErrorFormat("Unhandled exception from frameworkFacade {0} {1}", e.GetType(), e.Message);
                return ErrorResult(e);
            }
            finally {
                try {
                    FrameworkFacade.End(success);
                }
                catch (Exception e) {
                    // can't return from finally 
                    endTransactionError = e;
                }
            }

            if (endTransactionError != null) {
                return ErrorResult(endTransactionError);
            }

            try {
                return RepresentationResult(ss);
            }
            catch (ValidationException validationException) {
                var warning = RestUtils.ToWarningHeaderValue(199, validationException.Message);
                AppendWarningHeader(GetResponseHeaders(), warning.ToString());
                return StatusCode(validationException.StatusCode);
            }
            catch (NakedObjectsFacadeException e) {
                return ErrorResult(e);
            }
            catch (Exception e) {
                Logger.ErrorFormat("Unhandled exception while configuring message {0} {1}", e.GetType(), e.Message);
                return ErrorResult(e);
            }
        }

        private ActionResult ErrorResult(Exception e) => 
            RepresentationResult(new RestSnapshot(OidStrategy, e, Request));

        private ResponseHeaders GetResponseHeaders() =>
            ControllerContext.HttpContext.Response.GetTypedHeaders();

        private ActionResult RepresentationResult(RestSnapshot ss) {
            ss.Populate();
            SetHeaders(ss);

            if (ss.Representation is NullRepresentation) {
                return new NoContentResult();
            }

            // there maybe better way of doing 
            var attachmentRepresentation = ss.Representation as AttachmentRepresentation;

            if (attachmentRepresentation != null) {
                var responseHeaders = GetResponseHeaders();

                responseHeaders.Append(HeaderNames.ContentDisposition, attachmentRepresentation.ContentDisposition.ToString());

                return File(attachmentRepresentation.AsStream, attachmentRepresentation.GetContentType().ToString());
            }


            return new JsonResult(ss.Representation);
        }

        private static void RejectRequestIfReadOnly() {
            if (IsReadOnly) {
                var msg = RestSnapshot.DebugWarnings ?  "In readonly mode" : "";
                throw new ValidationException((int) HttpStatusCode.Forbidden, msg);
            }
        }

        private static void ValidateArguments(ArgumentMap arguments, bool errorIfNone = true) {
            if (arguments.IsMalformed) {
                var msg = $"Malformed arguments{(RestSnapshot.DebugWarnings ? " : " + arguments.MalformedReason : "")}";

                throw new BadRequestNOSException(msg); // todo i18n
            }

            if (!arguments.HasValue && errorIfNone) {
                throw new BadRequestNOSException("Missing arguments"); // todo i18n
            }
        }

        private static void ValidateArguments(SingleValueArgument arguments, bool errorIfNone = true) {
            if (arguments.IsMalformed) {
                var msg = $"Malformed arguments{(RestSnapshot.DebugWarnings ? " : " + arguments.MalformedReason : "")}"; 

                throw new BadRequestNOSException(msg); // todo i18n
            }

            if (!arguments.HasValue && errorIfNone) {
                throw new BadRequestNOSException("Missing arguments"); // todo i18n
            }
        }

        private static T HandleMalformed<T>(Func<T> f) {
            try {
                return f();
            }
            catch (BadRequestNOSException) {
                throw;
            }
            catch (ResourceNotFoundNOSException) {
                throw;
            }
            catch (Exception) {
                throw new BadRequestNOSException("Malformed arguments");
            }
        }

        private (object, RestControlFlags) ExtractValueAndFlags(SingleValueArgument argument) {
            return HandleMalformed(() => {
                ValidateArguments(argument);
                var flags = argument.ReservedArguments == null ? GetFlags() : GetFlags(argument);
                object parm = argument.Value.GetValue(FrameworkFacade, new UriMtHelper(OidStrategy, Request), OidStrategy);
                return (parm, flags);
            });
        }

        private IDictionary<string, object> ExtractValues(ArgumentMap arguments, bool errorIfNone) {
            return HandleMalformed(() => {
                ValidateArguments(arguments, errorIfNone);
                return arguments.Map.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetValue(FrameworkFacade, new UriMtHelper(OidStrategy, Request), OidStrategy));
            });
        }

        private (IDictionary<string, object>, RestControlFlags) ExtractValuesAndFlags(ArgumentMap arguments, bool errorIfNone) {
            return HandleMalformed(() => {
                ValidateArguments(arguments, errorIfNone);
                var dictionary = arguments.Map.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetValue(FrameworkFacade, new UriMtHelper(OidStrategy, Request), OidStrategy));
                return (dictionary, GetFlags(arguments));
            });
        }

        private (IDictionary<string, object>, RestControlFlags) ExtractValuesAndFlags(PromptArgumentMap arguments, bool errorIfNone) {
            return HandleMalformed(() => {
                ValidateArguments(arguments, errorIfNone);
                var dictionary = arguments.MemberMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.GetValue(FrameworkFacade, new UriMtHelper(OidStrategy, Request), OidStrategy));
                return (dictionary, GetFlags(arguments));
            });
        }

        private TypeActionInvokeContext GetIsTypeOf(string actionName, string typeName, ArgumentMap arguments) {
            ValidateArguments(arguments);
            var context = new TypeActionInvokeContext(actionName, typeName);
            if (!arguments.Map.ContainsKey(context.ParameterId)) {
                throw new BadRequestNOSException("Malformed arguments");
            }

            ITypeFacade thisSpecification = FrameworkFacade.GetDomainType(typeName);
            IValue parameter = arguments.Map[context.ParameterId];
            object value = parameter.GetValue(FrameworkFacade, new UriMtHelper(OidStrategy, Request), OidStrategy);
            var otherSpecification = (ITypeFacade) (value is ITypeFacade ? value : FrameworkFacade.GetDomainType((string) value));
            context.ThisSpecification = thisSpecification;
            context.OtherSpecification = otherSpecification;
            return context;
        }

        private (ArgumentsContextFacade, RestControlFlags) ProcessPersistArguments(PersistArgumentMap persistArgumentMap) {
            var (map, flags) = ExtractValuesAndFlags(persistArgumentMap, true);

            return (new ArgumentsContextFacade {
                Digest = GetIfMatchTag(),
                Values = map,
                Page = flags.Page,
                PageSize = flags.PageSize,
                ValidateOnly = flags.ValidateOnly
            }, flags);
        }

        private ArgumentsContextFacade ProcessPromptArguments(PromptArgumentMap promptArgumentMap) {
            var (dictionary, flags) = ExtractValuesAndFlags(promptArgumentMap, false);

            return new ArgumentsContextFacade {
                Digest = GetIfMatchTag(),
                Values = dictionary,
                Page = flags.Page,
                PageSize = flags.PageSize,
                ValidateOnly = flags.ValidateOnly
            };
        }

        private (ArgumentsContextFacade, RestControlFlags) ProcessArgumentMap(ArgumentMap arguments, bool errorIfNone, bool ignoreConcurrency) {
            var (map, flags) = arguments.ReservedArguments == null
                ? (ExtractValues(arguments, errorIfNone), GetFlags())
                : ExtractValuesAndFlags(arguments, errorIfNone);

            var facade = new ArgumentsContextFacade {
                Digest = ignoreConcurrency ? null : GetIfMatchTag(),
                Values = map,
                ValidateOnly = flags.ValidateOnly,
                Page = flags.Page,
                PageSize = flags.PageSize,
                SearchTerm = arguments.ReservedArguments?.SearchTerm,
                ExpectedActionType = GetExpectedMethodType(new HttpMethod(Request.Method))
            };
            return (facade, flags);
        }

        private (ArgumentContextFacade, RestControlFlags) ProcessArgument(SingleValueArgument argument) {
            var (value, flags) = ExtractValueAndFlags(argument);
            return (new ArgumentContextFacade {
                Digest = GetIfMatchTag(),
                Value = value,
                ValidateOnly = flags.ValidateOnly
            }, flags);
        }

        private (ArgumentContextFacade, RestControlFlags) ProcessDeleteArgument() {
            var flags = GetFlags();
            return (new ArgumentContextFacade {
                Digest = GetIfMatchTag(),
                Value = null,
                ValidateOnly = flags.ValidateOnly
            }, flags);
        }

        private static IDictionary<string, string> GetOptionalCapabilities() {
            return new Dictionary<string, string> {
                {"protoPersistentObjects", "yes"},
                {"deleteObjects", "no"},
                {"validateOnly", "yes"},
                {"domainModel", "simple"},
                {"blobsClobs", "attachments"},
                {"inlinedMemberRepresentations", "yes"}
            };
        }

        #endregion
    }
}