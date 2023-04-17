using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	public class QuotationNoteRepository : RepositoryBase<QuotationNote>, IQuotationNoteRepository
	{
		public QuotationNoteRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

	}
	public interface IQuotationNoteRepository : IRepository<QuotationNote>
	{
	
	}
}
