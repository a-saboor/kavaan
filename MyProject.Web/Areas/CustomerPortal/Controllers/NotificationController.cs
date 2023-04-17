using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal.Controllers
{
    [AuthorizeCustomer]

    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationReceiverService _notificationReceiverService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerSessionService _customerSessionService;

        public NotificationController(INotificationService notificationService, INotificationReceiverService notificationReceiverService, ICustomerService customerService, ICustomerSessionService customerSessionService)
        {
            this._customerSessionService = customerSessionService;
            this._customerService = customerService;
            this._notificationService = notificationService;
            this._notificationReceiverService = notificationReceiverService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetNotifications(string culture = "en-ae")
        {
            if (Session["ReceiverType"] != null)
            {
                string ReceiverType = "Customer";
                //Int64 UserId = 0;
                Int64 clientid = Convert.ToInt64(Session["CustomerID"].ToString());
                int pageNo = 1;
                string lang = "en";
                if (culture.Contains('-'))
                {
                    lang = culture.Split('-')[0];
                }

                var Notifications = _notificationService.GetNotifications((Int64)clientid, ReceiverType, pageNo, lang);

                _notificationReceiverService.MarkNotificationsAsDelivered((Int64)clientid, ReceiverType);//Marked Notifications as Delivered

                return Json(new
                {
                    success = true,
                    message = "Data retrieved successfully !",
                    data = Notifications
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetNewNotificationCount()
        {
            if (Session["ReceiverType"] != null)
            {
                string ReceiverType = "Customer";
                Int64 clientid = Convert.ToInt64(Session["CustomerID"].ToString());

                var Notifications = _notificationReceiverService.GetNewNotificationCount((Int64)clientid, ReceiverType);

                return Json(new
                {
                    success = true,
                    message = "Data retrieved successfully !",
                    data = Notifications
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LoadNotifications(int pageNo, string culture = "en-ae")
        {
            if (Session["ReceiverType"] != null)
            {
                string ReceiverType = "Customer";
                //Int64 UserId = 0;
                Int64 clientid = Convert.ToInt64(Session["CustomerID"].ToString());
                string lang = "en";
                if (culture.Contains('-'))
                {
                    lang = culture.Split('-')[0];
                }

                var Notifications = _notificationService.GetNotifications((Int64)clientid, ReceiverType, pageNo, lang);
                var TotalNotifcations = _notificationReceiverService.GetNotificationCount((Int64)clientid, ReceiverType);
                _notificationReceiverService.MarkNotificationsAsDelivered((Int64)clientid, ReceiverType);//Marked Notifications as Delivered

                return Json(new
                {
                    success = true,
                    message = "Data retrieved successfully !",
                    data = Notifications,
                    TotalRecord = TotalNotifcations,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult MarkNotificationsAsDelivered(string id)
        {
            if (Session["ReceiverType"] != null)
            {
                string ReceiverType = "Customer";
                Int64 UserId = Convert.ToInt64(Session["CustomerID"].ToString());

                if (_notificationReceiverService.MarkNotificationsAsDelivered((Int64)UserId, ReceiverType))
                {
                    return Json(new { success = true, message = "Notification delivered successfully !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MarkNotificationAsRead(long notificationId)
        {
            if (notificationId > 0)
            {
                if (_notificationReceiverService.MarkNotificationAsRead(notificationId))
                {
                    return Json(new { success = true, message = "Notification read successfully !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MarkNotificationsAsSeen(long receiverId)
        {
            if (Session["ReceiverType"] != null)
            {
                if (_notificationReceiverService.MarkNotificationsAsSeen(receiverId, Session["ReceiverType"].ToString()))
                {
                    return Json(new { success = true, message = "Notification seen successfully !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NotificationsReadAll(long receiverId)
        {
            if (Session["ReceiverType"] != null)
            {
                if (_notificationReceiverService.MarkNotificationsAsRead(receiverId, Session["ReceiverType"].ToString()))
                {
                    return Json(new { success = true, message = "All notification read successfully !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}