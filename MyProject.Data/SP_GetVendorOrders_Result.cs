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
    
    public partial class SP_GetVendorOrders_Result
    {
        public long ID { get; set; }
        public string Date { get; set; }
        public string BookingNo { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> TaxPercent { get; set; }
        public Nullable<bool> PaymentStatus { get; set; }
        public string Status1 { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
