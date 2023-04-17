using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	public class QuotationRepository : RepositoryBase<Quotation>, IQuotationRepository
	{
		public QuotationRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

	}
	public interface IQuotationRepository : IRepository<Quotation>
	{
	
	}
}
