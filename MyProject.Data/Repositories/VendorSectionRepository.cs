using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class VendorSectionRepository : RepositoryBase<VendorSection>, IVendorSectionRepository
	{
		public VendorSectionRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public VendorType GetTagByName(string name, long id = 0)
		{
			var user = this.DbContext.VendorTypes.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IVendorSectionRepository : IRepository<VendorSection>
	{
		VendorType GetTagByName(string name, long id = 0);
	}
}
