using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	public interface IBusinessBranchSettingRepository : IRepository<BusinessBranchSetting>
	{

	}

	public class BusinessBranchSettingRepository : RepositoryBase<BusinessBranchSetting>, IBusinessBranchSettingRepository
	{
		public BusinessBranchSettingRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}
	}
}
