using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class CustomerRelationRepository : RepositoryBase<CustomerRelation>, ICustomerRelationRepository
	{
		public CustomerRelationRepository(IDbFactory dbFactory)
					: base(dbFactory) { }

		//public CustomerDocument GetCustomerDocumentByType(string relation, long id = 0)
		//{
		//	var customerDocument = this.DbContext.CustomerDocuments.Where(c => c.CustomerDocumentTypeID == id && c.Relation.ToLower() == relation.ToLower() && c.IsDeleted == false).FirstOrDefault();
		//	return customerDocument;
		//}
	}
	public interface ICustomerRelationRepository : IRepository<CustomerRelation>
	{
		//CustomerDocument GetCustomerDocumentByType(string relation, long id = 0);
	}
}
