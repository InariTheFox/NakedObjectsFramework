// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Linq;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Core.Resolve;
using NakedObjects.Core.Util;

namespace NakedObjects.Core.Spec {
    public sealed class OneToManyAssociationSpec : AssociationSpecAbstract, IOneToManyAssociationSpec {
        public OneToManyAssociationSpec(IMetamodelManager metamodel, IOneToManyAssociationSpecImmutable association, ISession session, ILifecycleManager lifecycleManager, INakedObjectManager manager, IObjectPersistor persistor)
            : base(metamodel, association, session, lifecycleManager, manager, persistor) {
            IsASet = association.ContainsFacet<IIsASetFacet>();

            ElementSpec = MetamodelManager.GetSpecification(association.ElementSpec);
        }

        public override bool IsAutoCompleteEnabled => false;

        #region IOneToManyAssociationSpec Members

        public override INakedObjectAdapter GetNakedObject(INakedObjectAdapter inObjectAdapter) => GetCollection(inObjectAdapter);

        public override IObjectSpec ElementSpec { get; }

        public bool IsASet { get; }

        public override bool IsEmpty(INakedObjectAdapter inObjectAdapter) => Count(inObjectAdapter) == 0;

        public int Count(INakedObjectAdapter inObjectAdapter) => Persistor.CountField(inObjectAdapter, Id);

        public override bool IsInline => false;

        public override bool IsMandatory => false;

        public override INakedObjectAdapter GetDefault(INakedObjectAdapter nakedObjectAdapter) => null;

        public override TypeOfDefaultValue GetDefaultType(INakedObjectAdapter nakedObjectAdapter) => TypeOfDefaultValue.Implicit;

        public override void ToDefault(INakedObjectAdapter target) { }

        #endregion

        public override INakedObjectAdapter[] GetCompletions(INakedObjectAdapter nakedObjectAdapter, string autoCompleteParm) => new INakedObjectAdapter[0];

        private INakedObjectAdapter GetCollection(INakedObjectAdapter inObjectAdapter) {
            var collection = GetFacet<IPropertyAccessorFacet>().GetProperty(inObjectAdapter);
            if (collection == null) {
                return null;
            }

            var adapterFor = Manager.CreateAggregatedAdapter(inObjectAdapter, ((IAssociationSpec) this).Id, collection);
            SetResolveStateForDerivedCollections(adapterFor);
            return adapterFor;
        }

        private void SetResolveStateForDerivedCollections(INakedObjectAdapter adapterFor) {
            var isDerived = !IsPersisted;
            if (isDerived && !adapterFor.ResolveState.IsResolved()) {
                if (adapterFor.GetAsEnumerable(Manager).Any()) {
                    adapterFor.ResolveState.Handle(Events.StartResolvingEvent);
                    adapterFor.ResolveState.Handle(Events.EndResolvingEvent);
                }
            }
        }

        public override string ToString() {
            var str = new AsString(this);
            str.Append(base.ToString());
            str.Append(",");
            str.Append("persisted", IsPersisted);
            str.Append("type", ReturnSpec == null ? "unknown" : ReturnSpec.ShortName);
            return str.ToString();
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}