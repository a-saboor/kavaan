using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using System.Web.Mvc;
using System.IO;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class EmailController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Message = string.Empty;
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        bool status = false;

        private readonly IEmailSettingService _emailService;
        private readonly IMail _email;

        public EmailController(IEmailSettingService emailService, IMail email)
        {
            this._emailService = emailService;
            this._email = email;
        }

        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            EmailSetting email = _emailService.GetDefaultEmailSetting();

            string[] filePaths = Directory.GetFiles($"{Server.MapPath("~/Assets/EmailTemplates")}", "*.html", SearchOption.AllDirectories);

            ViewDataDictionary viewData = new ViewDataDictionary();

            foreach (var file in filePaths)
            {
                if (!viewData.Keys.Contains(Path.GetFileNameWithoutExtension(file)))
                {
                    viewData.Add(Path.GetFileNameWithoutExtension(file), Path.GetFileNameWithoutExtension(file));
                }
            }

            ViewBag.EmailTemplates = viewData;

            return View(email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(long? id, EmailSetting email)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (id.HasValue && id > 0)
                {
                    if (_emailService.UpdateEmailSetting(ref email, ref message))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated email settings.");
                        TempData["SuccessMessage"] = message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    if (_emailService.CreateEmailSetting(email, ref message))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created email settings.");
                        TempData["SuccessMessage"] = message;
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            ViewBag.ErrorMessage = message;
            return View("Index", email);
        }

        public ActionResult TestEmail(string User, string Email, string EmailTemplate)
        {
            var path = Server.MapPath("~/");
            try
            {
                if (_email.SendTestEmail(User, Email, "Testing Email", EmailTemplate, path))
                {
                    Message = "Email sent succesfully!";
                    status = true;
                }
                else
                {
                    Message = "Email sending failed!";
                }
            }
            catch (System.Exception ex)
            {
                Message = "Email sending failed! Error: {" + ex.Message + "}";
            }

            return Json(new { success = status, message = Message });
        }

    }
}