using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class AssetContractorsRepository : RepositoryBase<AssetsContractor>, IAssetContractorsRepository
    {
        public AssetContractorsRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IAssetContractorsRepository : IRepository<AssetsContractor>
    {

    }
}
