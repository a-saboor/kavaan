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
    
    public partial class UnitBookingsPaymentHistory
    {
        public long ID { get; set; }
        public Nullable<long> UnitBookingID { get; set; }
        public Nullable<long> UnitPaymentPlanID { get; set; }
        public string Milestones { get; set; }
        public Nullable<double> Percentage { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual UnitBooking UnitBooking { get; set; }
        public virtual UnitPaymentPlan UnitPaymentPlan { get; set; }
    }
}
