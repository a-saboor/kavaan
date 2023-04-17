using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Bookings
{
    public class QuotationDetailViewModel
    {
        public long? ID { get; set; }
        public long QuotationID { get; set; }
        public long VendorID { get; set; }
        public long ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Charges { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
    }
}