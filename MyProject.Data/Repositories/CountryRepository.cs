using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Country GetCountryByName(string name, long id = 0)
        {
            var user = this.DbContext.Countries.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return user;
        }
        public bool InsertCountry(string Name, string NameAr, bool IsActive, System.DateTime CreatedOn, bool IsDeleted)
        {
            try
            {
                DbContext.PR_InsertCountry(Name, NameAr, IsActive, CreatedOn, IsDeleted);
                return true;
            }
            catch (System.Exception ex)
            {
                //log.Error("Error", ex);
                //log.Error("Error", ex);
                return false;
            }
        }
    }

    public interface ICountryRepository : IRepository<Country>
    {
        Country GetCountryByName(string name, long id = 0);
        bool InsertCountry(string Name, string NameAr, bool IsActive, System.DateTime CreatedOn, bool IsDeleted);
    }
}
