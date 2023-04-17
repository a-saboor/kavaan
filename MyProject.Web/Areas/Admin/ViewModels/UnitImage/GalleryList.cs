using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.Admin.ViewModels.UnitImage
{
    public class GalleryList
    {
        public List<Data.UnitImage> GalleryBanners { get; set; }
    }
    public class GalleryImagePositionViewModel
    {

        public long ID { get; set; }
        public int Position { get; set; }

    }
}