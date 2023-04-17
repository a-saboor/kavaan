using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

    class CustomerSessionRepository : RepositoryBase<CustomerSession>, ICustomerSessionRepository
    {
        public CustomerSessionRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public CustomerSession GetCustomerSession(long? customerId, string deviceId, string FirebaseToken, long id = 0)
        {
            var CustomerSession = this.DbContext.CustomerSessions.Where(c => (!customerId.HasValue || c.CustomerID == customerId) && c.DeviceID == deviceId && c.FirebaseToken == FirebaseToken && c.ID != id).Where(i => i.SessionState == true).FirstOrDefault();
            return CustomerSession;
        }

        public IEnumerable<CustomerSession> GetCustomerSessions(long customerId, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed)
        {
            var CustomerSessions = this.DbContext.CustomerSessions.Where(c => c.CustomerID == customerId
																	//&& (isBookingNoticationAllowed.HasValue
																	//    || (c.Customer != null
																	//    && c.Customer.IsBookingNoticationAllowed.HasValue
																	//    && c.Customer.IsBookingNoticationAllowed.Value))
																	&& (isPushNotificationAllowed.HasValue
																		|| (c.Customer != null
																		&& c.Customer.IsPushNotificationAllowed.HasValue
																		&& c.Customer.IsPushNotificationAllowed.Value))
																	&& c.SessionState == true).ToList();
            return CustomerSessions;
        }

        public void DeleteMany(long customerId)
        {
            var CustomerSessions = this.DbContext.CustomerSessions.Where(i => i.CustomerID == customerId).ToList();
            this.DbContext.CustomerSessions.RemoveRange(CustomerSessions);
        }

        public bool ExpireSession(long customerId, string deviceId)
        {
            try
            {
                this.DbContext.SP_ExpireSession(customerId, deviceId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public interface ICustomerSessionRepository : IRepository<CustomerSession>
    {
        CustomerSession GetCustomerSession(long? customerId, string deviceId, string FirebaseToken, long id = 0);
        IEnumerable<CustomerSession> GetCustomerSessions(long customerId, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed);
        void DeleteMany(long customerId);
        bool ExpireSession(long customerId, string deviceId);
    }
}
