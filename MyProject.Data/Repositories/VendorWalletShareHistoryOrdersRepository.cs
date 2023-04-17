using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	public interface IVendorWalletShareHistoryOrdersRepository : IRepository<VendorWalletShareHistoryOrder>
	{

	}
	public class VendorWalletShareHistoryOrdersRepository : RepositoryBase<VendorWalletShareHistoryOrder>, IVendorWalletShareHistoryOrdersRepository
	{
		public VendorWalletShareHistoryOrdersRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
	}
}
