using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.Admin.ViewModels
{
    public class GalleryList
    {
        public List<PropertyImage> GalleryBanners { get; set; }

        public DateTime UploadedOn { get; set; }
    }
    public class GalleryImagePositionViewModel
    {

        public long ID { get; set; }
        public int Position { get; set; }

    }
}