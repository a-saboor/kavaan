using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	class IntroductionSettingRepository : RepositoryBase<IntroductionSetting>, IIntroductionSettingRepository
	{
		public IntroductionSettingRepository(IDbFactory dbFactory)
	   : base(dbFactory) { }
	}
	public interface IIntroductionSettingRepository : IRepository<IntroductionSetting>
	{
	}
}
