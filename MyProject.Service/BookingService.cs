using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Service
{
	public class BookingService : IBookingService
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly ICustomerService _customerService;
        private readonly INumberRangeService _numberRangeService;
		private readonly IMail _email;
		private readonly IUnitOfWork _unitOfWork;

		public BookingService(IBookingRepository bookingRepository, ICustomerService customerService, INumberRangeService numberRangeService, IMail email, IUnitOfWork unitOfWork)
		{
			this._bookingRepository = bookingRepository;
			this._customerService = customerService;
            this._numberRangeService = numberRangeService;
			this._email = email;
			this._unitOfWork = unitOfWork;
		}

		#region IBookingService Members

		//public IEnumerable<UnitBooking> GetBookings()
		//{
		//    var bookings = _bookingRepository.GetAll();
		//    return bookings;
		//}

		//public List<UnitBooking> GetBookingsDateWise(DateTime FromDate, DateTime ToDate)
		//{
		//    var Bookings = _bookingRepository.GetFilteredPendingBookings(FromDate, ToDate);
		//    if (Bookings.Count != 0)
		//        Bookings = Bookings.BookingByDescending(x => x.CreatedOn).ToList();
		//    return Bookings;
		//}

		//public List<UnitBooking> GetCompletedBookingsDateWise(DateTime FromDate, DateTime ToDate)
		//{
		//    var Bookings = _bookingRepository.GetFilteredCompletedBookings(FromDate, ToDate);
		//    if (Bookings.Count != 0)
		//        Bookings = Bookings.BookingByDescending(x => x.CreatedOn).ToList();
		//    return Bookings;
		//}

		//public IEnumerable<UnitBooking> GetPendingBookings()
		//{
		//    var bookings = _bookingRepository.GetAllPendingBookings();
		//    return bookings;
		//}

		//public IEnumerable<UnitBooking> GetCompletedBookings()
		//{
		//    var bookings = _bookingRepository.GetAllCompletedBookings();
		//    return bookings;
		//}

		//public IEnumerable<UnitBooking> GetBookings(DateTime startDate, DateTime endDate)
		//{
		//    endDate = endDate.AddMinutes(1439);
		//    var bookings = _bookingRepository.GetAll(startDate, endDate);
		//    return bookings;
		//}

		//public IEnumerable<UnitBooking> GetTodaysBookings()
		//{
		//    DateTime startDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-365).Date;
		//    DateTime endDate = Helpers.TimeZone.GetLocalDateTime().Date.AddMinutes(1439);

		//    var bookings = _bookingRepository.GetAll(startDate, endDate, "Pending");
		//    return bookings;
		//}

		//public IEnumerable<object> GetBookingsForDropDown()
		//{
		//    var Bookings = _bookingRepository.GetAll();
		//    var dropdownList = from bookings in Bookings
		//                       select new { value = bookings.ID, text = bookings.BookingNo };
		//    return dropdownList;
		//}


		//public bool SendBookingEmail(string email, string subject, string body)
		//{
		//    try
		//    {

		//        var result = _email.SendBookingMail(email, subject, body);
		//        return result;
		//    }
		//    catch (Exception ex)
		//    {
		//        return false;
		//    }
		//}

		//public bool UpdateCarStock(long bookingId)
		//{
		//    try
		//    {
		//        return _bookingRepository.UpdateCarStock(bookingId);
		//    }
		//    catch (Exception ex)
		//    {
		//        return false;
		//    }
		//}

		//public List<SP_GetFilteredBookings_Result> GetFilteredBookings(DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int sortBy = 1)
		//{
		//    var Bookings = _bookingRepository.GetFilteredBookings(startDate, endDate, vendorId, status, pageNumber, sortBy).ToList();
		//    return Bookings;
		//}

		//public bool CheckScheduleAvailibity(long carId, DateTime startDate, DateTime endDate)
		//{
		//    try
		//    {
		//        return _bookingRepository.CheckScheduleAvailibity(carId, startDate, endDate);
		//    }
		//    catch (Exception ex)
		//    {
		//        return false;
		//    }
		//}

		public IEnumerable<SP_GetFilteredBookings_Result> GetFilteredBookings(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID)
		{
			var bookings = _bookingRepository.GetFilteredBookings(search, pageSize, pageNumber, sortBy, lang, customerID);
			return bookings;
		}

        public UnitBooking GetBooking(long id)
        {
            var booking = _bookingRepository.GetById(id);
            return booking;
        }

        public bool CreateBooking(ref UnitBooking booking, ref string message)
		{
			try
			{
				//booking.Status = "Confirmed";
				//booking.Tracking = "Confirmed";
				booking.BookingNo = _numberRangeService.GetNextValueFromNumberRangeByName("BOOKING");
				booking.IsPaid = false;
				booking.Status = true;
				booking.IsEarningCaptured = false;
				booking.PaymentMethod = "Card";
				booking.BookingStatus = "Pending";
				booking.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				booking.Amount = booking.Amount ?? 0;

				_bookingRepository.Add(booking);
				if (SaveBooking())
				{
					string customerMessage = string.Empty;
					string customerStatus = string.Empty;

					//try
					//{
					//	var customer = _customerService.GetCustomer((long)booking.CustomerID);
					//	if (customer != null)
					//	{
					//		customer.UserName = booking.FullName;
					//		customer.Email = booking.Email;
					//		//customer.Contact = booking.Contact;
					//		customer.Address = booking.Address;
					//		customer.Address2 = booking.Address2;
					//		customer.CountryID = booking.CountryID;
					//		customer.CityID = booking.CityID;
					//		customer.AreaID = booking.AreaID;
					//		customer.CustomerCountry = booking.CustomerCountry;
					//		customer.CustomerCity = booking.CustomerCity;
					//		//customer.IsActive = booking.IsActive;
					//		customer.FirstName = booking.FirstName;
					//		customer.LastName = booking.LastName;
					//		//customer.Contact = booking.Contact;
					//		customer.PoBox = booking.PoBox;
					//		customer.ZipCode = booking.ZipCode;
					//		customer.PassportNo = booking.PassportNo;
					//		customer.PassportExpiry = booking.PassportExpiry;
					//		customer.CNICNo = booking.EmiratesID;
					//		customer.CNICExpiry = booking.EmiratesIDExpiry;
					//		_customerService.UpdateCustomer(ref customer, ref customerMessage, ref customerStatus);
					//	}

					//}
					//catch (Exception ex)
					//{
					//}
					message = "Booking # "+booking.BookingNo+" has been Placed, processing for payment.";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
        }

        public bool UpdateBooking(ref UnitBooking booking, ref string message)
        {
            try
            {
                UnitBooking CurrentBooking = _bookingRepository.GetById(booking.ID);

                CurrentBooking.Amount = booking.Amount;
                CurrentBooking.TaxPercent = booking.TaxPercent;
                CurrentBooking.TaxAmount = booking.TaxAmount;
                CurrentBooking.TotalAmount = booking.TotalAmount;
                CurrentBooking.Status = booking.Status;
                CurrentBooking.PaymentMethod = booking.PaymentMethod;
                CurrentBooking.IsPaid = booking.IsPaid;
                CurrentBooking.IsEarningCaptured = booking.IsEarningCaptured;

                booking = null;

                _bookingRepository.Update(CurrentBooking);
                if (SaveBooking())
                {
                    booking = CurrentBooking;
                    message = "Booking updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool SaveBooking()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface IBookingService
	{
		//IEnumerable<UnitBooking> GetPendingBookings();
		//IEnumerable<UnitBooking> GetCompletedBookings();
		//IEnumerable<UnitBooking> GetBookings();
		//IEnumerable<UnitBooking> GetBookings(DateTime startDate, DateTime endDate);
		//IEnumerable<UnitBooking> GetTodaysBookings();
		//IEnumerable<object> GetBookingsForDropDown();

		//Task<bool> SendBookingEmailAsync(string email, string subject, string body);
		//bool SendBookingEmail(string email, string subject, string body);
		//bool UpdateCarStock(long bookingId);
		//List<UnitBooking> GetBookingsDateWise(DateTime FromDate, DateTime ToDate);
		//List<UnitBooking> GetCompletedBookingsDateWise(DateTime FromDate, DateTime ToDate);
		//List<SP_GetFilteredBookings_Result> GetFilteredBookings(DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int sortBy = 1);
		//bool CheckScheduleAvailibity(long carId, DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetFilteredBookings_Result> GetFilteredBookings(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID);

		UnitBooking GetBooking(long id);

		bool CreateBooking(ref UnitBooking booking, ref string message);
        bool UpdateBooking(ref UnitBooking booking, ref string message);

        bool SaveBooking();

	}
}
