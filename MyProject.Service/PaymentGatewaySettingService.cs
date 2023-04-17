using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class PaymentGatewaySettingService : IPaymentGatewaySettingService
	{
		private readonly IPaymentGatewaySettingRepository _paymentgatewaysettingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentGatewaySettingService(IPaymentGatewaySettingRepository paymentgatewaysettingRepository, IUnitOfWork unitOfWork)
		{
			this._paymentgatewaysettingRepository = paymentgatewaysettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IPaymentGatewaySettingService Members

		public IEnumerable<PaymentGatewaySetting> GetPaymentGatewaySettings()
		{
			var paymentgatewaysettings = _paymentgatewaysettingRepository.GetAll();
			return paymentgatewaysettings;
		}

		public PaymentGatewaySetting GetPaymentGatewaySetting(long id)
		{
			var paymentgatewaysetting = _paymentgatewaysettingRepository.GetById(id);
			return paymentgatewaysetting;
		}

		public PaymentGatewaySetting GetDefaultPaymentGatewaySetting()
		{
			var paymentgatewaysetting = _paymentgatewaysettingRepository.GetAll().FirstOrDefault();
			return paymentgatewaysetting;
		}

		public bool CreatePaymentGatewaySetting(PaymentGatewaySetting paymentgatewaysetting, ref string message)
		{
			try
			{
				paymentgatewaysetting.IsDeleted = false;
				paymentgatewaysetting.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_paymentgatewaysettingRepository.Add(paymentgatewaysetting);
				if (SavePaymentGatewaySetting())
				{
					message = "PaymentGatewaySetting added successfully ...";
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

		public bool UpdatePaymentGatewaySetting(ref PaymentGatewaySetting paymentgatewaysetting, ref string message)
		{
			try
			{
				PaymentGatewaySetting CurrentPaymentGatewaySetting = _paymentgatewaysettingRepository.GetById(paymentgatewaysetting.ID);

				CurrentPaymentGatewaySetting.Debug = paymentgatewaysetting.Debug;
				CurrentPaymentGatewaySetting.UseSSL = paymentgatewaysetting.UseSSL;
				CurrentPaymentGatewaySetting.IgnoreSSLError = paymentgatewaysetting.IgnoreSSLError;
				CurrentPaymentGatewaySetting.GatewayHost = paymentgatewaysetting.GatewayHost;
				CurrentPaymentGatewaySetting.Version = paymentgatewaysetting.Version;
				CurrentPaymentGatewaySetting.GatewayUrl = paymentgatewaysetting.GatewayUrl;
				CurrentPaymentGatewaySetting.UseProxy = paymentgatewaysetting.UseProxy;
				CurrentPaymentGatewaySetting.ProxyHost = paymentgatewaysetting.ProxyHost;
				CurrentPaymentGatewaySetting.ProxyUser = paymentgatewaysetting.ProxyUser;
				CurrentPaymentGatewaySetting.ProxyPassword = paymentgatewaysetting.ProxyPassword;
				CurrentPaymentGatewaySetting.ProxyDomain = paymentgatewaysetting.ProxyDomain;
				CurrentPaymentGatewaySetting.MerchantID = paymentgatewaysetting.MerchantID;
				CurrentPaymentGatewaySetting.Password = paymentgatewaysetting.Password;
				CurrentPaymentGatewaySetting.UserName = paymentgatewaysetting.UserName;
				CurrentPaymentGatewaySetting.Currency = paymentgatewaysetting.Currency;

				paymentgatewaysetting = null;

				_paymentgatewaysettingRepository.Update(CurrentPaymentGatewaySetting);
				if (SavePaymentGatewaySetting())
				{
					paymentgatewaysetting = CurrentPaymentGatewaySetting;
					message = "PaymentGatewaySetting updated successfully ...";
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

		public bool DeletePaymentGatewaySetting(long id, ref string message)
		{
			try
			{
				PaymentGatewaySetting paymentgatewaysetting = _paymentgatewaysettingRepository.GetById(id);

				_paymentgatewaysettingRepository.Delete(paymentgatewaysetting);
				if (SavePaymentGatewaySetting())
				{
					message = "PaymentGatewaySetting deleted successfully ...";
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

		public bool SavePaymentGatewaySetting()
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

	public interface IPaymentGatewaySettingService
	{
		IEnumerable<PaymentGatewaySetting> GetPaymentGatewaySettings();
		PaymentGatewaySetting GetPaymentGatewaySetting(long id);
		PaymentGatewaySetting GetDefaultPaymentGatewaySetting();
		bool CreatePaymentGatewaySetting(PaymentGatewaySetting paymentgatewaysetting, ref string message);
		bool UpdatePaymentGatewaySetting(ref PaymentGatewaySetting paymentgatewaysetting, ref string message);
		bool DeletePaymentGatewaySetting(long id, ref string message);
		bool SavePaymentGatewaySetting();
	}
}
