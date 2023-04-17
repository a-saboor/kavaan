using MyProject.Data;
using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyProject.Data.Repositories
{
    public class ConsultantRepository : RepositoryBase<Consultant>, IConsultantRepository
    {
        public ConsultantRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public Consultant GetConsultantByName(string name, long id = 0)
        {
            var consultant = this.DbContext.Consultants.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return consultant;
        }

        public IEnumerable<SP_GetFilteredConsultants_Result> GetFilteredConsultants(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
            var consultants = this.DbContext.SP_GetFilteredConsultants(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return consultants;
        }
    }
    public interface IConsultantRepository : IRepository<Consultant>
    {
        Consultant GetConsultantByName(string name, long id = 0);

        IEnumerable<SP_GetFilteredConsultants_Result> GetFilteredConsultants(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

    }
}

