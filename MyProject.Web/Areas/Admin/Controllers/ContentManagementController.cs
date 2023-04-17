using Project.Data;
using Project.Service;
using Project.Web.Areas.Admin.ViewModels;
using Project.Web.AuthorizationProvider;
using Project.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{

    [AuthorizeAdmin]
    public class ContentManagementController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IContentManagementService _contentManagementService;
        public ContentManagementController(IContentManagementService contentManagementService)
        {
            this._contentManagementService = contentManagementService;
        }

        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            ContentViewModel contentViewModel = new ContentViewModel()
            {
                VideoFirst = _contentManagementService.GetContentByType("Video"),
                Image = _contentManagementService.GetContentByType("Image"),

            };

            return View(contentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, string Title, string TitleAr, string description, string descriptionar, string Type, bool isenable = false)
        {
            string message = string.Empty;

            if (file != null && file.ContentLength > 0)
                try
                {
                    ContentManagement objContentManagement = new ContentManagement();
                    string replacelement = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Guid.NewGuid().ToString();
                    if (file != null)
                    {
                        if (Type == "Image")
                        {
                            string absolutePath = Server.MapPath("~");
                            string relativePath = string.Format("/Assets/AppFiles/WebsiteContent/Image/{0}/", replacelement.Replace(" ", "_"));
                            objContentManagement.Path = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "BannerImage", ref message, "file");
                            objContentManagement.Type = "Image";

                        }
                        else
                        {

                            string absolutePath = Server.MapPath("~");
                            string relativePath = string.Format("/Assets/AppFiles/WebsiteContent/Video/{0}/", replacelement.Replace(" ", "_"));
                            objContentManagement.Path = Uploader.UploadVideo(Request.Files, absolutePath, relativePath, "Video", ref message, "file");
                            objContentManagement.Type = "Video";
                        }

                    }




                    objContentManagement.Title = Title;
                    objContentManagement.TitleAr = TitleAr;
                    objContentManagement.Description = description;
                    objContentManagement.DescriptionAr = descriptionar;
                    objContentManagement.IsEnable = isenable;
                    string togglemessage = String.Empty;
                    if (Type == "Image" && isenable == true)
                    {

                        var videodisable = _contentManagementService.GetContentByType("Video");
                        if (videodisable != null)
                        {
                            videodisable.IsEnable = false;
                            _contentManagementService.UpdateContent(ref videodisable, ref togglemessage);
                        }


                    }
                    else if (Type == "Video" && isenable == true)
                    {
                        var imagedisable = _contentManagementService.GetContentByType("Image");
                        if (imagedisable != null)
                        {
                            imagedisable.IsEnable = false;
                            _contentManagementService.UpdateContent(ref imagedisable, ref togglemessage);
                        }
                    }

                    if (_contentManagementService.CreateContent(objContentManagement, ref message))
                    {

                    }
                    TempData["SuccessMessage"] = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                TempData["ErrorMessage"] = "You have not specified a file.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(long id, string Title, string TitleAr, string description, string descriptionar, string type, bool isenable = false)
        {
            try
            {
                string message = string.Empty;
                ContentManagement content = _contentManagementService.GetContent(id);
                //content.Title = Title;
                //content.TitleAr = TitleAr;
                //content.Description = description;
                //content.DescriptionAr = descriptionar;
              
                content.IsEnable = isenable;
                if (_contentManagementService.UpdateContent(ref content, ref message))
                {
                    string togglemessage = String.Empty;
                    if (type == "Image"&&isenable==true)
                    {
                        
                       var videodisable=_contentManagementService.GetContentByType("Video");
                        if (videodisable != null)
                        {
                            videodisable.IsEnable=false;
                            _contentManagementService.UpdateContent(ref videodisable,ref togglemessage);
                        }


                    }
                    else if (type == "Video" && isenable==true)
                    {
                        var imagedisable = _contentManagementService.GetContentByType("Image");
                        if (imagedisable != null)
                        {
                            imagedisable.IsEnable = false;
                            _contentManagementService.UpdateContent(ref imagedisable, ref togglemessage);
                        }
                    }
                    return Json(new
                    {
                        success = true,
                        message = "Banner Settings updated successfully!",
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(long id)
        {
            try
            {
                string message = string.Empty;
                string absolutePath = Server.MapPath("~");
                ContentManagement content = _contentManagementService.GetContent(id);
                if (content != null)
                {
                    string togglemessage = String.Empty;
                    if (content.Type == "Image" && content.IsEnable == true)
                    {

                        var videodisable = _contentManagementService.GetContentByType("Video");
                        if (videodisable != null)
                        {
                            videodisable.IsEnable = false;
                            _contentManagementService.UpdateContent(ref videodisable, ref togglemessage);
                        }


                    }
                    else if (content.Type == "Video" && content.IsEnable == true)
                    {
                        var imagedisable = _contentManagementService.GetContentByType("Image");
                        if (imagedisable != null)
                        {
                            imagedisable.IsEnable = false;
                            _contentManagementService.UpdateContent(ref imagedisable, ref togglemessage);
                        }
                    }

                    if (_contentManagementService.DeleteContent(id, true))
                    {
                        if (System.IO.File.Exists(absolutePath + content.Path))
                        {
                            System.IO.File.Delete(absolutePath + content.Path);
                        }
                        return Json(new
                        {
                            success = true,
                            message = "File deleted successfully ..."
                        });
                    }
                }
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
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
    }

}