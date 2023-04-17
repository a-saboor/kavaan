using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
    public class PropertyFeaturesRepository : RepositoryBase<PropertyFeature>, IPropertyFeaturesRepository
    {

        public PropertyFeaturesRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public PropertyFeature GetPropertyFeatureByID(long propertyid,long featureid, int featurepoisiton, long id = 0)
        {
            PropertyFeature feature = this.DbContext.PropertyFeatures.Where(c => (c.FeatureID == featureid || c.Position == featurepoisiton)&& c.PropertyID == propertyid && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return feature;
        }

        public IEnumerable<PropertyFeature> GetPropertyFeaturesByPropertyID(long projectID)
        {
            var features = this.DbContext.PropertyFeatures.Where(mod => mod.PropertyID == projectID && mod.IsDeleted == false).ToList();
            return features;
        }

    }
    public interface IPropertyFeaturesRepository : IRepository<PropertyFeature>
    {
        PropertyFeature GetPropertyFeatureByID(long propertyid,long featureid, int featurepoisiton, long id = 0);
        IEnumerable<PropertyFeature> GetPropertyFeaturesByPropertyID(long projectID);
    }
}
