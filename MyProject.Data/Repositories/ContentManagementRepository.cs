using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class ContentManagementRepository : RepositoryBase<ContentManagement>, IContentManagementRepository
	{
		public ContentManagementRepository(IDbFactory dbFactory) : base(dbFactory) { }

		public ContentManagement GetContentByType(string type)
		{
			var content = this.DbContext.ContentManagements.Where(c => c.Type == type).FirstOrDefault();
			return content;
		}
	}

	public interface IContentManagementRepository : IRepository<ContentManagement>
	{
		ContentManagement GetContentByType(string type);
	}
}
