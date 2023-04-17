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
    public class PropertyFeaturesController : Controller
    {
        private readonly IPropertyFeaturesService propertyFeaturesService;
        private readonly IPropertyService propertyService;
        private readonly IFeatureService featureService;



        public PropertyFeaturesController(IPropertyFeaturesService propertyFeaturesService, IPropertyService propertyService, IFeatureService featureService)
        {
            this.propertyFeaturesService = propertyFeaturesService;
            this.propertyService = propertyService;
            this.featureService = featureService;
        }
        // GET: Admin/Newsfeed
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        // GET: Admin/CommunityFeatures
        public ActionResult Index(long propertyid)
        {

            ViewBag.PropertyID = propertyid;
            return View();
        }
        // GET: Admin/CommunityFeatures/List
        public ActionResult List(int propertyid)
        {


            Property property = this.propertyService.GetPropertyFeatures(propertyid);
            return PartialView(property);

        }

        // GET: Admin/CommunityFeatures/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyFeature data = this.propertyFeaturesService.GetPropertyFeaturesByID(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.FeatureID = new SelectList(this.featureService.GetFeatureForDropDown(), "value", "text", data.FeatureID);

            return View(data);
        }

        // GET: Admin/CommunityFeatures/Create
        public ActionResult Create(long propertyid)
        {
            TempData["PropertyID"] = propertyid;

            ViewBag.FeatureID = new SelectList(this.featureService.GetFeatureForDropDown(), "value", "text");
            //   var propertyFeature = this.propertyFeaturesService.GetPropertyFeatures(propertyid);

            return PartialView();
        }

        // POST: Admin/CommunityFeatures/Create
        [HttpPost]
        public ActionResult Create(PropertyFeature propertyFeature)
        {
            string message = String.Empty;
            if (ModelState.IsValid)
            {
                if (propertyFeature.Cover != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = "/Assets/AppFiles/PropertyFeatures/Cover/";
                    propertyFeature.Cover = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Cover", ref message, "Cover");
                }
                if (this.propertyFeaturesService.Create(propertyFeature, ref message))
                {
                    var feature = this.featureService.GetFeatureByID((long)propertyFeature.FeatureID);

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/FeatureProperty/Index",
                        message = message,
                        data = new
                        {
                            FeatureImage = feature.Image,
                            FeatureName = feature.Name,
                            Poisition = propertyFeature.Position,
                            ID = propertyFeature.ID
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

        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyFeature data = this.propertyFeaturesService.GetPropertyFeaturesByID(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.FeatureID = new SelectList(this.featureService.GetFeatureForDropDown(), "value", "text", data.FeatureID);

            return View(data);
        }


        [HttpPost]
        public ActionResult Update(PropertyFeature propertyFeature)
        {
            string message = String.Empty;
            if (ModelState.IsValid)
            {
                var db = this.propertyFeaturesService.GetPropertyFeaturesByID(propertyFeature.ID);
                //db.Cover = propertyFeature.Cover;
                db.Description = propertyFeature.Description;
                db.DescriptionAr = propertyFeature.DescriptionAr;
                db.Position = propertyFeature.Position;
                db.FeatureID = propertyFeature.FeatureID;
                if (propertyFeature.Cover != null)
                {
                    string absolutePath = Server.MapPath("~");
                    if (System.IO.File.Exists(absolutePath + db.Cover))
                    {
                        System.IO.File.Delete(absolutePath + db.Cover);
                    }

                    string relativePath = "/Assets/AppFiles/PropertyFeatures/Cover/";
                    db.Cover = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Cover", ref message, "Cover");
                }

                if (this.propertyFeaturesService.UpdateProperyFeatures(ref db, ref message, true))
                {
                    var feature = this.featureService.GetFeatureByID((long)propertyFeature.FeatureID);

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/FeatureProperty/Index",
                        message = message,
                        data = new
                        {
                            FeatureImage = feature.Image,
                            FeatureName = feature.Name,
                            Poisition = propertyFeature.Position,
                            ID = propertyFeature.ID
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

        // GET: Admin/CommunityFeatures/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/CommunityFeatures/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, bool softDelete = true)
        {
            PropertyFeature propfeature = this.propertyFeaturesService.GetPropertyFeaturesByID(id);
            string message = string.Empty;
            bool hasChilds = true;

            //soft delete of data updating delete column
            if (this.propertyFeaturesService.DeletePropertyFeatures((Int16)id, ref message, ref hasChilds, softDelete))
            {
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);




        }
    }
}
