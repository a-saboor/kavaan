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
   public class VendorSessionService : IVendorSessionService
	{
		private readonly IVendorSessionRepository _VendorSessionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorSessionService(IVendorSessionRepository VendorSessionRepository, IUnitOfWork unitOfWork)
		{
			this._VendorSessionRepository = VendorSessionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorSessionService Members

		public IEnumerable<VendorSession> GetVendorSessions()
		{
			var VendorSessions = _VendorSessionRepository.GetAll();
			return VendorSessions;
		}

		public IEnumerable<VendorSession> GetVendorSessions(long VendorId)
		{
			var VendorSessions = _VendorSessionRepository.GetVendorSessions(VendorId);
			return VendorSessions;
		}

		public VendorSession GetVendorSession(long id)
		{
			var VendorSession = _VendorSessionRepository.GetById(id);
			return VendorSession;
		}

		public string GetVendorSessionFirebaseToken(long id)
		{
			var VendorSession = _VendorSessionRepository.GetAll().Where(x => x.VendorID == id).Select(x => x.FirebaseToken).FirstOrDefault();
			return VendorSession;
		}

		public string[] GetVendorSessionFirebaseTokens(long id)
		{
			var VendorSession = GetVendorSessions(id).Select(x => x.FirebaseToken).ToArray();
			return VendorSession;
		}

		public bool CreateVendorSession(ref VendorSession VendorSession, ref string message, ref string status)
		{
			try
			{
				if (_VendorSessionRepository.GetVendorSession((long)VendorSession.VendorID, VendorSession.DeviceID, VendorSession.FirebaseToken) == null)
				{
					VendorSession.SessionState = true;
					VendorSession.CreatedOn = Helpers.TimeZone.GetLocalDateTime();

					_VendorSessionRepository.Add(VendorSession);
					if (SaveVendorSession())
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

		public bool UpdateVendorSession(ref VendorSession VendorSession, ref string message)
		{
			try
			{
				if (_VendorSessionRepository.GetVendorSession((long)VendorSession.VendorID, VendorSession.DeviceID, VendorSession.FirebaseToken, VendorSession.ID) == null)
				{
					_VendorSessionRepository.Update(VendorSession);
					if (SaveVendorSession())
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

		public bool DeleteVendorSession(long id, ref string message)
		{
			try
			{
				VendorSession VendorSession = _VendorSessionRepository.GetById(id);
				_VendorSessionRepository.Delete(VendorSession);
				if (SaveVendorSession())
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

		public bool DeleteVendorSessions(long id, ref string message)
		{
			try
			{
				_VendorSessionRepository.DeleteMany(id);
				if (SaveVendorSession())
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

		public bool ExpireSession(long VendorId, string deviceId)
		{
			try
			{
				_VendorSessionRepository.ExpireSession(VendorId, deviceId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SaveVendorSession()
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

	public interface IVendorSessionService
	{
		string GetVendorSessionFirebaseToken(long id);

		string[] GetVendorSessionFirebaseTokens(long id);

		IEnumerable<VendorSession> GetVendorSessions();
		IEnumerable<VendorSession> GetVendorSessions(long VendorId);
		VendorSession GetVendorSession(long id);
		bool CreateVendorSession(ref VendorSession VendorSession, ref string message, ref string status);
		bool UpdateVendorSession(ref VendorSession VendorSession, ref string message);
		bool DeleteVendorSession(long id, ref string message);
		bool DeleteVendorSessions(long id, ref string message);
		bool ExpireSession(long VendorId, string deviceId);
		bool SaveVendorSession();
	}
}
