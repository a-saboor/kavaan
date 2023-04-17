using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class VendorTypeService : IVendorTypeService
	{
		private readonly IVendorTypeRepository _vendorTypeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorTypeService(IVendorTypeRepository vendorTypeRepository, IUnitOfWork unitOfWork)
		{
			this._vendorTypeRepository = vendorTypeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorTypeService Members

		public IEnumerable<VendorType> GetVendorTypes()
		{
			var VendorTypes = _vendorTypeRepository.GetAll();
			var filterVendorTypes = _vendorTypeRepository.GetAll().Where(x => x.IsDeleted == false);
			return filterVendorTypes;
		}

		public IEnumerable<object> GetVendorTypesForDropDown()
		{
			var VendorTypes = _vendorTypeRepository.GetAll();
			var dropdownList = from VendorType in VendorTypes
							   select new { value = VendorType.ID, text = VendorType.Name };
			return dropdownList;
		}

		public VendorType GetVendorType(long id)
		{
			var VendorType = _vendorTypeRepository.GetById(id);
			return VendorType;
		}
		//public IEnumerable<object> GetVendorTypeForDropDown(string lang = "en")
		//{
		//	var types = GetVendorTypes().Where(x => x.IsActive == true && x.IsDeleted == false);
		//	var dropdownList = from type in types
		//					   select new { value = type.ID, text = lang == "en" ? type.Name : type.NameAr };
		//	return dropdownList;
		//}
		public bool CreateVendorType(ref VendorType VendorType, ref string message)
		{
			try
			{
				if (_vendorTypeRepository.GetTagByName(VendorType.Name) == null)
				{
					VendorType.IsActive = true;
					VendorType.IsDeleted = false;
					VendorType.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_vendorTypeRepository.Add(VendorType);
					if (SaveVendorType())
					{

						message = "Vendor Type added successfully ...";
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
					message = "Vendor Type already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorType(ref VendorType VendorType, ref string message)
		{
			try
			{
				if (_vendorTypeRepository.GetTagByName(VendorType.Name, VendorType.ID) == null)
				{
					VendorType CurrentVendorType = _vendorTypeRepository.GetById(VendorType.ID);

					CurrentVendorType.Name = VendorType.Name;
					CurrentVendorType.NameAr = VendorType.NameAr;

					VendorType = null;

					_vendorTypeRepository.Update(CurrentVendorType);
					if (SaveVendorType())
					{
						VendorType = CurrentVendorType;
						message = "VendorType updated successfully ...";
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
					message = "VendorType already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorType(long id, ref string message, bool softDelete = true)
		{
			try
			{
				VendorType VendorType = _vendorTypeRepository.GetById(id);
				if (softDelete)
				{
					VendorType.IsDeleted = true;
					_vendorTypeRepository.Update(VendorType);
				}
				else
				{
					_vendorTypeRepository.Delete(VendorType);
				}
				if (SaveVendorType())
				{
					message = "VendorType deleted successfully ...";
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

		public bool SaveVendorType()
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

	public interface IVendorTypeService
	{
		IEnumerable<VendorType> GetVendorTypes();
		IEnumerable<object> GetVendorTypesForDropDown();
		VendorType GetVendorType(long id);
		bool CreateVendorType(ref VendorType VendorType, ref string message);
		bool UpdateVendorType(ref VendorType VendorType, ref string message);
		bool DeleteVendorType(long id, ref string message, bool softDelete = true);
		bool SaveVendorType();
	}
}
