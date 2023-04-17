using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Prescription
{
    public class PrescriptionFilterViewModel
    {
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int SortBy { get; set; }
		public string Lang { get; set; }
    }
}