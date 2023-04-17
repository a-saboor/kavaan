using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{
	class CustomerCouponsService : ICustomerCouponsService
	{
		private readonly ICustomerCouponsRepository _customerCouponsRepository;
		private readonly ICouponRepository _couponRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CustomerCouponsService(ICustomerCouponsRepository customerCouponsRepository, ICouponRepository couponRepository, IUnitOfWork unitOfWork)
		{
			this._customerCouponsRepository = customerCouponsRepository;
			this._couponRepository = couponRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<SP_GetCustomerCoupons_Result> GetSPCustomerCoupons(long customerID, int pageNumber = 1)
		{
			var Coupons = _customerCouponsRepository.GetCustomerCoupons(customerID, pageNumber);
			return Coupons;
		}

		public CustomerCoupon GetCoupon(long CustomerID, long CouponsID)
		{
			var coupons = _customerCouponsRepository.GetAll().Where(x => x.CustomerID == CustomerID && x.CouponID == CouponsID).FirstOrDefault();
			return coupons;
		}

		public IEnumerable<CustomerCoupon> GetCouponsByCouponID(long CouponsID)
		{
			var coupons = _customerCouponsRepository.GetAll().Where(x => x.CouponID == CouponsID).ToList();
			return coupons;
		}

		public int GetCustomerCouponsCount(long customerID)
		{
			return this.GetCustomerCoupons(customerID).Count();
		}

		public IEnumerable<Coupon> GetCustomerCoupons(long customerID)
		{
			var customercoupons = _customerCouponsRepository.GetAll().Where(x => x.CustomerID == customerID).Select(i => i.CouponID).ToList();

			var coupons = _couponRepository.GetAll().Where(x => (customercoupons.Contains(x.ID) || x.IsOpenToAll == true) && x.IsDeleted == false && x.IsActive == true).ToList();
			return coupons;
		}

		public bool CreateCustomerCoupon(CustomerCoupon coupon, ref string message)
		{
			try
			{
				var getdata = _customerCouponsRepository.GetAll().Where(x => x.CustomerID == coupon.CustomerID && x.CouponID == coupon.CouponID).FirstOrDefault();

				if (getdata == null)
				{
					coupon.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_customerCouponsRepository.Add(coupon);
					if (SaveCustomerCoupon())
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
					message = "Customer Already added in this offer!";
					return false;
				}

			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool SaveCustomerCoupon()
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

		public bool DeleteCustomerCoupon(long id, ref string message, bool softDelete = true)
		{
			try
			{
				CustomerCoupon coupon = _customerCouponsRepository.GetById(id);
				if (softDelete)
				{

					_customerCouponsRepository.Delete(coupon);
					SaveCustomerCoupon();
					return true;
				}


				return false;
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

	}
	public interface ICustomerCouponsService
	{
		IEnumerable<SP_GetCustomerCoupons_Result> GetSPCustomerCoupons(long customerID, int pageNumber = 1);
		CustomerCoupon GetCoupon(long CustomerID, long CouponsID);
		IEnumerable<CustomerCoupon> GetCouponsByCouponID(long CouponsID);
		int GetCustomerCouponsCount(long customerID);
		IEnumerable<Coupon> GetCustomerCoupons(long customerID);
		bool CreateCustomerCoupon(CustomerCoupon coupon, ref string message);
		bool DeleteCustomerCoupon(long id, ref string message, bool softDelete = true);
		bool SaveCustomerCoupon();
	}
}
