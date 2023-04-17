using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class TagRepository : RepositoryBase<Tag>, ITagRepository
	{
		public TagRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public Tag GetTagByName(string name, long id = 0)
		{
			var user = this.DbContext.Tags.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface ITagRepository : IRepository<Tag>
	{
		Tag GetTagByName(string name, long id = 0);
	}
}
