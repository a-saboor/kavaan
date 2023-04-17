using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class BrandsRepository : RepositoryBase<Brand>, IBrandsRepository
	{
		public BrandsRepository(IDbFactory dbFactory) : base(dbFactory)
		{ }
		public Brand GetBrandByName(string name, long id = 0)
		{
			var user = this.DbContext.Brands.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

		public Brand GetBrandBySlug(string slug)
		{
			var user = this.DbContext.Brands.Where(c => c.Slug == slug && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

		//public IEnumerable<SP_GetBrandFilters_Result> GetBrandFilters(long brandId, string lang)
		//{
		//	var filters = this.DbContext.SP_GetBrandFilters(brandId, lang).ToList();
		//	return filters;
		//}

		//public IEnumerable<SP_GetBrandCategories_Result> GetBrandCategories(long brandId, string lang)
		//{
		//	var brandCategories = this.DbContext.SP_GetBrandCategories(brandId, lang).ToList();
		//	return brandCategories;
		//}
	}
	public interface IBrandsRepository : IRepository<Brand>
	{
		Brand GetBrandByName(string name, long id = 0);
		Brand GetBrandBySlug(string slug);

		//IEnumerable<SP_GetBrandFilters_Result> GetBrandFilters(long brandId, string lang);
		//IEnumerable<SP_GetBrandCategories_Result> GetBrandCategories(long brandId, string lang);
	}
}
