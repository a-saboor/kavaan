using MyProject.Service;
using MyProject.Web.AuthorizationProvider; 
using MyProject.Web.Helpers;
using MyProject.Web.ViewModels.Account;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Areas.Admin.Controllers
{
	public class AccountController : Controller
	{
		#region Dependecy Settings

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		string Message = string.Empty;
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;
		bool status = false;

		private readonly IUserService _userService;

		public AccountController(IUserService userService)
		{
			this._userService = userService;
		}

		#endregion

		public ActionResult Login(string returnUrl)
		{
			if (TempData["ReturnUrl"] == null && !string.IsNullOrEmpty(returnUrl))
				TempData["ReturnUrl"] = returnUrl;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel Login)
		{
			Session.Clear();
			string url = string.Empty;

			if (ModelState.IsValid)
			{
				var User = _userService.Authenticate(Login.EmailAddress, Login.Password, ref Message);
				if (User != null)
				{
					Session["AdminUserID"] = User.ID;
					Session["UserName"] = User.Name;
					Session["Role"] = User.UserRole.RoleName;
					Session["Email"] = User.EmailAddress;
					Session["ReceiverType"] = User.UserRole.RoleName;
					Session["UserNameChar"] = AdminSessionHelper.MakeUserNameChar;

					Response.Cookies["Admin-Session"]["Access-Token"] = User.AuthorizationCode;

					url = "/Admin/Dashboard/Index";
					if (TempData["ReturnUrl"] != null)
						url = $"{TempData["ReturnUrl"]}";

					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} logged in.");
					status = true;
				}
				else
				{
					Message = "Invalid credentials!";
				}
			}
			else
			{
				Message = "Please enter email and password first!";
			}

			return Json(new { success = status, message = Message, url });
		}
		[HttpGet]
		public ActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ForgetPassword(ForgotPasswordViewModel forgotPasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var path = Server.MapPath("~/");
				if (_userService.ForgotPassword(forgotPasswordViewModel.EmailAddress, ref Message, path))
				{
					status = true;
				}
			}
			else
			{
				Message = "Please fill the form properly ...";
			}

			return Json(new { success = status, message = Message });
		}

		public ActionResult ResetPassword()
		{
			var AuthCode = Request.QueryString["auth"];
			if (string.IsNullOrEmpty(AuthCode))
			{
				ViewBag.ErrorMessage = "Invalid Session!";
			}
			else
			{
				var user = _userService.GetByAuthCode(AuthCode);
				if (user != null)
				{
					if (user.AuthorizationExpiry >= Helpers.TimeZone.GetLocalDateTime())
					{
						Session["UserResetID"] = user.ID;
					}
					else
					{
						ViewBag.ErrorMessage = "Session expired!";
					}
				}
				else
				{
					ViewBag.ErrorMessage = "Invalid Session!";
				}
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
			string url = string.Empty;
			if (ModelState.IsValid)
			{
				if (Session["UserResetID"] != null)
				{
					long UserId = Convert.ToInt64(Session["UserResetID"]);

					if (_userService.ResetPassword(resetPasswordViewModel.NewPassword, UserId, ref Message))
					{
						log.Info($"ID: {UserId} reset its password.");
						Session.Remove("UserResetID");
						url = "Admin/Dashboard/Index";
						status = true;
					}
				}
				else
				{
					Message = "Session expired!";
				}
			}
			else
			{
				Message = "Please fill the form properly ...";
			}
			return Json(new { success = status, message = Message, url });
		}

		[AuthorizeAdmin]
		public ActionResult ChangePassword()
		{
			long userId = AdminSessionHelper.ID;
			var user = _userService.GetUser(userId);

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeAdmin]
		public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				long UserId = AdminSessionHelper.ID;

				if (_userService.ChangePassword(changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword, UserId, ref Message))
				{
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} changed its password.");
					status = true;
					Message = "Password Changed Successfully";
				}
			}
			else
			{
				Message = "Please fill the form properly ...";
				ErrorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
			}

			return Json(new { success = status, message = Message, error = ErrorMessage });
		}

		[AuthorizeAdmin]
		public ActionResult Logout()
		{
			log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} logged out.");

			Session.Remove("AdminUserID");
			Session.Remove("UserName");
			Session.Remove("Role");
			Session.Remove("Email");
			Session.Remove("Access-Token");

			Request.Cookies.Remove("Admin-Session");
			Response.Cookies.Remove("Admin-Session");
			Session.Abandon();
			Session.Clear();
			Response.Cookies["Admin-Session"]["Access-Token"] = "";

			return RedirectToAction("Login");
		}
	}
}