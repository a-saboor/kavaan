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
	public class TaxSettingService : ITaxSettingService
	{
		private readonly ITaxSettingRepository _taxsettingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public TaxSettingService(ITaxSettingRepository taxsettingRepository, IUnitOfWork unitOfWork)
		{
			this._taxsettingRepository = taxsettingRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ITaxSettingService Members

		public IEnumerable<TaxSetting> GetTaxSettings()
		{
			var taxsettings = _taxsettingRepository.GetAll().Where(i => i.IsActive == true);
            return taxsettings;
		}

		public IEnumerable<object> GetTaxSettingsForDropDown()
		{
			var TaxSettings = _taxsettingRepository.GetAll();
			var dropdownList = from taxsettings in TaxSettings
							   select new { value = taxsettings.ID, text = taxsettings.TaxName };
			return dropdownList;
		}

		public TaxSetting GetTaxSetting(long id)
		{
			var taxsetting = _taxsettingRepository.GetById(id);
			return taxsetting;
		}

		public bool CreateTaxSetting(TaxSetting taxsetting, ref string message)
		{
			try
			{
				if (_taxsettingRepository.GetTaxSettingByName(taxsetting.TaxName) == null)
				{
					taxsetting.IsActive = true;
					//taxsetting.IsDeleted = false;
					//taxsetting.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_taxsettingRepository.Add(taxsetting);
					if (SaveTaxSetting())
					{
						message = "TaxSetting added successfully ...";
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
					message = "TaxSetting already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateTaxSetting(ref TaxSetting taxsetting, ref string message)
		{
			try
			{
				if (_taxsettingRepository.GetTaxSettingByName(taxsetting.TaxName, taxsetting.ID) == null)
				{
					TaxSetting CurrentTaxSetting = _taxsettingRepository.GetById(taxsetting.ID);

					CurrentTaxSetting.TaxName = taxsetting.TaxName;
					CurrentTaxSetting.TaxNameAr = taxsetting.TaxNameAr;
					CurrentTaxSetting.TaxDescription = taxsetting.TaxDescription;
					CurrentTaxSetting.TaxDescriptionAr = taxsetting.TaxDescriptionAr;
					CurrentTaxSetting.TaxPercentage = taxsetting.TaxPercentage;
					taxsetting = null;

					_taxsettingRepository.Update(CurrentTaxSetting);
					if (SaveTaxSetting())
					{
						taxsetting = CurrentTaxSetting;
						message = "TaxSetting updated successfully ...";
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
					message = "TaxSetting already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteTaxSetting(long id, ref string message, bool softDelete = true)
		{
			try
			{
				TaxSetting taxsetting = _taxsettingRepository.GetById(id);
				if (softDelete)
				{
					//taxsetting.IsDeleted = true;
					_taxsettingRepository.Update(taxsetting);
				}
				else
				{
					_taxsettingRepository.Delete(taxsetting);
				}

				if (SaveTaxSetting())
				{
					message = "TaxSetting deleted successfully ...";
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

		public decimal GetTotalTax()
		{
			var taxsettings = _taxsettingRepository.GetAll().Where(i => i.IsActive == true && i.IsActive == true).Sum(i => i.TaxPercentage);

			return taxsettings.HasValue ? taxsettings.Value : 0m;
		}

		public bool SaveTaxSetting()
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

	public interface ITaxSettingService
	{
		IEnumerable<TaxSetting> GetTaxSettings();
		IEnumerable<object> GetTaxSettingsForDropDown();
		TaxSetting GetTaxSetting(long id);
		bool CreateTaxSetting(TaxSetting taxsetting, ref string message);
		bool UpdateTaxSetting(ref TaxSetting taxsetting, ref string message);
		bool DeleteTaxSetting(long id, ref string message, bool softDelete = true);
		decimal GetTotalTax();
		bool SaveTaxSetting();
	}
}
