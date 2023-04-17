using MyProject.Data.Infrastructure;
using System.Linq;
namespace MyProject.Data.Repositories
{
    class ServiceBookingImageRepository : RepositoryBase<ServicebookingImage>, IServiceBookingImageRepository
    {
        public ServiceBookingImageRepository(IDbFactory dbFactory)
          : base(dbFactory) { }

    
    }
    public interface IServiceBookingImageRepository : IRepository<ServicebookingImage>
    {
    }
}
