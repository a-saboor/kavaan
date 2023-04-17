using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class CouponRedemptionService : ICouponRedemptionService
	{
		private readonly ICouponRedemptionRepository _couponRedemptionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CouponRedemptionService(ICouponRedemptionRepository couponRedemptionRepository, IUnitOfWork unitOfWork)
		{
			this._couponRedemptionRepository = couponRedemptionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICouponRedemptionService Members

		public IEnumerable<CouponRedemption> GetCouponRedemptions()
		{
			var couponRedemptions = _couponRedemptionRepository.GetAll();
			return couponRedemptions;
		}

		public CouponRedemption GetCouponRedemption(long id)
		{
			var couponRedemption = _couponRedemptionRepository.GetById(id);
			return couponRedemption;
		}

		public IEnumerable<CouponRedemption> GetCouponRedemptions(long couponId, long customerId)
		{
			var couponRedemption = _couponRedemptionRepository.GetCouponRedemptions(couponId, customerId);
			return couponRedemption;
		}

		public CouponRedemption GetCouponRedemptionsByBookingID(long couponId, long serviceBookingID)
		{
			var couponRedemption = _couponRedemptionRepository.GetCouponRedemptionsByBookingID(couponId, serviceBookingID);
			return couponRedemption;
		}

		public bool CreateCouponRedemption(CouponRedemption couponRedemption, ref string message)
		{
			try
			{
				couponRedemption.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_couponRedemptionRepository.Add(couponRedemption);
				if (SaveCouponRedemption())
				{
					message = "Coupon Redemption added successfully ...";
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

		public bool DeleteCouponRedemption(long id, ref string message, bool softDelete = true)
		{
			try
			{
				CouponRedemption couponRedemption = _couponRedemptionRepository.GetById(id);

				_couponRedemptionRepository.Delete(couponRedemption);

				if (SaveCouponRedemption())
				{
					message = "Coupon Redemption deleted successfully ...";
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

		public bool SaveCouponRedemption()
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

	public interface ICouponRedemptionService
	{
		IEnumerable<CouponRedemption> GetCouponRedemptions();
		CouponRedemption GetCouponRedemptionsByBookingID(long couponId, long serviceBookingID);
		CouponRedemption GetCouponRedemption(long id);
		IEnumerable<CouponRedemption> GetCouponRedemptions(long couponId, long customerId);
		bool CreateCouponRedemption(CouponRedemption couponRedemption, ref string message);
		bool DeleteCouponRedemption(long id, ref string message, bool softDelete = true);
		bool SaveCouponRedemption();
	}
}
