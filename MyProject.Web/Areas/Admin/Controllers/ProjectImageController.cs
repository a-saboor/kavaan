using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProjectImageController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IProjectImageService _projectImageService;
        private readonly IProjectService _projectService;

        public ProjectImageController(IProjectImageService projectImageService, IProjectService projectService)
        {
            this._projectImageService = projectImageService;
            this._projectService = projectService;
        }

        [HttpGet]
        public ActionResult GetProjectImages(long id)
        {
            var projectImages = _projectImageService.GetProjectImages(id).Select(i => new
            {
                id = i.ID,
                Image = i.Image,
                position = i.Position,
            }).ToList();

            return Json(new
            {
                success = true,
                message = "Data recieved successfully!",
                projectImages = projectImages
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(long? id, int count)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Data.Project project = _projectService.GetProject((long)id);
                if (project == null)
                {
                    return HttpNotFound();
                }

                string message = string.Empty;

                string absolutePath = Server.MapPath("~");
                string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/Gallery/", project.Slug.Replace(" ", "_"));

                List<string> Pictures = new List<string>();

                Dictionary<long, string> data = new Dictionary<long, string>();
                Uploader.UploadImages(Request.Files, absolutePath, relativePath, "g", ref Pictures, ref message, "GalleryImages");
                if (Pictures.Count() == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Incorrect format..!",
                    });
                }
                foreach (var item in Pictures)
                {
                    ProjectImage projectImage = new ProjectImage();
                    projectImage.ProjectID = id;
                    projectImage.Image = item;
                    projectImage.Position = ++count;
                    if (_projectImageService.Create(projectImage, ref message))
                    {
                        data.Add(projectImage.ID, item);
                    }
                }

                return Json(new
                {
                    success = true,
                    message = message,
                    data = data.ToList()
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            string filePath = string.Empty;
            if (_projectImageService.DeleteProjectImages(id, ref message, ref filePath))
            {
                if (System.IO.File.Exists(Server.MapPath(filePath)))
                    System.IO.File.Delete(Server.MapPath(filePath));
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}