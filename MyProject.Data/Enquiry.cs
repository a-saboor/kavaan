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
    
    public partial class Enquiry
    {
        public long ID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public string PhoneCode { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string EnquiryStatus { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
