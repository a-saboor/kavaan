using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.OzoneGallery
{
    public class GalleryList
    {
        public List<OzoneImage> GalleryBanners { get; set; }
    }
    public class GalleryImagePositionViewModel
    {

        public long ID { get; set; }
        public int Position { get; set; }

    }
}