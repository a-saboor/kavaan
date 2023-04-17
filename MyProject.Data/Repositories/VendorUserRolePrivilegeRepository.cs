using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class VendorUserRolePrivilegeRepository : RepositoryBase<VendorUserRolePrivilege>, IVendorUserRolePrivilegeRepository
	{
		public VendorUserRolePrivilegeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public void DeleteMany(long userRoleId)
		{
			var VendorUserRolePrivileges = this.DbContext.VendorUserRolePrivileges.Where(i => i.UserRoleID == userRoleId).ToList();
			this.DbContext.VendorUserRolePrivileges.RemoveRange(VendorUserRolePrivileges);
		}

		public List<SP_GetRoutesWithVendorUserRolePrivileges_Result> GetRoutesWithVendorUserRolePrivileges(string type, long userRoleId)
		{
			var Privileges = this.DbContext.SP_GetRoutesWithVendorUserRolePrivileges(type, userRoleId).ToList();
			return Privileges;
		}
	}

	public interface IVendorUserRolePrivilegeRepository : IRepository<VendorUserRolePrivilege>
	{
		void DeleteMany(long userRoleId);
		List<SP_GetRoutesWithVendorUserRolePrivileges_Result> GetRoutesWithVendorUserRolePrivileges(string type, long userRoleId);
	}
}
