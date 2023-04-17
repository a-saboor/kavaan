using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class CityRepository : RepositoryBase<City>, ICityRepository
	{
		public CityRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<City> GetAllByCountry(long countryId)
		{
			var Cities = this.DbContext.Cities.Where(c => c.CountryID == countryId && c.IsDeleted == false).ToList();
			return Cities;
		}

		public City GetCityByName(string name, long id = 0)
		{
			var user = this.DbContext.Cities.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}

        public bool InsertCity(string Name, string NameAr, string Country, System.DateTime CreatedOn, bool IsDeleted)
        {
            try
            {
                DbContext.PR_InsertCity(Name,NameAr,Country, CreatedOn, IsDeleted);
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

	public interface ICityRepository : IRepository<City>
	{
		bool InsertCity(string Name, string NameAr, string Country, System.DateTime CreatedOn, bool IsDeleted);


		IEnumerable<City> GetAllByCountry(long countryId);

		City GetCityByName(string name, long id = 0);
	}

}
