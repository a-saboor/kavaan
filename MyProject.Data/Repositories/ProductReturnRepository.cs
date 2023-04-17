using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ProductReturnRepository : RepositoryBase<ProductReturn>, IProductReturnRepository
	{
		public ProductReturnRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProductReturn GetProductReturn(long orderDetailId, long customerId, long id = 0)
		{
			var user = this.DbContext.ProductReturns.Where(c => c.OrderDetailID == orderDetailId && c.ID != id && c.CustomerID == customerId).FirstOrDefault();
			return user;
		}

		public ProductReturn GetOrderDetailReturn(long orderDetailId)
		{
			var user = this.DbContext.ProductReturns.Where(c => c.OrderDetailID == orderDetailId).FirstOrDefault();
			return user;
		}

		//public IEnumerable<SP_GetCustomerProductReturns_Result> GetCustomerReturns(long customerId, string status, int pageNumber = 1, int sortBy = 1, string lang = "en")
		//{
		//	var Orders = this.DbContext.SP_GetCustomerProductReturns(customerId, status, pageNumber, sortBy, lang).ToList();
		//	return Orders;
		//}

		public List<ProductReturn> GetFilteredProductReturnOrders(DateTime FromDate, DateTime ToDate)
		{
			var Orders = this.DbContext.ProductReturns.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
			return Orders;
		}

	}

	public interface IProductReturnRepository : IRepository<ProductReturn>
	{
		ProductReturn GetProductReturn(long orderDetailId, long customerId, long id = 0);
		ProductReturn GetOrderDetailReturn(long orderDetailId);
		//IEnumerable<SP_GetCustomerProductReturns_Result> GetCustomerReturns(long customerId, string status, int pageNumber = 1, int sortBy = 1, string lang = "en");
		List<ProductReturn> GetFilteredProductReturnOrders(DateTime FromDate, DateTime ToDate);
	}
}
