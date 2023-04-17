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
    
    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            this.CouponCategories = new HashSet<CouponCategory>();
            this.CouponRedemptions = new HashSet<CouponRedemption>();
            this.CustomerCoupons = new HashSet<CustomerCoupon>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string CouponCode { get; set; }
        public Nullable<long> Frequency { get; set; }
        public Nullable<int> MaxRedumption { get; set; }
        public Nullable<decimal> DicountAmount { get; set; }
        public Nullable<decimal> DicountPercentage { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<System.DateTime> Expiry { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> MaxAmount { get; set; }
        public string CoverImage { get; set; }
        public Nullable<bool> IsOpenToAll { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CouponCategory> CouponCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CouponRedemption> CouponRedemptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerCoupon> CustomerCoupons { get; set; }
    }
}