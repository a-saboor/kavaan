using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{

    public class DepartmentsRepository : RepositoryBase<Department>, IDepartmentsRepository
	{
		public DepartmentsRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
		
	}
	public interface IDepartmentsRepository : IRepository<Department>
	{

	}
}
