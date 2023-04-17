
using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	public class TaxSettingRepository : RepositoryBase<TaxSetting>, ITaxSettingRepository
	{
		public TaxSettingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public TaxSetting GetTaxSettingByName(string name, long id = 0)
		{
			var user = this.DbContext.TaxSettings.Where(c => c.TaxName == name && c.ID != id).FirstOrDefault();
			return user;
		}
	}
	public interface ITaxSettingRepository : IRepository<TaxSetting>
	{
		TaxSetting GetTaxSettingByName(string name, long id = 0);
	}
}
