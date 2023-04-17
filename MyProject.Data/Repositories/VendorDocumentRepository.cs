using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;
using System.Data;

namespace MyProject.Data.Repositories
{
    class VendorDocumentRepository : RepositoryBase<VendorDocument>, IVendorDocumentRepository
	{
		public VendorDocumentRepository(IDbFactory dbFactory)
		: base(dbFactory) { }


		public IEnumerable<VendorDocument> GetAllByVendorID(long vendorId)
		{
			var documents = this.DbContext.VendorDocuments.Where(c => c.VendorID == vendorId).ToList();
			return documents;
		}

		public VendorDocument GetDocumentByName(string name, long id = 0)
		{
			var documents = this.DbContext.VendorDocuments.Where(c => c.Name == name && c.ID != id).FirstOrDefault();
			return documents;
		}
	}


	public interface IVendorDocumentRepository : IRepository<VendorDocument>
	{

		IEnumerable<VendorDocument> GetAllByVendorID(long vendorId);

		VendorDocument GetDocumentByName(string name, long id = 0);

	}
}
