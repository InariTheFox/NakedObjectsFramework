// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;

using NakedFunctions;

namespace AdventureWorksModel {
    public record ProductReview {
        #region Injected Services
        
        #endregion

        #region Life Cycle Methods
        //public virtual void Persisting() {
        //    ModifiedDate = DateTime.Now;
        //}

        //public virtual void Updating() {
        //    ModifiedDate = DateTime.Now;
        //}
        #endregion

        [Hidden]
        public virtual int ProductReviewID { get; init; }

        [MemberOrder(1)]
        public virtual string ReviewerName { get; init; }

        [MemberOrder(2)]
        public virtual DateTime ReviewDate { get; init; }

        [MemberOrder(3)]
        public virtual string EmailAddress { get; init; }

        [MemberOrder(4)]
        public virtual int Rating { get; init; }

        [MemberOrder(5)]
        public virtual string Comments { get; init; }

        [Hidden]
        public int ProductID { get; init; }

        [Hidden]
        public virtual Product Product { get; init; }

        #region ModifiedDate

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; init; }

        #endregion

        public override string ToString() {
            return "*****".Substring(0, Rating);
        }
    }
}