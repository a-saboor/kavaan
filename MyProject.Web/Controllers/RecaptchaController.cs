using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProject.Web.ViewModels.Recaptcha;

namespace MyProject.Web.Controllers{
    public class RecaptchaController : Controller
    {
        public RecaptchaController()
        {
        }

        [HttpPost]

        public ActionResult Validate(RecaptchaViewModel recaptcha)
        {
            try
            {
                string response = string.Empty;
                string message = "";
                string Secret = "6LcSzjogAAAAAFxETYlAHgBH4po0gq4rbeSHnBPP";
                string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + Secret + "&response=" + recaptcha.response;
                string result = (new WebClient()).DownloadString(url);

                return Json(new
                {
                    success = true,
                    result = result
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ""
                });
            }
        }

    }
}