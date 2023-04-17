using System.Linq;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class ShippingClassRepository : RepositoryBase<ShippingClass>, IShippingClassRepository
    {
        public ShippingClassRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public ShippingClass GetShippingClassByName(string name, long id = 0)
        {
            var user = this.DbContext.ShippingClasses.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return user;
        }

    }

    public interface IShippingClassRepository : IRepository<ShippingClass>
    {
        ShippingClass GetShippingClassByName(string name, long id = 0);
    }
}
