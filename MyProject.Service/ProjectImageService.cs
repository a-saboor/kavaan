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
    public class ProjectImageService : IProjectImageService
    {
        private readonly IProjectImageRepository _projectImagesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProjectImageService(IProjectImageRepository projectImagesRepository, IUnitOfWork unitOfWork)
        {
            this._projectImagesRepository = projectImagesRepository;
            _unitOfWork = unitOfWork;
        }

        public int GalleyProjectCount(long projectID)
        {
            var count = _projectImagesRepository.GetAll().Where(x => x.ProjectID == projectID).ToList().Count();
            return count;
        }

        public bool Create(ProjectImage projectimage, ref string message)
        {
            this._projectImagesRepository.Add(projectimage);
            SaveProjectImages();
            message = "Project images added successfully...";
            return true;
        }

        public bool DeleteProjectImage(long projectid, int position)
        {
            try
            {

                this._projectImagesRepository.Delete(x => x.ProjectID == projectid && x.Position == position);
                return true;
            }
            catch (Exception ex)
            {

                return false;
                throw;
            }
        }
        public bool SaveItemImagePosition(long id, int position, ref string message)
        {
            try
            {
                ProjectImage image = GetProjectImage(id);
                image.Position = position;

                if (UpdateProjectImage(ref image, ref message))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return false;
            }
        }

        public bool DeleteProjectImages(long id, ref string message, ref string filepath)
        {
            try
            {
                ProjectImage projectImage = _projectImagesRepository.GetById(id);
                filepath = projectImage.Image;
                _projectImagesRepository.Delete(projectImage);
                if (SaveProjectImages())
                {
                    message = "Project image deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public ProjectImage GetProjectImage(long id)
        {


            ProjectImage projectImage = this._projectImagesRepository.GetById(id);
            return projectImage;

        }

        public ProjectImage GetProjectImageByPosition(long projectid, int position)
        {
            ProjectImage projectImage = this._projectImagesRepository.GetAll().Where(x => x.ProjectID == projectid && x.Position == position).FirstOrDefault();
            return projectImage;
        }

        public IEnumerable<ProjectImage> GetProjectImages(long projectid)
        {
            try
            {
                return this._projectImagesRepository.GetAll().Where(x => x.ProjectID == projectid);

            }
            catch (Exception)
            {

                return null;
            }

        }
        #region Project Service
        public bool SaveProjectImages()
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

        public bool UpdateProjectImage(ref ProjectImage projectImage, ref string message)
        {
            try
            {
                _projectImagesRepository.Update(projectImage);
                if (SaveProjectImages())
                {
                    message = "Project image updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        #endregion
    }
    public interface IProjectImageService
    {
        int GalleyProjectCount(long projectID);
        ProjectImage GetProjectImage(long id);
        ProjectImage GetProjectImageByPosition(long projectid, int position);
        IEnumerable<ProjectImage> GetProjectImages(long projectid);
        bool Create(ProjectImage projectimage, ref string message);
        bool UpdateProjectImage(ref ProjectImage projectImage, ref string message);
        bool DeleteProjectImage(long projectid, int position);
        bool SaveItemImagePosition(long id, int position, ref string message);

        bool DeleteProjectImages(long id, ref string message, ref string filepath);


    }
}
