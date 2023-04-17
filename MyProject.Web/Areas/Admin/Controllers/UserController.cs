using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider; 
using MyProject.Web.Helpers;
using OfficeOpenXml;
using MyProject.Service.Helpers.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace MyProject.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class UserController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IUserService _userService;
		private readonly IUserRoleService _userRoleService;

		public UserController(IUserService userService, IUserRoleService userRoleService)
		{
			this._userService = userService;
			this._userRoleService = userRoleService;
		}

		public ActionResult Index()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			return View();
		}

		public ActionResult List()
		{
			var users = _userService.GetUsers();
			return PartialView(users);
		}

		public ActionResult ListReport()
		{
			var users = _userService.GetUsers();
			return View(users);
		}

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = _userService.GetUser((Int16)id);
			user.MobileNo = user.MobileNo.Replace("92", "");
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		public ActionResult Create()
		{
			ViewBag.UserRoleID = new SelectList(_userRoleService.GetUserRolesForDropDown(), "value", "text");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(User user)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_userService.CreateUser(user, ref message, true))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created user {user.Name}.");
					var role = _userRoleService.GetUserRole((long)user.UserRoleID);
					return Json(new
					{
						success = true,
						url = "/Admin/User/Index",
						message = message,
						data = new
						{
							Date = user.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
							Name = user.Name,
							MobileNo = user.MobileNo,
							EmailAddress = user.EmailAddress,
							Role = role.RoleName,
							IsActive = user.IsActive.HasValue ? user.IsActive.Value.ToString() : bool.FalseString,
							ID = user.ID
						}
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}

			return Json(new { success = false, message = message });
		}

		public ActionResult Edit(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = _userService.GetUser((long)id);
			//user.MobileNo = user.MobileNo.Replace("92", "");
			//string RetrievedPass = new Crypto().RetrieveHash(user.Password, Convert.ToString(user.Salt.Value));
			//user.Password = RetrievedPass;
			if (user == null)
			{
				return HttpNotFound();
			}

			ViewBag.UserRoleID = new SelectList(_userRoleService.GetUserRolesForDropDown(), "value", "text", user.UserRoleID);

			TempData["AdminUserID"] = id;
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(User user)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				long Id;
				if (TempData["AdminUserID"] != null && Int64.TryParse(TempData["AdminUserID"].ToString(), out Id) && user.ID == Id)
				{

					if (_userService.UpdateUser(ref user, ref message))
					{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated user {user.Name}.");
						var role = _userRoleService.GetUserRole((long)user.UserRoleID);
						return Json(new
						{
							success = true,
							url = "/Admin/User/Index",
							message = "User updated successfully ...",
							data = new
							{
								Date = user.CreatedOn.Value.ToString(MyProject.Web.Helpers.CustomHelper.GetDateFormat),
								Name = user.Name,
								MobileNo = user.MobileNo,
								EmailAddress = user.EmailAddress,
								Role = role.RoleName,
								IsActive = user.IsActive.HasValue ? user.IsActive.Value.ToString() : bool.FalseString,
								ID = user.ID
							}
						});
					}
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}

		new public ActionResult Profile()
		{
			long id = Convert.ToInt64(Session["AdminUserID"]);

			User user = _userService.GetUser((Int16)id);
			if (user == null)
			{
				return RedirectPermanent("/Admin/Dashboard/Index");
			}

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		new public ActionResult Profile(User user)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				long Id = Convert.ToInt64(Session["AdminUserID"]);

				if (user.ID == Id)
				{
					if (_userService.UpdateUser(ref user, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated profile {user.Name}.");
						ViewBag.SuccessMessage = "Profile updated successfully ...";
						return View();
					}

					ViewBag.ErrorMessage = message;
				}
				else
				{
					ViewBag.ErrorMessage = "Oops! Something went wrong. Please try later...";
				}
			}
			else
			{
				ViewBag.ErrorMessage = "Please fill the form properly ...";
			}
			return View(user);
		}

		public ActionResult EmailVerification(string value, long ID = 0)
		{
			try
			{
				if (ModelState.IsValid)
				{

					if (_userService.EmailVerification(ref value, ID, ref ErrorMessage))
					{
						return Json(new { success = true, message = ErrorMessage, Email = value }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					ErrorMessage = "Something went wrong!";
				}
				return Json(new { success = false, Email = value, message = ErrorMessage });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}
		public ActionResult ContactVerification(string value, long ID = 0)
		{
			try
			{
				string code = "", contact = "";
				if (ModelState.IsValid)
				{
					if (value.Contains("|"))
					{
						code = value.Split('|')[0];
						contact = value.Split('|')[1];
					}
					//contact = code + contact;
					if (_userService.ContactVerification(ref code, ref contact, ID, ref ErrorMessage))
					{
						return Json(new { success = true, message = ErrorMessage, Contact = value }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					ErrorMessage = "Something went wrong!";
				}
				return Json(new { success = false, Contact = value, message = ErrorMessage });
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = "Oops! Something went wrong. Please try later."
				});
			}
		}
		public ActionResult Activate(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var user = _userService.GetUser((long)id);
			if (user == null)
			{
				return HttpNotFound();
			}

			if (!(bool)user.IsActive)
			{
				user.IsActive = true;
			}
			else
			{
				user.IsActive = false;
			}
			string message = string.Empty;
			if (_userService.UpdateUser(ref user, ref message))
			{
				var role = _userRoleService.GetUserRole((long)user.UserRoleID);
				SuccessMessage = "User " + ((bool)user.IsActive ? "activated" : "deactivated") + "  successfully ...";
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)user.IsActive ? "activated" : "deactivated")} {user.Name}.");
				return Json(new
				{
					success = true,
					message = SuccessMessage,
					data = new
					{
						Date = user.CreatedOn.Value.ToString(MyProject.Web.Helpers.CustomHelper.GetDateFormat),
						Name = user.Name,
						MobileNo = user.MobileNo,
						EmailAddress = user.EmailAddress,
						Role = role.RoleName,
						IsActive = user.IsActive.HasValue ? user.IsActive.Value.ToString() : bool.FalseString,
						ID = user.ID
					}
				}, JsonRequestBehavior.AllowGet);
			}
			else
			{
				ErrorMessage = "Oops! Something went wrong. Please try later...";
			}

			return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Delete(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = _userService.GetUser((Int16)id);
			if (user == null)
			{
				return HttpNotFound();
			}
			TempData["AdminUserID"] = id;
			return View(user);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(long id)
		{
			string message = string.Empty;

			if (_userService.DeleteUser((Int16)id, ref message))
			{
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted user ID: {id}.");
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UsersReport()
		{
			var getAllUsers = _userService.GetUsers().ToList();
			if (getAllUsers.Count() > 0)
			{
				using (ExcelPackage excel = new ExcelPackage())
				{
					excel.Workbook.Worksheets.Add("UsersReport");

					var headerRow = new List<string[]>()
					{
					new string[] {
						"Creation Date"
						,"Full Name"
						,"Email"
						,"Mobile"
						,"Role"
						,"Status"
						}
					};

					// Determine the header range (e.g. A1:D1)
					string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

					// Target a worksheet
					var worksheet = excel.Workbook.Worksheets["UsersReport"];

					// Popular header row data
					worksheet.Cells[headerRange].LoadFromArrays(headerRow);

					var cellData = new List<object[]>();

					foreach (var i in getAllUsers)
					{
						cellData.Add(new object[] {
						i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(MyProject.Web.Helpers.CustomHelper.GetDateFormat) : "-"
						,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
						,!string.IsNullOrEmpty(i.EmailAddress) ? i.EmailAddress : "-"
						,!string.IsNullOrEmpty(i.MobileNo) ? i.MobileNo : "-"
						,!string.IsNullOrEmpty(i.UserRole.RoleName) ? i.UserRole.RoleName : "-"
						,i.IsActive == true ? "Active" : "InActive"
						});
					}

					worksheet.Cells[2, 1].LoadFromArrays(cellData);

					return File(excel.GetAsByteArray(), "application/msexcel", "Users Report.xlsx");
				}
			}
			return RedirectToAction("Index");
		}

	}
}