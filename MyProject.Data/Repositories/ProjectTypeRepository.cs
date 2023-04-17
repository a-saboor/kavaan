using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class ProjectTypeRepository : RepositoryBase<ProjectType>, IProjectTypeRepository
    {
        public ProjectTypeRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public ProjectType GetProjectTypeByName(string name, long id = 0)
        {
            var unit = this.DbContext.ProjectTypes.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return unit;
        }
    }

    public interface IProjectTypeRepository : IRepository<ProjectType>
    {
        ProjectType GetProjectTypeByName(string name, long id = 0);
    }
}
