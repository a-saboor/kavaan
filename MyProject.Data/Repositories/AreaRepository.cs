using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class AreaRepository : RepositoryBase<Area>, IAreaRepository
	{
		public AreaRepository(IDbFactory dbFactory)
			: base(dbFactory) { }


		public IEnumerable<Area> GetAllByCity(long cityId)
		{
			var Areas = this.DbContext.Areas.Where(c => c.CityID == cityId && c.IsDeleted == false).ToList();
			return Areas;
		}

		public Area GetAreaByName(long cityId, string name, long id = 0)
		{
			var user = this.DbContext.Areas.Where(c => c.CityID == cityId && c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
		public Area GetAreaByNameOnly(string name, long id = 0)
		{
			var user = this.DbContext.Areas.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
		public bool InsertArea(string Name,string NameAr, string CountryName ,string CityName,bool IsActive, System.DateTime CreatedOn, bool IsDeleted)
        {
            try
            {
                DbContext.PR_InsertArea(Name, NameAr,CountryName,CityName , CreatedOn, IsDeleted, IsActive);
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

	public interface IAreaRepository : IRepository<Area>
	{
		bool InsertArea(string Name, string NameAr, string CountryName, string CityName, bool IsActive, System.DateTime CreatedOn, bool IsDeleted);

        IEnumerable<Area> GetAllByCity(long countryId);
		Area GetAreaByName(long cityId, string name, long id=0);
		Area GetAreaByNameOnly(string name, long id = 0);
	}
}
