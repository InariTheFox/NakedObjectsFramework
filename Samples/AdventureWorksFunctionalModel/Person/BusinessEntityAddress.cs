using NakedFunctions;
using System;



namespace AdventureWorksModel
{
    [Named("Address")]
    public record BusinessEntityAddress: IHasRowGuid, IHasModifiedDate
    {
        public BusinessEntityAddress(
            //int businessEntityID,
            BusinessEntity businessEntity,
            //int addressTypeID,           
            AddressType addressType,
            int addressID,
            Address address,
            Guid guid, 
            DateTime now)
        {
            AddressID = addressID;
            Address = address;
            //AddressTypeID = addressTypeID;
            AddressType = addressType;
            //BusinessEntityID = businessEntityID;
            BusinessEntity = businessEntity;
            rowguid = guid;
            ModifiedDate = now;
        }

        public BusinessEntityAddress() { }

        
        public virtual int BusinessEntityID { get; set; }
        [MemberOrder(3)]
        public virtual BusinessEntity BusinessEntity { get; set; }

        
        public virtual int AddressTypeID { get; set; }

        [MemberOrder(1)]
        
        public virtual AddressType AddressType { get; set; }

        
        public virtual int AddressID { get; set; }

        [MemberOrder(2)]
        
        public virtual Address Address { get; set; }

        #region Row Guid and Modified Date

        #region rowguid

        [Hidden]
        public virtual Guid rowguid { get; set; }

        #endregion

        #region ModifiedDate

        [MemberOrder(99)]
        
        [ConcurrencyCheck]
        public virtual DateTime ModifiedDate { get; set; }

        #endregion

        #endregion
    }

    public static class BusinessEntityAddressFunctions {
        
    public static BusinessEntityAddress Updating(BusinessEntityAddress a, [Injected] DateTime now)
    {
            return a with {ModifiedDate =  now};
    }

    public static string Title(this BusinessEntityAddress a)
    {
            return a.CreateTitle($"{AddressTypeFunctions.Title(a.AddressType)}: { AddressFunctions.Title(a.Address)}");
    }

}
}
