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
    public class IntroductionController : Controller
    {
        // GET: Admin/Introduction
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IIntroductionService _introductionService;

        public IntroductionController(IIntroductionService introductionService)
        {
            this._introductionService = introductionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var result = this._introductionService.GetIntroductions();
            return PartialView(result);
        }
        public ActionResult Create()
        {

            Introduction introduction = _introductionService.GetIntroductionfirstordefault();

            if (introduction == null)
            {
                introduction = new Introduction();

            }

            return View(introduction);
        }
        [HttpPost]
        public ActionResult Create(long? id, Introduction data)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {

                Introduction record = new Introduction();

                record.ID = data.ID;
                record.Description = data.Description;
                record.DescriptionAr = data.DescriptionAr;

				if (Request.Files["thumbnailFile"].ContentLength > 0)
				//if (data.Thumbnail != null)
				{
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Introduction/IntroductionImages/");
                    record.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "thumbnailFile", ref message, "thumbnailFile");
                }
                if (data.Video == null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Introduction/Video/");
                    record.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "VideoFile", ref message, "VideoFile");
                }
                if (_introductionService.UpdateIntroduction(ref record, ref message))
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectPermanent("/Admin/Introduction/Create");
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return RedirectPermanent("/Admin/Introduction/Create");
        }
    }
}