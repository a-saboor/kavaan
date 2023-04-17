using MyProject.Data.Infrastructure;
using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class CouponRedemptionRepository : RepositoryBase<CouponRedemption>, ICouponRedemptionRepository
	{
		public CouponRedemptionRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public List<CouponRedemption> GetCouponRedemptions(long couponId, long customerId)
		{
			var couponRedemptions = this.DbContext.CouponRedemptions.Where(c => c.CouponID == couponId && c.CustomerID == customerId).ToList();
			return couponRedemptions;
		}
		public CouponRedemption GetCouponRedemptionsByBookingID(long couponId, long serviceBookingID)
		{
			var couponRedemptions = this.DbContext.CouponRedemptions.Where(c => c.CouponID == couponId && c.BookingID == serviceBookingID).FirstOrDefault();
			return couponRedemptions;
		}

	}

	public interface ICouponRedemptionRepository : IRepository<CouponRedemption>
	{
		List<CouponRedemption> GetCouponRedemptions(long couponId, long customerId);
		CouponRedemption GetCouponRedemptionsByBookingID(long couponId, long serviceBookingID);
	}
}
