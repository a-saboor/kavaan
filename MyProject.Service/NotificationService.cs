using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IUnitOfWork _unitOfWork;

		public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
		{
			this._notificationRepository = notificationRepository;
			this._unitOfWork = unitOfWork;
		}

		#region INotificationService Members

		public IEnumerable<Notification> GetNotifications()
		{
			var notifications = _notificationRepository.GetAll();
			return notifications;
		}

		public IEnumerable<SP_GetNotifications_Result> GetNotifications(long receiverID, string receiverType, int pageNumber = 1, string lang = "en")
		{
			var Notifications = _notificationRepository.GetNotifications(receiverID, receiverType, pageNumber, lang);
			return Notifications;
		}

		public IEnumerable<SP_GetNewNotifications_Result> GetNewNotifications(long receiverID, string receiverType, long offset, string lang = "en")
		{
			var Notifications = _notificationRepository.GetNewNotifications(receiverID, receiverType, offset, lang);
			return Notifications;
		}

		public Notification GetNotification(long id)
		{
			var notification = _notificationRepository.GetById(id);
			return notification;
		}
       
        public Notification GetNotificationsbyReceiverId(long id)
        {
            var notification = _notificationRepository.GetById(id);
            return notification;
        }

        public bool CreateNotification(Notification notification, ref string message)
		{
			try
			{
				notification.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_notificationRepository.Add(notification);
				if (SaveNotification())
				{
					message = "Notification added successfully ...";
					return true;

				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteNotification(long id, ref string message)
		{
			try
			{
				Notification notification = _notificationRepository.GetById(id);

				_notificationRepository.Delete(notification);
				if (SaveNotification())
				{
					message = "Notification deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool SaveNotification()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface INotificationService
	{
		IEnumerable<Notification> GetNotifications();
		IEnumerable<SP_GetNotifications_Result> GetNotifications(long receiverID, string receiverType, int pageNumber = 1, string lang = "en");
		IEnumerable<SP_GetNewNotifications_Result> GetNewNotifications(long receiverID, string receiverType, long offset, string lang = "en");
		Notification GetNotification(long id);
		bool CreateNotification(Notification notification, ref string message);
		bool DeleteNotification(long id, ref string message);
		bool SaveNotification();
	}
}
