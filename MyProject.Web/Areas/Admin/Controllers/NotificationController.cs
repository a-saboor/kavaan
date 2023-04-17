using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using System;
using System.Web.Mvc;
using MyProject.Web.ViewModels.CustomNotification;
using MyProject.Web.Helpers.PushNotification;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
	public class NotificationController : Controller
	{
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
		public ActionResult GetNotifications()
		{

			if (!string.IsNullOrEmpty(AdminSessionHelper.ReceiverType))
			{
				string ReceiverType = "Admin";
				//Int64 UserId = 0;
				long clientid = AdminSessionHelper.ID;
				int pageNo = 1;
				string lang = "en";

				var Notifications = _notificationService.GetNotifications((Int64)clientid, ReceiverType, pageNo, lang);
				_notificationReceiverService.MarkNotificationsAsDelivered((Int64)clientid, ReceiverType);

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
		public ActionResult LoadNotifications(int pageNo, string lang = "en")
		{
			if (!string.IsNullOrEmpty(AdminSessionHelper.ReceiverType))
			{
				string ReceiverType = "Admin";
				//Int64 UserId = 0;
				long clientid = AdminSessionHelper.ID;

				var Notifications = _notificationService.GetNotifications((Int64)clientid, ReceiverType, pageNo, lang);

				_notificationReceiverService.MarkNotificationsAsDelivered((Int64)clientid, ReceiverType);
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
		public ActionResult MarkNotificationsAsDelivered(string id)
		{
			if (!string.IsNullOrEmpty(AdminSessionHelper.ReceiverType))
			{
				string ReceiverType = "Admin";
				Int64 UserId = Convert.ToInt64(Session["AdminUserID"].ToString());

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
			if (!string.IsNullOrEmpty(AdminSessionHelper.ReceiverType))
			{
				if (_notificationReceiverService.MarkNotificationsAsSeen(receiverId, AdminSessionHelper.ReceiverType.ToString()))
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
			if (!string.IsNullOrEmpty(AdminSessionHelper.ReceiverType))
			{
				if (_notificationReceiverService.MarkNotificationsAsRead(receiverId, AdminSessionHelper.ReceiverType.ToString()))
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

		#region Push Notifications for Mobile App

		[AuthorizeAdmin]
		public ActionResult SendNotification()
		{
			var AdminID = Convert.ToInt64(Session["AdminID"]);
			ViewBag.CustomerID = new SelectList(_customerService.GetCustomersDropDownForNotifications(), "value", "text");
			
			return View();
		}

		[HttpPost]
		public ActionResult SendNotification(CustomNotificationViewModel notificationModel)
		{
			try
			{
				string message = string.Empty;
				for (int i = 0; i < notificationModel.Customers.Count; i++)
				{
					var tokens = _customerSessionService.GetCustomerSessionFirebaseTokens(notificationModel.Customers[i], null, null);

					Notification not = new Notification();
					not.Title = notificationModel.Title;
					not.TitleAr = notificationModel.Title;
					not.Description = notificationModel.Body;
					not.DescriptionAr = notificationModel.Body;
					not.OriginatorID = Convert.ToInt64(Session["AdminUserID"]);
					not.OriginatorName = Session["UserName"].ToString();
					not.Module = notificationModel.Module;
					not.OriginatorType = "Admin";
					not.RecordID = notificationModel.CarID;
					if (_notificationService.CreateNotification(not, ref message))
					{
						NotificationReceiver notRec = new NotificationReceiver();
						notRec.ReceiverID = notificationModel.Customers[i];
						notRec.ReceiverType = "Customer";
						notRec.NotificationID = not.ID;
						if (_notificationReceiverService.CreateNotificationReceiver(notRec, ref message))
						{
							if (tokens.Length > 0)
							{
								var response = PushNotification.SendPushNotification(tokens, notificationModel.Title, notificationModel.Body, new
								{
									Module = notificationModel.Module,
									RecordID = notificationModel.CarID,
									NotificationID = notRec.ID
								});
							}
						}
					}
				}
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} notification sent {notificationModel.Title}.");
				return Json(new { success = true, message = "Notification sent sucessfully... " }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{

				return Json(new { success = false, message = "Error !" }, JsonRequestBehavior.AllowGet);
			}
		}

		#endregion
	}
}