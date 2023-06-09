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
    
    public partial class ServiceRating
    {
        public long ID { get; set; }
        public long CustomerID { get; set; }
        public long ServiceID { get; set; }
        public Nullable<long> ServiceCategoryID { get; set; }
        public Nullable<long> ServiceBookingID { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ServiceBooking ServiceBooking { get; set; }
        public virtual Service Service { get; set; }
    }
}
