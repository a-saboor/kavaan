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
    
    public partial class Appointment
    {
        public long ID { get; set; }
        public string AppointmentNo { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public string Type { get; set; }
        public string TypeAr { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCompleted { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
