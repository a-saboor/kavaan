using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    
    class CouponCategoriesRepository : RepositoryBase<CouponCategory>, ICouponCategoriesRepository
    {
        public CouponCategoriesRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
        public IEnumerable<CouponCategory> GetCouponCategoriesByID(long Id)
        {
            var CouponCategory = this.DbContext.CouponCategories.Where(mod => mod.ID == Id).OrderByDescending(x => x.ID).ToList();
            return CouponCategory;
        }
    }
    public interface ICouponCategoriesRepository : IRepository<CouponCategory>
    {

        IEnumerable<CouponCategory> GetCouponCategoriesByID(long id);

    }
}
