using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{

    public class VendorServiceRepository : RepositoryBase<VendorService>, IVendorServiceRepository
	{
		public VendorServiceRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
		
	}
	public interface IVendorServiceRepository : IRepository<VendorService>
	{

	}
}
