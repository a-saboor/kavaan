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
    
    public partial class ProductReturn
    {
        public long ID { get; set; }
        public string ReturnCode { get; set; }
        public Nullable<long> OrderDetailID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string Reason { get; set; }
        public string ReturnMethod { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
