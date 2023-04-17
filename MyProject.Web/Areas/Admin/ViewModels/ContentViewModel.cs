using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.Admin.ViewModels
{
    public class ContentViewModel
    {
        public ContentManagement VideoFirst = new ContentManagement();
        public ContentManagement Image = new ContentManagement();
    }
}