// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;

using NakedFunctions;
using NakedFunctions;

namespace AdventureWorksModel {
        [Bounded]
        public record Department:IHasModifiedDate {


        #region Properties

        [Hidden]
        public virtual short DepartmentID { get; set; }

        [MemberOrder(1)]
        public virtual string Name { get; set; }

        [MemberOrder(2)]
        public virtual string GroupName { get; set; }

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; set; }

        #endregion
    }
    public static class DepartmentFunctions
    {
        public static string Title(Department d)
        {
            return d.Name;
        }

        #region Life Cycle Methods
        public static Department Updating(Department p, [Injected] DateTime now)
        {
            return LifeCycleFunctions.UpdateModified(p, now);
        }
        #endregion
    }
}