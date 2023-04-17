using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class UnitEnquiriesRepository : RepositoryBase<UnitEnquiry>, IUnitEnquiriesRepository
    {
        public UnitEnquiriesRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public bool CreateUnitEnquiry(ref UnitPaymentPlan paymentPlan, ref string message)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateUnitEnquiry(ref UnitEnquiry unitEnquiry, ref string message)
        {
            var unitenquiry = this.DbContext.UnitEnquiries.Add(unitEnquiry);
            var unitid = unitenquiry.UnitID;
            var unit = this.DbContext.Units.FirstOrDefault(x => x.ID == unitid);
            unitenquiry.Unit = unit;

            return true;
        }

        //public UnitPaymentPlan GetUnitPaymentPlaneByName(string name, long id = 0)
        //{
        //    var user = this.DbContext.UnitPaymentPlans.Where(c => c.Milestones == name&&c.IsDeleted==false && c.ID != id).FirstOrDefault();
        //    return user;
        //}
    
    }

    public interface IUnitEnquiriesRepository : IRepository<UnitEnquiry>
    {
      //  UnitPaymentPlan GetUnitPaymentPlaneByName(string name, long id = 0);
        bool CreateUnitEnquiry(ref UnitEnquiry unitEnquiry, ref string message);


    }
}
