using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class AssetsSpecificationsRepository : RepositoryBase<AssetsSpecification>, IAssetsSpecificationsRepository
    {
        public AssetsSpecificationsRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IAssetsSpecificationsRepository : IRepository<AssetsSpecification>
    {

    }
}
