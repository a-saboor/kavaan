using MyProject.Data.Infrastructure;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MyProject.Data.Repositories
{
    class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        public UnitRepository(IDbFactory dbFactory)
            : base(dbFactory) {

          


        }

        public bool DeleteUnitByProp(long propertyid)
        {
            return false;
        }

        public Unit GetUnitByTitle(string name, long id = 0)
        {
            var unit = this.DbContext.Units.Where(c => c.Title == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return unit;
        }
        public IEnumerable<SP_GetFilteredUnits_Result> GetFilteredUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID)
        {
            var units = this.DbContext.SP_GetFilteredUnits(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID).ToList();
            return units;
        }

        public IEnumerable<SP_GetFilteredProjectUnits_Result> GetFilteredProjectUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type)
        {
            var units = this.DbContext.SP_GetFilteredProjectUnits(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID, features , amenities , bedRooms , bathRooms , minPrice , maxPrice , type).ToList();
            return units;
        }
    }

    public interface IUnitRepository : IRepository<Unit>
    {
        Unit GetUnitByTitle(string name, long id = 0);
        bool DeleteUnitByProp(long propertyid);

        IEnumerable<SP_GetFilteredUnits_Result> GetFilteredUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID);
        IEnumerable<SP_GetFilteredProjectUnits_Result> GetFilteredProjectUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type);
    }
}
