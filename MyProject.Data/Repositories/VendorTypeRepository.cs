using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class VendorTypeRepository : RepositoryBase<VendorType>, IVendorTypeRepository
	{
		public VendorTypeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public VendorType GetTagByName(string name, long id = 0)
		{
			var user = this.DbContext.VendorTypes.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IVendorTypeRepository : IRepository<VendorType>
	{
		VendorType GetTagByName(string name, long id = 0);
	}
}
