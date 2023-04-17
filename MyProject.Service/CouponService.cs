using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class CouponService : ICouponService
	{
		private readonly ICouponRepository _couponRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CouponService(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
		{
			this._couponRepository = couponRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICouponService Members

		public IEnumerable<Coupon> GetCoupons(bool? IsOpen)
		{
			var coupons = _couponRepository.GetAll().Where(i => (!IsOpen.HasValue || i.IsOpenToAll == IsOpen) && i.IsDeleted == false).ToList();
			return coupons;
		}

		public IEnumerable<Coupon> GetActiveCoupons(bool? IsOpen)
		{
			var coupons = _couponRepository.GetAll().Where(i => (!IsOpen.HasValue || i.IsOpenToAll == IsOpen) && i.IsDeleted == false && (i.Expiry.HasValue && i.Expiry.Value <= Helpers.TimeZone.GetLocalDateTime())).ToList();
			return coupons;
		}

		public IEnumerable<object> GetCouponsForDropDown(bool? IsOpen)
		{
			var Coupons = GetCoupons(IsOpen);
			var dropdownList = from coupons in Coupons
							   select new { value = coupons.ID, text = coupons.Name };
			return dropdownList;
		}

		public Coupon GetCoupon(long id)
		{
			var coupon = _couponRepository.GetById(id);
			return coupon;
		}

		public Coupon GetCoupon(string couponCode)
		{
			var coupon = _couponRepository.GetCouponByCode(couponCode);
			return coupon;
		}

		public bool CreateCoupon(ref Coupon coupon, ref string message)
		{
			try
			{
				if (_couponRepository.GetCouponByCode(coupon.CouponCode) == null)
				{
					coupon.IsActive = true;
					coupon.IsDeleted = false;
					coupon.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_couponRepository.Add(coupon);
					if (SaveCoupon())
					{
						message = "Coupon added successfully ...";
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
					message = "Coupon already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateCoupon(ref Coupon coupon, ref string message, ref bool isNotifyRequired)
		{
			try
			{
				if (_couponRepository.GetCouponByCode(coupon.CouponCode, coupon.ID) == null)
				{
					Coupon CurrentCoupon = _couponRepository.GetById(coupon.ID);
					if (CurrentCoupon.IsOpenToAll != coupon.IsOpenToAll)
					{
						isNotifyRequired = true;
					}
					else
					{
						isNotifyRequired = false;
					}

					CurrentCoupon.Name = coupon.Name;
					CurrentCoupon.CouponCode = coupon.CouponCode;
					CurrentCoupon.Frequency = coupon.Frequency;
					CurrentCoupon.MaxRedumption = coupon.MaxRedumption;
					CurrentCoupon.Expiry = coupon.Expiry;
					CurrentCoupon.DicountAmount = coupon.DicountAmount;
					CurrentCoupon.DicountPercentage = coupon.DicountPercentage;
					CurrentCoupon.IsOpenToAll = coupon.IsOpenToAll;
					CurrentCoupon.Value = coupon.Value;
					CurrentCoupon.Type = coupon.Type;
					CurrentCoupon.MaxAmount = coupon.MaxAmount;
					coupon = null;

					_couponRepository.Update(CurrentCoupon);
					if (SaveCoupon())
					{
						coupon = CurrentCoupon;
						message = "Coupon updated successfully ...";
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
					message = "Coupon already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteCoupon(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Coupon coupon = _couponRepository.GetById(id);
				if (softDelete)
				{
					coupon.IsDeleted = true;
					_couponRepository.Update(coupon);
				}
				else
				{
					_couponRepository.Delete(coupon);
				}
				if (SaveCoupon())
				{
					message = "Coupon deleted successfully ...";
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

		public bool SaveCoupon()
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

		public bool PostExcelData(string Name, string CouponCode, int Frequency, int MaxRedumption, string type, decimal value, decimal maxAmount, DateTime Expiry, bool IsOpenToAll)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				bool IsActive = true;
				bool IsDeleted = false;

				_couponRepository.InsertCoupons(Name, CouponCode, Frequency, MaxRedumption, type, value, maxAmount, Expiry, IsOpenToAll, CreatedOn, IsActive, IsDeleted);
				return true;
			}
			catch (Exception ex)
			{
				//log.Error("Error", ex);
				//log.Error("Error", ex);
				return false;
			}
		}

		#endregion
	}

	public interface ICouponService
	{
		IEnumerable<Coupon> GetCoupons(bool? IsOpen);
		IEnumerable<Coupon> GetActiveCoupons(bool? IsOpen);
		IEnumerable<object> GetCouponsForDropDown(bool? IsOpen);
		Coupon GetCoupon(long id);
		Coupon GetCoupon(string couponCode);
		bool CreateCoupon(ref Coupon coupon, ref string message);
		bool UpdateCoupon(ref Coupon coupon, ref string message, ref bool isNotifyRequired);
		bool DeleteCoupon(long id, ref string message, bool softDelete = true);
		bool SaveCoupon();

		bool PostExcelData(string Name, string CouponCode, int Frequency, int MaxRedumption, string type, decimal value, decimal maxAmount, DateTime Expiry, bool IsOpenToAll);
	}
}
