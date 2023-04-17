using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class SubscribersService : ISubscribersService
	{
		private readonly ISubscribersRepository _subscribersRepository;
		private readonly IUnitOfWork _unitOfWork;

		public SubscribersService(ISubscribersRepository subscribersRepository, IUnitOfWork unitOfWork)
		{
			this._subscribersRepository = subscribersRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<Subscriber> GetSubscribers()
		{
			var Subscriber = _subscribersRepository.GetAll();
			return Subscriber;
		}
		public Subscriber GetSubscriberByID(long id)
		{
			var Subscriber = _subscribersRepository.GetById(id);
			return Subscriber;
		}
		public IEnumerable<Subscriber> Getsubscribers()
		{
            DateTime ToDate= Helpers.TimeZone.GetLocalDateTime();
          
            DateTime FromDate = ToDate.AddDays(-(int)ToDate.DayOfWeek - 6);
            var data = _subscribersRepository.GetFilteredSubscribers(FromDate, ToDate);
			return data;
		}
        public List<Subscriber> GetsubscribersDateWise(DateTime FromDate,DateTime ToDate)
        {
            var Subscribers = _subscribersRepository.GetFilteredSubscribers(FromDate, ToDate);
            return Subscribers;
        }
		public IEnumerable<object> GetSubscribersForDropDown()
		{
			var subscribers = _subscribersRepository.GetAll();
			var dropdownList = from subscriber in subscribers
							   select new { value = subscriber.ID, text = subscriber.Email };
			return dropdownList;
		}

		public bool CreateSubscriber(ref Subscriber subscriber, ref string message)
		{
			try
			{
				if (_subscribersRepository.GetSubscriberByEmail(subscriber.Email) == null)
				{
					subscriber.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_subscribersRepository.Add(subscriber);
					if (SaveSubscriber())
					{
						message = "Subscriber added successfully ...";
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
					message = "Subscriber already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool SaveSubscriber()
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
	public interface ISubscribersService
	{
		Subscriber GetSubscriberByID(long id);
		IEnumerable<object> GetSubscribersForDropDown();
		IEnumerable<Subscriber> Getsubscribers();

		List<Subscriber> GetsubscribersDateWise(DateTime FromDate, DateTime ToDate);
		bool CreateSubscriber(ref Subscriber subscriber, ref string message);
		bool SaveSubscriber();
	}
}
