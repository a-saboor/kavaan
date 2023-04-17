using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class FeatureRepository:RepositoryBase<Feature>, IFeatureRepository
    {
        
        public FeatureRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

        public Feature GetFeatureByTitle(string name, long id = 0)
        {
            
                Feature feature = this.DbContext.Features.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
                return feature;
            
        }
    }
    public interface IFeatureRepository: IRepository<Feature>
    {
        Feature GetFeatureByTitle(string name, long id = 0);
    } 
}
