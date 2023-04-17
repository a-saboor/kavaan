using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProgressRepository progressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;

        public ProjectService(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IProgressRepository progressRepository, IUnitRepository unitRepository)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            this.progressRepository = progressRepository;
            this._unitRepository = unitRepository;
        }
        #region Project Service


        public bool Create(Data.Project project, ref string message)
        {
            try
            {
                project.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                project.IsDeleted = false;
                project.IsActive = true;

                _projectRepository.Add(project);
                //IEnumerable<Progress> progresses = this.progressRepository.GetAll();

                if (this._projectRepository.GetProjectByName(project.Name) == null)
                {
                    if (SaveProject())
                    {
                        string propmessage = string.Empty;
                        message = "Project added successfully ...";
                        return true;
                    }

                }
                else
                {
                    message = "Project already exist ...";
                    return false;
                }
                message = "Oops! something went wrong ...";
                return false;


            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public IEnumerable<Data.Project> GetProjects()
        {
            var project = _projectRepository.GetAll().Where(x => x.IsDeleted == false);
            return project;
        }

        public Data.Project GetProject(long id)
        {
            Data.Project project = _projectRepository.GetById(id);
            return project;
        }

        public Data.Project GetProjectBySlug(string slug)
        {
            Data.Project project = this.GetProjects().FirstOrDefault(x => x.Slug == slug);
            return project;
        }

        public bool ActivateProjects(long projectId, ref string message, bool isActive = true)
        {
            try
            {
                var project = GetProject(projectId);

                project.IsActive = isActive;
                _projectRepository.Update(project);

                if (SaveProject())
                {
                    message = "Project updated successfully ...";
                    return true;
                }
                message = "Oops! something went wrong ...";
                return false;
            }
            catch (Exception ex)
            {
                message = "Oops Something Went Wrong";
                return false;
            }
        }
        public bool DeleteProjects(long projectId, ref string message, bool softDelete = true)
        {
            try
            {
                var project = GetProject(projectId);
                project.IsDeleted = true;
                if (project != null && softDelete)
                {
                    _projectRepository.Update(project);
                }
                else if (project != null && softDelete == false)
                {
                    _projectRepository.Delete(project);
                }

                if (SaveProject())
                {
                    message = "Project deleted successfully ...";
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                message = "Oops! Something Went Wrong";
                return false;
            }

        }

        public bool UpdateProject(ref Data.Project project, ref string message, bool updater = true)
        {
            try
            {
                if (_projectRepository.GetProjectByName(project.Name, project.ID) == null || true)
                {
                    var CurrentProject = new Data.Project();
                    if (updater)
                    {
                        CurrentProject = _projectRepository.GetById(project.ID);

                        //CurrentProject.SKU = project.SKU;
                        CurrentProject.Name = project.Name;
                        CurrentProject.NameAr = project.NameAr;
                        CurrentProject.Slug = project.Slug;
                        CurrentProject.Purpose = project.Purpose;
                        CurrentProject.Bedrooms = project.Bedrooms;
                        CurrentProject.Baths = project.Baths;
                        CurrentProject.AreaStart = project.AreaStart;
                        CurrentProject.AreaEnd = project.AreaEnd;
                        CurrentProject.ProjectTypeID = project.ProjectTypeID;
                        CurrentProject.PriceStart = project.PriceStart;
                        CurrentProject.PriceEnd = project.PriceEnd;
                        CurrentProject.Address = project.Address;
                        CurrentProject.Latitude = project.Latitude;
                        CurrentProject.Longitude = project.Longitude;
                        CurrentProject.Country = project.Country;
                        CurrentProject.City = project.City;
                        CurrentProject.Locality = project.Locality;
                        CurrentProject.Developer = project.Developer;
                        //CurrentProject.MobileDescription = project.MobileDescription;
                        //CurrentProject.MobileDescriptionAr = project.MobileDescriptionAr;
                        CurrentProject.ShortDescription = project.ShortDescription;
                        CurrentProject.ShortDescriptionAr = project.ShortDescriptionAr;
                        CurrentProject.Description = project.Description;
                        CurrentProject.DescriptionAr = project.DescriptionAr;
                        //CurrentProject.RegularPrice = project.RegularPrice;
                        //CurrentProject.SalePrice = project.SalePrice;
                        //CurrentProject.SalePriceFrom = project.SalePriceFrom;
                        //CurrentProject.SalePriceTo = project.SalePriceTo;
                        //CurrentProject.Stock = project.Stock;
                        //CurrentProject.Threshold = project.Threshold;
                        //CurrentProject.StockStatus = project.StockStatus;
                        //CurrentProject.Weight = project.Weight;
                        //CurrentProject.Length = project.Length;
                        //CurrentProject.Width = project.Width;
                        //CurrentProject.Height = project.Height;
                        //CurrentProject.Type = project.Type;
                        //CurrentProject.BrandID = project.BrandID;

                        //CurrentProject.PurchaseNote = project.PurchaseNote;
                        //CurrentProject.EnableReviews = project.EnableReviews;
                        //CurrentProject.IsSoldIndividually = project.IsSoldIndividually;

                        //CurrentProject.IsManageStock = project.IsManageStock;
                        //CurrentProject.IsPublished = project.IsPublished;
                        //CurrentProject.IsRecommended = project.IsRecommended;
                        //CurrentProject.IsFeatured = project.IsFeatured;
                        //CurrentProject.Status = project.Status;

                        //CurrentProject.PublishStartDate = project.PublishStartDate;
                        //CurrentProject.PublishEndDate = project.PublishEndDate;

                        project = null;

                        _projectRepository.Update(CurrentProject);
                    }
                    else
                    {
                        _projectRepository.Update(project);
                    }
                    if (SaveProject())
                    {
                        project = project == null ? CurrentProject : project;
                        message = "Project updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later.";
                        return false;
                    }
                }
                else
                {
                    message = "Project not found  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool DeleteProject(long id, ref string message, bool hasChilds, bool softDelete = true)
        {

            try
            {
                var project = _projectRepository.GetById(id);

                project.IsDeleted = true;
                if (project != null && softDelete)
                {

                    _projectRepository.Update(project);

                    if (SaveProject())
                    {
                        message = "Project deleted successfully ...";
                        return true;
                    }
                }
                else if (project != null && softDelete == false)
                {

                    _projectRepository.Delete(project);
                    message = "Project permanently deleted successfully ...";
                    return true;
                }

                message = "Oops! Something Went Wrong";
                return true;
            }
            catch (Exception)
            {
                message = "Oops! Something Went Wrong";
                return false;
            }



        }
        public bool SaveProject()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var exp = ex.Message;
                return false;
            }
        }

        public IEnumerable<object> GetProjectsForDropDown()
        {
            var Projects = GetProjects().Where(x => x.IsActive == true);
            var dropdownList = from project in Projects
                               select new { value = project.ID, text = project.Name };
            return dropdownList;
        }

        public IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> ProjectTypeID)
        {
            var projects = _projectRepository.GetFilteredProjects(search, pageSize, pageNumber, sortBy, lang, imageServer, ProjectTypeID);
            return projects;
        }

        //public IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID, Nullable<bool> isfeatured, Nullable<bool> vrTour)
        //{
        //    var properties = _projectRepository.GetFilteredProjects(search, pageSize, pageNumber, sortBy, lang, imageServer, developmentID, isfeatured, vrTour);
        //    return properties;
        //}


        #endregion
    }
    public interface IProjectService
    {
        bool Create(Data.Project Project, ref string message);
        IEnumerable<Data.Project> GetProjects();
        Data.Project GetProject(long id);
        Data.Project GetProjectBySlug(string slug);

        bool ActivateProjects(long projectId, ref string message, bool isActive = true);
        bool DeleteProjects(long projectId, ref string message, bool softDelete = true);

        bool UpdateProject(ref Data.Project Project, ref string message, bool updater = true);

        bool DeleteProject(long id, ref string message, bool hasChilds, bool softDelete = true);
        IEnumerable<object> GetProjectsForDropDown();

        IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> ProjectTypeID);

        //IEnumerable<SP_GetFilteredProjects_Result> GetFilteredProjects(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID, Nullable<bool> isfeatured, Nullable<bool> vrTour);
    }
}
