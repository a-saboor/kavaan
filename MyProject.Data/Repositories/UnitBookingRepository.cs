using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class UnitBookingRepository : RepositoryBase<UnitBooking>, IUnitBookingRepository
    {
        public UnitBookingRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<UnitBooking> GetFilteredUnitBookings(DateTime FromDate, DateTime ToDate, string status)
        {
            List<UnitBooking> unitbookings = new List<UnitBooking>();

            if (string.IsNullOrEmpty(status) || status == "All")
            {
                unitbookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            }
            else
            {
                unitbookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.BookingStatus == status).ToList();
            }
            return unitbookings;
        }

        public bool CreateUnitBooking(ref UnitBooking unitBooking, ref string message)
        {
            var unitbooking = this.DbContext.UnitBookings.Add(unitBooking);
            var unitid = unitbooking.UnitID;
            var unit = this.DbContext.Units.FirstOrDefault(x => x.ID == unitid);
            unitbooking.Unit = unit;

            return true;
        }

        public UnitPaymentPlan GetUnitPaymentPlaneByName(string name, long id = 0)
        {
            var user = this.DbContext.UnitPaymentPlans.Where(c => c.Milestones == name && c.IsDeleted == false && c.ID != id).FirstOrDefault();
            return user;
        }

        public IEnumerable<UnitBooking> GetUnitBookingsByCustomer(long customerId)
        {
            var unitBooking = this.DbContext.UnitBookings.Where(mod => mod.CustomerID == customerId).OrderByDescending(x=>x.ID).ToList();
            return unitBooking;
        }

    }

    public interface IUnitBookingRepository : IRepository<UnitBooking>
    {
        //    UnitPaymentPlan GetUnitPaymentPlaneByName(string name, long id = 0);
        List<UnitBooking> GetFilteredUnitBookings(DateTime FromDate, DateTime ToDate, string status);

        bool CreateUnitBooking(ref UnitBooking unitBooking, ref string message);

        IEnumerable<UnitBooking> GetUnitBookingsByCustomer(long customerId);


    }
}
