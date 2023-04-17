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
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IPropertyImagesRepository propertyImagesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PropertyImageService(IPropertyImagesRepository propertyImagesRepository, IUnitOfWork unitOfWork)
        {
            this.propertyImagesRepository = propertyImagesRepository;
            _unitOfWork = unitOfWork;
        }

        public int GalleyPropertyCount(long propertyID)
        {
            var count = propertyImagesRepository.GetAll().Where(x => x.PropertyID == propertyID).ToList().Count();
            return count;
        }

        public bool Create(PropertyImage propertyimage, ref string message)
        {


            propertyimage.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
            propertyimage.IsActive = true;
            propertyimage.IsDeleted = false;
            this.propertyImagesRepository.Add(propertyimage);
            SavePropertyImages();


            message = "Project images added successfully...";
            return true;
        }

        public bool DeletePropertyImage(long propertyid, int position)
        {
            try
            {

                this.propertyImagesRepository.Delete(x => x.PropertyID == propertyid && x.Position == position);
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
                PropertyImage image = GetPropertyImage(id);
                image.Position = position;

                if (UpdatePropertyImage(ref image, ref message))
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

        public bool DeletePropertyImages(long id, ref string message, ref string filepath)
        {
            try
            {
                PropertyImage propertyImage = propertyImagesRepository.GetById(id);
                filepath = propertyImage.Image;
                propertyImagesRepository.Delete(propertyImage);
                if (SavePropertyImages())
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

        public PropertyImage GetPropertyImage(long id)
        {


            PropertyImage propertyImage = this.propertyImagesRepository.GetById(id);
            return propertyImage;

        }

        public PropertyImage GetPropertyImageByPosition(long propertyid, int position)
        {
            PropertyImage propertyImage = this.propertyImagesRepository.GetAll().Where(x => x.PropertyID == propertyid && x.Position == position).FirstOrDefault();
            return propertyImage;
        }

        public IEnumerable<PropertyImage> GetPropertyImages(long propertyid)
        {
            try
            {
                return this.propertyImagesRepository.GetAll().Where(x => x.PropertyID == propertyid);

            }
            catch (Exception)
            {

                return null;
            }

        }
        #region Property Service




        public bool SavePropertyImages()
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

        public bool UpdatePropertyImage(ref PropertyImage propertyImage, ref string message)
        {
            try
            {
                propertyImagesRepository.Update(propertyImage);
                if (SavePropertyImages())
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
    public interface IPropertyImageService
    {
        int GalleyPropertyCount(long propertyID);
        PropertyImage GetPropertyImage(long id);
        PropertyImage GetPropertyImageByPosition(long propertyid, int position);
        IEnumerable<PropertyImage> GetPropertyImages(long propertyid);
        bool Create(PropertyImage propertyimage, ref string message);
        bool UpdatePropertyImage(ref PropertyImage propertyImage, ref string message);
        bool DeletePropertyImage(long propertyid, int position);
        bool SaveItemImagePosition(long id, int position, ref string message);

        bool DeletePropertyImages(long id, ref string message, ref string filepath);


    }
}
