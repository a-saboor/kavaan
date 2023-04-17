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
    public class OzoneController : Controller
    {
        // GET: Admin/Ozone
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IOzoneService _ozoneService;

        public OzoneController(IOzoneService ozoneService)
        {
            this._ozoneService = ozoneService;
        }
        public ActionResult Create()
        {

            Ozone ozone = _ozoneService.GetOzonefirstordefault();

            if (ozone == null)
            {
                ozone = new Ozone();

            }

            return View(ozone);
        }
        [HttpPost]
        public ActionResult Create(long? id, Ozone data)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {

                Ozone record = new Ozone();

                record.ID = data.ID;

                if (Request.Files["thumbnailFile"].ContentLength > 0)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Ozone/OzoneImages/");
                    record.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "thumbnailFile", ref message, "thumbnailFile");
                }
                if (data.Video == null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Ozone/Video/");
                    record.Video = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "VideoFile", ref message, "VideoFile");
                }
                if (_ozoneService.UpdateOzone(ref record, ref message))
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectPermanent("/Admin/Ozone/Create");
                }
            }
            else
            {
                message = "Please fill the form properly ...";
            }
            return RedirectPermanent("/Admin/Ozone/Create");
        }
    }
}