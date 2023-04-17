using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public IEnumerable<SP_GetFilteredServices_Result> GetFilteredServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, long? CategoryID)
        {
            var services = this.DbContext.SP_GetFilteredServices(search, pageSize, pageNumber, sortBy, lang, imageServer, CategoryID).ToList();
            return services;
        }
        public IEnumerable<SP_GetPopularService_Result> GetPopularServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var services = this.DbContext.SP_GetPopularService(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return services;
        }
        public List<Data.Service> GetServicesByDate(DateTime FromDate, DateTime ToDate, int id = 0)
        {
            var services = this.DbContext.Services.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsDeleted == false).ToList();
            return services;
        }
        public Data.Service GetServiceBySlug(string slug)
        {
            var service = this.DbContext.Services.Where(c => c.Slug == slug).FirstOrDefault();
            if (service == null)
                service = this.DbContext.Services.Where(c => c.Slug == slug + "?").FirstOrDefault();
            return service;
        }
    }
    public interface IServiceRepository : IRepository<Service>
    {
        IEnumerable<SP_GetPopularService_Result> GetPopularServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
        Data.Service GetServiceBySlug(string slug);
        IEnumerable<SP_GetFilteredServices_Result> GetFilteredServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, long? CategoryID);

        List<Data.Service> GetServicesByDate(DateTime FromDate, DateTime ToDate, int id = 0);
    }
}
