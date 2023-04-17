using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class JobOpeningRepository : RepositoryBase<JobOpening>, IJobOpeningRepository
    {
        public JobOpeningRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public JobOpening GetJobOpeningByTitle(string title, long id = 0)
        {
            var jobOpening = this.DbContext.JobOpenings.Where(c => c.Title == title && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return jobOpening;
        }
        public JobOpening GetJobOpeningByCategory(long id = 0, long jobId = 0)
        {
            var jobOpening = this.DbContext.JobOpenings.Where(c => c.Category.ID == id && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return jobOpening;
        }

        public JobOpening GetJobOpeningByCategoryAndTitle(long categoryId, string jobTitle, long id = 0)
        {
            var jobOpening = this.DbContext.JobOpenings.Where(c => c.Category.ID == categoryId && c.Title == jobTitle && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return jobOpening;
        }

        public IEnumerable<JobOpening> GetJobOpeningByCategory(long categoryId)
        {
            var jobOpenings = this.DbContext.JobOpenings.Where(c => c.CategoryID == categoryId && c.IsActive == true && c.IsDeleted == false).ToList();
            return jobOpenings;
        }
    }

    public interface IJobOpeningRepository : IRepository<JobOpening>
    {
        JobOpening GetJobOpeningByTitle(string name, long id = 0);
        JobOpening GetJobOpeningByCategory(long id = 0, long jobId = 0);
        JobOpening GetJobOpeningByCategoryAndTitle(long categoryId, string jobTitle, long id = 0);
        IEnumerable<JobOpening> GetJobOpeningByCategory(long categoryId);
    }
}
