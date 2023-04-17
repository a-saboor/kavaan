using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class BookingRepository : RepositoryBase<UnitBooking>, IBookingRepository
	{
		public BookingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<UnitBooking> GetAll(DateTime startDate, DateTime endDate)
		{
			var bookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= startDate && c.CreatedOn <= endDate).ToList();
			return bookings;
		}

		//public IEnumerable<UnitBooking> GetAll(DateTime startDate, DateTime endDate, string bookingStatus)
		//{
		//	var bookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= startDate && c.CreatedOn <= endDate && c.Status == bookingStatus).ToList().BookingByDescending(x => x.ID);
		//	return bookings;
		//}

		//public IEnumerable<UnitBooking> GetAllCompletedBookings()
		//{
		//	var bookings = this.DbContext.UnitBookings.Where(c => c.Status == "Completed" || c.Status == "Canceled").ToList();
		//	return bookings;
		//}

		//public IEnumerable<UnitBooking> GetAllPendingBookings()
		//{
		//	var bookings = this.DbContext.UnitBookings.Where(c => c.Status == "Inprocess" || c.Status == "Pending" || c.Status == "Confirmed").ToList();
		//	return bookings;
		//}

		//public List<SP_GetBookingDetails_Result> GetBookingDetailByBookingID(long id, string lang)
		//{
		//	List<SP_GetBookingDetails_Result> booking = DbContext.SP_GetBookingDetails(id, lang).ToList();
		//	return booking;
		//}

		//public List<UnitBooking> GetFilteredPendingBookingsForDashboard(DateTime FromDate, DateTime ToDate)
		//{
		//	var Bookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && (c.Status == "Pending")).ToList();
		//	return Bookings;
		//}

		//public List<UnitBooking> GetFilteredPendingBookings(DateTime FromDate, DateTime ToDate)
		//{
		//	var Bookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && (c.Status == "Processing" || c.Status == "Pending" || c.Status == "Confirmed")).ToList();

		//	//var bookingDetails = this.DbContext.BookingDetails.ToList();
		//	//var list = from booking in Bookings
		//	//		   join details in bookingDetails
		//	//		   on booking.ID equals details.BookingID
		//	//		   select new { booking.ID , booking. }


		//	return Bookings;
		//}

		//public List<UnitBooking> GetFilteredCompletedBookings(DateTime FromDate, DateTime ToDate)
		//{
		//	var Bookings = this.DbContext.UnitBookings.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && (c.Status == "Completed" || c.Status == "Canceled")).ToList();
		//	return Bookings;
		//}

		//public List<SP_GetFilteredBookings_Result> GetFilteredBookings(DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int sortBy = 1)
		//{
		//	var Bookings = this.DbContext.SP_GetFilteredBookings(startDate, endDate, vendorId, status, pageNumber, sortBy).ToList();
		//	return Bookings;
		//}
		public IEnumerable<SP_GetFilteredBookings_Result> GetFilteredBookings(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID)
		{
			var bookings = this.DbContext.SP_GetFilteredBookings(search, pageSize, pageNumber, sortBy, lang, customerID).ToList();
			return bookings;
		}

	}
	public interface IBookingRepository : IRepository<UnitBooking>
	{
		IEnumerable<UnitBooking> GetAll(DateTime startDate, DateTime endDate);

		IEnumerable<SP_GetFilteredBookings_Result> GetFilteredBookings(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID);
	}
}
