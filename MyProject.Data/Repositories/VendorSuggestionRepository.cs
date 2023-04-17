using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class VendorSuggestionRepository : RepositoryBase<VendorSuggestion>, IVendorSuggestionRepository
	{
		public VendorSuggestionRepository(IDbFactory dbFactory) : base(dbFactory)
		{ }

		public List<VendorSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate)
		{
			var Subscriber = this.DbContext.VendorSuggestions.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Subscriber;
		}

	}
	public interface IVendorSuggestionRepository : IRepository<VendorSuggestion>
	{
		List<VendorSuggestion> GetFilteredCustomerSuggestions(DateTime FromDate, DateTime ToDate);
	}
}
