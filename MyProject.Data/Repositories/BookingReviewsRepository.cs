using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Repositories
{
    class BookingReviewsRepository : RepositoryBase<BookingReview>, IBookingReviewRepository
    {
        public BookingReviewsRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
        public IEnumerable<BookingReview> GetBookingReviewByID(long Id)
        {
            var BookingReview = this.DbContext.BookingReviews.Where(mod => mod.ID == Id).OrderByDescending(x => x.ID).ToList();
            return BookingReview;
        }
    }
    public interface IBookingReviewRepository : IRepository<BookingReview>
    {

        IEnumerable<BookingReview> GetBookingReviewByID(long id);


    }
}
