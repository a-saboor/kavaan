using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class UnitTypeRepository : RepositoryBase<UnitType>, IUnitTypeRepository
    {
        public UnitTypeRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public UnitType GetUnitTypeByName(string name, long id = 0)
        {
            var unit = this.DbContext.UnitTypes.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return unit;
        }
        //public bool InsertCountry(string Name, string NameAr, bool IsActive, System.DateTime CreatedOn, bool IsDeleted)
        //{
        //    try
        //    {
        //        DbContext.PR_InsertCountry(Name, NameAr, IsActive, CreatedOn, IsDeleted);
        //        return true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //log.Error("Error", ex);
        //        //log.Error("Error", ex);
        //        return false;
        //    }
        //}
    }

    public interface IUnitTypeRepository : IRepository<UnitType>
    {
        UnitType GetUnitTypeByName(string name, long id = 0);
        //bool InsertCountry(string Name, string NameAr, bool IsActive, System.DateTime CreatedOn, bool IsDeleted);
    }
}
