using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class OzoneImageRepository : RepositoryBase<OzoneImage>, IOzoneImageRepository
	{
		public OzoneImageRepository(IDbFactory dbFactory)
	   : base(dbFactory) { }
	}
	public interface IOzoneImageRepository : IRepository<OzoneImage>
	{
	}
}
