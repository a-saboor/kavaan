using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class StaffRolesRepository : RepositoryBase<StaffRole>, IStaffRolesRepository
	{

		public StaffRolesRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<StaffRole> GetAllByDepartmentID(long departmentID)
		{
			var Roles = this.DbContext.StaffRoles.Where(c => c.DepartmentID == departmentID && c.IsDeleted == false).ToList();
			return Roles;
		}

	}
	public interface IStaffRolesRepository : IRepository<StaffRole>
	{
		IEnumerable<StaffRole> GetAllByDepartmentID(long countryId);
	}
	
}
