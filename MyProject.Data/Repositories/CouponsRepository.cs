using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Repositories
{
    class CouponsRepository : RepositoryBase<Coupon>, ICouponssRepository
    {
        public CouponsRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
        public IEnumerable<Coupon> GetCouponsByID(long Id)
        {
            var Coupon = this.DbContext.Coupons.Where(mod => mod.ID == Id).OrderByDescending(x => x.ID).ToList();
            return Coupon;
        }
    }
    public interface ICouponssRepository : IRepository<Coupon>
    {

        IEnumerable<Coupon> GetCouponsByID(long id);

    }
}
