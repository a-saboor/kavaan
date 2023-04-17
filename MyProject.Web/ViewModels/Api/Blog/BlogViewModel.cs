using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Blog
{
    public class BlogViewModel
    {
  
        public Nullable<int> pgno { get; set; }
        public Nullable<int> pageSize { get; set; }
        public Nullable<int> sortBy { get; set; }
        public string lang { get; set; }
        public Nullable<DateTime> startDate { get; set; }
        public Nullable<DateTime> endDate { get; set; }
        public string search  { get; set; }
    }
}