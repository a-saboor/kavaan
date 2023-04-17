using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	public class BankRepository : RepositoryBase<Bank>, IBankRepository
	{
		public BankRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

	}
	public interface IBankRepository : IRepository<Bank>
	{

	}
}
