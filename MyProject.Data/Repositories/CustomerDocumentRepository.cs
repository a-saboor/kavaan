using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Data.Repositories
{
	class CustomerDocumentRepository : RepositoryBase<CustomerDocument>, ICustomerDocumentRepository
	{
		public CustomerDocumentRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public CustomerDocument GetCustomerDocumentByType( string relation,long id = 0)
		{
			var customerDocument = this.DbContext.CustomerDocuments.Where(c => c.CustomerDocumentTypeID == id && c.Relation.ToLower()==relation.ToLower() && c.IsDeleted == false).FirstOrDefault();
			return customerDocument;
		}
		public CustomerDocument CheckCustomerDocumentIsExist(long RelationID,long customerid, long id = 0)
		{
			var customerDocument = this.DbContext.CustomerDocuments.Where(c =>c.CustomerID == customerid && c.CustomerDocumentTypeID == id && c.CustomerRelationID == RelationID && c.IsDeleted == false).FirstOrDefault();
			return customerDocument;
		}
	}
	public interface ICustomerDocumentRepository : IRepository<CustomerDocument>
	{
		CustomerDocument GetCustomerDocumentByType(string relation, long id = 0);
		CustomerDocument CheckCustomerDocumentIsExist(long RelationID, long customerid, long id = 0);
	}
}
