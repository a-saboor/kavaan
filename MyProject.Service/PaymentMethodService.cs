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
	public class PaymentMethodService : IPaymentMethodService
	{
		private readonly IPaymentMethodRepository _paymentmethodRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentMethodService(IPaymentMethodRepository paymentmethodRepository, IUnitOfWork unitOfWork)
		{
			this._paymentmethodRepository = paymentmethodRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IPaymentMethodService Members

		public IEnumerable<PaymentMethod> GetPaymentMethods()
		{
			var paymentmethods = _paymentmethodRepository.GetAll().Where(i => i.IsDeleted == false);
			return paymentmethods;
		}

		public IEnumerable<object> GetPaymentMethodsForDropDown()
		{
			var PaymentMethods = _paymentmethodRepository.GetAll();
			var dropdownList = from paymentmethods in PaymentMethods
							   select new { value = paymentmethods.ID, text = paymentmethods.Method };
			return dropdownList;
		}

		public PaymentMethod GetPaymentMethod(long id)
		{
			var paymentmethod = _paymentmethodRepository.GetById(id);
			return paymentmethod;
		}

		public bool CreatePaymentMethod(PaymentMethod paymentmethod, ref string message)
		{
			try
			{
				if (_paymentmethodRepository.GetPaymentMethodByName(paymentmethod.Method) == null)
				{
					paymentmethod.IsActive = true;
					paymentmethod.IsDeleted = false;
					paymentmethod.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_paymentmethodRepository.Add(paymentmethod);
					if (SavePaymentMethod())
					{
						message = "PaymentMethod added successfully ...";
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
					message = "PaymentMethod already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdatePaymentMethod(ref PaymentMethod paymentmethod, ref string message)
		{
			try
			{
				if (_paymentmethodRepository.GetPaymentMethodByName(paymentmethod.Method, paymentmethod.ID) == null)
				{
					PaymentMethod CurrentPaymentMethod = _paymentmethodRepository.GetById(paymentmethod.ID);

					CurrentPaymentMethod.Method = paymentmethod.Method;
					CurrentPaymentMethod.MethodAr = paymentmethod.MethodAr;
					paymentmethod = null;

					_paymentmethodRepository.Update(CurrentPaymentMethod);
					if (SavePaymentMethod())
					{
						paymentmethod = CurrentPaymentMethod;
						message = "PaymentMethod updated successfully ...";
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
					message = "PaymentMethod already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeletePaymentMethod(long id, ref string message, bool softDelete = true)
		{
			try
			{
				PaymentMethod paymentmethod = _paymentmethodRepository.GetById(id);
				if (softDelete)
				{
					paymentmethod.IsDeleted = true;
					_paymentmethodRepository.Update(paymentmethod);
				}
				else
				{
					_paymentmethodRepository.Delete(paymentmethod);
				}
				if (SavePaymentMethod())
				{
					message = "PaymentMethod deleted successfully ...";
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

		public bool SavePaymentMethod()
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

	public interface IPaymentMethodService
	{
		IEnumerable<PaymentMethod> GetPaymentMethods();
		IEnumerable<object> GetPaymentMethodsForDropDown();
		PaymentMethod GetPaymentMethod(long id);
		bool CreatePaymentMethod(PaymentMethod paymentmethod, ref string message);
		bool UpdatePaymentMethod(ref PaymentMethod paymentmethod, ref string message);
		bool DeletePaymentMethod(long id, ref string message, bool softDelete = true);
		bool SavePaymentMethod();
	}
}
