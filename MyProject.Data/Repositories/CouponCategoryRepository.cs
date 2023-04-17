using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class CouponCategoryRepository : RepositoryBase<CouponCategory>, ICouponCategoryRepository
	{
		public CouponCategoryRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public CouponCategory GetCouponCategory(long couponId, long categoryId, long id = 0)
		{
			var user = this.DbContext.CouponCategories.Where(c => c.CouponID == couponId && c.ServiceID == categoryId && c.ID != id).FirstOrDefault();
			return user;
		}

		public IEnumerable<CouponCategory> GetCouponCategories(long couponId)
		{
			var couponCategories = this.DbContext.CouponCategories.Where(c => c.CouponID == couponId).ToList();
			return couponCategories;
		}

	}

	public interface ICouponCategoryRepository : IRepository<CouponCategory>
	{
		CouponCategory GetCouponCategory(long couponId, long categoryId, long id = 0);
		IEnumerable<CouponCategory> GetCouponCategories(long couponId);
	}
}
