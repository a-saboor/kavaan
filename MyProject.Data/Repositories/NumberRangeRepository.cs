using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class NumberRangeRepository : RepositoryBase<NumberRange>, INumberRangeRepository
	{
		public NumberRangeRepository(IDbFactory dbFactory) : base(dbFactory)
		{ }
		public NumberRange GetNumberRangeByName(string name, long id = 0)
		{
			var number = this.DbContext.NumberRanges.Where(n => n.Name == name && n.Id != id).FirstOrDefault();
			return number;
		}
	}
	public interface INumberRangeRepository : IRepository<NumberRange>
	{
		NumberRange GetNumberRangeByName(string name, long id = 0);
	}
}
