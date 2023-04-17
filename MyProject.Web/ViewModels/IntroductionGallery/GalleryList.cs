using System;
using MyProject.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.IntroductionGallery
{
    public class GalleryList
    {
        public List<IntroductionImage> GalleryBanners { get; set; }
    }
    public class GalleryImagePositionViewModel
    {

        public long ID { get; set; }
        public int Position { get; set; }

    }
}