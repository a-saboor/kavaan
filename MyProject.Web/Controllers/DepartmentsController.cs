using MyProject.Service;
using MyProject.Web.Helpers.Routing;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Controllers{
    public class DepartmentsController : Controller
    {

        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly ICategoryService _categoryService;

        public DepartmentsController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

		[Route("careers/departments", Name = "careers/departments")]
		//[Route("careers/departments")]
		public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
		[Route("departments", Name = "departments")]
		//[Route("departments")]
        public ActionResult GetAll(string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            try
            {
                string ImageServer = "";
                var departments = _categoryService.GetCategories(ImageServer, lang);

                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = departments
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
    }
}