using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Vendor.VendorService
{
    public class PageFilterViewModel
    {
        public Nullable<int> pgno { get; set; }
        public Nullable<int> pageSize { get; set; }

        public Nullable<DateTime> startDate { get; set; }

        public Nullable<DateTime> endDate { get; set; }
    }

}