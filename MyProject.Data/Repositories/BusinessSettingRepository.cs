using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	public interface IBusinessSettingRepository : IRepository<BusinessSetting>
	{

	}

	public class BusinessSettingRepository : RepositoryBase<BusinessSetting>, IBusinessSettingRepository
	{
		public BusinessSettingRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}
	}
}
