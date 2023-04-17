//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Data
{
    using System;
    
    public partial class SP_GetProductDetails_Result
    {
        public long ID { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string MobileDescription { get; set; }
        public string ProductSlug { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> RegularPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<System.DateTime> SalePriceFrom { get; set; }
        public Nullable<System.DateTime> SalePriceTo { get; set; }
        public string SKU { get; set; }
        public int Stock { get; set; }
        public Nullable<bool> IsManageStock { get; set; }
        public Nullable<int> StockStatus { get; set; }
        public Nullable<long> VendorID { get; set; }
        public Nullable<bool> IsSoldIndividually { get; set; }
        public Nullable<bool> IsRecommended { get; set; }
        public string Logo { get; set; }
        public string Slug { get; set; }
        public string VendorName { get; set; }
        public Nullable<long> BrandID { get; set; }
        public string BrandName { get; set; }
        public string Images { get; set; }
        public string Categories { get; set; }
        public string Tags { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public string CreatedOn { get; set; }
    }
}
