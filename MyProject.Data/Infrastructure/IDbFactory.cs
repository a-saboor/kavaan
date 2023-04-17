using MyProject.Data;
using System;

namespace MyProject.Data.Infrastructure
{
	public interface IDbFactory : IDisposable
	{
        Project_DB_Entities Init();
	}
}
