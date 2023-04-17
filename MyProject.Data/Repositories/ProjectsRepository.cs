using MyProject.Data.Infrastructure;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MyProject.Data.Repositories
{
    class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDbFactory dbFactory)
            : base(dbFactory) {
        }

        public bool DeleteProjectByProp(long propertyid)
        {
            return false;
        }

        public Project GetProjectByName(string name, long id = 0)
        {
            var unit = this.DbContext.Projects.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return unit;
        }

        public IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> ProjectTypeID)
        {
            var projects = this.DbContext.SP_GetFilteredProjects(search, pageSize, pageNumber, sortBy, lang, imageServer, ProjectTypeID).ToList();
            return projects;
        }

        //public IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID)
        //{
        //    var units = this.DbContext.SP_GetFilteredProjects(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID).ToList();
        //    return units;
        //}

        //public IEnumerable<SP_GetFilteredProjectProjects_Result> GetFilteredProjectProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type)
        //{
        //    var units = this.DbContext.SP_GetFilteredProjectProjects(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID, features , amenities , bedRooms , bathRooms , minPrice , maxPrice , type).ToList();
        //    return units;
        //}
    }

    public interface IProjectRepository : IRepository<Project>
    {
        Project GetProjectByName(string name, long id = 0);
        bool DeleteProjectByProp(long propertyid);

        	IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> ProjectTypeID);

        //IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID);
        //IEnumerable<SP_GetFilteredProjectProjects_Result> GetFilteredProjectProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type);
    }
}
