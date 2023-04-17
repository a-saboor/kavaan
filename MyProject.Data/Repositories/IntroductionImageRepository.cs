using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class IntroductionImageRepository : RepositoryBase<IntroductionImage>, IIntroductionImageRepository
	{
		public IntroductionImageRepository(IDbFactory dbFactory)
   : base(dbFactory) { }
	}
	public interface IIntroductionImageRepository : IRepository<IntroductionImage>
	{
	}
}
