using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class StaffSessionRepository : RepositoryBase<StaffSession>, IStaffSessionRepository
    {
		public StaffSessionRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public StaffSession GetStaffSession(long StaffID, string deviceId, string FirebaseToken, long id = 0)
		{
			var StaffSession = this.DbContext.StaffSessions.Where(c => c.StaffID == StaffID && c.DeviceID == deviceId && c.FirebaseToken == FirebaseToken && c.ID != id).Where(i => i.SessionState == true).FirstOrDefault();
			return StaffSession;
		}

		public IEnumerable<StaffSession> GetStaffSessions(long StaffID)
		{
			var StaffSessions = this.DbContext.StaffSessions.Where(c => c.StaffID == StaffID && c.SessionState == true).ToList();
			return StaffSessions;
		}

		public void DeleteMany(long StaffID)
		{
			var StaffSessions = this.DbContext.StaffSessions.Where(i => i.StaffID == StaffID).ToList();
			this.DbContext.StaffSessions.RemoveRange(StaffSessions);
		}

		public bool ExpireSession(long StaffID, string deviceId)
		{
			try
			{
				this.DbContext.SP_ExpireSession(StaffID, deviceId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
	public interface IStaffSessionRepository : IRepository<StaffSession>
	{
		StaffSession GetStaffSession(long StaffID, string deviceId, string FirebaseToken, long id = 0);
		IEnumerable<StaffSession> GetStaffSessions(long StaffID);
		void DeleteMany(long StaffID);
		bool ExpireSession(long StaffID, string deviceId);
	}
}
