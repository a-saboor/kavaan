using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class CouponRepository : RepositoryBase<Coupon>, ICouponRepository
	{
		public CouponRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public Coupon GetCouponByCode(string code, long id = 0)
		{
			var user = this.DbContext.Coupons.Where(c => c.CouponCode == code && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
		public bool InsertCoupons(string Name, string CouponCode, int Frequency, int MaxRedumption, string type, decimal value, decimal maxAmount, System.DateTime Expiry, bool IsOpenToAll, System.DateTime CreatedOn, bool IsActive, bool IsDeleted)
		{
			try
			{
				DbContext.PR_InsertCoupons(Name, CouponCode, Frequency, MaxRedumption, type, value, maxAmount, Expiry, IsOpenToAll, CreatedOn, IsDeleted, IsActive);
				return true;
			}
			catch (System.Exception ex)
			{
				return false;
			}
		}
	}

	public interface ICouponRepository : IRepository<Coupon>
	{
		Coupon GetCouponByCode(string code, long id = 0);
		bool InsertCoupons(string Name, string CouponCode, int Frequency, int MaxRedumption, string type, decimal value, decimal maxAmount, System.DateTime Expiry, bool IsOpenToAll, System.DateTime CreatedOn, bool IsActive, bool IsDeleted);
	}
}
