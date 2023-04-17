using MyProject.Data.Infrastructure;
using System;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class VendorWalletShareRepository : RepositoryBase<VendorWalletShare>, IVendorWalletShareRepository
	{
		public VendorWalletShareRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public VendorWalletShare GetByVendor(long vendorId)
		{
			var VendorWalletShare = this.DbContext.VendorWalletShares.Where(c => c.VendorID == vendorId).FirstOrDefault();
			return VendorWalletShare;
		}

		public bool UpdateVendorEarning(long orderID, DateTime createdOn)
		{
			try
			{
				this.DbContext.PR_UpdateVendorEarning(orderID, createdOn);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}

	public interface IVendorWalletShareRepository : IRepository<VendorWalletShare>
	{
		VendorWalletShare GetByVendor(long vendorId);
		bool UpdateVendorEarning(long orderID, DateTime createdOn);
	}
}
