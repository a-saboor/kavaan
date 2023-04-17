using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using MyProject.Web.Areas.Admin.ViewModels.Banners;
using MyProject.Web.Helpers.POCO;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml;
using System.IO;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class BannerController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            this._bannerService = bannerService;
        }

        public ActionResult Manage()
        {
            BannersViewModel objBannersViewModel = new BannersViewModel()
            {
                VideoBanners = _bannerService.GetBannersByContentType("Video").OrderByDescending(x => x.Type).ToList(),
                ImageBanners = _bannerService.GetBannersByContentType("Image").OrderByDescending(x => x.Type).ToList()
            };

            ViewBag.Message = TempData["Message"];

            return View(objBannersViewModel);
        }

        [HttpPost]
        public ActionResult Manage(HttpPostedFileBase file, string Url, string Lang, string ContentType, string Type, string Device)
        {
            string message = string.Empty;
            if (file != null && file.ContentLength > 0)
                try
                {
                    string newfilename = "";
                    if (ContentType == "Video")
                    {
                        var oldHeaderVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Header");
                        var oldCenteredVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Centered");
                        var oldFooterVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Footer");
                        var oldAboutVideo = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "About");
                        Banner objBanner = new Banner();

                        bool update = false;

                        if (Type == "Header" && oldHeaderVideo.Count() > 0)
                        {
                            objBanner = oldHeaderVideo.FirstOrDefault();
                            newfilename = oldHeaderVideo.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else if (Type == "Centered" && oldCenteredVideo.Count() > 0)
                        {
                            objBanner = oldCenteredVideo.FirstOrDefault();
                            newfilename = oldCenteredVideo.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else if (Type == "Footer" && oldFooterVideo.Count() > 0)
                        {
                            objBanner = oldFooterVideo.FirstOrDefault();
                            newfilename = oldFooterVideo.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else if (Type == "About" && oldAboutVideo.Count() > 0)
                        {
                            objBanner = oldAboutVideo.FirstOrDefault();
                            newfilename = oldAboutVideo.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else
                        {
                            newfilename = "web-banner-video-" + Guid.NewGuid().ToString() + ".mp4";
                        }

                        string path = Path.Combine(Server.MapPath("~/Assets/AppFiles/Banners/"));

                        Path.GetFileName(file.FileName);
                        file.SaveAs(path + newfilename);

                        objBanner.Path = "/Assets/AppFiles/Banners/" + newfilename;
                        objBanner.Url = Url;
                        objBanner.Language = Lang;
                        objBanner.ContentType = ContentType;
                        objBanner.Type = Type;
                        objBanner.Device = Device;

                        if (update)
                        {
                            if (_bannerService.UpdateBanner(ref objBanner, ref message))
                            {
                                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} banner video updated {objBanner.Type} {objBanner.ContentType}.");
                            }
                        }
                        else
                        {
                            if (_bannerService.CreateBanner(objBanner, ref message))
                            {
                                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} banner video created {objBanner.Type} {objBanner.ContentType}.");
                            }
                        }

                        TempData["Message"] = "Video uploaded successfully";
                    }
                    else
                    {
                        var oldCenteredImage = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Centered");
                        var oldFooterImage = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Footer");
                        var oldAboutImage = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "About");

                        Banner objBanner = new Banner();

                        bool update = false;

                        if (Type == "Centered" && oldCenteredImage.Count() > 0)
                        {
                            objBanner = oldCenteredImage.FirstOrDefault();
                            newfilename = oldCenteredImage.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else if (Type == "Footer" && oldFooterImage.Count() > 0)
                        {
                            objBanner = oldFooterImage.FirstOrDefault();
                            newfilename = oldFooterImage.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else if (Type == "About" && oldAboutImage.Count() > 0)
                        {
                            objBanner = oldAboutImage.FirstOrDefault();
                            newfilename = oldAboutImage.FirstOrDefault().Path.Replace("/Assets/AppFiles/Banners/", "");
                            update = true;
                        }
                        else
                        {
                            newfilename = "web-banner-image-" + Guid.NewGuid().ToString() + ".jpg";
                        }

                        string path = Path.Combine(Server.MapPath("~/Assets/AppFiles/Banners/"));

                        // file.FileName = newfilename;

                        Path.GetFileName(file.FileName);
                        file.SaveAs(path + newfilename);
                        objBanner.Path = "/Assets/AppFiles/Banners/" + newfilename;
                        objBanner.Url = Url;
                        objBanner.Language = Lang;
                        objBanner.ContentType = ContentType;
                        objBanner.Type = Type;
                        objBanner.Device = Device;

                        if (update)
                        {
                            if (_bannerService.UpdateBanner(ref objBanner, ref message))
                            {
                                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} banner image updated {objBanner.Type} {objBanner.ContentType}.");
                            }
                        }
                        else
                        {
                            if (_bannerService.CreateBanner(objBanner, ref message))
                            {
                                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} banner image created {objBanner.Type} {objBanner.ContentType}.");
                            }
                        }
                      
                        TempData["Message"] = "Image uploaded successfully";
                    }

                }
                catch (Exception ex)
                {
                    TempData["Message"] = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                TempData["Message"] = "You have not specified a file.";
            }
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult ManageImageForMobile(HttpPostedFileBase file, string Url, string Lang)
        {
            string message = string.Empty;
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Assets/AppFiles/MobileBanners/"));
                    string newfilename = "Mob-banner-" + Guid.NewGuid().ToString() + ".jpg";
                    // file.FileName = newfilename;


                    Path.GetFileName(file.FileName);
                    file.SaveAs(path + newfilename);
                    Banner obj = new Banner();
                    obj.Path = "/Assets/AppFiles/MobileBanners/" + newfilename;
                    obj.Url = Url;
                    obj.ContentType = "Image";
                    obj.Device = "Mobile";
                    if (_bannerService.CreateBanner(obj, ref message))
                    {

                    }
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Manage");
        }

        public ActionResult Edit(long id)
        {
            Banner bannerData = _bannerService.GetBanner(id);
            return View(bannerData);
        }

        [HttpPost]
        public ActionResult Edit(long id, string Url, string Lang)
        {
            try
            {
                string message = string.Empty;
                Banner objBanner = _bannerService.GetBanner(id);
                objBanner.Url = Url;
                objBanner.Language = Lang;
                if (_bannerService.UpdateBanner(ref objBanner, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        message = "Banner updated successfully...",
                        data = new { lang = objBanner.Language }
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

        public ActionResult EditMobileBanners(long id)
        {
            Banner bannerData = _bannerService.GetBanner(id);
            return View(bannerData);
        }

        [HttpPost]
        public ActionResult EditMobileBanners(long id, string Url, string Lang)
        {
            string message = string.Empty;
            Banner bannerData = _bannerService.GetBanner(id);
            bannerData.Language = Lang;
            bannerData.Url = Url;
            try
            {
                if (_bannerService.UpdateBanner(ref bannerData, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        data = new { lang = bannerData.Language, url = bannerData.Language },
                        message = "Banner updated successfully ..."
                    });
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

        public ActionResult Delete(long id)
        {

            try
            {
                string message = string.Empty;
                string absolutePath = Server.MapPath("~");

                Banner bannerData = _bannerService.GetBanner(id);
                if (bannerData != null)
                {
                    if (_bannerService.DeleteBanner(id, true))
                    {
                        //BannersViewModel objBannersViewModel = new BannersViewModel()
                        //{
                        //    VideoBanners = _bannerService.GetBannersByDevice("Website").ToList(),
                        //    ImageBanners = _bannerService.GetBannersByDevice("Mobile").ToList()
                        //};
                        //var WebsiteBannersCount = objBannersViewModel.VideoBanners.Count();
                        //var MobileBannersCount = objBannersViewModel.ImageBanners.Count();

                        //var WebsiteHeaderVideoCount = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Header").Count();
                        //var WebsiteHeaderImageCount = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Header").Count();
                        //var WebsiteFooterVideoCount = _bannerService.GetBannersByContentTypeAndDeviceAndType("Video", "Website", "Footer").Count();
                        //var WebsiteFooterImageCount = _bannerService.GetBannersByContentTypeAndDeviceAndType("Image", "Website", "Footer").Count();

                        //var WebsiteBannersVideoCount = WebsiteHeaderVideoCount + WebsiteFooterVideoCount;
                        //var WebsiteBannersImageCount = WebsiteHeaderImageCount + WebsiteFooterImageCount;
                        if (System.IO.File.Exists(absolutePath + bannerData.Path))
                        {
                            System.IO.File.Delete(absolutePath + bannerData.Path);
                        }
                        log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} banner deleted {bannerData.Type} {bannerData.ContentType}.");
                        return Json(new
                        {
                            //WebsiteBannersCount = WebsiteBannersCount,
                            //MobileBannersCount = MobileBannersCount,
                            //WebsiteBannersVideoCount = WebsiteBannersVideoCount,
                            //WebsiteBannersImageCount = WebsiteBannersImageCount,

                            //WebsiteHeaderVideoCount = WebsiteHeaderVideoCount,
                            //WebsiteHeaderImageCount = WebsiteHeaderImageCount,
                            //WebsiteFooterVideoCount = WebsiteFooterVideoCount,
                            //WebsiteFooterImageCount = WebsiteFooterImageCount,
                            success = true,
                            message = "Banner deleted successfully ..."
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

        public ActionResult Promotion()
        {
            BannersViewModel objBannersViewModel = new BannersViewModel()
            {
                PromotionBanners = _bannerService.GetBannersByDevice("Promotion").ToList(),
                ImageBanners = _bannerService.GetBannersByDevice("PromotionMobile").ToList()
            };

            return View(objBannersViewModel);
        }

        [HttpPost]
        public ActionResult Promotion(HttpPostedFileBase file, string Url, string Lang)
        {
            string message = string.Empty;
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Assets/AppFiles/Banners/"));
                    string newfilename = "web-banner-" + Guid.NewGuid().ToString() + ".jpg";

                    Banner objBanner = new Banner();
                    objBanner.Path = "/Assets/AppFiles/Banners/" + newfilename;
                    objBanner.Url = Url;
                    objBanner.Language = Lang;
                    objBanner.Device = "Promotion";

                    if (_bannerService.CreateBanner(objBanner, ref message))
                    {
                        Path.GetFileName(file.FileName);
                        file.SaveAs(path + newfilename);

                    }
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Promotion");
        }

        public ActionResult EditPromotionBanners(long id)
        {
            Banner bannerData = _bannerService.GetBanner(id);
            return View(bannerData);
        }

        [HttpPost]
        public ActionResult EditPromotionBanners(long id, string Url, string Lang)
        {
            try
            {
                string message = string.Empty;
                Banner objBanner = _bannerService.GetBanner(id);
                objBanner.Url = Url;
                objBanner.Language = Lang;
                if (_bannerService.UpdateBanner(ref objBanner, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        message = "Banner updated successfully...",
                        data = new { lang = objBanner.Language }
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

        public ActionResult DeletePromotionBanners(long id)
        {
            try
            {
                string message = string.Empty;
                string absolutePath = Server.MapPath("~");
                Banner bannerData = _bannerService.GetBanner(id);
                if (bannerData != null)
                {
                    if (_bannerService.DeleteBanner(id, true))
                    {
                        if (System.IO.File.Exists(absolutePath + bannerData.Path))
                        {
                            System.IO.File.Delete(absolutePath + bannerData.Path);
                        }
                        return Json(new
                        {
                            success = true,
                            message = "Banner deleted successfully ..."
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

        [HttpPost]
        public ActionResult PromotionImageForMobile(HttpPostedFileBase file, string Url, string Lang)
        {
            string message = string.Empty;
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Assets/AppFiles/MobileBanners/"));
                    string newfilename = "Mob-banner-" + Guid.NewGuid().ToString() + ".jpg";
                    // file.FileName = newfilename;


                    Path.GetFileName(file.FileName);
                    file.SaveAs(path + newfilename);
                    Banner obj = new Banner();
                    obj.Path = "/Assets/AppFiles/MobileBanners/" + newfilename;
                    obj.Url = Url;
                    obj.Language = Lang;
                    obj.Device = "PromotionMobile";
                    if (_bannerService.CreateBanner(obj, ref message))
                    {

                    }
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Promotion");
        }

        public ActionResult EditPromotionMobileBanners(long id)
        {
            Banner bannerData = _bannerService.GetBanner(id);
            return View(bannerData);
        }

        [HttpPost]
        public ActionResult EditPromotionMobileBanners(long id, string Lang)
        {
            string message = string.Empty;
            Banner bannerData = _bannerService.GetBanner(id);
            bannerData.Language = Lang;
            try
            {
                if (_bannerService.UpdateBanner(ref bannerData, ref message))
                {
                    return Json(new
                    {
                        success = true,
                        data = new { lang = bannerData.Language },
                        message = "Banner updated successfully ..."
                    });
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