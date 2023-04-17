using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    
	class BannerRepository : RepositoryBase<Banner>, IBannersRepository
	{
		public BannerRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public List<Banner> GetBannerByDevice(string device)
		{
			var banner = this.DbContext.Banners.Where(c => c.Device == device).ToList();
			return banner;
		}
		public List<Banner> GetBannerByContentType(string contentType)
		{
			var banner = this.DbContext.Banners.Where(c => c.ContentType == contentType).ToList();
			return banner;
		}
		public List<Banner> GetBannerByContentTypeAndDevice(string contentType, string device)
		{
			var banner = this.DbContext.Banners.Where(c => c.ContentType == contentType && c.Device == device).ToList();
			return banner;
		}
		public List<Banner> GetBannerByContentTypeAndDeviceAndType(string contentType, string device, string Type)
		{
			var banner = this.DbContext.Banners.Where(c => c.ContentType == contentType && c.Device == device && c.Type == Type).ToList();
			return banner;
		}

		public List<Banner> GetBannerByTypeAndLang(string type, string lang = "en")
		{
			var banner = this.DbContext.Banners.Where(c => c.Device == type && c.Language == lang).ToList();
			return banner;
		}
	}
	public interface IBannersRepository : IRepository<Banner>
	{
		List<Banner> GetBannerByDevice(string Device);
		List<Banner> GetBannerByContentType(string contentType);
		List<Banner> GetBannerByContentTypeAndDevice(string contentType, string device);
		List<Banner> GetBannerByContentTypeAndDeviceAndType(string contentType, string device, string Type);
		List<Banner> GetBannerByTypeAndLang(string type, string lang = "en");
	}
}
