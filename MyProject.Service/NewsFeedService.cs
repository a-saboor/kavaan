using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class NewsFeedService : INewsFeedService
    {
        private readonly INewsFeedRepository _newsFeedRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewsFeedService(INewsFeedRepository newsFeedRepository, IUnitOfWork unitOfWork)
        {
            _newsFeedRepository = newsFeedRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<NewsFeed> GetNewsFeed()
        {
            var Newsfeed = _newsFeedRepository.GetAll().Where(d => d.IsDeleted == false);
            return Newsfeed;
        }

        public NewsFeed GetNewsFeedByID(long id)
        {
            var news = _newsFeedRepository.GetById(id);
            return news;
        }

        public NewsFeed GetNewsFeedBySlug(string slug)
        {
            var News = _newsFeedRepository.GetNewsFeedBySlug(slug);

            return News;
        }

        public bool CreateNewsFeed(NewsFeed newsFeed, ref string message)
        {
            try
            {
                if (_newsFeedRepository.GetNewsFeedBySlug(newsFeed.Title) == null)
                {
                    newsFeed.IsActive = true;

                    newsFeed.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _newsFeedRepository.Add(newsFeed);
                    if (SaveNewsfeed())
                    {
                        message = "News feed added successfully ...";
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
                    message = "News feed already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateNewsFeed(ref NewsFeed newsFeed, ref string message)
        {
            try
            {
                if (_newsFeedRepository.GetNewsFeedByTitle(newsFeed.Title, newsFeed.ID) == null)
                {
                    NewsFeed CurrentNews = _newsFeedRepository.GetById(newsFeed.ID);

                    CurrentNews.IsActive = newsFeed.IsActive;
                    _newsFeedRepository.Update(CurrentNews);
                    if (SaveNewsfeed())
                    {
                        newsFeed = CurrentNews;
                        message = "News feed updated successfully ...";
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
                    message = "News feed already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteNewsFeed(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {

                    //soft delete
                    NewsFeed News = _newsFeedRepository.GetById(id);
                    News.IsDeleted = true;
                    _newsFeedRepository.Update(News);

                    if (SaveNewsfeed())
                    {
                        message = "News feed deleted successfully ...";
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
                    NewsFeed newsFeed = _newsFeedRepository.GetById(id);

                    _newsFeedRepository.Delete(newsFeed);

                    if (SaveNewsfeed())
                    {
                        message = "News feed deleted successfully ...";
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
        public IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEventsAndNewsFeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var Events = _newsFeedRepository.GetFilteredEventsAndNewsFeeds(search, pageSize, pageNumber, sortBy, lang, imageServer);
            return Events;
        }

        public IEnumerable<SP_GetFilteredNewsfeeds_Result> GetFilteredNewsfeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var Newsfeeds = _newsFeedRepository.GetFilteredNewsfeeds(search, pageSize, pageNumber, sortBy, lang, imageServer);
            return Newsfeeds;
        }

    }
    public interface INewsFeedService
    {
        IEnumerable<NewsFeed> GetNewsFeed();
        NewsFeed GetNewsFeedByID(long id);
        NewsFeed GetNewsFeedBySlug(string slug);
        bool CreateNewsFeed(NewsFeed newsFeed, ref string message);
        bool UpdateNewsFeed(ref NewsFeed newsFeed, ref string message);
        bool DeleteNewsFeed(long id, ref string message, bool softdelete);
        bool SaveNewsfeed();
        IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEventsAndNewsFeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
        IEnumerable<SP_GetFilteredNewsfeeds_Result> GetFilteredNewsfeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
    }
}
