using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

	class ServiceRatingService : IServiceRatingService
	{
		private readonly IServiceRatingRepository _ServiceRatingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ServiceRatingService(IServiceRatingRepository ServiceRatingsRepository, IUnitOfWork unitOfWork)
		{
			this._ServiceRatingRepository = ServiceRatingsRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<ServiceRating> GetServiceRatingsByStatus(bool Status)
		{
			var ServiceRating = _ServiceRatingRepository.GetAll().Where(i => i.IsDeleted == false && i.IsApproved == Status);
			return ServiceRating;
		}
		public IEnumerable<ServiceRating> GetServiceRatings()
		{
			var ServiceRating = _ServiceRatingRepository.GetAll().Where(i => i.IsDeleted == false);
			return ServiceRating;
		}
		public ServiceRating GetServiceRatingByBookingId(long bookingId)
		{
			var ServiceRating = _ServiceRatingRepository.GetAll().Where(i => i.ServiceBookingID == bookingId).FirstOrDefault();
			return ServiceRating;
		}

		public IEnumerable<ServiceRating> GetServiceRatings(long ServiceId)
		{
			var ServiceRating = _ServiceRatingRepository.GetServiceRatings(ServiceId).Where(i => i.IsDeleted == false);
			return ServiceRating;
		}

		public IEnumerable<ServiceRating> GetServiceRatings(long ServiceId, bool? isApproved)
		{
			var ServiceRatings = _ServiceRatingRepository.GetServiceRatings(ServiceId, isApproved);
			return ServiceRatings;
		}

		public List<SP_GetserviceRatings_Result> GetServiceRatingsbyserviceId(long ServiceId)
		{
			var ServiceRatings = _ServiceRatingRepository.GetServiceRatingsbyServiceID(ServiceId).ToList();
			return ServiceRatings;
		}

		public ServiceRating GetServiceRatingsByServiceBooking(long serviceBookingID)
		{
			var ServiceRating = _ServiceRatingRepository.GetServiceRatingByServiceBooking(serviceBookingID);
			return ServiceRating;
		}

		public ServiceRating GetServiceRating(long id)
		{
			var ServiceRating = _ServiceRatingRepository.GetById(id);
			return ServiceRating;
		}

		public bool CreateServiceRatings(ServiceRating ServiceRating, ref string message)
		{
			try
			{
				ServiceRating.IsActive = true;
				ServiceRating.IsDeleted = false;
				ServiceRating.CreatedOn = MyProject.Service.Helpers.TimeZone.GetLocalDateTime();
				_ServiceRatingRepository.Add(ServiceRating);
				if (SaveServiceRating())
				{
					message = "ServiceRating added successfully ...";
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

		public bool UpdateServiceRating(ref ServiceRating ServiceRating, ref string message)
		{
			try
			{
				ServiceRating CurrentServiceRating = _ServiceRatingRepository.GetById(ServiceRating.ID);

				CurrentServiceRating.CustomerID = ServiceRating.CustomerID;
				CurrentServiceRating.ServiceID = ServiceRating.ServiceID;
				CurrentServiceRating.Rating = ServiceRating.Rating;
				CurrentServiceRating.Review = ServiceRating.Review;
				CurrentServiceRating.IsApproved = ServiceRating.IsApproved;

				ServiceRating = null;

				_ServiceRatingRepository.Update(CurrentServiceRating);
				if (SaveServiceRating())
				{
					ServiceRating = CurrentServiceRating;
					message = "ServiceRating updated successfully ...";
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

		public bool DeleteServiceRating(long id, ref string message, bool softDelete = true)
		{
			try
			{
				ServiceRating ServiceRating = _ServiceRatingRepository.GetById(id);

				if (softDelete)
				{
					ServiceRating.IsDeleted = true;
					_ServiceRatingRepository.Update(ServiceRating);
				}
				else
				{
					_ServiceRatingRepository.Delete(ServiceRating);
				}
				if (SaveServiceRating())
				{
					message = "ServiceRating deleted successfully ...";
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

		public bool SaveServiceRating()
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

	}
	public interface IServiceRatingService
	{
		IEnumerable<ServiceRating> GetServiceRatings();
		IEnumerable<ServiceRating> GetServiceRatingsByStatus(bool Status);
		ServiceRating GetServiceRatingByBookingId(long bookingId);
		IEnumerable<ServiceRating> GetServiceRatings(long ServiceId, bool? isApproved);
		IEnumerable<ServiceRating> GetServiceRatings(long ServiceId);
		List<SP_GetserviceRatings_Result> GetServiceRatingsbyserviceId(long ServiceId);
		ServiceRating GetServiceRatingsByServiceBooking(long serviceBookingID);
		ServiceRating GetServiceRating(long id);
		bool CreateServiceRatings(ServiceRating ServiceRating, ref string message);
		bool UpdateServiceRating(ref ServiceRating ServiceRating, ref string message);
		bool DeleteServiceRating(long id, ref string message, bool softDelete = true);
		bool SaveServiceRating();
	}
}
