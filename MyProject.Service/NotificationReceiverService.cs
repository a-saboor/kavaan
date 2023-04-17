using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
    public class NotificationReceiverService : INotificationReceiverService
    {
        private readonly INotificationReceiverRepository _notificationreceiverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationReceiverService(INotificationReceiverRepository notificationreceiverRepository, IUnitOfWork unitOfWork)
        {
            this._notificationreceiverRepository = notificationreceiverRepository;
            this._unitOfWork = unitOfWork;
        }

        #region INotificationReceiverService Members

        public IEnumerable<NotificationReceiver> GetNotificationReceivers()
        {
            var notificationreceivers = _notificationreceiverRepository.GetAll();
            return notificationreceivers;
        }

        public NotificationReceiver GetNotificationReceiver(long id)
        {
            var notificationreceiver = _notificationreceiverRepository.GetById(id);
            return notificationreceiver;
        }

        public IEnumerable<NotificationReceiver> GetAllByNotificationId(long notificationId)
        {
            var not = _notificationreceiverRepository.GetByNotificationId(notificationId);
            return not;
        }

        public IEnumerable<NotificationReceiver> GetNotificationsbyReceiverId(long id)
        {
            var NotificationReceiver = _notificationreceiverRepository.GetByReceiverId(id);
            return NotificationReceiver;
        }

        public int GetNotificationCount(long receiverID, string receiverType)
        {
            try
            {
                var count = _notificationreceiverRepository.GetNotificationCount(receiverID, receiverType);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int GetNewNotificationCount(long receiverID, string receiverType)
        {
            try
            {
                var count = _notificationreceiverRepository.GetNewNotificationCount(receiverID, receiverType);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool CreateNotificationReceiver(NotificationReceiver notificationreceiver, ref string message)
        {
            try
            {
                notificationreceiver.IsDelivered = false;
                notificationreceiver.IsRead = false;
                notificationreceiver.IsSeen = false;
                _notificationreceiverRepository.Add(notificationreceiver);
                if (SaveNotificationReceiver())
                {
                    message = "Notification Receiver added successfully ...";
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

        public bool MarkNotificationsAsDelivered(long receiverID, string receiverType)
        {
            try
            {
                _notificationreceiverRepository.MarkNotificationsAsDelivered(receiverID, receiverType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MarkNotificationsAsSeen(long receiverID, string receiverType)
        {
            try
            {
                _notificationreceiverRepository.MarkNotificationsAsSeen(receiverID, receiverType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MarkNotificationsAsRead(long receiverID, string receiverType)
        {
            try
            {
                _notificationreceiverRepository.MarkNotificationsAsRead(receiverID, receiverType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MarkNotificationAsRead(long id)
        {
            try
            {
                NotificationReceiver notificationreceiver = _notificationreceiverRepository.GetById(id);
                notificationreceiver.IsRead = true;
                notificationreceiver.IsSeen = true;
                _notificationreceiverRepository.Update(notificationreceiver);
                if (SaveNotificationReceiver())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool NotifyAdminAndVendors(long notificationId, string receiverType, long? vendorId)
        {
            try
            {
                _notificationreceiverRepository.NotifyAdminAndVendors(notificationId, receiverType, vendorId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveNotification(long recordID, string module, long? notificationId)
        {
            try
            {
                _notificationreceiverRepository.RemoveNotification(recordID, module, notificationId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveNotificationReceiver()
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

    public interface INotificationReceiverService
    {
        IEnumerable<NotificationReceiver> GetNotificationReceivers();
        NotificationReceiver GetNotificationReceiver(long id);
        IEnumerable<NotificationReceiver> GetAllByNotificationId(long notificationId);
        IEnumerable<NotificationReceiver> GetNotificationsbyReceiverId(long id);
        bool CreateNotificationReceiver(NotificationReceiver notificationreceiver, ref string message);
        int GetNotificationCount(long receiverID, string receiverType);
        int GetNewNotificationCount(long receiverID, string receiverType);
        bool MarkNotificationsAsDelivered(long receiverID, string receiverType);
        bool MarkNotificationsAsSeen(long receiverID, string receiverType);
        bool MarkNotificationsAsRead(long receiverID, string receiverType);
        bool MarkNotificationAsRead(long id);
        bool NotifyAdminAndVendors(long notificationId, string receiverType, long? vendorId);
        bool RemoveNotification(long recordID, string module, long? notificationId);
        bool SaveNotificationReceiver();

    }
}
