using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
    {
        
        public PropertyRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
        public Property GetPropertyByTitle(string name, long id = 0)
        {
            var property = this.DbContext.Properties.Where(c => c.Title == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return property;
        }

        public IEnumerable<SP_GetFilteredProperties_Result> GetFilteredProperties(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID,Nullable<bool> isfeatured , Nullable<bool> vrTour)
        {
            var properties = this.DbContext.SP_GetFilteredProperties(search, pageSize, pageNumber, sortBy, vrTour, lang, imageServer, developmentID, isfeatured).ToList();
            return properties;
        }

    }
    public interface IPropertyRepository: IRepository<Property>
	{
		Property GetPropertyByTitle(string name, long id = 0);

        IEnumerable<SP_GetFilteredProperties_Result> GetFilteredProperties(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID, Nullable<bool> isfeatured, Nullable<bool> vrTour);

    }
}
