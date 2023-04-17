using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class StaffSuggestionRepository : RepositoryBase<StaffSuggestion>, IStaffSuggestionRepository
	{
		public StaffSuggestionRepository(IDbFactory dbFactory) : base(dbFactory)
		{ }

		public List<StaffSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate)
		{
			var Subscriber = this.DbContext.StaffSuggestions.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Subscriber;
		}

	}
	public interface IStaffSuggestionRepository : IRepository<StaffSuggestion>
	{
		List<StaffSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate);
	}
}
