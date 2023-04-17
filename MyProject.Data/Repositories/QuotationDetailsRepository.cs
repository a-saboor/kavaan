using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	public class QuotationDetailsRepository : RepositoryBase<QuotationDetail>, IQuotationDetailsRepository
	{
		public QuotationDetailsRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

	}
	public interface IQuotationDetailsRepository : IRepository<QuotationDetail>
	{
	
	}
}
