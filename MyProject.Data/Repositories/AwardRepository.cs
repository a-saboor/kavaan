using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class AwardRepository : RepositoryBase<Award>, IAwardRepository
	{
		public AwardRepository(IDbFactory dbFactory)
	   : base(dbFactory) { }
		public Award GetAwardsByName(string name, long id = 0)
		{
			var award = this.DbContext.Awards.Where(c => c.Title == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return award;
		}
	}
	public interface IAwardRepository : IRepository<Award>
	{
		Award GetAwardsByName(string name, long id = 0);
	}
}
