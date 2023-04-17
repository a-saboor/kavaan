using Project.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Data.Repositories
{
    
    class BookingImagesRepository : RepositoryBase<BookingImage>, IBookingImagesRepository
    {
        public BookingImagesRepository(IDbFactory dbFactory)
           : base(dbFactory) { }
        public IEnumerable<BookingImage> GetBookingsByID(long Id)
        {
            var BookingImage = this.DbContext.BookingImages.Where(mod => mod.ID == Id).OrderByDescending(x => x.ID).ToList();
            return BookingImage;
        }
    }
    public interface IBookingImagesRepository : IRepository<BookingImage>
    {
        
        IEnumerable<BookingImage> GetBookingsByID(long id);


    }
}
