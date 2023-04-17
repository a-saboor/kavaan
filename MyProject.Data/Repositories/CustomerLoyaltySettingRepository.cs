using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class CustomerLoyaltySettingRepository : RepositoryBase<CustomerLoyaltySetting>, ICustomerLoyaltySettingRepository
	{
		public CustomerLoyaltySettingRepository(IDbFactory dbFactory) : base(dbFactory) { }

		public CustomerLoyaltySetting GetCustomerLoyaltySettingByType(string type, long id = 0)
		{
			var data = this.DbContext.CustomerLoyaltySettings.Where(c => c.CustomerType == type && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return data;
		}

	}
	public interface ICustomerLoyaltySettingRepository : IRepository<CustomerLoyaltySetting>
	{
		CustomerLoyaltySetting GetCustomerLoyaltySettingByType(string type, long id = 0);
	}
}
