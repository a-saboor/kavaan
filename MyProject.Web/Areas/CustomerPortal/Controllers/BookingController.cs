using MyProject.Data;
using MyProject.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.Controllers;
using MyProject.Web.AuthorizationProvider;
using System.Web;
using MyProject.Web.Helpers;
using MyProject.Web.ViewModels;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
    [AuthorizeCustomer]
    public class BookingController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IServiceBookingService _BookingService;
        private readonly IServiceBookingImageService _serviceBookingImageService;
        private readonly IServiceRatingService _ratingService;
        private readonly IServicesService _servicesService;
        private readonly ICustomerService _customerService;
        private readonly INumberRangeService _numberRangeService;
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;

        public BookingController(IServiceBookingService BookingService, IServiceBookingImageService serviceBookingImageService, IServiceRatingService ratingService, INotificationReceiverService notificationReceiverService, INotificationService notificationService, IServicesService servicesService, ICustomerService customerService, INumberRangeService numberRangeService)
        {
            this._BookingService = BookingService;
            this._serviceBookingImageService = serviceBookingImageService;
            this._ratingService = ratingService;
            this._servicesService = servicesService;
            this._customerService = customerService;
            this._numberRangeService = numberRangeService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(long ID)
        {
            var service = _servicesService.GetService(ID);
            ViewData["Customer"] = _customerService.GetCustomer(CustomerSessionHelper.ID) != null ? _customerService.GetCustomer(CustomerSessionHelper.ID) : new Customer();

            if (service == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceBooking booking, string culture = "en-ae")
        {
            string message = string.Empty;
            string description = string.Empty;
            object data = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                long CustomerID = CustomerSessionHelper.ID;
                var CustomerName = CustomerSessionHelper.UserName;

                try
                {
                    booking.CustomerID = CustomerID;
                    booking.BookingNo = _numberRangeService.GetNextValueFromNumberRangeByName("BOOKING");
                    booking.IsPayed = false;
                    booking.Status = "Pending";
                    booking.VendorID = null;
                    booking.CustomerName = booking.FirstName + booking.LastName;
                    booking.CustomerAddress = booking.MapLocation;
                    booking.CouponDiscount = 0m;

                    if (_BookingService.CreateServiceBooking(ref booking, ref message))
                    {
                        HttpFileCollectionBase files = Request.Files;
                        if (files.Count > 0)
                        {
                            string absolutePath = Server.MapPath("~");
                            string relativePath = string.Format("/Assets/AppFiles/Bookings/{0}/", booking.BookingNo.Replace(" ", "_"));

                            for (int i = 0; i < files.Count; i++)
                            {
                                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                                //string filename = Path.GetFileName(Request.Files[i].FileName);  

                                HttpPostedFileBase file = files[i];
                                string fname;

                                // Checking for Internet Explorer  
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }

                                ServicebookingImage bookingImage = new ServicebookingImage();
                                bookingImage.Image = Uploader.UploadImage(file, absolutePath, relativePath, "Booking-", ref message, fname);
                                bookingImage.ServiceBookingID = booking.ID;
                                bookingImage.CustomerID = CustomerID;
                                bookingImage.ServiceCategoryID = booking.ServiceCategoryID;

                                bookingImage.Image = CustomURL.GetImageServer() + bookingImage.Image.Remove(0, 1);
                                try
                                {
                                    _serviceBookingImageService.CreateServiceBookingImage(bookingImage, ref message);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else
                        {
                            message = "";
                        }

                        TempData["NewBooking"] = booking;
                        TempData["NewBookingURL"] = $"/customer/dashboard#my-bookings";
                        TempData["NewBookingText"] = "My Bookings";
                        status = true;

                        /*Booking Notification For Customer*/
                        Notification not = new Notification();
                        not.Title = "Booking Placed";
                        not.TitleAr = "Booking Placed";
                        not.Description = string.Format("Your booking # {0} has been placed. You can check the booking status via booking tracker", booking.BookingNo);
                        not.DescriptionAr = string.Format("Your booking # {0} has been placed. You can check the booking status via booking tracker", booking.BookingNo);

                        not.Module = "Booking";
                        not.OriginatorType = "System";
                        not.RecordID = CustomerID;
                        if (_notificationService.CreateNotification(not, ref message))
                        {
                            NotificationReceiver notRec = new NotificationReceiver();
                            notRec.ReceiverID = CustomerID;
                            notRec.ReceiverType = "Customer";
                            notRec.NotificationID = not.ID;
                            if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
                            {
                            }
                        }
                        not = new Notification();
                        /*Booking Notification For Admin*/
                        not.Title = "Booking Placed";
                        not.TitleAr = "Booking Placed";
                        not.Description = string.Format("New booking # {0} has been placed ", booking.BookingNo);
                        not.DescriptionAr = string.Format("New booking # {0} has been placed ", booking.BookingNo);
                        not.Url = "/Admin/ServiceBooking/Index";
                        not.Module = "Booking";
                        not.OriginatorType = "Customer";
                        not.RecordID = CustomerID;
                        if (_notificationService.CreateNotification(not, ref message))
                        {
                            if (_notificationReceiverService.NotifyAdminAndVendors(not.ID, "Admin", null))
                            {
                            }
                        }

                    }
                }
                catch (Exception)
                {
                    message = "Something went wrong! Please try later.";
                }
            }
            else
            {
                message = "Please fill the form properly!";
                description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new { success = status, message, description, data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(long ID)
        {
            var booking = _BookingService.GetServiceBooking(ID);

            if (booking == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ServiceBooking bookingModel, string culture = "en-ae")
        {
            string message = string.Empty;
            string description = string.Empty;
            object data = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                long CustomerID = CustomerSessionHelper.ID;

                try
                {
                    var booking = _BookingService.GetServiceBooking(bookingModel.ID);

                    //updation code
                    booking.CouponDiscount = 0m;

                    if (_BookingService.UpdateServiceBooking(ref booking, ref message))
                    {
                        var rating = _ratingService.GetServiceRating(bookingModel.ID);
                        message = $"Your Booking {booking.BookingNo} process has been completed ...";
                        status = true;
                        data = new
                        {

                            ID = booking.ID,
                            Date = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            CreationDate = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            VisitDate = booking.DateOfVisit.Value.ToString("MMM dd, yyyy ") + booking.TimeOfVisit.Value.ToString(""),
                            BookingNo = booking.BookingNo,
                            Status = booking.Status,
                            IsPayed = booking.IsPayed,
                            Service = culture == "en-ae" ? booking.Service.Name : booking.Service.NameAr,
                            Category = culture == "en-ae" ? booking.Category.CategoryName : booking.Category.CategoryNameAr,
                            Address = !string.IsNullOrEmpty(booking.CustomerAddress) ? booking.CustomerAddress : booking.MapLocation,
                            ServiceThumbnail = booking.Service.Thumbnail,
                            booking.DateOfVisit,
                            booking.TimeOfVisit,
                            booking.Total,
                            booking.IsQuoteApproved,
                            IsCanceled = booking.Status == "Cancelled" || booking.Status == "Canceled" ? true : false,
                            IsCompleted = booking.Status == "Completed" ? true : false,
                            IsReviewed = rating != null ? true : false,
                        };

                    }
                }
                catch (Exception)
                {
                    message = "Something went wrong! Please try later.";
                }
            }
            else
            {
                message = "Please fill the form properly!";
                description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new { success = status, message, description, data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Review(long ID)
        {
            var booking = _BookingService.GetServiceBooking(ID);
            var bookingRating = _ratingService.GetServiceRatingByBookingId(ID);

            ViewBag.Rating = bookingRating != null ? bookingRating.Rating : 0;
            ViewBag.Review = bookingRating != null ? bookingRating.Review : "";

            if (booking == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(ServiceRating model, string culture = "en-ae")
        {
            string message = string.Empty;
            string description = string.Empty;
            object data = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                long CustomerID = CustomerSessionHelper.ID;

                try
                {
                    var booking = _BookingService.GetServiceBooking((long)model.ServiceBookingID);

                    //updation code
                    model.CustomerID = CustomerID;


                    if (_ratingService.CreateServiceRatings(model, ref message))
                    {
                        var ratings = _ratingService.GetServiceRating(booking.ID);
                        message = $"Your Booking {booking.BookingNo} process has been completed ...";
                        status = true;
                        data = new
                        {
                            ID = booking.ID,
                            Date = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            CreationDate = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            VisitDate = booking.DateOfVisit.Value.ToString("MMM dd, yyyy ") + booking.TimeOfVisit.Value.ToString(""),
                            BookingNo = booking.BookingNo,
                            Status = booking.Status,
                            IsPayed = booking.IsPayed,
                            Service = culture == "en-ae" ? booking.Service.Name : booking.Service.NameAr,
                            Category = culture == "en-ae" ? booking.Category.CategoryName : booking.Category.CategoryNameAr,
                            Address = !string.IsNullOrEmpty(booking.CustomerAddress) ? booking.CustomerAddress : booking.MapLocation,
                            ServiceThumbnail = booking.Service.Thumbnail,
                            booking.DateOfVisit,
                            booking.TimeOfVisit,
                            booking.Total,
                            booking.IsQuoteApproved,
                            IsCanceled = booking.Status == "Cancelled" || booking.Status == "Canceled" ? true : false,
                            IsCompleted = booking.Status == "Completed" ? true : false,
                            IsReviewed = ratings != null ? true : false,
                        };
                    }
                }
                catch (Exception)
                {
                    message = "Something went wrong! Please try later.";
                }
            }
            else
            {
                message = "Please fill the form properly!";
                description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new { success = status, message, description, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FilteredBookings(FilterViewModel filters, string culture = "en-ae")
        {
            try
            {
                string ImageServer = CustomURL.GetImageServer();
                var bookings = _BookingService.GetCustomerServiceBookings(CustomerSessionHelper.ID, filters.status, filters.pageSize, filters.pageNumber, filters.sortBy, filters.Lang, "");
                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = bookings.Select(x => new
                    {
                        x.TotalRecords,
                        x.FilteredRecords,
                        x.ID,
                        x.Date,
                        x.CreationDate,
                        x.VisitDate,
                        x.BookingNo,
                        x.Status,
                        x.IsPayed,
                        x.Service,
                        x.Category,
                        x.Address,
                        x.ServiceThumbnail,
                        x.DateOfVisit,
                        x.TimeOfVisit,
                        x.Total,
                        x.IsQuoteApproved,
                        x.IsCanceled,
                        x.IsCompleted,
                        x.IsReviewed
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong! Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Cancel(long ID)
        {
            var booking = _BookingService.GetServiceBooking(ID);

            if (booking == null)
            {
                throw new HttpException(404, "File Not Found");
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(ServiceBooking bookingModel, string culture = "en-ae")
        {
            string message = string.Empty;
            string description = string.Empty;
            object data = null;
            bool status = false;

            if (ModelState.IsValid)
            {
                long CustomerID = CustomerSessionHelper.ID;

                try
                {
                    var booking = _BookingService.GetServiceBooking(bookingModel.ID);
                    booking.CancellationReason = bookingModel.CancellationReason;
                    booking.IsCancelled = true;
                    booking.Status = "Canceled";

                    if (_BookingService.UpdateServiceBooking(ref booking, ref message))
                    {
                        var rating = _ratingService.GetServiceRating(bookingModel.ID);

                        message = $"Your Booking {booking.BookingNo} has been cancelled ...";
                        status = true;
                        data = new
                        {
                            ID = booking.ID,
                            Date = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            CreationDate = booking.CreatedOn.ToString("MMM dd, yyyy hh:mm:tt"),
                            VisitDate = booking.DateOfVisit.Value.ToString("MMM dd, yyyy ") + booking.TimeOfVisit.Value.ToString(""),
                            BookingNo = booking.BookingNo,
                            Status = booking.Status,
                            IsPayed = booking.IsPayed,
                            Service = culture == "en-ae" ? booking.Service.Name : booking.Service.NameAr,
                            Category = culture == "en-ae" ? booking.Category.CategoryName : booking.Category.CategoryNameAr,
                            Address = !string.IsNullOrEmpty(booking.CustomerAddress) ? booking.CustomerAddress : booking.MapLocation,
                            ServiceThumbnail = booking.Service.Thumbnail,
                            booking.DateOfVisit,
                            booking.TimeOfVisit,
                            booking.Total,
                            booking.IsQuoteApproved,
                            IsCanceled = booking.Status == "Cancelled" || booking.Status == "Canceled" ? true : false,
                            IsCompleted = booking.Status == "Completed" ? true : false,
                            IsReviewed = rating != null ? true : false,
                        };
                    }
                }
                catch (Exception e)
                {
                    message = "Something went wrong! Please try later.";
                }
            }
            else
            {
                message = "Please fill the form properly!";
                description = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return Json(new { success = status, message, description, data }, JsonRequestBehavior.AllowGet);
        }
    }
}