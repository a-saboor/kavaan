using MyProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels
{
    public class HomeViewModel
    {
        public Banner HeaderVideo { get; set; }
        public Banner CenteredVideo { get; set; }
        public Banner FooterVideo { get; set; }
        public List<Banner> HeaderBanners { get; set; }
        public Banner CenteredBanners { get; set; }
        public Banner FooterBanners { get; set; }

        public List<ProjectType> ProjectTypes { get; set; }

        public List<Contractor> Contractors { get; set; }

        public List<Amenity> Amenities { get; set; }

        public List<Data.Blog> Blogs { get; set; }
        public List<NewsFeed> NewsFeeds { get; set; }

    }

    public class AboutViewModel
    {
        public Banner AboutVideo { get; set; }
        public Banner AboutImage { get; set; }

        public List<Team> Teams { get; set; }
    }
}