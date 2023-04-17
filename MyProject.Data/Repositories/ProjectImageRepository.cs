using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class ProjectImageRepository : RepositoryBase<ProjectImage>, IProjectImageRepository
    {

        public ProjectImageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }


    }
    public interface IProjectImageRepository : IRepository<ProjectImage>
    {

    }
}
