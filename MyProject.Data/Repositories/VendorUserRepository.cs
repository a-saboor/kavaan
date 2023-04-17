using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{

	public class VendorUserRepository : RepositoryBase<VendorUser>, IVendorUserRepository
	{
		public VendorUserRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<VendorUser> GetVendorUsers(long vendorId)
		{
			var users = this.DbContext.VendorUsers.Where(c => c.VendorID == vendorId && c.IsDeleted == false).ToList();
			return users;
		}

		public VendorUser GetVendorUserByEmail(string email, long id = 0)
		{
			var user = this.DbContext.VendorUsers.Where(c => c.EmailAddress == email && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

		public VendorUser GetVendorUserByVendorID(long vendorid)
		{
			var user = this.DbContext.VendorUsers.Where(c => c.VendorID == vendorid && c.IsDeleted ==  false ).FirstOrDefault();
			return user;
		}
		public VendorUser GetVendorUserByContact(string contact, long id = 0)
		{
			var user = this.DbContext.VendorUsers.Where(c => c.MobileNo == contact && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

        public bool IsVendorExistByContact(string contact, long id = 0)
        {
            var user = this.DbContext.VendorUsers.Where(c => c.MobileNo == contact && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return user == null ? true : false;
        }

        public VendorUser GetVendorUserByRole(long vendorId,long roleId)
        {
            var user = this.DbContext.VendorUsers.Where(c => c.VendorID == vendorId && c.UserRoleID == roleId &&c.IsDeleted == false).FirstOrDefault();
            return user;
        }

        public VendorUser GetByAuthCode(string authCode)
		{
			var user = this.DbContext.VendorUsers.Where(c => c.AuthorizationCode == authCode && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
     
       
    }

	public interface IVendorUserRepository : IRepository<VendorUser>
	{
        IEnumerable<VendorUser> GetVendorUsers(long vendorId);
		VendorUser GetVendorUserByEmail(string email, long id = 0);
        VendorUser GetVendorUserByRole(long vendorId, long roleId);
        VendorUser GetByAuthCode(string authCode);
		VendorUser GetVendorUserByContact(string contact, long id = 0);
		VendorUser GetVendorUserByVendorID(long vendorid);
		bool IsVendorExistByContact(string contact, long id = 0);
	}
}
