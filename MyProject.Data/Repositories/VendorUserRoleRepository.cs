	using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class VendorUserRoleRepository : RepositoryBase<VendorUserRole>, IVendorUserRoleRepository
	{
		public VendorUserRoleRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<VendorUserRole> GetUserRolesByVendor(long vendorId)
		{
			var Roles = this.DbContext.VendorUserRoles.Where(c => c.VendorID == vendorId && c.IsDeleted == false).ToList();
			return Roles;
		}
        

        public VendorUserRole GetRoleByName(long vendorId, string name, long id = 0)
		{
			var Role = this.DbContext.VendorUserRoles.Where(c => c.VendorID == vendorId && c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			//var Role = this.DbContext.VendorUsers.Where(c => c.VendorID == vendorId && c.UserRole == name && c.ID != id).FirstOrDefault();
			return Role;
		}
        public VendorUserRole GetvendorRoleByName(string name)
        {
            var Role = this.DbContext.VendorUserRoles.Where(c => c.Name == name ).FirstOrDefault();
            return Role;
        }
    }

	public interface IVendorUserRoleRepository : IRepository<VendorUserRole>
	{
        VendorUserRole GetvendorRoleByName(string name);
        IEnumerable<VendorUserRole> GetUserRolesByVendor(long vendorId);
		VendorUserRole GetRoleByName(long vendorId, string name, long id = 0);
	}
}
