using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class AssetImageRepository : RepositoryBase<AssetImage>, IAssetImageRepository
	{
		public AssetImageRepository(IDbFactory dbFactory)
		: base(dbFactory) { }
		public IEnumerable<AssetImage> GetAssetImages(long AssetID)
		{
			var assetImages = this.DbContext.AssetImages.Where(c => c.AssetID == AssetID).ToList();
			return assetImages;
		}
	}
	public interface IAssetImageRepository : IRepository<AssetImage>
	{
		IEnumerable<AssetImage> GetAssetImages(long AssetID);
	}
}
