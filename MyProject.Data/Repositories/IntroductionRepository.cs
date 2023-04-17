﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	class IntroductionRepository : RepositoryBase<Introduction>, IIntroductionRepository
	{
		public IntroductionRepository(IDbFactory dbFactory)
	   : base(dbFactory) { }
	}
	public interface IIntroductionRepository : IRepository<Introduction>
	{
	}
}
