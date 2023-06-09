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
    
    public partial class BookingReview
    {
        public long ID { get; set; }
        public Nullable<long> BookingID { get; set; }
        public Nullable<long> ServiceID { get; set; }
        public Nullable<long> CustomeID { get; set; }
        public Nullable<double> Rating { get; set; }
        public string Remarks { get; set; }
        public string Suggestion { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Booking Booking { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }
    }
}
