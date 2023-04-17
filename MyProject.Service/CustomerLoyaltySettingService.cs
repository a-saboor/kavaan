using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class CustomerLoyaltySettingService : ICustomerLoyaltySettingService
	{
		private readonly ICustomerLoyaltySettingRepository _CustomerLoyaltySettingServiceRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CustomerLoyaltySettingService(ICustomerLoyaltySettingRepository CustomerLoyaltySettingRepository, IUnitOfWork unitOfWork)
		{
			this._CustomerLoyaltySettingServiceRepository = CustomerLoyaltySettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICustomerLoyaltySettingService Members

		public IEnumerable<CustomerLoyaltySetting> GetList()
		{
			var getdata = _CustomerLoyaltySettingServiceRepository.GetAll().Where(i => i.IsDeleted == false);
			return getdata;
		}

		public CustomerLoyaltySetting GetCustomerLoyaltySetting(string type)
		{
			var CustomerLoyaltySetting = _CustomerLoyaltySettingServiceRepository.GetCustomerLoyaltySettingByType(type);
			return CustomerLoyaltySetting;
		}

		public IEnumerable<object> GetCustomerTypeForDropDown()
		{
			var settings = GetList().OrderBy(i => i.CustomerTypeMaxSlab).Select(i => new { value = i.CustomerType, text = i.CustomerType }).Distinct();
			return settings;
		}

		public CustomerLoyaltySetting GetSettings(long id)
		{
			var data = _CustomerLoyaltySettingServiceRepository.GetById(id);
			return data;
		}

		public bool CreateCustomerLoyaltySetting(CustomerLoyaltySetting Create, ref string message)
		{
			try
			{
				if (_CustomerLoyaltySettingServiceRepository.GetCustomerLoyaltySettingByType(Create.CustomerType) == null)
				{


					Create.IsActive = true;
					Create.IsDeleted = false;
					Create.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_CustomerLoyaltySettingServiceRepository.Add(Create);
					if (SaveCustomerLoyaltySetting())
					{
						message = "Customer loyalty setting successfully added ...";
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
					message = "Customer loyalty already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateCustomerLoyaltySettings(ref CustomerLoyaltySetting data, ref string message)
		{
			try
			{
				if (_CustomerLoyaltySettingServiceRepository.GetCustomerLoyaltySettingByType(data.CustomerType, data.ID) == null)
				{
					CustomerLoyaltySetting CurrentSettings = _CustomerLoyaltySettingServiceRepository.GetById(data.ID);

					CurrentSettings.CustomerType = data.CustomerType;
					CurrentSettings.PGRatio = data.PGRatio;
					CurrentSettings.PRRatio = data.PRRatio;
					CurrentSettings.PointsLimit = data.PointsLimit;
					CurrentSettings.CustomerTypeMaxSlab = data.CustomerTypeMaxSlab;
					CurrentSettings.ReferralPoint = data.ReferralPoint;
					data = null;

					_CustomerLoyaltySettingServiceRepository.Update(CurrentSettings);
					if (SaveCustomerLoyaltySetting())
					{
						data = CurrentSettings;
						message = "Customer loyalty setting updated successfully ...";
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
					message = "Customer loyalty already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteCustomerLoyaltySettig(long id, ref string message, bool softDelete = true)
		{
			try
			{
				CustomerLoyaltySetting settings = _CustomerLoyaltySettingServiceRepository.GetById(id);

				if (softDelete)
				{
					settings.IsDeleted = true;
					_CustomerLoyaltySettingServiceRepository.Update(settings);
				}
				else
				{
					_CustomerLoyaltySettingServiceRepository.Delete(settings);
				}
				if (SaveCustomerLoyaltySetting())
				{
					message = "Customer loyalty settings deleted successfully ...";
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

		public bool SaveCustomerLoyaltySetting()
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
	public interface ICustomerLoyaltySettingService
	{
		IEnumerable<CustomerLoyaltySetting> GetList();
		bool CreateCustomerLoyaltySetting(CustomerLoyaltySetting data, ref string message);
		CustomerLoyaltySetting GetSettings(long id);
		IEnumerable<object> GetCustomerTypeForDropDown();
		CustomerLoyaltySetting GetCustomerLoyaltySetting(string type);
		bool UpdateCustomerLoyaltySettings(ref CustomerLoyaltySetting data, ref string message);
		bool DeleteCustomerLoyaltySettig(long id, ref string message, bool softDelete = true);
		bool SaveCustomerLoyaltySetting();
	}
}
