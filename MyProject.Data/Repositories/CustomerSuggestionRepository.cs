using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class CustomerSuggestionRepository : RepositoryBase<CustomerSuggestion>, ICustomerSuggestionRepository
	{
		public CustomerSuggestionRepository(IDbFactory dbFactory) : base(dbFactory)
		{ }

		public List<CustomerSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate)
		{
			var Subscriber = this.DbContext.CustomerSuggestions.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Subscriber;
		}

	}
	public interface ICustomerSuggestionRepository : IRepository<CustomerSuggestion>
	{
		List<CustomerSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate);
	}
}
