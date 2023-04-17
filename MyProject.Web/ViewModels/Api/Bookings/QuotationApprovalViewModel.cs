using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Bookings
{
    public class QuotationApprovalViewModel
    {
        public long QuotationId { get; set; }
        public string Status { get; set; }
    }
}