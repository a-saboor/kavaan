using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class DevelopmentRepository : RepositoryBase<Development>, IDevelopmentRepository
    {
        public DevelopmentRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Development GerDevelopmentByName(string name, long id = 0)
        {
            var development = this.DbContext.Developments.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return development;
        }

        public IEnumerable<SP_GetFilteredDevelopment_Result> GetFilteredDevelopment(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var development = this.DbContext.SP_GetFilteredDevelopment(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return development;
        }

    }
    public interface IDevelopmentRepository:IRepository<Development>
    {
        IEnumerable<SP_GetFilteredDevelopment_Result> GetFilteredDevelopment(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
        Development GerDevelopmentByName(string name, long id = 0);
    }
}
