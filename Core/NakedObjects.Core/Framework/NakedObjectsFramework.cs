﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using NakedObjects.Architecture.Component;
using NakedObjects.Core.Component;
using NakedObjects.Core.Spec;

namespace NakedObjects.Service {
    public sealed class NakedObjectsFramework : INakedObjectsFramework {
        public NakedObjectsFramework(IMessageBroker messageBroker,
                                     ISession session,
                                     ILifecycleManager lifecycleManager,
                                     IServicesManager servicesManager,
                                     INakedObjectManager nakedObjectManager,
                                     IObjectPersistor persistor,
                                     IReflector reflector,
                                     IMetamodelManager metamodelManagerManager,
                                     IDomainObjectInjector domainObjectInjector,
                                     NakedObjectFactory nakedObjectFactory,
                                     SpecFactory memberFactory,
                                     ITransactionManager transactionManager,
                                     IFrameworkResolver frameworkResolver) {
            this.MessageBroker = messageBroker;
            this.Session = session;
            this.LifecycleManager = lifecycleManager;
            this.ServicesManager = servicesManager;
            this.NakedObjectManager = nakedObjectManager;
            this.Persistor = persistor;
            this.Reflector = reflector;
            this.MetamodelManager = metamodelManagerManager;
            this.DomainObjectInjector = domainObjectInjector;
            this.TransactionManager = transactionManager;
            this.FrameworkResolver = frameworkResolver;
            domainObjectInjector.Framework = this;
            memberFactory.Initialize(this);
            nakedObjectFactory.Initialize(metamodelManagerManager, session, lifecycleManager, persistor, nakedObjectManager);
        }

        #region INakedObjectsFramework Members

        public IDomainObjectInjector DomainObjectInjector { get; }

        public ITransactionManager TransactionManager { get; }

        public IFrameworkResolver FrameworkResolver { get; }

        public IMessageBroker MessageBroker { get; }

        public ISession Session { get; }

        public ILifecycleManager LifecycleManager { get; }

        public INakedObjectManager NakedObjectManager { get; }

        public IServicesManager ServicesManager { get; }

        public IObjectPersistor Persistor { get; }

        public IReflector Reflector { get; }

        public IMetamodelManager MetamodelManager { get; }

        #endregion
    }
}