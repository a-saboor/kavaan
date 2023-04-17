namespace MyProject.Data.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDbFactory dbFactory;
		private Project_DB_Entities dbContext;

		public UnitOfWork(IDbFactory dbFactory)
		{
			this.dbFactory = dbFactory;
		}

		public Project_DB_Entities DbContext
		{
			get { return dbContext ?? (dbContext = dbFactory.Init()); }
		}

		public void Commit()
		{
			DbContext.SaveChanges();
		}
	}
}
