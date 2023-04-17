namespace MyProject.Data.Infrastructure
{
	public class DbFactory : Disposable, IDbFactory
	{
        Project_DB_Entities dbContext;

		public Project_DB_Entities Init()
		{
			return dbContext ?? (dbContext = new Project_DB_Entities());
		}

		protected override void DisposeCore()
		{
			if (dbContext != null)
				dbContext.Dispose();
		}
	}
}
