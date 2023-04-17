using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class RouteRepository : RepositoryBase<Route>, IRouteRepository
	{
		public RouteRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<Route> GetAll(string type)
		{
			var Routes = this.DbContext.Routes.Where(c => c.Type == type).ToList();
			return Routes;
		}

		public Route GetRouteByName(string name, long id = 0)
		{
			var Route = this.DbContext.Routes.Where(c => c.Name == name && c.ID != id).FirstOrDefault();
			return Route;
		}
	}

	public interface IRouteRepository : IRepository<Route>
	{
		IEnumerable<Route> GetAll(string type);
		Route GetRouteByName(string name, long id = 0);
	}
}
