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
    
    public partial class VendorWalletShareHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VendorWalletShareHistory()
        {
            this.VendorWalletShareHistoryOrders = new HashSet<VendorWalletShareHistoryOrder>();
        }
    
        public long ID { get; set; }
        public Nullable<long> VendorID { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Vendor Vendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorWalletShareHistoryOrder> VendorWalletShareHistoryOrders { get; set; }
    }
}
