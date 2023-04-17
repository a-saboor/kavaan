using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProjectTypeController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IProjectTypeService _projectTypeService;

        public ProjectTypeController(IProjectTypeService projectTypeService)
        {
            this._projectTypeService = projectTypeService;
        }
      
        // GET: Admin/ProjectType
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.ExcelUploadErrorMessage = TempData["ExcelUploadErrorMessage"];
            return View();
        }
        public ActionResult List()
        {
            var projectType = _projectTypeService.GetProjectTypes();
            return PartialView(projectType);

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProjectType data)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (_projectTypeService.CreateProjectType( ref data, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} project type created {data.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/ProjectType/Index",
                        message = message,
                        data = new
                        {
                            Date = data.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                            Name = data.Name,
                            IsActive = data.IsActive.HasValue ? data.IsActive.Value.ToString() : bool.FalseString,
                            ID = data.ID
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
            ProjectType projectType = _projectTypeService.GetProjectType((long)id);
            if (projectType == null)
            {
                return HttpNotFound();
            }

            TempData["projectTypeID"] = id;
            return View(projectType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectType projectType)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                long Id;
                if (TempData["projectTypeID"] != null && Int64.TryParse(TempData["projectTypeID"].ToString(), out Id) && projectType.ID == Id)
                {
                    if (_projectTypeService.UpdateProjectType(ref projectType, ref message))
                    {
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} projectType updated {projectType.Name}.");
                        return Json(new
                        {
                            success = true,
                            url = "/Admin/Brands/Index",
                            message = "ProjectType updated successfully ...",
                            data = new
                            {
                                Date = projectType.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                                Name = projectType.Name,
                                IsActive = projectType.IsActive.HasValue ? projectType.IsActive.Value.ToString() : bool.FalseString,
                                ID = projectType.ID
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
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectType projectType = _projectTypeService.GetProjectType((Int16)id);
            if (projectType == null)
            {
                return HttpNotFound();
            }
            TempData["projectTypeID"] = id;
            return View(projectType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (_projectTypeService.DeleteProjectType((Int16)id, ref message))
            {
                ProjectType projectType = _projectTypeService.GetProjectType((Int16)id);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} projectType deleted {projectType.Name}.");
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
            var projectType = _projectTypeService.GetProjectType((long)id);
            if (projectType == null)
            {
                return HttpNotFound();
            }

            if (!(bool)projectType.IsActive) 
            {
                projectType.IsActive = true;
            }
            else
            {
                projectType.IsActive = false;
            }
            string message = string.Empty;
            if (_projectTypeService.UpdateProjectType(ref projectType, ref message))
            {
                SuccessMessage = "ProjectType " + ((bool)projectType.IsActive ? "activated" : "deactivated") + "  successfully ...";
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)projectType.IsActive ? "activated" : "deactivated")} {projectType.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = projectType.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                        Name = projectType.Name,
                        IsActive = projectType.IsActive.HasValue ? projectType.IsActive.Value.ToString() : bool.FalseString,
                        ID = projectType.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectType projectType = _projectTypeService.GetProjectType((Int16)id);
            if (projectType == null)
            {
                return HttpNotFound();
            }
            return View(projectType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectTypeReport()
        {
            var getAllProjectType = _projectTypeService.GetProjectTypes().ToList();
            if (getAllProjectType.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("ProjectTypeReport");

                    var headerRow = new List<string[]>()
                    {
                    new string[] {
                        "Creation Date"
                        ,"Name"
                        ,"Status"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["ProjectTypeReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var i in getAllProjectType)
                    {
                        cellData.Add(new object[] {
                        i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString(CustomHelper.GetDateFormat) : "-"
                        ,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
                        ,i.IsActive == true ? "Active" : "InActive"
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "ProjectType Report.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

    }
}