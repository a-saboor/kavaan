using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class UserRolePrivilegeRepository : RepositoryBase<UserRolePrivilege>, IUserRolePrivilegeRepository
	{
		public UserRolePrivilegeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public void DeleteMany(long userRoleId)
		{
			var UserRolePrivileges = this.DbContext.UserRolePrivileges.Where(i => i.UserRoleID == userRoleId).ToList();
			this.DbContext.UserRolePrivileges.RemoveRange(UserRolePrivileges);
		}
	}

	public interface IUserRolePrivilegeRepository : IRepository<UserRolePrivilege>
	{
		void DeleteMany(long userRoleId);
	}
}
