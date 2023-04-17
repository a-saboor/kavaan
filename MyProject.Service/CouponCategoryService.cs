using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class CouponCategoryService : ICouponCategoryService
	{
		private readonly ICouponCategoryRepository _couponCategoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CouponCategoryService(ICouponCategoryRepository couponCategoryRepository, IUnitOfWork unitOfWork)
		{
			this._couponCategoryRepository = couponCategoryRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICouponCategoryService Members

		public IEnumerable<CouponCategory> GetCouponCategories()
		{
			var couponCategories = _couponCategoryRepository.GetAll();
			return couponCategories;
		}

		public IEnumerable<CouponCategory> GetCouponCategories(long couponId)
		{
			var couponCategories = _couponCategoryRepository.GetCouponCategories(couponId);
			return couponCategories;
		}

		public CouponCategory GetCouponCategory(long id)
		{
			var couponCategory = _couponCategoryRepository.GetById(id);
			return couponCategory;
		}

		public bool CreateCouponCategory(ref CouponCategory couponCategory, ref string message)
		{
			try
			{
				if (_couponCategoryRepository.GetCouponCategory((long)couponCategory.CouponID, (long)couponCategory.ServiceID) == null)
				{
					_couponCategoryRepository.Add(couponCategory);
					if (SaveCouponCategory())
					{

						message = "Coupon Category added successfully ...";
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
					message = "Coupon Category already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateCouponCategory(ref CouponCategory couponCategory, ref string message)
		{
			try
			{
				if (_couponCategoryRepository.GetCouponCategory((long)couponCategory.CouponID, (long)couponCategory.ServiceID, couponCategory.ID) == null)
				{
					_couponCategoryRepository.Update(couponCategory);
					if (SaveCouponCategory())
					{
						message = "Coupon Category updated successfully ...";
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
					message = "Coupon Category already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteCouponCategory(long id, ref string message)
		{
			try
			{
				CouponCategory couponCategory = _couponCategoryRepository.GetById(id);
				_couponCategoryRepository.Delete(couponCategory);
				if (SaveCouponCategory())
				{
					message = "Coupon Category deleted successfully ...";
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

		public bool DeleteCouponCategory(CouponCategory couponCategory, ref string message)
		{
			try
			{
				couponCategory = _couponCategoryRepository.GetCouponCategory((long)couponCategory.CouponID, (long)couponCategory.ServiceID);
				_couponCategoryRepository.Delete(couponCategory);
				if (SaveCouponCategory())
				{
					message = "Coupon Category deleted successfully ...";
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

		public bool SaveCouponCategory()
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

	public interface ICouponCategoryService
	{
		IEnumerable<CouponCategory> GetCouponCategories();
		IEnumerable<CouponCategory> GetCouponCategories(long couponId);
		CouponCategory GetCouponCategory(long id);
		bool CreateCouponCategory(ref CouponCategory couponCategory, ref string message);
		bool UpdateCouponCategory(ref CouponCategory couponCategory, ref string message);
		bool DeleteCouponCategory(long id, ref string message);
		bool DeleteCouponCategory(CouponCategory couponCategory, ref string message);
		bool SaveCouponCategory();
	}
}
