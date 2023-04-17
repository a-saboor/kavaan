using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class VendorIndustryRepository : RepositoryBase<VendorIndustry>, IVendorIndustryRepository
	{
		public VendorIndustryRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public VendorIndustry GetTagByName(string name, long id = 0)
		{
			var user = this.DbContext.VendorIndustries.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IVendorIndustryRepository : IRepository<VendorIndustry>
	{
		VendorIndustry GetTagByName(string name, long id = 0);
	}
}
