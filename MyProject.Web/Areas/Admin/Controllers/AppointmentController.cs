using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Project.Data;
using Project.Service;
using Project.Service.Helpers;
using Project.Web.AuthorizationProvider;

namespace Project.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class AppointmentController : Controller
	{
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		private readonly IAppointmentsService appointmentsService;
		private readonly IMail _email;
		public AppointmentController(IAppointmentsService appointmentsService, IMail email)
		{
			this.appointmentsService = appointmentsService;
			this._email = email;
		}

		// GET: Admin/Appointments
		public ActionResult Index(int id = 0)
		{
			ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
			ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
			ViewBag.Id = id;
			return View();
		}
		public ActionResult List(int id = 0)
		{
			DateTime ToDate = Helpers.TimeZone.GetLocalDateTime();
			DateTime FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30);
			string status = string.Empty;
			IEnumerable<Appointment> appointments = new List<Appointment>();
			appointments = this.appointmentsService.GetAppointments(FromDate, ToDate, id);
			if (id == 1)
			{
				status = "Pending";
			}
			else if (id == 2)
			{
				status = "Completed";
			}
			else if (id == 3)
			{
				status = "Cancelled";
			}

			ViewBag.Status = status + " ";
			return View(appointments.ToList());
		}

		// Appointment Filterd List
		[HttpPost]
		public ActionResult List(DateTime fromDate, DateTime ToDate, int id = 0)
		{
			ToDate = ToDate.AddMinutes(1439);
			string status = string.Empty;
			IEnumerable<Appointment> appointments = new List<Appointment>();
			appointments = this.appointmentsService.GetAppointments(fromDate, ToDate, id);
			if (id == 1)
			{
				status = "Pending";
			}
			else if (id == 2)
			{
				status = "Completed";
			}
			else if (id == 3)
			{
				status = "Cancelled";
			}

			ViewBag.Status = status + " ";
			return View(appointments.ToList());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AppointmentsReport(DateTime? FromDate, DateTime? ToDate, int id = 0)
		{
			DateTime EndDate = ToDate.Value.AddMinutes(1439);
			string status = string.Empty;
			IEnumerable<Appointment> getAllTransactions = new List<Appointment>();
			getAllTransactions = this.appointmentsService.GetAppointments(FromDate.Value, EndDate, id).ToList();

			if (getAllTransactions.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("AppointmentsReport");

					var headerRow = new List<string[]>()
					{
					new string[] {
						"Creation Date"
						,"Customer"
						,"Appointment Date"
						,"Type"
						,"Completed"
						,"Status"
						}
					};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["AppointmentsReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					if (getAllTransactions.Count() != 0)
						getAllTransactions = getAllTransactions.OrderByDescending(x => x.ID).ToList();

					foreach (var i in getAllTransactions)
					{


                        if (i.IsCancelled == true) 
						{
							status = "Cancelled";
						}
						else if (i.IsApproved == true) 
						{
							status = "Approved";
						}
						else if (i.IsCancelled == false && i.IsApproved == false)
						{
							status = "Pending";
						}
                        else
                        {
							status = "Pending";
						}
						cellData.Add(new object[] {
						i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
						,!string.IsNullOrEmpty(i.Customer.UserName) ? i.Customer.UserName:"-"
						,i.AppointmentDate.HasValue ? i.AppointmentDate.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
						,!string.IsNullOrEmpty(i.Type) ? i.Type:"-"
						,i.IsCompleted ? "Yes" : "No"
						,!string.IsNullOrEmpty(status) ? status : "-"
					});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "Appointment Report.xlsx");
				}
			}
			return RedirectToAction("Index");
		}
		// GET: Admin/Appointments/Details/5
		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Appointment appointment = this.appointmentsService.GetAppointment((long)id);
			if (appointment == null)
			{
				return HttpNotFound();
			}
			return View(appointment);
		}

		// GET: Admin/Appointments/Create
		public ActionResult Create()
		{
			// ViewBag.UserID = new SelectList(db.Users, "ID", "Name");
			return View();
		}

		// POST: Admin/Appointments/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,UserID,AppointmentDate,Type,TypeAr,Remarks,IsDeleted,CreatedOn,IsApproved,IsCompleted,IsCancelled")] Appointment appointment)
		{
			if (ModelState.IsValid)
			{
				//db.Appointments.Add(appointment);
				//db.SaveChanges();
				return RedirectToAction("Index");
			}

			//ViewBag.UserID = new SelectList(db.Users, "ID", "Name", appointment.UserID);
			return View(appointment);
		}

		// GET: Admin/Appointments/Edit/5
		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Appointment appointment = this.appointmentsService.GetAppointment((long)id);
			if (appointment == null)
			{
				return HttpNotFound();
			}
			return View(appointment);
		}

		// POST: Admin/Appointments/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,UserID,AppointmentDate,Type,TypeAr,Remarks,IsDeleted,CreatedOn,IsApproved,IsCompleted,IsCancelled")] Appointment appointment)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (this.appointmentsService.UpdateAppointment(ref appointment, ref message))
				{
					return Json(new
					{
						success = true,
						url = "/Admin/appointment/Index",
						message = "Appointment updated successfully ...",
						data = new
						{
							ID = appointment.ID,
							Date = appointment.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
							UserName = appointment.Customer.UserName,
							AppointmentDate = appointment.AppointmentDate.Value.ToString("dd MMM yyyy, h: mm tt"),
							Type = appointment.Type,
							IsApproved = appointment.IsApproved.ToString(),
							IsCompleted = appointment.IsCompleted.ToString(),
							IsCancelled = appointment.IsCancelled.ToString(),
						}
					});
				}

			}
			return Json(new { success = false, message = message });
		}

		// GET: Admin/Appointments/Delete/5
		public ActionResult Delete(long? id)
		{
			//  if (id == null)
			//  {
			//      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//  }
			////  Appointment appointment = db.Appointments.Find(id);
			//  if (appointment == null)
			//  {
			//      return HttpNotFound();
			//  }
			return View();
		}

		// POST: Admin/Appointments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;

			if (this.appointmentsService.DeleteAppointment((Int16)id, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var appointment = this.appointmentsService.GetAppointment((long)id);
			if (appointment == null)
			{
				return HttpNotFound();
			}

			if (!(bool)appointment.IsApproved)
				appointment.IsApproved = true;
			else
			{
				appointment.IsApproved = false;
			}
			string message = string.Empty;
			var path = Server.MapPath("~/");
			if (this.appointmentsService.UpdateAppointment(ref appointment, ref message))
			{
				//send appointment confirmation email to customer
				if (appointment.IsApproved == true)
				{
					_email.SendAppointmentMail(appointment, path);
				}

				SuccessMessage = "Appointment " + ((bool)appointment.IsApproved ? "approved" : "unapproved") + "  successfully ...";
				return Json(new
				{
					success = true,
					url = "/Admin/appointment/Index",
					message = SuccessMessage,
					data = new
					{
						ID = appointment.ID,
						Date = appointment.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						UserName = appointment.Customer.UserName,
						AppointmentDate = appointment.AppointmentDate.Value.ToString("dd MMM yyyy, h: mm tt"),
						Type = appointment.Type,
						IsApproved = appointment.IsApproved.ToString(),
						IsCompleted = appointment.IsCompleted.ToString(),
						IsCancelled = appointment.IsCancelled.ToString(),
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Cancel(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var appointment = this.appointmentsService.GetAppointment((long)id);
			if (appointment == null)
			{
				return HttpNotFound();
			}

			if (appointment.IsCancelled == null || appointment.IsCancelled == false)
				appointment.IsCancelled = true;
			else
			{
				appointment.IsCancelled = false;
			}
			string message = string.Empty;
			var path = Server.MapPath("~/");
			if (this.appointmentsService.UpdateAppointment(ref appointment, ref message))
			{
				//send appointment confirmation email to customer
				if (appointment.IsCancelled == true)
				{
					_email.SendAppointmentMail(appointment, path);
				}

				SuccessMessage = "Appointment " + ((bool)appointment.IsCancelled ? "cancelled" : "kept") + "  successfully ...";
				return Json(new
				{
					success = true,
					url = "/Admin/appointment/Index",
					message = SuccessMessage,
					data = new
					{
						ID = appointment.ID,
						Date = appointment.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						UserName = appointment.Customer.UserName,
						AppointmentDate = appointment.AppointmentDate.Value.ToString("dd MMM yyyy, h: mm tt"),
						Type = appointment.Type,
						IsApproved = appointment.IsApproved.ToString(),
						IsCompleted = appointment.IsCompleted.ToString(),
						IsCancelled = appointment.IsCancelled.ToString(),
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Remarks(long id)
		{
			var model = appointmentsService.GetAppointment(id);
			return View(model);
		}
		[HttpPost]
		public ActionResult Remarks(Appointment app)
		{
			string message = string.Empty;
			Appointment appointment = appointmentsService.GetAppointment((long)app.ID);
			appointment.Remarks = "<p>" + app.Remarks + "</p><hr /><p>" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "</p><br /><p>" + appointment.Remarks+ "</p>";
			if (appointmentsService.UpdateAppointment(ref appointment, ref message))
			{
				var Feed = appointment;
				return Json(new
				{
					success = true,
					url = "/Admin/Appointment/Index",
					message = "Remarks saved successfully ...",

					data = new
					{
						ID = appointment.ID,
						Date = appointment.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
						UserName = appointment.Customer.UserName,
						AppointmentDate = appointment.AppointmentDate.Value.ToString("dd MMM yyyy, h: mm tt"),
						Type = appointment.Type,
						IsApproved = appointment.IsApproved.ToString(),
						IsCompleted = appointment.IsCompleted.ToString(),
						IsCancelled = appointment.IsCancelled.ToString(),
					}
				});
			}
			return View("Index");
		}


	}
}
