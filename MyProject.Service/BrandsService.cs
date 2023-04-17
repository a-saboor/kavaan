using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class BrandsService : IBrandsService
	{
		private readonly IBrandsRepository _brandsRepository;
		private readonly IUnitOfWork _unitOfWork;

		public BrandsService(IBrandsRepository brandsRepository, IUnitOfWork unitOfWork)
		{
			this._brandsRepository = brandsRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<Brand> GetBrands()
		{
			var brands = _brandsRepository.GetAll().Where(x => x.IsDeleted == false);
			return brands;
		}

		public Brand GetBrand(long id)
		{
			var brand = _brandsRepository.GetById(id);
			return brand;
		}

		public Brand GetBrand(string slug)
		{
			var brand = _brandsRepository.GetBrandBySlug(slug);
			return brand;
		}

		public IEnumerable<object> GetBrandsForDropDown()
		{
			var Brands = _brandsRepository.GetAll().Where(x => x.IsDeleted == false);
			var dropdownList = from Brand in Brands
							   select new { value = Brand.ID, text = Brand.Name };
			return dropdownList;
		}

		public bool Createbrands(Brand data, ref string message)
		{
			try
			{
				if (_brandsRepository.GetBrandByName(data.Name) == null)
				{
					data.IsActive = true;
					data.IsDeleted = false;
					data.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_brandsRepository.Add(data);
					if (SaveBrand())
					{
						message = "Brand added successfully ...";
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
					message = "Brand already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateBrand(ref Brand brand, ref string message)
		{
			try
			{
				if (_brandsRepository.GetBrandByName(brand.Name, brand.ID) == null)
				{
					Brand Currentbrand = _brandsRepository.GetById(brand.ID);

					Currentbrand.Name = brand.Name;
					Currentbrand.NameAr = brand.NameAr;
					brand = null;

					_brandsRepository.Update(Currentbrand);
					if (SaveBrand())
					{
						brand = Currentbrand;
						message = "Country updated successfully ...";
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
					message = "Country already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteBrand(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Brand brand = _brandsRepository.GetById(id);

				if (softDelete)
				{
					brand.IsDeleted = true;
					_brandsRepository.Update(brand);
				}
				else
				{
					_brandsRepository.Delete(brand);
				}
				if (SaveBrand())
				{
					message = "Brand deleted successfully ...";
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

		public bool SaveBrand()
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

		//public IEnumerable<SP_GetBrandFilters_Result> GetBrandFilters(long brandId, string lang)
		//{
		//	var filters = _brandsRepository.GetBrandFilters(brandId, lang).ToList();
		//	return filters;
		//}

		//public IEnumerable<SP_GetBrandCategories_Result> GetBrandCategories(long brandId, string lang)
		//{
		//	var brandCategories = _brandsRepository.GetBrandCategories(brandId, lang).ToList();
		//	return brandCategories;
		//}
	}

	public interface IBrandsService
	{
		IEnumerable<Brand> GetBrands();
		Brand GetBrand(long id);
		Brand GetBrand(string slug);
		IEnumerable<object> GetBrandsForDropDown();
		bool Createbrands(Brand data, ref string message);
		bool UpdateBrand(ref Brand brand, ref string message);
		bool DeleteBrand(long id, ref string message, bool softDelete = true);
		bool SaveBrand();

		//IEnumerable<SP_GetBrandFilters_Result> GetBrandFilters(long brandId, string lang);
		//IEnumerable<SP_GetBrandCategories_Result> GetBrandCategories(long brandId, string lang);
	}
}
