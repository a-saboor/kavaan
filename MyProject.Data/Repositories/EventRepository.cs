using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class EventRepository : RepositoryBase<Event>, IEventRepository
	{
		public EventRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Event GeteventsByTitle(string title, long id = 0)
		{
			var events = this.DbContext.Events.Where(c => c.Title == title && c.ID != id &&c.IsDeleted==false).FirstOrDefault();
			return events;
		}

		public Event GeteventsBySlug(string slug)
		{
			var events = this.DbContext.Events.Where(c => c.Slug == slug).FirstOrDefault();
			if (events == null)
				events = this.DbContext.Events.Where(c => c.Slug == slug + "?").FirstOrDefault();
			return events;
		}

		public IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEvents(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var events = this.DbContext.SP_GetFilteredEvents(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
			return events;
		}

	}

	public interface IEventRepository : IRepository<Event>
	{
		Event GeteventsByTitle(string title, long id = 0);
		Event GeteventsBySlug(string slug);
		IEnumerable<SP_GetFilteredEvents_Result> GetFilteredEvents(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
	}
}
