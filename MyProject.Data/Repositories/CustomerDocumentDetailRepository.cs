using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class CustomerDocumentDetailRepository : RepositoryBase<CustomerDocumentDetail>, ICustomerDocumentDetailRepository
	{
		public CustomerDocumentDetailRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public bool GetExistCustomerDocument(long customerID, long customerDocumentID)
		{
			var customerDocumentDetail = this.DbContext.CustomerDocumentDetails.Where(mod => mod.CustomerID == customerID && mod.CustomerDocumentID == customerDocumentID).FirstOrDefault();
			return customerDocumentDetail == null ;
		}

	}
	public interface ICustomerDocumentDetailRepository : IRepository<CustomerDocumentDetail>
	{
		bool GetExistCustomerDocument(long customerID, long customerDocumentID);
	}
}
