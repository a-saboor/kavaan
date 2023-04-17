using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Repositories
{
    class BookingCancellationRepository : RepositoryBase<BookingCancellation>, IBookingCancellationRepository
    {
        public BookingCancellationRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
        public IEnumerable<BookingCancellation> GetBookingCancellationByID(long Id)
        {
            var BookingCancellation = this.DbContext.BookingCancellations.Where(mod => mod.ID == Id).OrderByDescending(x => x.ID).ToList();
            return BookingCancellation;
        }
    }
    public interface IBookingCancellationRepository : IRepository<BookingCancellation>
    {

        IEnumerable<BookingCancellation> GetBookingCancellationByID(long id);


    }
}
