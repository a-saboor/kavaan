using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class OzoneImageService : IOzoneImageService
    {
        private readonly IOzoneImageRepository _ozoneImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OzoneImageService(IOzoneImageRepository ozoneImageRepository, IUnitOfWork unitOfWork)
        {
            this._ozoneImageRepository = ozoneImageRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IOzoneImageService Members

        public int GalleyOzoneCount(long OzoneID)
        {
            var count = _ozoneImageRepository.GetAll().Where(x => x.OzoneID == OzoneID).ToList().Count();
            return count;
        }
        public IEnumerable<OzoneImage> GetOzoneImages()
        {
            var ozoneImage = _ozoneImageRepository.GetAll();
            return ozoneImage;
        }

        public IEnumerable<OzoneImage> GetOzoneImagesByOzoneID(long OzoneID)
        {
            var ozoneImage = _ozoneImageRepository.GetAll().Where(x => x.OzoneID == OzoneID);
            return ozoneImage;
        }

        public OzoneImage GetOzoneImage(long id)
        {
            var OzoneImage = _ozoneImageRepository.GetById(id);
            return OzoneImage;
        }

        public bool CreateOzoneImage(ref OzoneImage ozoneImage, ref string message)
        {
            try
            {
                _ozoneImageRepository.Add(ozoneImage);
                if (SaveOzoneImage())
                {

                    message = "Ozone image added successfully ...";
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

        public bool UpdateOzoneImage(ref OzoneImage ozoneImage, ref string message)
        {
            try
            {
                _ozoneImageRepository.Update(ozoneImage);
                if (SaveOzoneImage())
                {
                    message = "Ozone image updated successfully ...";
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
        public bool SaveItemImagePosition(long id, int position, ref string message)
        {
            try
            {
                OzoneImage image = GetOzoneImage(id);
                image.Position = position;

                if (UpdateOzoneImage(ref image, ref message))
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

        public bool DeleteOzoneImage(long id, ref string message, ref string filepath)
        {
            try
            {
                OzoneImage ozoneImage = _ozoneImageRepository.GetById(id);
                filepath = ozoneImage.Image;
                _ozoneImageRepository.Delete(ozoneImage);
                if (SaveOzoneImage())
                {
                    message = "Ozone image deleted successfully ...";
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

        public bool SaveOzoneImage()
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
    public interface IOzoneImageService
    {
        int GalleyOzoneCount(long OzoneID);
        IEnumerable<OzoneImage> GetOzoneImages();
        IEnumerable<OzoneImage> GetOzoneImagesByOzoneID(long OzoneID);
        OzoneImage GetOzoneImage(long id);
        bool CreateOzoneImage(ref OzoneImage ozoneImage, ref string message);
        bool UpdateOzoneImage(ref OzoneImage ozoneImage, ref string message);
        bool SaveItemImagePosition(long id, int position, ref string message);
        bool DeleteOzoneImage(long id, ref string message, ref string filepath);
        bool SaveOzoneImage();
    }
}
