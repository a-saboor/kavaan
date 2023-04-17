using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    class AssetsCategoriesRepository : RepositoryBase<AssetsCategory>, IAssetsCategoriesRepository
    {
        public AssetsCategoriesRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public AssetsCategory GetAssetsCategoryByName(string name, long id = 0)
        {
            var category = this.DbContext.AssetsCategories.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return category;
        }

        public AssetsCategory GetAssetsCategoryByNameAndParentId(string name, long? parentId, long id = 0)
        {
            var category = this.DbContext.AssetsCategories.Where(c => c.Name == name && c.ParentID == parentId && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return category;
        }
        public IEnumerable <AssetsCategory> GetAssetsCategoryBySameParentId(long? parentId)
        {
            var category = this.DbContext.AssetsCategories.Where(c => c.ParentID == parentId && c.IsDeleted == false).ToList();
            return category;
        }

    }
    public interface IAssetsCategoriesRepository : IRepository<AssetsCategory>
    {
        AssetsCategory GetAssetsCategoryByName(string name, long id = 0);
        AssetsCategory GetAssetsCategoryByNameAndParentId(string name, long? parentId, long id = 0);
        IEnumerable <AssetsCategory> GetAssetsCategoryBySameParentId(long? parentId);
    }
}
