// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NakedFunctions;

namespace AdventureWorksModel
{
        public record ProductModel: IHasRowGuid, IHasModifiedDate
    {
        [Hidden]
        public virtual int ProductModelID { get; init; }

        [MemberOrder(10)]
        public virtual string Name { get; init; }

        [Hidden]
        public virtual string CatalogDescription { get; init; }

        [MemberOrder(30)]
        public virtual string Instructions { get; init; }

        [TableView(true, "Name", "Number", "Color", "ProductInventory")]
        public virtual ICollection<Product> ProductVariants { get; init; }

        [Hidden]
        public virtual ICollection<ProductModelIllustration> ProductModelIllustration { get; init; }

        [Hidden]
        public virtual ICollection<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCulture { get; init; }

        #region Row Guid and Modified Date

        #region rowguid

        [Hidden]
        public virtual Guid rowguid { get; init; }

        #endregion

        #region ModifiedDate

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; init; }

        #endregion

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
