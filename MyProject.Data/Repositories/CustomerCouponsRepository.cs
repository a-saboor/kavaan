using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Data.Repositories
{
	public class CustomerCouponsRepository : RepositoryBase<CustomerCoupon>, ICustomerCouponsRepository
	{
		public CustomerCouponsRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<SP_GetCustomerCoupons_Result> GetCustomerCoupons(long customerID, int pageNumber = 1)
		{
			var Notifications = this.DbContext.SP_GetCustomerCoupons(customerID, pageNumber).ToList();
			return Notifications;
		}

	}
	public interface ICustomerCouponsRepository : IRepository<CustomerCoupon>
	{
		IEnumerable<SP_GetCustomerCoupons_Result> GetCustomerCoupons(long customerID, int pageNumber = 1);
	}
}
