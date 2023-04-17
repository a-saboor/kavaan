using LinqToExcel;
using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.POCO;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.Util;
using MyProject.Web.ViewModels.DataTables;
//using MyProject.Web.ViewModels.Project;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static MyProject.Web.Helpers.Enumerations.Enumeration;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProjectController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IProjectService _projectService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly IVendorService _vendorService;
        private readonly IAttributeService _attributeService;
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;
        private readonly IBrandsService _brandService;
        private readonly IProjectImageService _projectImageService;

        public ProjectController(
            IProjectService projectService
            , IProjectTypeService projectTypeService
            , IVendorService vendorService
            , IAttributeService attributeService
            , INotificationService notificationService
            , INotificationReceiverService notificationReceiverService
            , IBrandsService brandService
            , IProjectImageService projectImageService
            )
        {
            this._projectService = projectService;
            this._projectTypeService = projectTypeService;
            this._vendorService = vendorService;
            this._attributeService = attributeService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
            this._brandService = brandService;
            this._projectImageService = projectImageService;

        }

        #region Project

        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult List()
        {
            var projects = _projectService.GetProjects();
            return PartialView(projects);
        }

        //[HttpPost]
        //public JsonResult List(DataTableAjaxPostModel model)
        //{
        //	var searchBy = (model.search != null) ? model.search.value : "";
        //	int sortBy = 0;
        //	string sortDir = "";
        //	if (model.order != null)
        //	{
        //		sortBy = model.order[0].column;
        //		sortDir = model.order[0].dir.ToLower();
        //	}
        //	var Projects = _projectService.GetVendorProjects(model.length, model.start, sortBy, sortDir, searchBy, model.VendorID);

        //	int filteredResultsCount = Projects != null && Projects.Count() > 0 ? (int)Projects.FirstOrDefault().FilteredResultsCount : 0;
        //	int totalResultsCount = Projects != null && Projects.Count() > 0 ? (int)Projects.FirstOrDefault().TotalResultsCount : 0;

        //	var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text");
        //	ViewBag.Vendor = obj;
        //	//ViewBag.VendorID = _vendorService.GetVendorsForDropDown().ToList();

        //	return Json(new
        //	{
        //		draw = model.draw,
        //		recordsTotal = totalResultsCount,
        //		recordsFiltered = filteredResultsCount,
        //		data = Projects
        //	});
        //}

        //[HttpPost]
        //public JsonResult ListVendor(DataTableAjaxPostModel model, long? VendorID)
        //{
        //	var searchBy = (model.search != null) ? model.search.value : "";
        //	int sortBy = 0;
        //	string sortDir = "";
        //	if (model.order != null)
        //	{
        //		sortBy = model.order[0].column;
        //		sortDir = model.order[0].dir.ToLower();
        //	}
        //	var Projects = _projectService.GetVendorProjects(model.length, model.start, sortBy, sortDir, searchBy, VendorID);

        //	int filteredResultsCount = Projects != null && Projects.Count() > 0 ? (int)Projects.FirstOrDefault().FilteredResultsCount : 0;
        //	int totalResultsCount = Projects != null && Projects.Count() > 0 ? (int)Projects.FirstOrDefault().TotalResultsCount : 0;

        //	var obj = new SelectList(_vendorService.GetVendorsForDropDown(), "value", "text");
        //	ViewBag.Vendor = obj;
        //	//ViewBag.VendorID = _vendorService.GetVendorsForDropDown().ToList();

        //	return Json(new
        //	{
        //		draw = model.draw,
        //		recordsTotal = totalResultsCount,
        //		recordsFiltered = filteredResultsCount,
        //		data = Projects
        //	});
        //}

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data.Project project = _projectService.GetProject((Int16)id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        //public ActionResult ApproveDetails(long? id)
        //{
        //	if (id == null)
        //	{
        //		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //	}
        //	Data.Project project = _projectService.GetProject((Int16)id);
        //	ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
        //	ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text", project.BrandID);
        //	if (project == null)
        //	{
        //		return HttpNotFound();
        //	}
        //	return View(project);
        //}
        public ActionResult Create(long? id)
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

            //ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
            //ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text");
            //TempData["ProjectID"] = id;
            return View(project);
        }

        public ActionResult QuickCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuickCreate(Data.Project project)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                project.Slug = Slugify.GenerateSlug(project.Name);

                if (_projectService.Create(project, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created project {project.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Project/Edit/" + project.ID,
                        message = message
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
            Data.Project project = _projectService.GetProject((long)id);
            if (project == null)
            {
                return HttpNotFound();
            }

            //ViewBag.AttributeID = new SelectList(_attributeService.GetAttributesForDropDown(), "value", "text");
            //ViewBag.BrandID = new SelectList(_brandService.GetBrandsForDropDown(), "value", "text", project.BrandID);
            ViewBag.ProjectTypeID = new SelectList(_projectTypeService.GetProjectTypeForDropDown(), "value", "text", project.ProjectTypeID);
            return View(project);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Data.Project project)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                project.Slug = Slugify.GenerateSlug(project.Name);
                if (_projectService.UpdateProject(ref project, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated project {project.Name}.");
                    return Json(new
                    {
                        success = true,
                        //url = "/Admin/Project/Index",
                        message = "Project updated successfully ...",
                        data = new
                        {
                            ID = project.ID,
                            Date = project.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                            Name = project.Name,
                            ProjectType = project.ProjectType != null ? project.ProjectType.Name : "-",
                            IsActive = project.IsActive.HasValue ? project.IsActive.Value.ToString() : bool.FalseString
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

        public ActionResult Thumbnail(long? id)
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
                string filePath = !string.IsNullOrEmpty(project.Thumbnail) ? project.Thumbnail : string.Empty;


                string message = string.Empty;

                string absolutePath = Server.MapPath("~");
                string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/", project.Name.Replace(" ", "_"));

                project.Thumbnail = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Thumbnail", ref message, "Image");

                if (_projectService.UpdateProject(ref project, ref message, false))
                {

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        System.IO.File.Delete(Server.MapPath(filePath));
                    }
                    return Json(new
                    {
                        success = true,
                        message = message,
                        data = project.Thumbnail
                    });
                }
                return Json(new { success = false, message = message });
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

        public ActionResult Delete(long? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Data.Project project = _projectService.GetProject((Int16)id);
            if (project == null)
            {
                return HttpNotFound();
            }
            TempData["ProjectID"] = id;
            return View(project);
        }

        //public ActionResult Publish(long? id)
        //{
        //	if (id == null)
        //	{
        //		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //	}
        //	var project = _projectService.GetProject((long)id);
        //	if (project == null)
        //	{
        //		return HttpNotFound();
        //	}
        //	string message = string.Empty;
        //	if (_projectService.UpdateProject(ref project, ref message))
        //	{
        //		SuccessMessage = "Project " + (project.IsPublished.Value ? "Published" : "Unpublished") + "  successfully ...";
        //              log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {(project.IsPublished.Value ? "Published" : "Unpublished")} project {project.Name}.");
        //		return Json(new
        //		{
        //			success = true,
        //			message = SuccessMessage,
        //			data = new
        //			{
        //				IsPublished = project.IsPublished.HasValue ? project.IsPublished.Value.ToString() : bool.FalseString,
        //				ID = project.ID
        //			}
        //		}, JsonRequestBehavior.AllowGet);
        //	}
        //	else
        //	{
        //		ErrorMessage = "Oops! Something went wrong. Please try later.";
        //	}

        //	return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Data.Project project = _projectService.GetProject((Int16)id);
            string message = string.Empty;
            if (_projectService.DeleteProject(id, ref message, false, true))
            {
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted project {project.Name}.");
                return Json(new
                {
                    success = true,
                    message = message

                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BulkUpload()
        {

            return View();

        }

        //[HttpPost]
        //public ActionResult BulkUpload(HttpPostedFileBase FileUpload, string connectionId)
        //{
        //	try
        //	{

        //		if (FileUpload != null)
        //		{
        //			if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //			{
        //				string filename = FileUpload.FileName;

        //				if (filename.EndsWith(".xlsx"))
        //				{
        //					string targetpath = Server.MapPath("~/assets/AppFiles/Documents/ExcelFiles");
        //					FileUpload.SaveAs(targetpath + filename);
        //					string pathToExcelFile = targetpath + filename;

        //					string sheetName = "BulkProject";
        //					long VendorID = (long)Session["VendorID"];


        //					string data = "";
        //					string message = "";
        //					List<string> ErrorItems = new List<string>();
        //					int count = 0;
        //					var total = 0;
        //					int successCount = 0;



        //					var excelFile = new ExcelQueryFactory(pathToExcelFile);
        //					IList<ProjectWorkSheet> Project = (from a in excelFile.Worksheet<ProjectWorkSheet>(sheetName) select a).ToList();
        //					total = MyProject.Count();
        //					//if (MyProject.Count() != MyProject.Select(i => i.Slug).Distinct().Count())
        //					//{
        //					//	return Json(new
        //					//	{
        //					//		success = false,
        //					//		message = "All Projects Must Contain"
        //					//	}, JsonRequestBehavior.AllowGet);
        //					//}

        //					foreach (ProjectWorkSheet item in Project)
        //					{
        //						try
        //						{
        //							var results = new List<ValidationResult>();
        //							var context = new ValidationContext(item, null, null);
        //							if (Validator.TryValidateObject(item, context, results))
        //							{
        //								if (string.IsNullOrEmpty(item.VariantName))
        //								{
        //									/* IF PRODUCT IS SIMPLE*/

        //									/*Upload Project Thumnail*/
        //									string filePath = string.Empty;
        //									if (!string.IsNullOrEmpty(item.Thumbnail))
        //									{
        //										filePath = item.Thumbnail;

        //										message = string.Empty;

        //										string absolutePath = Server.MapPath("~");
        //										string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/", item.Name.Replace(" ", "_"));

        //										item.Thumbnail = Uploader.SaveImage(item.Thumbnail, absolutePath, relativePath, "PTI", ImageFormat.Jpeg);

        //									}
        //									if (!string.IsNullOrEmpty(item.ProjectTags))
        //									{
        //										item.ProjectTags = item.ProjectTags.Replace("&", "&amp;");
        //									}

        //									if (!string.IsNullOrEmpty(item.ProjectCategories))
        //									{
        //										item.ProjectCategories = item.ProjectCategories.Replace("&", "&amp;");
        //									}

        //									if (_projectService.PostExcelData(VendorID
        //																	, item.Brand
        //																	, item.Name
        //																	, string.IsNullOrEmpty(item.Slug) ? Slugify.GenerateSlug(item.Name + "-" + item.Name) : item.Slug
        //																	, item.Name
        //																	, item.Thumbnail
        //																	, item.ShortDescription
        //																	, item.Description
        //																	, item.MobileDescription
        //																	, item.RegularPrice
        //																	, item.SalePrice
        //																	, item.SalePriceFrom
        //																	, item.SalePriceTo
        //																	, item.Stock
        //																	, item.Threshold
        //																	, item.Type == "Simple" ? "1" : "2"
        //																	, item.Weight
        //																	, item.Length
        //																	, item.Width
        //																	, item.Height
        //																	, item.IsSoldIndividually == "Yes" ? true : false
        //																	, item.AllowMultipleOrder == "Yes" ? true : false
        //																	, item.StockStatus == "In Stock" ? 1 : 2
        //																	, item.IsFeatured == "Yes" ? true : false
        //																	, item.IsRecommended == "Yes" ? true : false
        //																	, item.IsManageStock == "Yes" ? true : false
        //																	, item.ProjectCategories
        //																	, item.ProjectTags))
        //									{
        //										Data.Project project = _projectService.GetProjectbyName(item.Name, VendorID);
        //										if (item.Attributes != null)
        //										{

        //											string[] values = item.Attributes.Split(',');
        //											bool VariationUsage;
        //											if (item.Type == "Simple")
        //											{
        //												VariationUsage = false;
        //											}
        //											else
        //											{
        //												VariationUsage = true;
        //											}
        //											for (int i = 0; i < values.Count(); i++)
        //											{
        //												string[] val1 = values[i].Split(':');
        //												if (val1 != null)
        //												{
        //													var result = _projectAttributeService.PostExcelData(project.ID, val1[0], val1[1], VariationUsage);
        //												}
        //											}

        //										}

        //										/*Uploading Project Images*/
        //										if (!string.IsNullOrEmpty(item.Images))
        //										{
        //											message = string.Empty;
        //											string absolutePath = Server.MapPath("~");
        //											string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/Gallery/", project.Name.Replace(" ", "_"));

        //											List<string> Pictures = new List<string>();

        //											Uploader.SaveImages(item.Images.Split(','), absolutePath, relativePath, "PGI", ImageFormat.Jpeg, ref Pictures);
        //											var imageCount = 0;
        //											foreach (var image in Pictures)
        //											{
        //												ProjectImage projectImage = new ProjectImage();
        //												projectImage.ProjectID = project.ID;
        //												projectImage.Image = image;
        //												projectImage.Position = ++imageCount;
        //												if (!_projectImageService.CreateProjectImage(ref projectImage, ref message))
        //												{
        //												}
        //											}
        //										}

        //										project = null;
        //										successCount++;
        //									}
        //									else
        //									{
        //										ErrorItems.Add(string.Format("Row Number {0} Not Inserted.<br>", count));

        //										if (!string.IsNullOrEmpty(item.Thumbnail))
        //										{
        //											System.IO.File.Delete(Server.MapPath(item.Thumbnail));
        //										}
        //									}
        //								}
        //								else
        //								{
        //									/* IF Project is VARIATION*/
        //									Data.Project project = _projectService.GetProjectbyName(item.Name, VendorID);
        //									if (project != null)
        //									{
        //										if (item.Type == "Variable" && item.VariantName != null)
        //										{
        //											/*Upload Project Variation Thumnail*/
        //											string filePath = string.Empty;
        //											if (!string.IsNullOrEmpty(item.VariantThumbnail))
        //											{
        //												filePath = item.VariantThumbnail;

        //												message = string.Empty;

        //												string absolutePath = Server.MapPath("~");
        //												string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/Variations/{1}/", project.Name.Replace(" ", "_"), item.VariantName.Replace(" ", "_"));

        //												item.VariantThumbnail = Uploader.SaveImage(item.VariantThumbnail, absolutePath, relativePath, "PVTI", ImageFormat.Jpeg);

        //											}

        //											_projectVariationService.PostExcelData(project.ID, item.VariantRegularPrice, item.VariantSalePrice, item.VariantSalePriceFrom, item.VariantSalePriceTo, item.VariantStock, item.VariantThreshold, item.VariantStockStatus == "In Stock" ? 1 : 2, item.VariantName, item.VariantThumbnail, item.VariantWeight, item.VariantLength, item.VariantWidth, item.VariantHeight, item.VariantDescription, item.VariantSoldIndividually == "Yes" ? true : false, item.VariantIsManageStock == "Yes" ? true : false);

        //										}

        //										ProjectVariation objProjectVariation = _projectVariationService.GetProjectbyName(item.VariantName);

        //										/*Creating Project Variation Attributes Images*/
        //										if (item.VariantAttributes != null)
        //										{
        //											string[] values = item.VariantAttributes.Split(',');
        //											for (int i = 0; i < values.Count(); i++)
        //											{
        //												string[] val1 = values[i].Split(':');
        //												if (val1 != null && val1.Count() > 1)
        //												{
        //													var result = _projectVariationAttributeService.PostExcelData(project.ID, val1[0], val1[1], objProjectVariation.ID);
        //												}
        //											}
        //										}

        //										/*Uploading Project Variation Images*/
        //										if (!string.IsNullOrEmpty(item.VariantImages))
        //										{
        //											message = string.Empty;
        //											string absolutePath = Server.MapPath("~");

        //											string relativePath = string.Format("/Assets/AppFiles/Images/Project/{0}/Variations/{1}/Gallery/", project.Name.Replace(" ", "_"), item.VariantName.Replace(" ", "_"));

        //											List<string> Pictures = new List<string>();

        //											Uploader.SaveImages(item.VariantImages.Split(','), absolutePath, relativePath, "PVGI", ImageFormat.Jpeg, ref Pictures);
        //											var imageCount = 0;
        //											foreach (var image in Pictures)
        //											{
        //												ProjectVariationImage projectVariationImage = new ProjectVariationImage();
        //												projectVariationImage.ProjectID = project.ID;
        //												projectVariationImage.ProjectVariationID = objProjectVariation.ID;
        //												projectVariationImage.Image = image;
        //												projectVariationImage.Position = ++imageCount;
        //												if (_projectVariationImageService.CreateProjectVariationImage(ref projectVariationImage, ref message))
        //												{
        //												}
        //											}
        //										}

        //										objProjectVariation = null;
        //										successCount++;
        //									}
        //									else
        //									{
        //										ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>Project not found, Please add project row first without variant", count));
        //									}
        //								}
        //							}
        //							else
        //							{
        //								ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>{1}",
        //									count,
        //									string.Join<string>("<br>", results.Select(i => i.ErrorMessage).ToList())));
        //							}
        //							count++;
        //						}
        //						catch (Exception ex)
        //						{
        //							ErrorItems.Add(string.Format("<b>Row Number {0} Not Inserted:</b><br>Internal Server Serror", count));
        //						}

        //						//CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
        //						Functions.SendProgress(connectionId, "Upload in progress...", count, total);
        //					}

        //					return Json(new
        //					{
        //						success = true,
        //						successMessage = string.Format("{0} Projects uploaded!", (successCount)),
        //						errorMessage = (ErrorItems.Count() > 0) ? string.Format("{0} Projects are not uploaded!", total - successCount) : null,
        //						detailedErrorMessages = (ErrorItems.Count() > 0) ? string.Join<string>("<br>", ErrorItems) : null,
        //					}, JsonRequestBehavior.AllowGet);
        //				}
        //				else
        //				{
        //					return Json(new
        //					{
        //						success = false,
        //						message = "Invalid file format, Only .xlsx format is allowed"
        //					}, JsonRequestBehavior.AllowGet);
        //				}
        //			}
        //			else
        //			{
        //				return Json(new
        //				{
        //					success = false,
        //					message = "Invalid file format, Only Excel file is allowed"
        //				}, JsonRequestBehavior.AllowGet);
        //			}
        //		}
        //		else
        //		{

        //			return Json(new
        //			{
        //				success = false,
        //				message = "Please upload Excel file first"
        //			}, JsonRequestBehavior.AllowGet);
        //		}

        //	}
        //	catch (Exception ex)
        //	{
        //		return Json(new
        //		{
        //			success = false,
        //			message = "Oops! Something went wrong. Please try later."
        //		}, JsonRequestBehavior.AllowGet);
        //	}
        //}

        #endregion

        #region Approval

        public ActionResult Approvals()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        public ActionResult ApprovalsList()
        {
            var projects = _projectService.GetProjects();
            return PartialView(projects);
        }

        //[HttpGet]
        //public ActionResult Approve(long id, bool approvalStatus)
        //{
        //	ViewBag.BuildingID = id;
        //	ViewBag.ApprovalStatus = approvalStatus;

        //	var project = _projectService.GetProject((long)id);
        //	project.IsApproved = approvalStatus;

        //	return View(project);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Approve(ProjectApprovalFormViewModel projectApprovalFormViewModel)
        //{

        //	var project = _projectService.GetProject(projectApprovalFormViewModel.ID);
        //	if (project == null)
        //	{
        //		return HttpNotFound();
        //	}

        //	if (projectApprovalFormViewModel.IsApproved)
        //	{
        //		project.IsApproved = true;
        //		project.IsActive = true;
        //		project.ApprovalStatus = (int)ProjectApprovalStatus.Approved;
        //	}
        //	else
        //	{
        //		project.IsApproved = false;
        //		project.IsActive = false;
        //		project.ApprovalStatus = (int)ProjectApprovalStatus.Rejected;
        //	}

        //	project.Remarks = project.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + projectApprovalFormViewModel.Remarks;

        //	string message = string.Empty;

        //	if (_projectService.UpdateProject(ref project, ref message, false))
        //	{
        //		SuccessMessage = "Project " + ((bool)project.IsActive ? "approved" : "rejected") + "  successfully ...";
        //		var vendor = _vendorService.GetVendor((long)project.VendorID);

        //		Notification not = new Notification();
        //		not.Title = "Project Approval";
        //		not.TitleAr = "الموافقة على المنتج";
        //		if (project.ApprovalStatus == 3)
        //		{
        //			not.Description = "Your project " + project.Name + " have been approved ";
        //			not.Url = "/Vendor/Project/Index";
        //		}
        //		else
        //		{
        //			not.Description = "Your " + project.Name + " have been rejected ";
        //			not.Url = "/Vendor/Project/ApprovalIndex";
        //		}
        //		not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
        //		not.OriginatorName = Session["UserName"].ToString();
        //		not.Module = "Project";
        //		not.OriginatorType = "Admin";
        //		not.RecordID = project.ID;
        //		if (_notificationService.CreateNotification(not, ref message))
        //		{
        //			NotificationReceiver notRec = new NotificationReceiver();
        //			notRec.ReceiverID = project.VendorID;
        //			notRec.ReceiverType = "Vendor";
        //			notRec.NotificationID = not.ID;
        //			if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
        //			{

        //			}

        //		}


        //              log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {(project.IsApproved.Value ? "Approved" : "Rejected")} project {project.Name}.");
        //		return Json(new
        //		{

        //			success = true,
        //			message = SuccessMessage,
        //			data = new
        //			{
        //				ID = project.ID,
        //				Date = project.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
        //				Vendor = vendor.Logo + "|" + vendor.VendorCode + "|" + vendor.Name,
        //				Project = project.Name + "|" + project.Name,
        //				ApprovalStatus = project.ApprovalStatus,
        //				IsActive = project.IsActive.HasValue ? project.IsActive.Value.ToString() : bool.FalseString,
        //				IsApproved = project.IsApproved.HasValue ? project.IsApproved.Value.ToString() : bool.FalseString
        //			}
        //		}, JsonRequestBehavior.AllowGet);
        //	}
        //	else
        //	{
        //		ErrorMessage = message;
        //	}

        //	return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]		
        //public ActionResult ApproveAll(List<long> ids)
        //{
        //	foreach (var items in ids)
        //	{
        //		var project = _projectService.GetProject(items);

        //		project.IsApproved = true;
        //		project.IsActive = true;
        //		project.ApprovalStatus = (int)ProjectApprovalStatus.Approved;


        //		//project.Remarks = project.Remarks + "<hr />" + Helpers.TimeZone.GetLocalDateTime().ToString("dd MMM yyyy, h:mm tt") + "<br />" + projectApprovalFormViewModel.Remarks;

        //		string message = string.Empty;

        //		if (_projectService.UpdateProject(ref project, ref message, false))
        //		{
        //			SuccessMessage = "Project " + ((bool)project.IsActive ? "Approved" : "Rejected") + "  successfully ...";
        //			var vendor = _vendorService.GetVendor((long)project.VendorID);
        //			Notification not = new Notification();
        //			not.Title = "Project Approval";
        //			not.TitleAr = "الموافقة على المنتج";
        //			if (project.ApprovalStatus == 3)
        //			{
        //				not.Description = "Your project " + project.Name + " have been approved ";
        //				not.Url = "/Vendor/Project/Index";
        //			}
        //			else
        //			{
        //				not.Description = "Your " + project.Name + " have been rejected ";
        //				not.Url = "/Vendor/Project/ApprovalIndex";
        //			}
        //			not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
        //			not.OriginatorName = Session["UserName"].ToString();
        //			not.Module = "Project";
        //			not.OriginatorType = "Admin";
        //			not.RecordID = project.ID;
        //			if (_notificationService.CreateNotification(not, ref message))
        //			{
        //				NotificationReceiver notRec = new NotificationReceiver();
        //				notRec.ReceiverID = project.VendorID;
        //				notRec.ReceiverType = "Vendor";
        //				notRec.NotificationID = not.ID;
        //				if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
        //				{

        //				}

        //			}
        //			else
        //			{
        //				ErrorMessage = "Oops! Something went wrong. Please try later.";
        //			}
        //		}
        //	log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} approved all project ");
        //	}
        //	// return RedirectToAction("Index");
        //	return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        //}

        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _projectService.GetProject((long)id);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (!(bool)project.IsActive)
                project.IsActive = true;

            else
            {
                project.IsActive = false;
            }
            string message = string.Empty;
            if (_projectService.UpdateProject(ref project, ref message))
            {
                SuccessMessage = "Project " + ((bool)project.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)project.IsActive ? "activated" : "deactivated")} project {project.Name}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        Date = project.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
                        ID = project.ID,
                        Name = project.Name,
                        ProjectType = project.ProjectType != null ? project.ProjectType.Name : "-",
                        IsActive = project.IsActive.HasValue ? project.IsActive.Value.ToString() : bool.FalseString
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later.";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reports

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ApprovalReport()
        //{
        //	var getAllApprovals = _projectService.GetProjects().ToList();
        //	if (getAllApprovals.Count() > 0)
        //	{
        //		using (ExcelPackage excel = new ExcelPackage())
        //		{
        //			excel.Workbook.Worksheets.Add("ApprovalReport");

        //			var headerRow = new List<string[]>()
        //			{
        //			new string[] {
        //				"Creation Date"
        //				,"Vendor Code"
        //				,"Vendor Name"
        //				,"Project Name"
        //				,"Status"
        //				}
        //			};

        //			// Determine the header range (e.g. A1:D1)
        //			string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

        //			// Target a worksheet
        //			var worksheet = excel.Workbook.Worksheets["ApprovalReport"];

        //			// Popular header row data
        //			worksheet.Cells[headerRange].LoadFromArrays(headerRow);

        //			var cellData = new List<object[]>();

        //			foreach (var i in getAllApprovals)
        //			{
        //				cellData.Add(new object[] {
        //				i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
        //				,!string.IsNullOrEmpty(i.Vendor.VendorCode) ? i.Vendor.VendorCode : "-"
        //				,!string.IsNullOrEmpty(i.Vendor.Name) ? i.Vendor.Name : "-"
        //				,!string.IsNullOrEmpty(i.Name) ? i.Name : "-"
        //				,i.ApprovalStatus != null ? ((ProjectApprovalStatus)i.ApprovalStatus).ToString() : "-"
        //				});
        //			}

        //			worksheet.Cells[2, 1].LoadFromArrays(cellData);

        //			return File(excel.GetAsByteArray(), "application/msexcel", "Projects Approval Report.xlsx");
        //		}
        //	}
        //	return RedirectToAction("Approvals");
        //}

        [HttpPost]
        public ActionResult ProjectsReport()
        {
            if (true)
            {

                var getAllProjects = _projectService.GetProjects();

                if (getAllProjects.Count() > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("ProjectsReport");

                        var headerRow = new List<string[]>()
                    {

                        new string[] {
                        "Creation Date"
                        ,"Name"
                        ,"ShortDescription"
                        ,"Description"
                        ,"Type"
                        ,"IsActive"
                        }
                    };

                        // Determine the header range (e.g. A1:D1)
                        string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        // Target a worksheet
                        var worksheet = excel.Workbook.Worksheets["ProjectsReport"];

                        // Popular header row data
                        worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        if (getAllProjects.Count() != 0)
                            getAllProjects = getAllProjects.OrderByDescending(x => x.ID).ToList();


                        foreach (var i in getAllProjects)
                        {
                            cellData.Add(new object[] {
                            i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,!string.IsNullOrEmpty(i.Name) ? i.Name: "-"
                            ,!string.IsNullOrEmpty(i.NameAr) ? i.NameAr: "-"
                            ,!string.IsNullOrEmpty(i.ShortDescription) ? i.ShortDescription: "-"
                            ,!string.IsNullOrEmpty(i.Description) ? i.Description: "-"
                            ,!string.IsNullOrEmpty(i.ProjectType.Name) ? i.ProjectType.Name: "-"
                            ,i.IsActive == true ? "Active" : "InActive"
                            });
                        }

                        worksheet.Cells[2, 1].LoadFromArrays(cellData);

                        return File(excel.GetAsByteArray(), "application/msexcel", "Project Report.xlsx");
                    }
                }
            }
            return RedirectToAction("Index");
        }


        #endregion
    }
}