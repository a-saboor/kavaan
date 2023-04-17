using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Data.Repositories
{
	class CustomerDocumentTypeRepository : RepositoryBase<CustomerDocumentType>, ICustomerDocumentTypeRepository
	{
		public CustomerDocumentTypeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public CustomerDocumentType GetCustomerDocumentType(long id = 0)
		{
			var customerDocumentType = this.DbContext.CustomerDocumentTypes.Where(c => c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return customerDocumentType;
		}
	}
	public interface ICustomerDocumentTypeRepository : IRepository<CustomerDocumentType>
	{
		CustomerDocumentType GetCustomerDocumentType(long id = 0);
	}
}
