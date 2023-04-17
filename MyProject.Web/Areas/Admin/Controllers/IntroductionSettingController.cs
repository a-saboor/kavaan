using Project.Data;
using Project.Service;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class IntroductionSettingController : Controller
    {
        // GET: Admin/Introduction
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IIntroductionSettingService _introductionSettingService;

        public IntroductionSettingController(IIntroductionSettingService introductionSettingService)
        {
            this._introductionSettingService = introductionSettingService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var result = this._introductionSettingService.GetIntroductionSetting();
            return PartialView(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(IntroductionSetting data)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {

                IntroductionSetting record = new IntroductionSetting();

                record.ID = data.ID;

                if (_introductionSettingService.CreateIntroductionSetting(ref data, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/IntroductionSetting/Index",
                        message = message,
                        data = new
                        {
                            Type = data.Type,
                            City = data.City,
                            Heading = data.Heading,
                            Paragraph = data.Paragraph,
                            ID = data.ID
                        },
                    });
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

		public ActionResult Details(long? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			IntroductionSetting introductionSetting = _introductionSettingService.GetIntroductionSetting((Int16)id);
			if (introductionSetting == null)
			{
				return HttpNotFound();
			}
			return View(introductionSetting);
		}
		public ActionResult Edit(long id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			IntroductionSetting introductionSetting = _introductionSettingService.GetIntroductionSetting(id);
			if (introductionSetting == null)
			{
				return HttpNotFound();
			}
			return View(introductionSetting);
		}

        [HttpPost]
        public ActionResult Edit(IntroductionSetting data)
        {
            string message = string.Empty;
            string replacement = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {

                IntroductionSetting currentintroductionSetting = _introductionSettingService.GetIntroductionSetting(data.ID);
                currentintroductionSetting.City = data.City;
                currentintroductionSetting.Heading = data.Heading;
                currentintroductionSetting.HeadingAr = data.HeadingAr;
                currentintroductionSetting.Paragraph = data.Paragraph;
                currentintroductionSetting.ParagraphAr = data.ParagraphAr;

                if (_introductionSettingService.UpdateIntroductionSetting(ref currentintroductionSetting, ref message))
                {
                    //log.Info($"{Session["UserName"]} | {Session["Email"]} udpated customer document {currentCustomerDocument.DocumentNo}.");
					log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} Introduction setting updated {data.Heading}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/IntroductionSetting/Index",
                        message = message,
                        data = new
                        {
                            Type = currentintroductionSetting.Type,
                            City = currentintroductionSetting.City,
                            Heading = currentintroductionSetting.Heading,
                            Paragraph = currentintroductionSetting.Paragraph,
                            ID = currentintroductionSetting.ID,
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                message = "Please fill the form correctly";
            }
            return Json(new { success = false, message = message });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bool introductionSetting = _introductionSettingService.DeleteIntroductionSetting(id, ref message, false); ;
            if (introductionSetting)
            {
                //log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted customer document ID: {id}.");
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);


        }
    }
}