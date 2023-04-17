using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class TicketDocumentRepository : RepositoryBase<TicketDocument>, ITicketDocumentRepository
    {
        public TicketDocumentRepository(IDbFactory dbFactory)
       : base(dbFactory) { }

    }
    public interface ITicketDocumentRepository : IRepository<TicketDocument>
    {

    }
}
