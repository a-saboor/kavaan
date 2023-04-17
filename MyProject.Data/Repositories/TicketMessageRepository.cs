using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class TicketMessageRepository : RepositoryBase<TicketMessage>, ITicketMessageRepository
    {
        public TicketMessageRepository(IDbFactory dbFactory)
        : base(dbFactory) { }

        public IEnumerable<SP_GetTicketConversation_Result> GetTicketConversation(int ticketID)
        {
            var facility = this.DbContext.SP_GetTicketConversation(ticketID).ToList();
            return facility;
        }
    }
    public interface ITicketMessageRepository : IRepository<TicketMessage>
    {
        IEnumerable<SP_GetTicketConversation_Result> GetTicketConversation(int ticketID);
    }
}
