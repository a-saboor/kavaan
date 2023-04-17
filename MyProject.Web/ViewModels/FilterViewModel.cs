using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels
{
    public class FilterViewModel
    {
        public string status { get; set; }
        public string search { get; set; }
		public string Lang { get; set; }
        public Nullable<int> pageSize { get; set; }
        public Nullable<int> pageNumber { get; set; }
        public Nullable<int> sortBy { get; set; }

        public Nullable<long> parentID { get; set; }

    }
}