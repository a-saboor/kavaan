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
    
    public partial class SP_GetCategorywiseProducts_Result
    {
        public long ID { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsSaleAvailable { get; set; }
        public Nullable<decimal> RegularPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> MinRegularPrice { get; set; }
        public Nullable<decimal> MaxRegularPrice { get; set; }
        public Nullable<decimal> MinSalePrice { get; set; }
        public Nullable<decimal> MaxSalePrice { get; set; }
    }
}
