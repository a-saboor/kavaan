using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class AttributeRepository : RepositoryBase<Attribute>, IAttributeRepository
	{
		public AttributeRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Attribute GetAttributeByName(string name, long id = 0)
		{
			var user = this.DbContext.Attributes.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IAttributeRepository : IRepository<Attribute>
	{
		Attribute GetAttributeByName(string name, long id = 0);
	}
}
