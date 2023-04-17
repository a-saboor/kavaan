using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class ContractorRepository : RepositoryBase<Contractor>, IContractorRepository
    {
        public ContractorRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
           


        }

        public Contractor GetCountractorByName(string name, long id = 0)
        {
            var contractor = this.DbContext.Contractors.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return contractor;
        }

        public IEnumerable<SP_GetFilteredContractors_Result> GetFilteredContractors(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var contractors = this.DbContext.SP_GetFilteredContractors(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return contractors;
        }

    }
    public interface IContractorRepository:IRepository<Contractor>
    {
        Contractor GetCountractorByName(string name, long id = 0);

        IEnumerable<SP_GetFilteredContractors_Result> GetFilteredContractors(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
    }
}
