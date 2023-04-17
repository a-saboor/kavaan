using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Repositories
{
	class CustomerDocumentTypesRepository : RepositoryBase<CustomerDocumentType>, ICustomerDocumentTypeRepository
	{
		public CustomerDocumentTypesRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public CustomerDocumentType GetCustomerDocumentTypeByType(long id)
		{
			var CustomerDocumentType = this.DbContext.CustomerDocumentTypes.Where(c => c.ID == id ).FirstOrDefault();
			return CustomerDocumentType;
		}
	}
	public interface ICustomerDocumentTypeRepository : IRepository<CustomerDocumentType>
	{
		CustomerDocumentType GetCustomerDocumentTypeByType(long id);
	}
}
