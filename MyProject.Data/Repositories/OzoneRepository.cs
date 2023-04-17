using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class OzoneRepository : RepositoryBase<Ozone>, IOzoneRepository
	{
		public OzoneRepository(IDbFactory dbFactory)
   : base(dbFactory) { }
		
	}
	public interface IOzoneRepository : IRepository<Ozone>
	{
	}
}
