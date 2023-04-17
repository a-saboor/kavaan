using System;
using MyProject.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.Admin.ViewModels.Banners
{
	public class BannersViewModel
	{
		public List<Banner> VideoBanners { get; set; }
		public List<Banner> ImageBanners { get; set; }
		public List<Banner> PromotionBanners { get; set; }
	}
}