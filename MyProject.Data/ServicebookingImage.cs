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
    
    public partial class ServicebookingImage
    {
        public long ID { get; set; }
        public long CustomerID { get; set; }
        public Nullable<long> ServiceCategoryID { get; set; }
        public Nullable<long> ServiceBookingID { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ServiceBooking ServiceBooking { get; set; }
    }
}
