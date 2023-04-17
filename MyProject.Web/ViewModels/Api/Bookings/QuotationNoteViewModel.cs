using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Bookings
{
    public class QuotationNoteViewModel
    {
        public long quotationID { get; set; }
        public string type { get; set; }
        public string quotationMessage { get; set; }
    }
}