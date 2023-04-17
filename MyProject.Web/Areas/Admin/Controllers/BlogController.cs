using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class BlogController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBlogService _newsFeedService;
        public BlogController(IBlogService newsFeedService)
        {
            this._newsFeedService = newsFeedService;
        }
        // GET: Admin/Newsfeed
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var Blog = _newsFeedService.GetBlog();
            return PartialView(Blog);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Create(Blog data, string Title)
        {
            string message = string.Empty;

            if (ModelState.IsValid)
            {
                Blog record = new Blog();
                record.Title = data.Title;
                record.TitleAr = data.TitleAr;
                record.Slug = Slugify.GenerateSlug(data.Title);
                record.TitleDescription = data.TitleDescription;
                record.TitleDescriptionAr = data.TitleDescriptionAr;
                record.Author = data.Author;
                record.PublishedOn = data.PublishedOn;
                record.TwitterUrl = data.TwitterUrl;
                record.FacebookURL = data.FacebookURL;
                record.InstagramURL = data.InstagramURL;
                record.LinkedinURL = data.LinkedinURL;
                record.Email = data.Email;
                record.IsFeatured = data.IsFeatured;
                record.Position = data.Position;
                record.Author = data.Author;
                record.PublishedOn = Helpers.TimeZone.GetLocalDateTime();
                record.IsDeleted = false;
                record.PostedDate = data.PostedDate;
                string replacelement = Title.Replace("?", "");

                string test = HttpContext.User.Identity.Name;
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        if (files.Count > 0)
                        {
                            string absolutePath = Server.MapPath("~");
                            if (data.BannerImage != null)
                            {
                                string relativePath = string.Format("/Assets/AppFiles/Blog/BannerImages/{0}/", replacelement.Replace(" ", "_"));
                                record.BannerImage = Uploader.UploadImage(files, absolutePath, relativePath, "BannerImage", ref message, "BannerImage");
                                if (!string.IsNullOrEmpty(record.BannerImage))
                                {
                                    record.BannerImage = CustomURL.GetImageServer() + record.BannerImage.Remove(0, 1);
                                }
                            }
                            if (data.Video != null)
                            {
                                string relativePath = string.Format("/Assets/AppFiles/Blog/Video/{0}/", replacelement.Replace(" ", "_"));
                                record.Video = Uploader.UploadVideo(files, absolutePath, relativePath, "Video", ref message, "Video");
                                if (!string.IsNullOrEmpty(record.Video))
                                {
                                    record.Video = CustomURL.GetImageServer() + record.Video.Remove(0, 1);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //add logs here...
                    }
                }

                if (_newsFeedService.CreateBlog(record, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created blog {record.Author}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Blog/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = record.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                            record.BannerImage,
                            Title = record.Title,
                            Author = record.Author,
                            IsActive = record.IsActive.ToString(),
                            ID = record.ID
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
            Blog feed = _newsFeedService.GetBlogByID((long)id);
            if (feed == null)
            {
                return HttpNotFound();
            }
            return View(feed);
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(Blog data, string Title)
        {
            string message = string.Empty;

            Blog updatefeed = _newsFeedService.GetBlogByID(data.ID);
            string previousImagepath = !string.IsNullOrEmpty(updatefeed.BannerImage) ? updatefeed.BannerImage.Replace(CustomURL.GetImageServer(), "") : updatefeed.BannerImage;
            string previousVideopath = !string.IsNullOrEmpty(updatefeed.Video) ? updatefeed.Video.Replace(CustomURL.GetImageServer(), "") : updatefeed.Video;
            string replacelement = Title.Replace("?", "");


            

            if (ModelState.IsValid)
            {
                updatefeed.Title = data.Title;
                updatefeed.TitleAr = data.TitleAr;
                updatefeed.Slug = Slugify.GenerateSlug(data.Title);
                //updatefeed.TitleTa = data.TitleTa;
                //updatefeed.TitleSi = data.TitleSi;
                updatefeed.TitleDescription = data.TitleDescription;
                updatefeed.TitleDescriptionAr = data.TitleDescriptionAr;
                updatefeed.Author = data.Author;
                updatefeed.TwitterUrl = data.TwitterUrl;
                updatefeed.FacebookURL = data.FacebookURL;
                updatefeed.InstagramURL = data.InstagramURL;
                updatefeed.LinkedinURL = data.LinkedinURL;
                updatefeed.Email = data.Email;
                updatefeed.IsFeatured = data.IsFeatured;
                updatefeed.Position = data.Position;
                updatefeed.Author = data.Author;
                updatefeed.PostedDate = data.PostedDate;
                updatefeed.LastUpdated = Helpers.TimeZone.GetLocalDateTime();

                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        if (files.Count > 0)
                        {
                            string absolutePath = Server.MapPath("~");
                            if (data.BannerImage != null && !data.BannerImage.Equals(CustomURL.GetImageServer() + previousImagepath))
                            {
                                string relativePath = string.Format("/Assets/AppFiles/Blog/BannerImages/{0}/", replacelement.Replace(" ", "_"));
                                updatefeed.BannerImage = Uploader.UploadImage(files, absolutePath, relativePath, "BannerImage", ref message, "BannerImage");
                                if (!string.IsNullOrEmpty(updatefeed.BannerImage))
                                {
                                    updatefeed.BannerImage = CustomURL.GetImageServer() + updatefeed.BannerImage.Remove(0, 1);
                                }
                                //delete old file
                                if (System.IO.File.Exists(previousImagepath))
                                {
                                    System.IO.File.Delete(previousImagepath);
                                }
                            }
                            if (data.Video != null && !data.Video.Equals(CustomURL.GetImageServer() + previousVideopath))
                            {
                                string relativePath = string.Format("/Assets/AppFiles/Blog/Video/{0}/", replacelement.Replace(" ", "_"));
                                updatefeed.Video = Uploader.UploadVideo(files, absolutePath, relativePath, "Video", ref message, "Video");
                                if (!string.IsNullOrEmpty(updatefeed.Video))
                                {
                                    updatefeed.Video = CustomURL.GetImageServer() + updatefeed.Video.Remove(0, 1);
                                }
                                //delete old file
                                if (System.IO.File.Exists(previousVideopath))
                                {
                                    System.IO.File.Delete(previousVideopath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //add logs here...
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(data.BannerImage))
                    {
                        updatefeed.BannerImage = "";
                        //delete old file
                        if (System.IO.File.Exists(previousImagepath))
                        {
                            System.IO.File.Delete(previousImagepath);
                        }
                    }
                    if (string.IsNullOrEmpty(data.Video))
                    {
                        updatefeed.Video = "";
                        //delete old file
                        if (System.IO.File.Exists(previousVideopath))
                        {
                            System.IO.File.Delete(previousVideopath);
                        }
                    }
                }
                if (_newsFeedService.UpdateBlog(ref updatefeed, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated blog {updatefeed.Author}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Blog/Index",
                        message = message,
                        data = new
                        {
                            CreatedOn = updatefeed.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                            updatefeed.BannerImage,
                            Title = updatefeed.Title,
                            Author = updatefeed.Author,
                            IsActive = updatefeed.IsActive.ToString(),
                            ID = updatefeed.ID
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
        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Feed = _newsFeedService.GetBlogByID((long)id);
            if (Feed == null)
            {
                return HttpNotFound();
            }

            if (!(bool)Feed.IsActive)
                Feed.IsActive = true;
            else
            {
                Feed.IsActive = false;
            }
            string message = string.Empty;
            if (_newsFeedService.UpdateBlog(ref Feed, ref message))
            {
                SuccessMessage = "Blog " + ((bool)Feed.IsActive ? "activated" : "deactivated") + "  successfully ...";
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)Feed.IsActive ? "activated" : "deactivated")} blog {Feed.Author}.");
                return Json(new
                {
                    success = true,
                    message = SuccessMessage,
                    data = new
                    {
                        CreatedOn = Feed.CreatedOn.Value.ToString(CustomHelper.GetDateFormat),
                        Feed.BannerImage,
                        Title = Feed.Title,
                        Author = Feed.Author,
                        IsActive = Feed.IsActive.HasValue ? Feed.IsActive.Value.ToString() : bool.FalseString,
                        ID = Feed.ID
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog Feed = _newsFeedService.GetBlogByID((Int16)id);

            if (Feed == null)
            {
                return HttpNotFound();
            }
            return View(Feed);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog Newsfeed = _newsFeedService.GetBlogByID((Int16)id);
            if (Newsfeed == null)
            {
                return HttpNotFound();
            }
            TempData["ID"] = id;
            return View(Newsfeed);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id, bool softDelete = true)
        {
            Blog Newsfeed = _newsFeedService.GetBlogByID((Int16)id);
            string message = string.Empty;
            if (softDelete)
            {
                //soft delete of data updating delete column
                if (_newsFeedService.DeleteBlog((Int16)id, ref message, softDelete))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted blog {Newsfeed.Author}.");
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                //permenant delete of data
                if (_newsFeedService.DeleteBlog((Int16)id, ref message, softDelete))
                {
                    if (Newsfeed.BannerImage != null)
                    {
                        string absolutePath = Server.MapPath("~");
                        if (System.IO.File.Exists(absolutePath + Newsfeed.BannerImage))
                        {
                            System.IO.File.Delete(absolutePath + Newsfeed.BannerImage);
                        }
                    }
                    if (Newsfeed.Video != null)
                    {
                        string absolutePath = Server.MapPath("~");
                        if (System.IO.File.Exists(absolutePath + Newsfeed.Video))
                        {
                            System.IO.File.Delete(absolutePath + Newsfeed.Video);
                        }
                    }
                    return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}