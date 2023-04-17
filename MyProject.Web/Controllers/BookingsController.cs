using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels.JobCandidate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Web.Controllers{
	[AuthorizeCustomer]
	public class BookingsController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IBookingService _BookingService;
		private readonly ICustomerService _CustomerService;
		private readonly IUnitService _UnitService;

		public BookingsController(IBookingService BookingService, ICustomerService CustomerService, IUnitService UnitService)
		{
			this._BookingService = BookingService;
			this._CustomerService = CustomerService;
			this._UnitService = UnitService;
		}

		[HttpGet]
		[Route("bookings/popup", Name = "bookings/popup")]
		public ActionResult BookNow()
		{
			return View();
		}

		[HttpGet]
		[Route("bookings/book-now/{id}", Name = "bookings/book-now/{id}")]
		public ActionResult BookNow(long id)//Unit ID
		{
			//Get Customer ID
			Int64 CustomerId = 0;
			if (Session["CustomerID"] != null)
				CustomerId = Convert.ToInt64(Session["CustomerID"].ToString());

			//Get New Booking
			var unitBooking = new UnitBooking();
			unitBooking.Unit = new Unit();
			unitBooking.Customer = new Customer();

			//Get Booking Unit
			unitBooking.UnitID = id;
			unitBooking.Unit = _UnitService.GetUnit(id);

			//Get Customer if customerId exists
			if (CustomerId > 0)
			{
				unitBooking.CustomerID = CustomerId;
				unitBooking.Customer = _CustomerService.GetCustomer(CustomerId);
			}

			return View(unitBooking);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("bookings/book", Name = "bookings/book")]
		public ActionResult BookNow(UnitBooking unitBooking)
		{
            string message = string.Empty;
            try
            {
				//Get Customer ID
				if (Session["CustomerID"] != null)
					unitBooking.CustomerID = Convert.ToInt64(Session["CustomerID"].ToString());
				else
					unitBooking.CustomerID = null;

				if (_BookingService.CreateBooking(ref unitBooking, ref message))
                {
                    return Json(new
                    {
                        success = true,
						booking = new
						{
							id = unitBooking.ID,
							bookingNo = unitBooking.BookingNo
						},
						message = message,
					});
                }
            }
            catch (Exception ex)
            {
                message = "";
            }
            return Json(new { success = false, message = message });
        }

	}
}