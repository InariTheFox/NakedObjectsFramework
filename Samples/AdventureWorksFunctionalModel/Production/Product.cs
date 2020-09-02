// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NakedFunctions;

namespace AdventureWorksModel
{

    public interface IProduct
    {
        string Name { get; }
        int ProductID { get; }
    }

        public record Product : IProduct, IHasModifiedDate, IHasRowGuid
    { //: IRedirected {

        public Product(ProductCategory productCategory, int productId, string name, string productNumber, string color, bool make, bool finishedGoods,
                       short safetyStockLevel, short reorderPoint, decimal standardCost, decimal listPrice, string size, string sizeUnitMeasureCode,
                       UnitMeasure sizeUnit, string weightUnitMeasureCode, decimal? weight, UnitMeasure weightUnit, int daysToManufacture,
                       string productLine, string @class, string style, DateTime sellStartDate, DateTime? sellEndDate, DateTime? discontinuedDate,
                       int? productModelId, ProductModel productModel, int? productSubcategoryId, ProductSubcategory productSubcategory,
                       ICollection<ProductReview> productReviews, ICollection<SpecialOfferProduct> specialOfferProduct, ICollection<ProductInventory> productInventory, ICollection<ProductProductPhoto> productProductPhoto)
        {
            this.productCategory = productCategory;
            ProductID = productId;
            Name = name;
            ProductNumber = productNumber;
            Color = color;
            Make = make;
            FinishedGoods = finishedGoods;
            SafetyStockLevel = safetyStockLevel;
            ReorderPoint = reorderPoint;
            StandardCost = standardCost;
            ListPrice = listPrice;
            Size = size;
            SizeUnitMeasureCode = sizeUnitMeasureCode;
            SizeUnit = sizeUnit;
            WeightUnitMeasureCode = weightUnitMeasureCode;
            Weight = weight;
            WeightUnit = weightUnit;
            DaysToManufacture = daysToManufacture;
            ProductLine = productLine;
            Class = @class;
            Style = style;
            SellStartDate = sellStartDate;
            SellEndDate = sellEndDate;
            DiscontinuedDate = discontinuedDate;
            ProductModelID = productModelId;
            ProductModel = productModel;
            ProductSubcategoryID = productSubcategoryId;
            ProductSubcategory = productSubcategory;
            ProductReviews = productReviews.ToList();
            SpecialOfferProduct = specialOfferProduct.ToList();
            ///SpecialOffers = specialOffers.ToList();
            ProductInventory = productInventory.ToList();
            ProductProductPhoto = productProductPhoto.ToList();
            ModifiedDate = DateTime.Now;
        }

        public Product() { }
        #region Properties

        #region ProductID

        [Hidden]
        public virtual int ProductID { get; set; }

        #endregion

        #region Name

       [MemberOrder(1)]
        public virtual string Name { get; set; }

        #endregion

        #region ProductNumber

        [MemberOrder(2)]
        public virtual string ProductNumber { get; set; }

        #endregion

        #region Color

        
        [MemberOrder(3)]
        public virtual string Color { get; set; }

        #endregion

        #region Photo

        private Image cachedPhoto;

        [MemberOrder(4)]
        public virtual Image Photo
        {
            get
            {
                if (cachedPhoto == null)
                {
                    ProductPhoto p = (from obj in ProductProductPhoto
                                      select obj.ProductPhoto).FirstOrDefault();

                    if (p != null)
                    {
                        cachedPhoto = new Image(p.LargePhoto, p.LargePhotoFileName);
                    }
                }
                return cachedPhoto;
            }
        }

        public void AddOrChangePhoto(Image newImage)
        {
            ProductPhoto p = (from obj in ProductProductPhoto
                              select obj.ProductPhoto).FirstOrDefault();

            p.LargePhoto = newImage.GetResourceAsByteArray();
            p.LargePhotoFileName = newImage.Name;
        }

        #endregion

        #region Make

        [MemberOrder(20)]
        public virtual bool Make { get; set; }

        #endregion

        #region FinishedGoods

        [MemberOrder(21)]
        public virtual bool FinishedGoods { get; set; }

        #endregion

        #region SafetyStockLevel

        [MemberOrder(22)]
        public virtual short SafetyStockLevel { get; set; }

        #endregion

        #region ReorderPoint

        [MemberOrder(23)]
        public virtual short ReorderPoint { get; set; }

        #endregion

        #region StandardCost

        [MemberOrder(90)]
        [Mask("C")]
        public virtual decimal StandardCost { get; set; }

        #endregion

        #region ListPrice

        [MemberOrder(11)]
        [Mask("C")]
        public virtual decimal ListPrice { get; set; }

        #endregion

        #region Size & Weight

        [Hidden]
        public virtual string Size { get; set; }

        [Hidden]
        public virtual string SizeUnitMeasureCode { get; set; }

        [Hidden]
        public virtual UnitMeasure SizeUnit { get; set; }

        [Named("Size")]
        [MemberOrder(16)]
        public virtual string SizeWithUnit
        {
            get
            {
                return $"{Size} {UnitMeasureFunctions.Title(SizeUnit)}";
            }
        }

        [Hidden]
        public virtual string WeightUnitMeasureCode { get; set; }

        [Hidden]
        public virtual decimal? Weight { get; set; }

        [Hidden]
        public virtual UnitMeasure WeightUnit { get; set; }

        [MemberOrder(17)]
        [Named("Weight")]
        public virtual string WeightWithUnit
        {
            get
            {
                return $"{Weight} {UnitMeasureFunctions.Title(WeightUnit)}";
            }
        }

        #endregion

        #region DaysToManufacture

        [MemberOrder(24), Range(1, 90)]
        public virtual int DaysToManufacture { get; set; }

        #endregion

        #region ProductLine

        
        [MemberOrder(14)]
        public virtual string ProductLine { get; set; }

        #endregion

        #region Class

        
        [MemberOrder(19)]
        public virtual string Class { get; set; }
        #endregion

        #region Style

        
        [MemberOrder(18)]
        public virtual string Style { get; set; }
        #endregion

        #region SellStartDate

        [MemberOrder(81)]
        [Mask("d")]
        public virtual DateTime SellStartDate { get; set; }

        #endregion

        #region SellEndDate

        [MemberOrder(82)]
        
        [Mask("d")]
        public virtual DateTime? SellEndDate { get; set; }

        #endregion

        #region Discontinued

        
        [MemberOrder(83)]
        [Mask("d")]
        [Range(0, 10)]
        public virtual DateTime? DiscontinuedDate { get; set; }

        #endregion

        #region ProductModel
        [Hidden]
        public virtual int? ProductModelID { get; set; }

        
        [MemberOrder(10)]
        public virtual ProductModel ProductModel { get; set; }
        #endregion

        #region ProductSubcategory
        private ProductCategory productCategory;

        
        
        [MemberOrder(12)]
        public virtual ProductCategory ProductCategory  //TODO: How to handle derived properties?
        {
            get
            {
                if (productCategory == null)
                {
                    return ProductSubcategory == null ? null : ProductSubcategory.ProductCategory;
                }
                return productCategory;
            }
            set { productCategory = value; }
        }

        #region ProductSubcategory
        [Hidden]
        public virtual int? ProductSubcategoryID { get; set; }

        
        [MemberOrder(12)]
        public virtual ProductSubcategory ProductSubcategory { get; set; }


        #endregion

        #endregion

        #region ModifiedDate and rowguid

        #region ModifiedDate

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; set; }
        #endregion

        #region rowguid

        [Hidden]
        public virtual Guid rowguid { get; set; }

        #endregion

        #endregion

        #endregion

        #region Collections

        #region Photos

        private ICollection<ProductProductPhoto> _ProductProductPhoto = new List<ProductProductPhoto>();

        [Hidden]
        public virtual ICollection<ProductProductPhoto> ProductProductPhoto
        {
            get { return _ProductProductPhoto; }
            set { _ProductProductPhoto = value; }
        }

        #endregion

        #region Reviews

        private ICollection<ProductReview> _ProductReviews = new List<ProductReview>();

        [TableView(true, nameof(ProductReview.Rating), nameof(ProductReview.Comments))]
        public virtual ICollection<ProductReview> ProductReviews
        {
            get { return _ProductReviews; } //deliberately returned as array to test Bug #13269
            set { _ProductReviews = value; }
        }

        #endregion

        #region Inventory

        private ICollection<ProductInventory> _ProductInventory = new List<ProductInventory>();

        [RenderEagerly]
        [TableView(false, nameof(AdventureWorksModel.ProductInventory.Quantity),
                nameof(AdventureWorksModel.ProductInventory.Location),
                    nameof(AdventureWorksModel.ProductInventory.Shelf),
                        nameof(AdventureWorksModel.ProductInventory.Bin))]
        public virtual ICollection<ProductInventory> ProductInventory
        {
            get { return _ProductInventory; }
            set { _ProductInventory = value; }
        }

        [Hidden]
        public virtual int NumberInStock()
        {
            return (from obj in ProductInventory
                    select obj).Sum(obj => obj.Quantity);
        }

        #endregion

        #region Special Offers

        [Hidden]
        public virtual ICollection<SpecialOfferProduct> SpecialOfferProduct { get; set; }


        // needs to be initialised for moment
        [NotMapped]
        [RenderEagerly]
        [TableView(true, "MinQty", "DiscountPct", "StartDate", "EndDate")]
        public virtual IList<SpecialOffer> SpecialOffers { get; private set; } = new List<SpecialOffer>();


        #endregion

        #endregion
    }

    public static class ProductFunctions2
    {//TODO: Temp name while Stef is using Product Functions for initial spiking
        public static string Title(this Product p)
        {
            return p.CreateTitle(p.Name);
        }

        #region Life Cycle Methods
        public static Product Updating(Product p, [Injected] DateTime now)
        {
            return p with {ModifiedDate =  now};
        }
        #endregion

        public static string[] ChoicesProductLine(Product p)
        {
            // nchar(2) in database so pad right with space
            return new[] { "R ", "M ", "T ", "S " };
        }

        public static string[] ChoicesClass(Product p)
        {
            // nchar(2) in database so pad right with space
            return new[] { "H ", "M ", "L " };
        }

        public static string[] ChoicesStyle(Product p)
        {
            // nchar(2) in database so pad right with space
            return new[] { "U ", "M ", "W " };
        }

        [Hidden]
        public static bool IsDiscontinued(this Product p, DateTime now)
        {
            return p.DiscontinuedDate != null ? p.DiscontinuedDate.Value < now : false;
        }

        public static IQueryable<ProductModel> AutoCompleteProductModel(Product p, string match, IQueryable<ProductModel> models)
        {
            return models.Where(pm => pm.Name.ToUpper().Contains(match.ToUpper()));
        }


        #region BestSpecialOffer

        
        [DescribedAs("Determines the best discount offered by current special offers for a specified order quantity")]
        public static (SpecialOffer, SpecialOfferProduct) BestSpecialOffer(
            
            Product p, 
            short quantity, 
            IQueryable<SpecialOfferProduct> sops,
            IQueryable<SpecialOffer> offers
            )
        {
            var best =  BestSpecialOfferProduct(p, quantity, sops);
            if (best != null)
            {
                return DisplayAndPersistDifferentItems(best.SpecialOffer, (SpecialOfferProduct) null);
            }
            var none = SpecialOfferRepository.AssociateSpecialOfferWithProduct(SpecialOfferRepository.NoDiscount(offers), p, sops).Item2;
            return DisplayAndPersistDifferentItems(none.SpecialOffer, none);
        }

        public static string ValidateBestSpecialOffer(Product p, short quantity)
        {
            return quantity <= 0 ? "Quantity must be > 0" : null;
        }

        public static string DisableBestSpecialOffer(Product p, [Injected] DateTime now)
        {
            if (p.IsDiscontinued(now))
            {
                return "Product is discontinued";
            }
            return null;
        }

        public static IList<ProductSubcategory> ChoicesProductSubcategory(
            Product p,
            ProductCategory productCategory, 
            IQueryable<ProductSubcategory> subCats)
        {
            if (productCategory != null)
            {
                return (from psc in subCats
                        where psc.ProductCategory.ProductCategoryID == productCategory.ProductCategoryID
                        select psc).ToList();
            }
            return Display(new ProductSubcategory[] { }.ToList());
        }
        [Hidden]
        public static SpecialOfferProduct BestSpecialOfferProduct(
            Product p, 
            short quantity, 
            IQueryable<SpecialOfferProduct> sops)
        {
            //reason for testing end date against 1/6/2004 is that in AW database, all offers terminate by 30/6/04
            return sops.Where(obj => obj.Product.ProductID == p.ProductID &&
                              obj.SpecialOffer.StartDate <= DateTime.Now &&
                              obj.SpecialOffer.EndDate >= new DateTime(2004, 6, 1) &&
                              obj.SpecialOffer.MinQty < quantity).
                        OrderByDescending(obj => obj.SpecialOffer.DiscountPct)
                        .FirstOrDefault();

        }

        #endregion


    }
}