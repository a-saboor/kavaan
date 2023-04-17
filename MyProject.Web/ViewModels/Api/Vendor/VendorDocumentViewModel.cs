using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Vendor
{
    public class VendorDocumentViewModel
    {
        public long? ID { get; set; }
        public string DocumentName { get; set; }
        public string ExpiryDate { get; set; }
    }
}