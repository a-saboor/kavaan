using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Customer GetCustomerByEmail(string email, long id = 0)
        {
            var customer = this.DbContext.Customers.Where(c => c.Email == email && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

        public Customer GetCustomerByUsername(string username, long id = 0)
        {
            var customer = this.DbContext.Customers.Where(c => c.UserName == username && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

        public Customer GetCustomerByEmailandContact(string email, string Contact)
        {
            var customer = this.DbContext.Customers.Where(c => c.Email == email || c.Contact == Contact && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

        public List<Customer> GetCustomersDateWise(DateTime FromDate, DateTime ToDate)
        {
            var Customers = this.DbContext.Customers.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Customers;
        }


        public Customer GetByAuthCode(string authCode)
        {
            var customer = this.DbContext.Customers.Where(c => c.AuthorizationCode == authCode && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

        public Customer GetByContact(string contact, long id = 0)
        {
            var customer = this.DbContext.Customers.Where(c => c.Contact == contact && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

        public Customer GetByContactAndPhoneCode(string contact,string phoneCode, long id = 0)
        {
            var customer = this.DbContext.Customers.Where(c => c.Contact == contact && c.PhoneCode==phoneCode && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return customer;
        }

    }

    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetCustomerByEmailandContact(string email, string Contact);
        List<Customer> GetCustomersDateWise(DateTime FromDate, DateTime ToDate);

        Customer GetCustomerByEmail(string email, long id = 0);
        Customer GetCustomerByUsername(string username, long id = 0);
        Customer GetByAuthCode(string authCode);
        Customer GetByContact(string contact, long id = 0);
        Customer GetByContactAndPhoneCode(string contact, string phoneCode, long id = 0);
    }
}
