using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class NewsFeedRepository : RepositoryBase<NewsFeed>, INewsFeedRepository
	{
		public NewsFeedRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public NewsFeed GetNewsFeedByTitle(string title, long id = 0)
		{
			var news = this.DbContext.NewsFeeds.Where(c => c.Title == title && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return news;
		}

		public NewsFeed GetNewsFeedBySlug(string slug)
		{
			var news = this.DbContext.NewsFeeds.Where(c => c.Slug == slug).FirstOrDefault();
			if (news == null)
				news = this.DbContext.NewsFeeds.Where(c => c.Slug == slug + "?").FirstOrDefault();
			return news;
		}

		public IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEventsAndNewsFeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var events = this.DbContext.SP_GetFilteredEvents(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
			return events;
		}

		public IEnumerable<SP_GetFilteredNewsfeeds_Result> GetFilteredNewsfeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var newsfeeds = this.DbContext.SP_GetFilteredNewsfeeds(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
			return newsfeeds;
		}
	}


	public interface INewsFeedRepository : IRepository<NewsFeed>
	{
		NewsFeed GetNewsFeedByTitle(string title, long id = 0);

		NewsFeed GetNewsFeedBySlug(string slug);
		IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEventsAndNewsFeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
		IEnumerable<SP_GetFilteredNewsfeeds_Result> GetFilteredNewsfeeds(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

	}

}
