using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class AssetsProductsRepository : RepositoryBase<AssetsProduct>, IAssetsProductsRepository
    {
        public AssetsProductsRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public AssetsProduct GetAssetsProductByName(string name, long id = 0)
        {
            var product = this.DbContext.AssetsProducts.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return product;
        }

        public AssetsProduct GetAssetsProductByNameAndParentId(string name, long id = 0)
        {
            var product = this.DbContext.AssetsProducts.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return product;
        }
        public IEnumerable<AssetsProduct> GetAssetsProductBySameParentId(long? parentId)
        {
            var products = this.DbContext.AssetsProducts.Where(c => c.IsDeleted == false).ToList();
            return products;
        }

    }
    public interface IAssetsProductsRepository : IRepository<AssetsProduct>
    {
        AssetsProduct GetAssetsProductByName(string name, long id = 0);
        AssetsProduct GetAssetsProductByNameAndParentId(string name, long id = 0);
        IEnumerable<AssetsProduct> GetAssetsProductBySameParentId(long? parentId);
        void Add(AssetsProduct assetsProduct);
    }
}
