using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{
    class StaffSessionService : IStaffSessionService
    {
		private readonly IStaffSessionRepository _StaffSessionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public StaffSessionService(IStaffSessionRepository StaffSessionRepository, IUnitOfWork unitOfWork)
		{
			this._StaffSessionRepository = StaffSessionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IStaffSessionService Members

		public IEnumerable<StaffSession> GetStaffSessions()
		{
			var StaffSessions = _StaffSessionRepository.GetAll();
			return StaffSessions;
		}

		public IEnumerable<StaffSession> GetStaffSessions(long StaffID)
		{
			var StaffSessions = _StaffSessionRepository.GetStaffSessions(StaffID);
			return StaffSessions;
		}

		public StaffSession GetStaffSession(long id)
		{
			var StaffSession = _StaffSessionRepository.GetById(id);
			return StaffSession;
		}

		public string GetStaffSessionFirebaseToken(long id)
		{
			var StaffSession = _StaffSessionRepository.GetAll().Where(x => x.StaffID == id).Select(x => x.FirebaseToken).FirstOrDefault();
			return StaffSession;
		}

		public string[] GetStaffSessionFirebaseTokens(long id)
		{
			var StaffSession = GetStaffSessions(id).Select(x => x.FirebaseToken).ToArray();
			return StaffSession;
		}

		public bool CreateStaffSession(ref StaffSession StaffSession, ref string message, ref string status)
		{
			try
			{
				if (_StaffSessionRepository.GetStaffSession((long)StaffSession.StaffID, StaffSession.DeviceID, StaffSession.FirebaseToken) == null)
				{
					StaffSession.SessionState = true;
					StaffSession.CreatedOn = Helpers.TimeZone.GetLocalDateTime();

					_StaffSessionRepository.Add(StaffSession);
					if (SaveStaffSession())
					{
						status = "success";
						message = "Vendor Session added successfully ...";
						return true;

					}
					else
					{
						status = "failure";
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					status = "error";
					message = "Vendor Session already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				status = "failure";
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateStaffSession(ref StaffSession StaffSession, ref string message)
		{
			try
			{
				if (_StaffSessionRepository.GetStaffSession((long)StaffSession.StaffID, StaffSession.DeviceID, StaffSession.FirebaseToken, StaffSession.ID) == null)
				{
					_StaffSessionRepository.Update(StaffSession);
					if (SaveStaffSession())
					{
						message = "Vendor Session updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Vendor Session already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteStaffSession(long id, ref string message)
		{
			try
			{
				StaffSession StaffSession = _StaffSessionRepository.GetById(id);
				_StaffSessionRepository.Delete(StaffSession);
				if (SaveStaffSession())
				{
					message = "Vendor Session deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteStaffSessions(long id, ref string message)
		{
			try
			{
				_StaffSessionRepository.DeleteMany(id);
				if (SaveStaffSession())
				{
					message = "Vendor Session deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool ExpireSession(long StaffID, string deviceId)
		{
			try
			{
				_StaffSessionRepository.ExpireSession(StaffID, deviceId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SaveStaffSession()
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
	public interface IStaffSessionService
	{
		string GetStaffSessionFirebaseToken(long id);

		string[] GetStaffSessionFirebaseTokens(long id);

		IEnumerable<StaffSession> GetStaffSessions();
		IEnumerable<StaffSession> GetStaffSessions(long StaffID);
		StaffSession GetStaffSession(long id);
		bool CreateStaffSession(ref StaffSession StaffSession, ref string message, ref string status);
		bool UpdateStaffSession(ref StaffSession StaffSession, ref string message);
		bool DeleteStaffSession(long id, ref string message);
		bool DeleteStaffSessions(long id, ref string message);
		bool ExpireSession(long StaffID, string deviceId);
		bool SaveStaffSession();
	}
}
