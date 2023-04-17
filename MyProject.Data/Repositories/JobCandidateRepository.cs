using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class JobCandidatesRepository : RepositoryBase<JobCandidate>, IJobCandidatesRepository
    {
        public JobCandidatesRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<JobCandidate> GetFilteredJobCandidates(DateTime FromDate, DateTime ToDate)
        {
            var Candidates = this.DbContext.JobCandidates.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            return Candidates;
        }


        public IEnumerable<SP_GetFilteredCandidates_Result> GetSPCandidates(int displayLength, int displayStart, int sortCol, string sortDir, string search, string imageServer, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            var Products = this.DbContext.SP_GetFilteredCandidates(displayLength, displayStart, sortCol, sortDir, search, imageServer, imageServer, "", FromDate, ToDate).ToList();
            return Products;
        }
    }
    public interface IJobCandidatesRepository : IRepository<JobCandidate>
    {
        List<JobCandidate> GetFilteredJobCandidates(DateTime FromDate, DateTime ToDate);
        
        IEnumerable<SP_GetFilteredCandidates_Result> GetSPCandidates(int displayLength, int displayStart, int sortCol, string sortDir, string search, string imageServer, DateTime? FromDate = null, DateTime? ToDate = null);

    }
}
