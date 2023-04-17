
using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class BusinessBranchSettingService : IBusinessBranchSettingService
	{
		private readonly IBusinessBranchSettingRepository _businessbranchsettingRepository;
		private readonly IUnitOfWork _unitOfWork;
		public BusinessBranchSettingService(IBusinessBranchSettingRepository businessbranchsettingRepository, IUnitOfWork unitOfWork)
		{
			this._businessbranchsettingRepository = businessbranchsettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region Business Branch Settings
		public IEnumerable<BusinessBranchSetting> GetBusinessBranchSettings(long businessbranchSettingId)
		{
			var businessbranchSetting = _businessbranchsettingRepository.GetAll().Where(x => x.BusinessSettingID == businessbranchSettingId && x.IsDeleted == false);
			return businessbranchSetting;
		}
		public BusinessBranchSetting GetBusinessBranchSetting(long id)
		{
			var businessbranchSetting = _businessbranchsettingRepository.GetById(id);
			return businessbranchSetting;
		}
		public bool CreateBusinessBranchSetting(BusinessBranchSetting businessbranchSetting, ref string message)
		{
			try
			{
				businessbranchSetting.IsDeleted = false;
				businessbranchSetting.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_businessbranchsettingRepository.Add(businessbranchSetting);
				if (SaveBusinessBranchSetting())
				{
					message = "Business Branch Settings added successfully ...";
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
		public bool UpdateBusinessBranchSetting(ref BusinessBranchSetting businessbranchSetting, ref string message)
		{
			try
			{
				BusinessBranchSetting CurrentBusinessBranchSetting = _businessbranchsettingRepository.GetById(businessbranchSetting.ID);

				CurrentBusinessBranchSetting.Name = businessbranchSetting.Name;
				CurrentBusinessBranchSetting.NameAr=businessbranchSetting.NameAr;	
				CurrentBusinessBranchSetting.MapIframe = businessbranchSetting.MapIframe;
				CurrentBusinessBranchSetting.StreetAddress = businessbranchSetting.StreetAddress;
				CurrentBusinessBranchSetting.StreetAddressAr = businessbranchSetting.StreetAddressAr;
				CurrentBusinessBranchSetting.Contact = businessbranchSetting.Contact;
				CurrentBusinessBranchSetting.Contact2 = businessbranchSetting.Contact2;
				CurrentBusinessBranchSetting.Fax = businessbranchSetting.Fax;
				CurrentBusinessBranchSetting.Email = businessbranchSetting.Email;
				CurrentBusinessBranchSetting.WorkingDays = businessbranchSetting.WorkingDays;

				businessbranchSetting = null;

				_businessbranchsettingRepository.Update(CurrentBusinessBranchSetting);
				if (SaveBusinessBranchSetting())
				{
					businessbranchSetting = CurrentBusinessBranchSetting;
					message = "Business Branch Settings updated successfully ...";
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
		public bool DeleteBusinessBranchSetting(long id, ref string message)
		{
			try
			{
				BusinessBranchSetting businessbranchsetting = _businessbranchsettingRepository.GetById(id);

				_businessbranchsettingRepository.Delete(businessbranchsetting);
				if (SaveBusinessBranchSetting())
				{
					message = "Business Branch Settings deleted successfully ...";
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
		public bool SaveBusinessBranchSetting()
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

	public interface IBusinessBranchSettingService
	{
		IEnumerable<BusinessBranchSetting> GetBusinessBranchSettings(long businessSettingId);
		BusinessBranchSetting GetBusinessBranchSetting(long id);
		bool CreateBusinessBranchSetting(BusinessBranchSetting businessbranchSetting, ref string message);
		bool UpdateBusinessBranchSetting(ref BusinessBranchSetting businessbranchSetting, ref string message);
		bool DeleteBusinessBranchSetting(long id, ref string message);
		bool SaveBusinessBranchSetting();
	}
}