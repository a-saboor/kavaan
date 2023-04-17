using MyProject.Data;
using MyProject.Service;
using MyProject.Service.Helpers;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels;
using MyProject.Web.ViewModels.JobCandidate;
using MyProject.Web.ViewModels.PropertyProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Web.Controllers
{
    public class ProjectsController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IProjectService _projectService;
        private readonly IProjectTypeService _projectTypeService;

        public ProjectsController(
            IProjectService projectService
            , IProjectTypeService projectTypeService
            )
        {
            this._projectService = projectService;
            this._projectTypeService = projectTypeService;
        }

        [HttpGet]
        [Route("projects", Name = "projects")]
        public ActionResult Index()
        {
            PropertyProjectViewModel projectViewModel = new PropertyProjectViewModel();

            var count = 0;
            long ID = 0;
            var urlType = Request.Url.Query;
            var ProjectTypes = _projectTypeService.GetProjectTypes();

            if (!string.IsNullOrEmpty(urlType))
            {
                ID = (long)Convert.ToDouble(urlType.Split('=')[1]);
                count = _projectTypeService.GetProjectType(ID).Projects.Count();
            }
            else
            {
                count = _projectService.GetProjects().Count();
            }

            projectViewModel.ProjectTypes = ProjectTypes.ToList();
            projectViewModel.TotalResults = count;
            projectViewModel.CurrentProjectTypeID = ID;
            return View(projectViewModel);
        }

        [HttpPost]
        [Route("projects/filters", Name = "projects/filters")]
        public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];
            
            var count = 0;
            if (filters.parentID > 0)
            {
                count = _projectTypeService.GetProjectType((long)filters.parentID).Projects.Count();
            }
            else
            {
                count = _projectService.GetProjects().Count();
            }

            try
            {
                string ImageServer = "";
                var projects = _projectService.GetFilteredProjects(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer, filters.parentID);
                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    count = count,
                    data = projects.Select(x => new
                    {
                        x.ID,
                        x.Purpose,
                        x.Thumbnail,
                        x.Slug,
                        x.Name,
                        x.Description,
                        x.Address,
                        x.Bedrooms,
                        x.Baths,
                        x.AreaStart,
                        x.AreaEnd,
                        x.PriceStart,
                        x.PriceEnd,
                        x.Developer,
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("projects/{slug}", Name = "projects/{slug}")]
        public ActionResult Details(string slug)
        {
            var project = _projectService.GetProjectBySlug(slug);//get single project

            if (project == null)
            {
                throw new HttpException(404, "File Not Found");
            }
            return View(project);
        }

        [HttpGet]
        [Route("projects/get-all", Name = "projects/get-all")]
        public ActionResult GetAll(string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            try
            {
                string ImageServer = "";
                var projects = _projectService.GetProjects().Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ID).Take(8);

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = projects.Select(x => new
                    {
                        x.ID,
                        x.Purpose,
                        x.Thumbnail,
                        x.Slug,
                        x.Name,
                        x.Description,
                        x.Address,
                        x.Bedrooms,
                        x.Baths,
                        x.AreaStart,
                        x.AreaEnd,
                        x.PriceStart,
                        x.PriceEnd,
                        x.Developer,
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}