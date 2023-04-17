using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class ServiceBookingRepository : RepositoryBase<ServiceBooking>, IServiceBookingRepository
    {
        public ServiceBookingRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public IEnumerable<ServiceBooking> GetAllByStatus(DateTime startDate, DateTime endDate, string orderStatus, long vendorId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.CreatedOn >= startDate
                                                                && c.CreatedOn <= endDate
                                                                && c.VendorID == vendorId
                                                                && (string.IsNullOrEmpty(orderStatus)
                                                                    || (orderStatus == "OnGoing" && c.Status != "Canceled" && c.Status != "Completed")
                                                                    || (orderStatus == "Past" && c.Status == "Canceled" && c.Status == "Completed")
                                                                    || (c.Status == orderStatus)
                                                                    )
                                                              ).ToList().OrderByDescending(x => x.ID);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllCompletedBookings(long vendorId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => (c.Status == "Canceled" ||  c.Status == "Completed") && (vendorId == null || vendorId < 1 ? true : c.VendorID == vendorId)).ToList();
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllCustomerUpcomingBookings(long customerID)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.CustomerID == customerID && (c.Status=="Pending" || c.Status== "Inprocess" || c.Status == "Diagnosis" || c.Status == "Invoiced")).ToList().OrderByDescending(i=>i.ID);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllStaffOngoingBookings(long staffId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.StaffID == staffId && (c.Status == "Pending" || c.Status == "Inprocess" || c.Status == "Diagnosis" || c.Status == "Invoiced")).ToList().OrderByDescending(i => i.ID);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllStaffCompletedBookings(long staffId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.StaffID == staffId && (c.Status == "Completed" )).ToList().OrderByDescending(i => i.ID);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllCustomerPastBookings(long customerID)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.CustomerID == customerID && (c.Status == "Completed" || c.Status == "Canceled")).ToList().OrderByDescending(i => i.ID);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllPendingBookings(long vendorId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => (c.Status == "Pending" || c.Status == "Inprocess" || c.Status == "Diagnosis" || c.Status == "Invoiced") && (vendorId == null || vendorId < 1 ? true : c.VendorID == vendorId)).ToList();
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllCompletedBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId)
        {
            var bookings = this.DbContext.ServiceBookings.Where(c => c.Status == "Canceled" || c.Status == "Completed" && (vendorId == null || vendorId < 1 ? true : c.VendorID == vendorId) && c.CreatedOn >= startDate && c.CreatedOn <= endDate).ToList();
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetAllPendingBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId)
        {
                                                             
            var bookings = this.DbContext.ServiceBookings.Where(c => c.CreatedOn >= startDate && c.CreatedOn <= endDate && (c.Status == "Inprocess" || c.Status == "Pending" || c.Status == "Invoiced" || c.Status == "Diagnosis") && (vendorId == null || vendorId < 1 ? true : c.VendorID == vendorId)).ToList();
            return bookings;
        }

        public IEnumerable<SP_GetCustomerServiceBookings_Result> GetCustomerServiceBookings(long customerID, string status, int? pageSize, int? pageNumber, int? sortBy, string lang, string imageServer)
        {
            var bookings = this.DbContext.SP_GetCustomerServiceBookings(customerID, status, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return bookings;
        }

        public List<SP_GetFilteredServiceBookings_Result> GetFilteredServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1)
        {
            var bookings = this.DbContext.SP_GetFilteredServiceBookings(search, startDate, endDate, vendorId, status, pageNumber, pageSize, sortBy).ToList();
            return bookings;
        }
        public List<SP_GetFVendorServiceBookings_Result> GetFilteredVendorServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1)
        {
            var bookings = this.DbContext.SP_GetFVendorServiceBookings(search, startDate, endDate, vendorId, status, pageNumber, pageSize, sortBy).ToList();
            return bookings;
        }
    }

    public interface IServiceBookingRepository : IRepository<ServiceBooking>
    {
        IEnumerable<ServiceBooking> GetAllByStatus(DateTime startDate, DateTime endDate, string orderStatus, long vendorId);
        IEnumerable<ServiceBooking> GetAllCustomerUpcomingBookings(long customerID);
        IEnumerable<ServiceBooking> GetAllStaffCompletedBookings(long staffId);
        IEnumerable<ServiceBooking> GetAllStaffOngoingBookings(long staffId);
        IEnumerable<ServiceBooking> GetAllCustomerPastBookings(long customerID);
        IEnumerable<ServiceBooking> GetAllCompletedBookings(long vendorId);
        IEnumerable<ServiceBooking> GetAllPendingBookings(long vendorId);
        IEnumerable<ServiceBooking> GetAllCompletedBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId);
        IEnumerable<ServiceBooking> GetAllPendingBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId);
        IEnumerable<SP_GetCustomerServiceBookings_Result> GetCustomerServiceBookings(long customerID, string status, int? pageSize, int? pageNumber, int? sortBy, string lang, string imageServer);
        List<SP_GetFVendorServiceBookings_Result> GetFilteredVendorServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1);

        List<SP_GetFilteredServiceBookings_Result> GetFilteredServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1);
    }
}
