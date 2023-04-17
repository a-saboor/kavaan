using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class BlogService : IBlogService
    {
        private readonly INewFeedRepository _newsFeedRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(INewFeedRepository newsFeedRepository, IUnitOfWork unitOfWork)
        {
            this._newsFeedRepository = newsFeedRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Blog> GetBlog()
        {
            var Newsfeed = _newsFeedRepository.GetAll().Where(d=>d.IsDeleted==false);
            return Newsfeed;
        }

        public Blog GetBlogByID(long id)
        {
            var Blog = _newsFeedRepository.GetById(id);
            return Blog;
        }

        public Blog GetBlogBySlug(string slug)
        {
            var Blog = _newsFeedRepository.GetBlogBySlug(slug);
            return Blog;
        }

        public bool CreateBlog(Blog newsFeed, ref string message)
        {
            try
            {
                if (_newsFeedRepository.GetBlogByTitle(newsFeed.Title) == null)
                {
                    newsFeed.IsActive = true;

                    newsFeed.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _newsFeedRepository.Add(newsFeed);
                    if (SaveNewsfeed())
                    {
                        message = "Blog added successfully ...";
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
                    message = "Blog already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateBlog(ref Blog newsFeed, ref string message)
        {
            try
            {
                if (_newsFeedRepository.GetBlogByTitle(newsFeed.Title, newsFeed.ID) == null)
                {
                   

                    _newsFeedRepository.Update(newsFeed);
                    if (SaveNewsfeed())
                    {
                        
                        message = "Blog updated successfully ...";
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
                    message = "Blog already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteBlog(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {

                    //soft delete
                    Blog Blog = _newsFeedRepository.GetById(id);
                    Blog.IsDeleted = true;
                    _newsFeedRepository.Update(Blog);

                    if (SaveNewsfeed())
                    {
                        message = "Blog deleted successfully ...";
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
                    Blog Blog = _newsFeedRepository.GetById(id);

                    _newsFeedRepository.Delete(Blog);

                    if (SaveNewsfeed())
                    {
                        message = "Blog deleted successfully ...";
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

        public IEnumerable<SP_GetFilteredBlogs_Result> GetFilteredBlog(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var Blogs = _newsFeedRepository.GetFilteredBlog(search, pageSize, pageNumber, sortBy, lang, imageServer, startDate, endDate);
            return Blogs;
        }

    }
    public interface IBlogService
    {
        IEnumerable<Blog> GetBlog();
        Blog GetBlogByID(long id);
        Blog GetBlogBySlug(string slug);
        bool CreateBlog(Blog newsFeed, ref string message);
        bool UpdateBlog(ref Blog newsFeed, ref string message);
        bool DeleteBlog(long id, ref string message, bool softdelete);
        bool SaveNewsfeed();

        IEnumerable<SP_GetFilteredBlogs_Result> GetFilteredBlog(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<DateTime> startDate, Nullable<DateTime> endDate);
    }
}
