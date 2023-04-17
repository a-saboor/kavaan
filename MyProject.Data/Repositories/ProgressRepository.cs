using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class ProgressRepository : RepositoryBase<Progress>, IProgressRepository
    {
        
        public ProgressRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }


    }
    public interface IProgressRepository : IRepository<Progress>
    {
       
    } 
}
