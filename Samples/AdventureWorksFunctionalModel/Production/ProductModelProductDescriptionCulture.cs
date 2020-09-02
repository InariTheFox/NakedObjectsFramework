// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedFunctions;

namespace AdventureWorksModel {
            public record ProductModelProductDescriptionCulture : IHasModifiedDate {

        public ProductModelProductDescriptionCulture(
            int productModelID,
            int productDescriptionID,
            string cultureID,
            Culture culture,
            ProductDescription productDescription,
            ProductModel productModel,
            DateTime modifiedDate
            )
        {
            ProductModelID = productModelID;
            ProductDescriptionID = productDescriptionID;
            CultureID = cultureID;
            Culture = culture;
            ProductDescription = productDescription;
            ProductModel = productModel;
            ModifiedDate = modifiedDate;
        }
        public ProductModelProductDescriptionCulture() { }
        [Hidden]
        public virtual int ProductModelID { get; set; }

        [Hidden]
        public virtual int ProductDescriptionID { get; set; }

        [Hidden]
        public virtual string CultureID { get; set; }

        public virtual Culture Culture { get; set; }
        public virtual ProductDescription ProductDescription { get; set; }

        [Hidden]
        public virtual ProductModel ProductModel { get; set; }

        #region ModifiedDate

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; set; }

        #endregion

    }

    public static class ProductModelProductDescriptionCultureFunctions
    {
        public static string Title(this ProductModelProductDescriptionCulture p)
        {
            return p.CreateTitle($"{p.Culture}");
        }
        public static ProductDocument Updating(ProductDocument c, [Injected] DateTime now)
        {
            return c with {ModifiedDate =  now};
        }
    }
}