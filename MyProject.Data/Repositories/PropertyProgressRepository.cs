using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class PropertyProgressRepository : RepositoryBase<PropertyProgress>, IPropertyProgressRepository
    {
        public PropertyProgressRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public IEnumerable<PropertyProgress> GetProgressByProject(long projectId)
        {
            var progress = this.DbContext.PropertyProgresses.Where(mod => mod.IsActive == true && mod.PropertyID == projectId ).ToList();
            return progress;
        }
    }
    public interface IPropertyProgressRepository:IRepository<PropertyProgress>
    {
        IEnumerable<PropertyProgress> GetProgressByProject(long projectId);
    }
}
