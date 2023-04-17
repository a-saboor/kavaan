using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class VendorSectionService : IVendorSectionService
	{
		private readonly IVendorSectionRepository _vendorSectionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorSectionService(IVendorSectionRepository vendorSectionRepository, IUnitOfWork unitOfWork)
		{
			this._vendorSectionRepository = vendorSectionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorSectionService Members

		public IEnumerable<VendorSection> GetVendorSections()
		{
			var VendorSections = _vendorSectionRepository.GetAll();
			var filterVendorSections = _vendorSectionRepository.GetAll().Where(x => x.IsDeleted == false);
			return filterVendorSections;
		}

		public IEnumerable<object> GetVendorSectionsForDropDown()
		{
			var VendorSections = _vendorSectionRepository.GetAll();
			var dropdownList = from VendorSection in VendorSections
							   select new { value = VendorSection.ID, text = VendorSection.Name };
			return dropdownList;
		}

		public VendorSection GetVendorSection(long id)
		{
			var VendorSection = _vendorSectionRepository.GetById(id);
			return VendorSection;
		}

		public bool CreateVendorSection(ref VendorSection VendorSection, ref string message)
		{
			try
			{
				if (_vendorSectionRepository.GetTagByName(VendorSection.Name) == null)
				{
					VendorSection.IsActive = true;
					VendorSection.IsDeleted = false;
					VendorSection.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_vendorSectionRepository.Add(VendorSection);
					if (SaveVendorSection())
					{

						message = "VendorSection added successfully ...";
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
					message = "VendorSection already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorSection(ref VendorSection VendorSection, ref string message)
		{
			try
			{
				if (_vendorSectionRepository.GetTagByName(VendorSection.Name, VendorSection.ID) == null)
				{
					VendorSection CurrentVendorSection = _vendorSectionRepository.GetById(VendorSection.ID);

					CurrentVendorSection.Name = VendorSection.Name;
					CurrentVendorSection.NameAr = VendorSection.NameAr;

					VendorSection = null;

					_vendorSectionRepository.Update(CurrentVendorSection);
					if (SaveVendorSection())
					{
						VendorSection = CurrentVendorSection;
						message = "VendorSection updated successfully ...";
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
					message = "VendorSection already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorSection(long id, ref string message, bool softDelete = true)
		{
			try
			{
				VendorSection VendorSection = _vendorSectionRepository.GetById(id);
				if (softDelete)
				{
					VendorSection.IsDeleted = true;
					_vendorSectionRepository.Update(VendorSection);
				}
				else
				{
					_vendorSectionRepository.Delete(VendorSection);
				}
				if (SaveVendorSection())
				{
					message = "VendorSection deleted successfully ...";
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

		public bool SaveVendorSection()
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

	public interface IVendorSectionService
	{
		IEnumerable<VendorSection> GetVendorSections();
		IEnumerable<object> GetVendorSectionsForDropDown();
		VendorSection GetVendorSection(long id);
		bool CreateVendorSection(ref VendorSection VendorSection, ref string message);
		bool UpdateVendorSection(ref VendorSection VendorSection, ref string message);
		bool DeleteVendorSection(long id, ref string message, bool softDelete = true);
		bool SaveVendorSection();
	}
}
