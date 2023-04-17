using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.CustomerPortal.ViewModels.Documents
{
    public class DocumentDetailsViewModel
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public bool IsGcc { get; set; }
        public string Path { get; set; }
    }
}