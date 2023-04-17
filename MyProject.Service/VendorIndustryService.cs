using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class VendorIndustryService : IVendorIndustryService
	{
		private readonly IVendorIndustryRepository _vendorIndustryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorIndustryService(IVendorIndustryRepository vendorIndustryRepository, IUnitOfWork unitOfWork)
		{
			this._vendorIndustryRepository = vendorIndustryRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorIndustryService Members

		public IEnumerable<VendorIndustry> GetVendorIndustrys()
		{
			var VendorIndustrys = _vendorIndustryRepository.GetAll();
			var filterVendorIndustrys = _vendorIndustryRepository.GetAll().Where(x => x.IsDeleted == false);
			return filterVendorIndustrys;
		}

		public IEnumerable<object> GetVendorIndustrysForDropDown()
		{
			var VendorIndustrys = _vendorIndustryRepository.GetAll();
			var dropdownList = from VendorIndustry in VendorIndustrys
							   select new { value = VendorIndustry.ID, text = VendorIndustry.Name };
			return dropdownList;
		}

		public VendorIndustry GetVendorIndustry(long id)
		{
			var VendorIndustry = _vendorIndustryRepository.GetById(id);
			return VendorIndustry;
		}

		public bool CreateVendorIndustry(ref VendorIndustry VendorIndustry, ref string message)
		{
			try
			{
				if (_vendorIndustryRepository.GetTagByName(VendorIndustry.Name) == null)
				{
					VendorIndustry.IsActive = true;
					VendorIndustry.IsDeleted = false;
					VendorIndustry.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_vendorIndustryRepository.Add(VendorIndustry);
					if (SaveVendorIndustry())
					{

						message = "Vendor Industry added successfully ...";
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
					message = "VendorIndustry already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorIndustry(ref VendorIndustry VendorIndustry, ref string message)
		{
			try
			{
				if (_vendorIndustryRepository.GetTagByName(VendorIndustry.Name, VendorIndustry.ID) == null)
				{
					VendorIndustry CurrentVendorIndustry = _vendorIndustryRepository.GetById(VendorIndustry.ID);

					CurrentVendorIndustry.Name = VendorIndustry.Name;
					CurrentVendorIndustry.NameAr = VendorIndustry.NameAr;

					VendorIndustry = null;

					_vendorIndustryRepository.Update(CurrentVendorIndustry);
					if (SaveVendorIndustry())
					{
						VendorIndustry = CurrentVendorIndustry;
						message = "Vendor Industry updated successfully ...";
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
					message = "Vendor Industry already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorIndustry(long id, ref string message, bool softDelete = true)
		{
			try
			{
				VendorIndustry VendorIndustry = _vendorIndustryRepository.GetById(id);
				if (softDelete)
				{
					VendorIndustry.IsDeleted = true;
					_vendorIndustryRepository.Update(VendorIndustry);
				}
				else
				{
					_vendorIndustryRepository.Delete(VendorIndustry);
				}
				if (SaveVendorIndustry())
				{
					message = "VendorIndustry deleted successfully ...";
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

		public bool SaveVendorIndustry()
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

	public interface IVendorIndustryService
	{
		IEnumerable<VendorIndustry> GetVendorIndustrys();
		IEnumerable<object> GetVendorIndustrysForDropDown();
		VendorIndustry GetVendorIndustry(long id);
		bool CreateVendorIndustry(ref VendorIndustry VendorIndustry, ref string message);
		bool UpdateVendorIndustry(ref VendorIndustry VendorIndustry, ref string message);
		bool DeleteVendorIndustry(long id, ref string message, bool softDelete = true);
		bool SaveVendorIndustry();
	}
}
