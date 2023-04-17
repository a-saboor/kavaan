using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(IDbFactory dbFactory)
       : base(dbFactory) { }

    }
    public interface ITicketRepository : IRepository<Ticket>
    {

    }
}
