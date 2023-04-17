using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

	public class CustomerSessionService : ICustomerSessionService
	{
		private readonly ICustomerSessionRepository _customerSessionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CustomerSessionService(ICustomerSessionRepository customerSessionRepository, IUnitOfWork unitOfWork)
		{
			this._customerSessionRepository = customerSessionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICustomerSessionService Members

		public IEnumerable<CustomerSession> GetCustomerSessions()
		{
			var customerSessions = _customerSessionRepository.GetAll();
			return customerSessions;
		}

		public IEnumerable<CustomerSession> GetCustomerSessions(long customerId, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed)
		{
			var customerSessions = _customerSessionRepository.GetCustomerSessions(customerId, isBookingNoticationAllowed, isPushNotificationAllowed);
			return customerSessions;
		}

		public CustomerSession GetCustomerSession(long id)
		{
			var customerSession = _customerSessionRepository.GetById(id);
			return customerSession;
		}

		public string GetCustomerSessionFirebaseToken(long id)
		{
			var customerSession = _customerSessionRepository.GetAll().Where(x => x.CustomerID == id).Select(x => x.FirebaseToken).FirstOrDefault();
			return customerSession;
		}

		public string[] GetCustomerSessionFirebaseTokens(long id, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed)
		{
			var customerSession = GetCustomerSessions(id, isBookingNoticationAllowed, isPushNotificationAllowed).Select(x => x.FirebaseToken).ToArray();
			return customerSession;
		}

		public bool CreateCustomerSession(ref CustomerSession customerSession, ref string message, ref string status)
		{
			try
			{
				if (_customerSessionRepository.GetCustomerSession(customerSession.CustomerID, customerSession.DeviceID, customerSession.FirebaseToken) == null)
				{
					customerSession.SessionState = true;
					customerSession.CreatedOn = Helpers.TimeZone.GetLocalDateTime();

					_customerSessionRepository.Add(customerSession);
					if (SaveCustomerSession())
					{
						status = "success";
						message = "Customer Session added successfully ...";
						return true;

					}
					else
					{
						status = "failure";
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					status = "error";
					message = "Customer Session already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				status = "failure";
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateCustomerSession(ref CustomerSession customerSession, ref string message)
		{
			try
			{
				if (_customerSessionRepository.GetCustomerSession((long)customerSession.CustomerID, customerSession.DeviceID, customerSession.FirebaseToken, customerSession.ID) == null)
				{
					_customerSessionRepository.Update(customerSession);
					if (SaveCustomerSession())
					{
						message = "Customer Session updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
				}
				else
				{
					message = "Customer Session already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteCustomerSession(long id, ref string message)
		{
			try
			{
				CustomerSession customerSession = _customerSessionRepository.GetById(id);
				_customerSessionRepository.Delete(customerSession);
				if (SaveCustomerSession())
				{
					message = "Customer Session deleted successfully ...";
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

		public bool DeleteCustomerSessions(long id, ref string message)
		{
			try
			{
				_customerSessionRepository.DeleteMany(id);
				if (SaveCustomerSession())
				{
					message = "Customer Session deleted successfully ...";
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

		public bool ExpireSession(long customerId, string deviceId)
		{
			try
			{
				_customerSessionRepository.ExpireSession(customerId, deviceId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SaveCustomerSession()
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

	public interface ICustomerSessionService
	{
		string GetCustomerSessionFirebaseToken(long id);
		string[] GetCustomerSessionFirebaseTokens(long id, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed);
		IEnumerable<CustomerSession> GetCustomerSessions();
		IEnumerable<CustomerSession> GetCustomerSessions(long customerId, bool? isBookingNoticationAllowed, bool? isPushNotificationAllowed);
		CustomerSession GetCustomerSession(long id);
		bool CreateCustomerSession(ref CustomerSession customerSession, ref string message, ref string status);
		bool UpdateCustomerSession(ref CustomerSession customerSession, ref string message);
		bool DeleteCustomerSession(long id, ref string message);
		bool DeleteCustomerSessions(long id, ref string message);
		bool ExpireSession(long customerId, string deviceId);
		bool SaveCustomerSession();
	}
}
