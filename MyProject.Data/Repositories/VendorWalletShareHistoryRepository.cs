using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{


	class VendorWalletShareHistoryRepository : RepositoryBase<VendorWalletShareHistory>, IVendorWalletShareHistoryRepository
	{
		public VendorWalletShareHistoryRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<VendorWalletShareHistory> GetAllByVendor(long vendorId)
		{
			var VendorWalletShareHistories = this.DbContext.VendorWalletShareHistories.Where(c => c.VendorID == vendorId).ToList();
			return VendorWalletShareHistories;
		}
		public List<VendorWalletShareHistory> GetFilteredWalletHistory(DateTime? FromDate, DateTime? ToDate, long? vendorId)
		{
			var History = this.DbContext.VendorWalletShareHistories.Where(c => (vendorId == null || c.VendorID == vendorId) && (FromDate == null || c.CreatedOn >= FromDate) && (ToDate == null || c.CreatedOn <= ToDate)).ToList();
			return History;
		}

	}

	public interface IVendorWalletShareHistoryRepository : IRepository<VendorWalletShareHistory>
	{
		List<VendorWalletShareHistory> GetFilteredWalletHistory(DateTime? FromDate, DateTime? ToDate, long? vendorId);


		IEnumerable<VendorWalletShareHistory> GetAllByVendor(long vendorId);
	}
}
