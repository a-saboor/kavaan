using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class BlogRepository : RepositoryBase<Blog>, INewFeedRepository
	{
		public BlogRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Blog GetBlogByTitle(string title, long id = 0)
		{
			var blog= this.DbContext.Blogs.Where(c => c.Title == title && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return blog;
		}

		public Blog GetBlogBySlug(string slug)
		{
			var blogs = this.DbContext.Blogs.Where(c => c.Slug == slug).FirstOrDefault();
			if(blogs == null)
                blogs = this.DbContext.Blogs.Where(c => c.Slug == slug+"?").FirstOrDefault();
			return blogs;
		}

        public IEnumerable<SP_GetFilteredBlogs_Result> GetFilteredBlog(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer,Nullable<DateTime> startDate , Nullable<DateTime> endDate)
        { 
            var blogs = this.DbContext.SP_GetFilteredBlogs(search, pageSize, pageNumber, sortBy, startDate, endDate, lang, imageServer).ToList();
            return blogs;
        }

    }

	public interface INewFeedRepository : IRepository<Blog>
	{
		Blog GetBlogByTitle(string title, long id = 0);
		Blog GetBlogBySlug(string slug);
		IEnumerable<SP_GetFilteredBlogs_Result> GetFilteredBlog(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<DateTime> startDate, Nullable<DateTime> endDate);
	}
}
