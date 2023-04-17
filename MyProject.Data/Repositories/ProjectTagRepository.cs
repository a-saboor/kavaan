using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class ProjectTagRepository : RepositoryBase<ProjectTag>, IProjectTagRepository
	{
		public ProjectTagRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public ProjectTag GetProjectTag(long productId, long tagId, long id = 0)
		{
			var ProjectTag = this.DbContext.ProjectTags.Where(c => c.ProjectID == productId && c.TagID == tagId && c.ID != id).FirstOrDefault();
			return ProjectTag;
		}

		public IEnumerable<ProjectTag> GetProjectTags(long productId)
		{
			var ProjectTags = this.DbContext.ProjectTags.Where(c => c.ProjectID == productId).ToList();
			return ProjectTags;
		}
        public bool InsertProjectTags(long ProjectID, string ProjectTags)
        {
            try
            {
                DbContext.PR_InsertProjectTags(ProjectID, ProjectTags);
                return true;
            }
            catch (System.Exception ex)
            {
                //log.Error("Error", ex);
                //log.Error("Error", ex);
                return false;
            }
        }
    }

	public interface IProjectTagRepository : IRepository<ProjectTag>
	{
        bool InsertProjectTags(long ProjectID, string ProjectTags);

        ProjectTag GetProjectTag(long productId, long categoryId, long id = 0);
		IEnumerable<ProjectTag> GetProjectTags(long productId);
	}
}
