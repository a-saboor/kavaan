using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class StaffRepository : RepositoryBase<Staff>, IStaffRepository
    {
        public StaffRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Staff GetStaffByEmail(string email, long id = 0)
        {
            var staff = this.DbContext.Staffs.Where(c => c.Email == email && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return staff;
        }
        public Staff GetByContact(string Code, string contact, long id = 0)
        {
            var staff = this.DbContext.Staffs.Where(c => c.PhoneCode == Code && c.Contact == contact && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return staff;
        }
        public Staff GetByAuthCode(string authCode)
        {
            var staff = this.DbContext.Staffs.Where(c => c.AuthorizationCode == authCode && c.IsDeleted == false).FirstOrDefault();
            return staff;
        }

        public Staff GetStaffByID(long id = 0)
        {
            var staff = this.DbContext.Staffs.Where(c => c.ID == id && c.IsDeleted == false).FirstOrDefault();
            return staff;
        }
        public List<Staff> GetStaffByDate(DateTime FromDate, DateTime ToDate, long VendorID = 0)
        {
            var staff = this.DbContext.Staffs.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsDeleted == false && c.VendorID == VendorID).ToList();
            return staff;
        }
    }
    public interface IStaffRepository : IRepository<Staff>
    {
        Staff GetStaffByEmail(string email, long id = 0);
        Staff GetStaffByID(long id = 0);
        Staff GetByContact(string Code, string contact, long id = 0);
        Staff GetByAuthCode(string authCode);
        List<Staff> GetStaffByDate(DateTime FromDate, DateTime ToDate,long VendorID = 0);
    }
}
