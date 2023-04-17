using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace MyProject.Web.ViewModels.Api.Bookings
{
    public class QuotationViewModel
    {
        public long? ID { get; set; }
        [Required(ErrorMessage = "Service Booking id is required")]
        public long ServiceBookingID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total{ get; set; }
        public decimal Discount{ get; set; }
        public decimal Charges{ get; set; }
        public decimal Tax{ get; set; }

        [EnsureOneElement(ErrorMessage = "At least one item is required")]
        public List<QuotationDetailViewModel> qoutationDetails { get; set; }
    }
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}