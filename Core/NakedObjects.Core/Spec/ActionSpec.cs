// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Interactions;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Core.Adapter;
using NakedObjects.Core.Interactions;
using NakedObjects.Core.Reflect;
using NakedObjects.Core.Util;

namespace NakedObjects.Core.Spec {
    public sealed class ActionSpec : MemberSpecAbstract, IActionSpec {
        private readonly IActionSpecImmutable actionSpecImmutable;
        private readonly ILogger<ActionSpec> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IMessageBroker messageBroker;
        private readonly INakedObjectManager nakedObjectManager;
        private readonly IServicesManager servicesManager;
        private readonly ITransactionManager transactionManager;
        private IObjectSpec elementSpec;
        private Where? executedWhere;
        private bool? hasReturn;
        private bool? isFinderMethod;

        private ITypeSpec onSpec;

        // cached values     
        private IObjectSpec returnSpec;

        public ActionSpec(SpecFactory memberFactory,
                          IMetamodelManager metamodel,
                          ILifecycleManager lifecycleManager,
                          ISession session,
                          IServicesManager servicesManager,
                          INakedObjectManager nakedObjectManager,
                          IActionSpecImmutable actionSpecImmutable,
                          IMessageBroker messageBroker,
                          ITransactionManager transactionManager,
                          IObjectPersistor persistor,
                          ILoggerFactory loggerFactory,
                          ILogger<ActionSpec> logger)
            : base(actionSpecImmutable?.Identifier?.MemberName, actionSpecImmutable, session, lifecycleManager, metamodel, persistor) {
            this.servicesManager = servicesManager ?? throw new InitialisationException($"{nameof(servicesManager)} is null");
            this.nakedObjectManager = nakedObjectManager ?? throw new InitialisationException($"{nameof(nakedObjectManager)} is null");
            this.actionSpecImmutable = actionSpecImmutable ?? throw new InitialisationException($"{nameof(actionSpecImmutable)} is null");
            this.messageBroker = messageBroker ?? throw new InitialisationException($"{nameof(messageBroker)} is null");
            this.transactionManager = transactionManager ?? throw new InitialisationException($"{nameof(transactionManager)} is null");
            this.loggerFactory = loggerFactory ?? throw new InitialisationException($"{nameof(loggerFactory)} is null");
            this.logger = logger ?? throw new InitialisationException($"{nameof(logger)} is null");
            var index = 0;
            Parameters = this.actionSpecImmutable.Parameters.Select(pp => memberFactory.CreateParameter(pp, this, index++)).ToArray();
        }

        private IActionInvocationFacet ActionInvocationFacet => actionSpecImmutable.GetFacet<IActionInvocationFacet>();

        public IActionSpec[] Actions => new IActionSpec[0];

        #region IActionSpec Members

        public override IObjectSpec ReturnSpec => returnSpec ??= MetamodelManager.GetSpecification(actionSpecImmutable.ReturnSpec);

        public override IObjectSpec ElementSpec => elementSpec ??= MetamodelManager.GetSpecification(actionSpecImmutable.ElementSpec);

        public ITypeSpec OnSpec => onSpec ??= MetamodelManager.GetSpecification(ActionInvocationFacet.OnType);

        public override Type[] FacetTypes => actionSpecImmutable.FacetTypes;

        public override IIdentifier Identifier => actionSpecImmutable.Identifier;

        public int ParameterCount => actionSpecImmutable.Parameters.Length;

        public Where ExecutedWhere {
            get {
                executedWhere ??= GetFacet<IExecutedFacet>().ExecutedWhere();

                return executedWhere.Value;
            }
        }

        public bool IsContributedMethod => actionSpecImmutable.IsContributedMethod;

        public bool IsFinderMethod {
            get {
                isFinderMethod ??= actionSpecImmutable.IsFinderMethod;

                return isFinderMethod.Value;
            }
        }

        public INakedObjectAdapter Execute(INakedObjectAdapter nakedObjectAdapter, INakedObjectAdapter[] parameterSet) {
            var parms = RealParameters(nakedObjectAdapter, parameterSet);
            var target = RealTarget(nakedObjectAdapter);
            var result = ActionInvocationFacet.Invoke(target, parms, LifecycleManager, MetamodelManager, Session, nakedObjectManager, messageBroker, transactionManager);

            if (result != null && result.Oid == null) {
                result.SetATransientOid(new CollectionMemento(LifecycleManager, nakedObjectManager, MetamodelManager, loggerFactory.CreateLogger<CollectionMemento>(), nakedObjectAdapter, this, parameterSet));
            }

            return result;
        }

        public INakedObjectAdapter RealTarget(INakedObjectAdapter target) =>
            target switch {
                null => FindService(),
                _ when target.Spec is IServiceSpec => target,
                _ when IsContributedMethod => FindService(),
                _ => target
            };

        public override bool ContainsFacet(Type facetType) => actionSpecImmutable.ContainsFacet(facetType);

        public override IFacet GetFacet(Type type) => actionSpecImmutable.GetFacet(type);

        public override IEnumerable<IFacet> GetFacets() => actionSpecImmutable.GetFacets();

        public IActionParameterSpec[] Parameters { get; }

        /// <summary>
        ///     Returns true if the represented action returns something, else returns false
        /// </summary>
        public bool HasReturn {
            get {
                hasReturn ??= ReturnSpec != null;

                return hasReturn.Value;
            }
        }

        /// <summary>
        ///     Checks declarative constraints, and then checks imperatively.
        /// </summary>
        public IConsent IsParameterSetValid(INakedObjectAdapter nakedObjectAdapter, INakedObjectAdapter[] parameterSet) {
            IInteractionContext ic;
            var buf = new InteractionBuffer();
            if (parameterSet != null) {
                var parms = RealParameters(nakedObjectAdapter, parameterSet);
                for (var i = 0; i < parms.Length; i++) {
                    ic = InteractionContext.ModifyingPropParam(Session, Persistor, false, RealTarget(nakedObjectAdapter), Identifier, parameterSet[i]);
                    InteractionUtils.IsValid(GetParameter(i), ic, buf);
                }
            }

            var target = RealTarget(nakedObjectAdapter);
            ic = InteractionContext.InvokingAction(Session, Persistor,false, target, Identifier, parameterSet);
            InteractionUtils.IsValid(this, ic, buf);
            return InteractionUtils.IsValid(buf);
        }

        public override IConsent IsUsable(INakedObjectAdapter target) {
            IInteractionContext ic = InteractionContext.InvokingAction(Session, Persistor,false, RealTarget(target), Identifier, new[] {target});
            return InteractionUtils.IsUsable(this, ic);
        }

        public override bool IsVisible(INakedObjectAdapter target) => base.IsVisible(RealTarget(target));

        public override bool IsVisibleWhenPersistent(INakedObjectAdapter target) => base.IsVisibleWhenPersistent(RealTarget(target));

        public INakedObjectAdapter[] RealParameters(INakedObjectAdapter target, INakedObjectAdapter[] parameterSet) {
            return parameterSet ?? (IsContributedMethod ? new[] {target} : new INakedObjectAdapter[0]);
        }

        public bool IsLocallyContributedTo(ITypeSpec typeSpec, string id) {
            var spec = MetamodelManager.Metamodel.GetSpecification(typeSpec.FullName) as IObjectSpecImmutable;
            return spec != null && actionSpecImmutable.IsContributedToLocalCollectionOf(spec, id);
        }

        #endregion

        private bool FindServiceOnSpecOrSpecSuperclass(ITypeSpec spec) => spec != null && (spec.Equals(OnSpec) || FindServiceOnSpecOrSpecSuperclass(spec.Superclass));

        private INakedObjectAdapter FindService() {
            foreach (var serviceAdapter in servicesManager.GetServices()) {
                if (FindServiceOnSpecOrSpecSuperclass(serviceAdapter.Spec)) {
                    return serviceAdapter;
                }
            }

            throw new FindObjectException(logger.LogAndReturn($"failed to find service for action {Name}"));
        }

        private IActionParameterSpec GetParameter(int position) {
            if (position >= Parameters.Length) {
                throw new ArgumentException(logger.LogAndReturn($"GetParameter(int): only {Parameters.Length} parameters, position={position}"));
            }

            return Parameters[position];
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("Action [");
            sb.Append(base.ToString());
            sb.Append(",returns=");
            sb.Append(ReturnSpec);
            sb.Append(",parameters={");
            for (var i = 0; i < ParameterCount; i++) {
                if (i > 0) {
                    sb.Append(",");
                }

                sb.Append(Parameters[i].Spec.ShortName);
            }

            sb.Append("}]");
            return sb.ToString();
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}