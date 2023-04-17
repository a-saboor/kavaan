using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.PropertyProject
{
    public class PropertyProjectViewModel
    {
        public int TotalResults { get; set; }
        public long CurrentProjectTypeID { get; set; }
        public List<ProjectType> ProjectTypes { get; set; }
    }
}