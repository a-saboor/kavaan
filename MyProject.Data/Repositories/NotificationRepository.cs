using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
	{
		public NotificationRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<SP_GetNotifications_Result> GetNotifications(long receiverID, string receiverType, int pageNumber = 1, string lang = "en")
		{
			var Notifications = this.DbContext.SP_GetNotifications(receiverID, receiverType, pageNumber, lang).ToList();
			return Notifications;
		}

		public IEnumerable<SP_GetNewNotifications_Result> GetNewNotifications(long receiverID, string receiverType, long offset, string lang = "en")
		{
			var Notifications = this.DbContext.SP_GetNewNotifications(receiverID, receiverType, offset, lang).ToList();
			return Notifications;
		}
	}

	public interface INotificationRepository : IRepository<Notification>
	{
		IEnumerable<SP_GetNotifications_Result> GetNotifications(long receiverID, string receiverType, int pageNumber = 1, string lang = "en");
		IEnumerable<SP_GetNewNotifications_Result> GetNewNotifications(long receiverID, string receiverType, long offset, string lang = "en");
	}
}
