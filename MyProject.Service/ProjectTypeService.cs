using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class ProjectTypeService : IProjectTypeService
    {
        private readonly IProjectTypeRepository _projectTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectTypeService(ICountryRepository countryRepository, IUnitOfWork unitOfWork, IProjectTypeRepository projectTypeRepository)
        {

            this._unitOfWork = unitOfWork;
            _projectTypeRepository = projectTypeRepository;
        }

        #region IProjectTypeService Members

        public IEnumerable<ProjectType> GetProjectTypes()
        {
            var projectTypes = _projectTypeRepository.GetAll().Where(i => i.IsDeleted == false);
            return projectTypes;
        }

        public IEnumerable<object> GetProjectTypeForDropDown(string lang = "en")
        {
            var Projects = GetProjectTypes().Where(x => x.IsActive == true);
            var dropdownList = from project in Projects
                               select new { value = project.ID, text = lang == "en" ? project.Name : project.NameAr };
            return dropdownList;
        }
        public ProjectType GetProjectType(long id)
        {
            var ProjectType = _projectTypeRepository.GetById(id);
            return ProjectType;
        }
        public ProjectType GetProjectTypeByName(string name)
        {
            var ProjectType = _projectTypeRepository.GetProjectTypeByName(name);
            return ProjectType;
        }

        public bool CreateProjectType(ref ProjectType ProjectType, ref string message)
        {
            try
            {
                if (_projectTypeRepository.GetProjectTypeByName(ProjectType.Name) == null)
                {


                    ProjectType.IsActive = true;
                    ProjectType.IsDeleted = false;
                    ProjectType.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _projectTypeRepository.Add(ProjectType);
                    if (SaveProjectType())
                    {
                        message = "ProjectType added successfully ...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! something went wrong ...";
                        return false;
                    }
                }
                else
                {
                    message = "ProjectType already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool UpdateProjectType(ref ProjectType ProjectType, ref string message)
        {
            try
            {
                if (_projectTypeRepository.GetProjectTypeByName(ProjectType.Name, ProjectType.ID) == null)
                {
                    ProjectType CurrentProjectType = _projectTypeRepository.GetById(ProjectType.ID);

                    CurrentProjectType.Name = ProjectType.Name;
                    CurrentProjectType.NameAr = ProjectType.NameAr;
                    ProjectType = null;

                    _projectTypeRepository.Update(CurrentProjectType);
                    if (SaveProjectType())
                    {
                        ProjectType = CurrentProjectType;
                        message = "ProjectType updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! something went wrong ...";
                        return false;
                    }
                }
                else
                {
                    message = "ProjectType already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool DeleteProjectType(long id, ref string message, bool softDelete = true)
        {
            try
            {
                ProjectType ProjectType = _projectTypeRepository.GetById(id);

                if (softDelete)
                {
                    ProjectType.IsDeleted = true;
                    _projectTypeRepository.Update(ProjectType);
                }
                else
                {
                    _projectTypeRepository.Delete(ProjectType);
                }
                if (SaveProjectType())
                {
                    message = "ProjectType deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! something went wrong ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool SaveProjectType()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }

    public interface IProjectTypeService
    {

        IEnumerable<ProjectType> GetProjectTypes();
        IEnumerable<object> GetProjectTypeForDropDown(string lang = "en");
        ProjectType GetProjectType(long id);
        bool CreateProjectType(ref ProjectType ProjectType, ref string message);
        bool UpdateProjectType(ref ProjectType ProjectType, ref string message);
        bool DeleteProjectType(long id, ref string message, bool softDelete = true);
        bool SaveProjectType();
        ProjectType GetProjectTypeByName(string name);
    }
}
