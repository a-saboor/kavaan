using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
	{
		public UserRoleRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public UserRole GetUserRoleByName(string name, long id = 0)
		{
			var UserRole = this.DbContext.UserRoles.Where(c => c.RoleName == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return UserRole;
		}

        public List<SP_GetRoutesWithUserRolePrivileges_Result> GetRoutesWithUserRolePrivileges(string type, long userRoleId)
        {
            var Privileges = this.DbContext.SP_GetRoutesWithUserRolePrivileges(type, userRoleId).ToList();
            return Privileges;
        }
    }

	public interface IUserRoleRepository : IRepository<UserRole>
	{
		UserRole GetUserRoleByName(string name, long id = 0);
        List<SP_GetRoutesWithUserRolePrivileges_Result> GetRoutesWithUserRolePrivileges(string type, long userRoleId);

    }
}
