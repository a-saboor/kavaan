using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public User GetUserByEmail(string email, long id = 0)
		{
			var user = this.DbContext.Users.Where(c => c.EmailAddress == email && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
		public User GetByContact(string Code, string contact, long id = 0)
		{
			var user = this.DbContext.Users.Where(c => c.PhoneCode == Code && c.MobileNo == contact && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
		public User GetByAuthCode(string authCode)
		{
			var user = this.DbContext.Users.Where(c => c.AuthorizationCode == authCode && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IUserRepository : IRepository<User>
	{
		User GetUserByEmail(string email, long id = 0);
		User GetByContact(string Code, string contact, long id = 0);
		User GetByAuthCode(string authCode);
	}
}
