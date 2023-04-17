using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class UnitPaymentPlanRepository : RepositoryBase<UnitPaymentPlan>, IUnitPaymentPlanRepository
    {
        public UnitPaymentPlanRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public bool CreateUnitPaymentPlan(ref UnitPaymentPlan paymentPlan, ref string message)
        {
            var unitpaymentplan = this.DbContext.UnitPaymentPlans.Add(paymentPlan);
            var unitid = paymentPlan.UnitID;
            var unit = this.DbContext.Units.FirstOrDefault(x => x.ID == unitid);
            unitpaymentplan.Unit = unit;

            return true;
        }

        public UnitPaymentPlan GetUnitPaymentPlaneByName(string name,long unitid, long id = 0)
        {
            var user = this.DbContext.UnitPaymentPlans.Where(c => c.Milestones == name && c.IsDeleted == false && c.ID != id &&c.UnitID==unitid).FirstOrDefault();
            return user;
        }

        public IEnumerable<UnitPaymentPlan> GetUnitPaymentPlaneByUnitId(long unitid)
        {
            var paymentPlan = this.DbContext.UnitPaymentPlans.Where(c =>  c.UnitID == unitid && c.IsDeleted == false).ToList();
            return paymentPlan;
        }
    }

    public interface IUnitPaymentPlanRepository : IRepository<UnitPaymentPlan>
    {
        UnitPaymentPlan GetUnitPaymentPlaneByName(string name, long unitid, long id = 0);
        bool CreateUnitPaymentPlan(ref UnitPaymentPlan paymentPlan, ref string message);


        IEnumerable<UnitPaymentPlan> GetUnitPaymentPlaneByUnitId(long unitid);


    }
}
