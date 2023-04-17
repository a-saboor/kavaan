using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Service
{
    public class EventsService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventsService(IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Event> GetEvent()
        {
            var Newsfeed = _eventRepository.GetAll().Where(d => d.IsDeleted == false);
            return Newsfeed;
        }

        public Event GetEventByID(long id)
        {
            var Event = _eventRepository.GetById(id);
            return Event;
        }

        public Event GetEventBySlug(string slug)
        {
            var Event = _eventRepository.GeteventsBySlug(slug);
         
            return Event;
        }

        public bool CreateEvent(Event newsFeed, ref string message)
        {
            try
            {
                if (_eventRepository.GeteventsBySlug(newsFeed.Title) == null)
                {
                    newsFeed.IsActive = true;

                    newsFeed.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _eventRepository.Add(newsFeed);
                    if (SaveNewsfeed())
                    {
                        message = "Event added successfully ...";
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
                    message = "Event already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateEvent(ref Event newsFeed, ref string message)
        {
            try
            {
                if (_eventRepository.GeteventsByTitle(newsFeed.Title, newsFeed.ID) == null)
                {
                    Event CurrentNews = _eventRepository.GetById(newsFeed.ID);

                    CurrentNews.IsActive = newsFeed.IsActive;
                    _eventRepository.Update(CurrentNews);
                    if (SaveNewsfeed())
                    {
                        newsFeed = CurrentNews;
                        message = "Event updated successfully ...";
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
                    message = "Event already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteEvent(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {

                    //soft delete
                    Event Event = _eventRepository.GetById(id);
                    Event.IsDeleted = true;
                    _eventRepository.Update(Event);

                    if (SaveNewsfeed())
                    {
                        message = "Event deleted successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            else
            {

                //hard delete
                try
                {
                    Event Event = _eventRepository.GetById(id);

                    _eventRepository.Delete(Event);

                    if (SaveNewsfeed())
                    {
                        message = "Event deleted successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
        }

        public bool SaveNewsfeed()
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

		public IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEvents(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var Events = _eventRepository.GetFilteredEvents(search, pageSize, pageNumber, sortBy, lang, imageServer);
			return Events;
		}

	}
    public interface IEventService
    {
        IEnumerable<Event> GetEvent();
        Event GetEventByID(long id);
        Event GetEventBySlug(string slug);
        bool CreateEvent(Event newsFeed, ref string message);
        bool UpdateEvent(ref Event newsFeed, ref string message);
        bool DeleteEvent(long id, ref string message, bool softdelete);
        bool SaveNewsfeed();

		IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEvents(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
	}
}
