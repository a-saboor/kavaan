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
    using System.Collections.Generic;
    
    public partial class CustomerLoyaltySetting
    {
        public long ID { get; set; }
        public string CustomerType { get; set; }
        public Nullable<decimal> PGRatio { get; set; }
        public Nullable<decimal> PRRatio { get; set; }
        public Nullable<int> PointsLimit { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ReferralPoint { get; set; }
        public Nullable<decimal> CustomerTypeMaxSlab { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
