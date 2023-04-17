using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class BusinessSettingService : IBusinessSettingService
	{
		private readonly IBusinessSettingRepository _businesssettingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public BusinessSettingService(IBusinessSettingRepository businesssettingRepository, IUnitOfWork unitOfWork)
		{
			this._businesssettingRepository = businesssettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IBusinessSettingService Members

		public IEnumerable<BusinessSetting> GetBusinessSettings()
		{
			var businesssetting = _businesssettingRepository.GetAll();
			return businesssetting;
		}

		public BusinessSetting GetBusinessSetting(long id)
		{
			var businesssetting = _businesssettingRepository.GetById(id);
			return businesssetting;
		}

		public BusinessSetting GetDefaultBusinessSetting()
		{
			var businesssetting = _businesssettingRepository.GetAll().FirstOrDefault();
			return businesssetting;
		}

		public bool CreateBusinessSetting(BusinessSetting businesssetting, ref string message)
		{
			try
			{
				//businesssetting.IsDeleted = false;
				//businesssetting.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_businesssettingRepository.Add(businesssetting);
				if (SaveBusinessSetting())
				{
					message = "Business setting added successfully ...";
					return true;

				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}

			}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
			catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateBusinessSetting(ref BusinessSetting businesssetting, ref string message)
		{
			try
			{
				BusinessSetting CurrentBusinessSetting = _businesssettingRepository.GetById(businesssetting.ID);

				CurrentBusinessSetting.Whatsapp = businesssetting.Whatsapp;
				CurrentBusinessSetting.FIrstMessage= businesssetting.FIrstMessage;
				CurrentBusinessSetting.MapIframe=businesssetting.MapIframe;
				CurrentBusinessSetting.Title = businesssetting.Title;
				CurrentBusinessSetting.TitleAr = businesssetting.TitleAr;
				CurrentBusinessSetting.StreetAddress = businesssetting.StreetAddress;
				CurrentBusinessSetting.StreetAddressAr = businesssetting.StreetAddressAr;
				CurrentBusinessSetting.Contact = businesssetting.Contact;
				CurrentBusinessSetting.Contact2 = businesssetting.Contact2;
				CurrentBusinessSetting.Fax = businesssetting.Fax;
				CurrentBusinessSetting.Email = businesssetting.Email;
				CurrentBusinessSetting.WorkingDays = businesssetting.WorkingDays;
				CurrentBusinessSetting.Facebook = businesssetting.Facebook;
				CurrentBusinessSetting.Instagram = businesssetting.Instagram;
				CurrentBusinessSetting.Youtube = businesssetting.Youtube;
				CurrentBusinessSetting.Twitter = businesssetting.Twitter;
				CurrentBusinessSetting.Snapchat = businesssetting.Snapchat;
				CurrentBusinessSetting.LinkedIn = businesssetting.LinkedIn;
				CurrentBusinessSetting.Behnace = businesssetting.Behnace;
				CurrentBusinessSetting.Pinterest = businesssetting.Pinterest;
				CurrentBusinessSetting.AndroidAppUrl = businesssetting.AndroidAppUrl;
				CurrentBusinessSetting.IosAppUrl = businesssetting.IosAppUrl;
				CurrentBusinessSetting.FCMKey = businesssetting.FCMKey;
				CurrentBusinessSetting.CustomerFCMKey = businesssetting.CustomerFCMKey;
				CurrentBusinessSetting.WorkForceFCMKey = businesssetting.WorkForceFCMKey;
				CurrentBusinessSetting.PartnerFCMKey = businesssetting.PartnerFCMKey;
				CurrentBusinessSetting.GoogleMapKey = businesssetting.GoogleMapKey;
				businesssetting = null;

				_businesssettingRepository.Update(CurrentBusinessSetting);
				if (SaveBusinessSetting())
				{
					businesssetting = CurrentBusinessSetting;
					message = "Business setting updated successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}

			}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
			catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteBusinessSetting(long id, ref string message)
		{
			try
			{
				BusinessSetting businesssetting = _businesssettingRepository.GetById(id);

				_businesssettingRepository.Delete(businesssetting);
				if (SaveBusinessSetting())
				{
					message = "Business setting deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
			catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool SaveBusinessSetting()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
			catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
			{
				return false;
			}
		}

		#endregion

	}

	public interface IBusinessSettingService
	{
		IEnumerable<BusinessSetting> GetBusinessSettings();
		BusinessSetting GetBusinessSetting(long id);
		BusinessSetting GetDefaultBusinessSetting();
		bool CreateBusinessSetting(BusinessSetting businesssetting, ref string message);
		bool UpdateBusinessSetting(ref BusinessSetting businesssetting, ref string message);
		bool DeleteBusinessSetting(long id, ref string message);
		bool SaveBusinessSetting();
	}
}
