using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class VendorSessionsRepository : RepositoryBase<VendorSession>, IVendorSessionRepository
	{

		public VendorSessionsRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public VendorSession GetVendorSession(long vendorId, string deviceId, string FirebaseToken, long id = 0)
		{
			var vendorSession = this.DbContext.VendorSessions.Where(c => c.VendorID == vendorId && c.DeviceID == deviceId && c.FirebaseToken == FirebaseToken && c.ID != id).Where(i => i.SessionState == true).FirstOrDefault();
			return vendorSession;
		}

		public IEnumerable<VendorSession> GetVendorSessions(long vendorId)
		{
			var vendorSessions = this.DbContext.VendorSessions.Where(c => c.VendorID == vendorId && c.SessionState == true).ToList();
			return vendorSessions;
		}

		public void DeleteMany(long vendorId)
		{
			var vendorSessions = this.DbContext.VendorSessions.Where(i => i.VendorID == vendorId).ToList();
			this.DbContext.VendorSessions.RemoveRange(vendorSessions);
		}

		public bool ExpireSession(long VendorId, string deviceId)
		{
			try
			{
				this.DbContext.SP_ExpireSession(VendorId, deviceId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	
	}

	public interface IVendorSessionRepository : IRepository<VendorSession>
	{
		VendorSession GetVendorSession(long VendorId, string deviceId, string FirebaseToken, long id = 0);
		IEnumerable<VendorSession> GetVendorSessions(long VendorId);
		void DeleteMany(long VendorId);
		bool ExpireSession(long VendorId, string deviceId);
	}
}
