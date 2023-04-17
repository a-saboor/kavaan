using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Controllers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels.Blog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
	[AuthorizeCustomer]

	public class AppointmentsController : Controller
	{
		private readonly IAppointmentsService _appointmentService;
		public AppointmentsController(IAppointmentsService appointmentService)
		{
			this._appointmentService = appointmentService;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(Appointment appointmentModel)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				try
				{
					Int64 CustomerId = Convert.ToInt64(Session["CustomerID"].ToString());
					appointmentModel.CustomerID = CustomerId;

					if (_appointmentService.Create(appointmentModel, ref message))
					{
						return Json(new
						{
							success = true,
							message = "Appointment request submitted successfully.",
						}, JsonRequestBehavior.AllowGet);
					}
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
			}
			return Json(new
			{
				success = false,
				message = "Something went wrong! Please try later."
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult FilteredAppointments(FilterViewModel filters, string culture = "en-ae")
		{
			string lang = "en";
			if (culture.Contains('-'))
				lang = culture.Split('-')[0];

			try
			{
				string ImageServer = CustomURL.GetImageServer();
				var bookings = _appointmentService.GetFilteredAppointments(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, filters.parentID);
				return Json(new
				{
					success = true,
					message = "Data recieved successfully!",
					data = bookings.Select(x => new
					{
						x.ID,
						x.Date,
						x.AppointmentNo,
						AppointmentDate = x.AppointmentDate.HasValue ? x.AppointmentDate.Value.ToString("MM/dd/yyyy") : "-",
						AppointmentTime = x.AppointmentDate.HasValue ? x.AppointmentDate.Value.ToString("hh:mm tt") : "-",
						Type = lang == "en" ? x.Type : x.TypeAr,
						//x.TypeAr,
						x.Remarks,
						status = x.IsApproved ? "Approved" : "Pending",
						x.IsCompleted,
						x.IsCancelled,
					})
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Something went wrong! Please try later."
				}, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public ActionResult Cancel(long id)
		{
			try
			{
				string message = string.Empty;
				string status = string.Empty;

				var appointment = _appointmentService.GetAppointment(id);
				appointment.IsCancelled = true;
				appointment.Remarks = "Appointment is cancelled by the customer.";

				if (_appointmentService.UpdateAppointment(ref appointment, ref message))
				{
					return Json(new
					{
						success = true,
						message = "Appointment cancelled successfully."/* + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt")*/
					}, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new
					{
						success = false,
						message = "Something went wrong! Please try later.",
					}, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Something went wrong! Please try later.",
				}, JsonRequestBehavior.AllowGet);
			}
		}

	}
}