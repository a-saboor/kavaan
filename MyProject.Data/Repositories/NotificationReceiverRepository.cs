using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

    class NotificationReceiverRepository : RepositoryBase<NotificationReceiver>, INotificationReceiverRepository
    {
        public NotificationReceiverRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public IEnumerable<NotificationReceiver> GetByReceiverId(long ReceiverID)
        {
            var not = this.DbContext.NotificationReceivers.Where(c => c.ReceiverID == ReceiverID).ToList();
            return not;
        }

        public IEnumerable<NotificationReceiver> GetByNotificationId(long notificationId)
        {
            var not = this.DbContext.NotificationReceivers.Where(c => c.NotificationID == notificationId).ToList();
            return not;
        }

        public int GetNotificationCount(long receiverID, string receiverType)
        {
            try
            {
                var Count = this.DbContext.NotificationReceivers.Where(i => i.ReceiverID == receiverID
                                                               && i.ReceiverType == receiverType
                                                               ).Count();
                return Count;
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
                var Count = this.DbContext.NotificationReceivers.Where(i => i.ReceiverID == receiverID
                                                               && i.ReceiverType == receiverType
                                                               && i.IsSeen == false
                                                               && i.IsDelivered == false
                                                               && i.IsRead == false).Count();
                return Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool MarkNotificationsAsDelivered(long receiverID, string receiverType)
        {
            try
            {
                this.DbContext.SP_MarkNotificationsAsDelivered(receiverID, receiverType);
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
                this.DbContext.SP_MarkNotificationsAsSeen(receiverID, receiverType);
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
                this.DbContext.SP_MarkNotificationsAsRead(receiverID, receiverType);
                return true;
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
                this.DbContext.SP_NotifyAdminAndVendors(notificationId, receiverType, vendorId);
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
                this.DbContext.SP_RemoveNotification(recordID, module, notificationId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public interface INotificationReceiverRepository : IRepository<NotificationReceiver>
    {
        IEnumerable<NotificationReceiver> GetByReceiverId(long ReceiverID);
        IEnumerable<NotificationReceiver> GetByNotificationId(long notificationId);
        int GetNotificationCount(long receiverID, string receiverType);
        int GetNewNotificationCount(long receiverID, string receiverType);
        bool MarkNotificationsAsDelivered(long receiverID, string receiverType);
        bool MarkNotificationsAsSeen(long receiverID, string receiverType);
        bool MarkNotificationsAsRead(long receiverID, string receiverType);
        bool NotifyAdminAndVendors(long notificationId, string receiverType, long? vendorId);
        bool RemoveNotification(long recordID, string module, long? notificationId);
    }
}
