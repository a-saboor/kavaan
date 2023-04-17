using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class AmenitiesRepository : RepositoryBase<Amenity>, IAmenitiesRepository
    {
        public AmenitiesRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Amenity GetAmenitiesByName(string name, long id = 0)
        {
            var amenity = this.DbContext.Amenities.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return amenity;
        }
    }

    public interface IAmenitiesRepository : IRepository<Amenity>
    {
        Amenity GetAmenitiesByName(string name, long id = 0);
    }
}
