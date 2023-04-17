using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Data.Repositories
{
	class SubscribersRepository : RepositoryBase<Subscriber>, ISubscribersRepository
	{
		public SubscribersRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Subscriber GetSubscriberByEmail(string email, long id = 0)
		{
			var Subscriber = this.DbContext.Subscribers.Where(c => c.Email == email && c.ID != id).FirstOrDefault();
			return Subscriber;
		}
        public List <Subscriber> GetFilteredSubscribers(DateTime FromDate, DateTime ToDate)
        {
            var Subscriber = this.DbContext.Subscribers.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Subscriber;
        }


    }
	public interface ISubscribersRepository : IRepository<Subscriber>
	{
        List<Subscriber> GetFilteredSubscribers(DateTime FromDate, DateTime ToDate);
        Subscriber GetSubscriberByEmail(string email, long id = 0);
	}
}
