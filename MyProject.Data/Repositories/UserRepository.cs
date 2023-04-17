using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	public class TeamRepository : RepositoryBase<Team>, ITeamRepository
	{
		public TeamRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

	

	
	}

	public interface ITeamRepository : IRepository<Team>
	{
		

		
	}
}
