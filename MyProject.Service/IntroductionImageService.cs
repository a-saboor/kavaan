using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class IntroductionImageService : IIntroductionImageService
    {
        private readonly IIntroductionImageRepository _introductionImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IntroductionImageService(IIntroductionImageRepository introductionImageRepository, IUnitOfWork unitOfWork)
        {
            this._introductionImageRepository = introductionImageRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IIntroductionImageService Members

        public int GalleyIntroductionCount(long IntroductionID)
        {
            var count = _introductionImageRepository.GetAll().Where(x => x.IntroductionID == IntroductionID).ToList().Count();
            return count;
        }
        public IEnumerable<IntroductionImage> GetIntroductionImages()
        {
            var introductionImages = _introductionImageRepository.GetAll();
            return introductionImages;
        }

        public IEnumerable<IntroductionImage> GetIntroductionImagesByIntroID(long introID)
        {
            var introductionImages = _introductionImageRepository.GetAll().Where(x=> x.IntroductionID==introID);
            return introductionImages;
        }

        public IntroductionImage GetIntroductionImage(long id)
        {
            var IntroductionImage = _introductionImageRepository.GetById(id);
            return IntroductionImage;
        }

        public bool CreateIntroductionImage(ref IntroductionImage introductionImage, ref string message)
        {
            try
            {
                _introductionImageRepository.Add(introductionImage);
                if (SaveIntroductionImage())
                {

                    message = "Introduction image added successfully ...";
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

        public bool UpdateIntroductionImage(ref IntroductionImage introductionImage, ref string message)
        {
            try
            {
                _introductionImageRepository.Update(introductionImage);
                if (SaveIntroductionImage())
                {
                    message = "Introduction image updated successfully ...";
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
                IntroductionImage image = GetIntroductionImage(id);
                image.Position = position;

                if (UpdateIntroductionImage(ref image, ref message))
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

        public bool DeleteIntroductionImage(long id, ref string message, ref string filepath)
        {
            try
            {
                IntroductionImage introductionImage = _introductionImageRepository.GetById(id);
                filepath = introductionImage.Image;
                _introductionImageRepository.Delete(introductionImage);
                if (SaveIntroductionImage())
                {
                    message = "Introduction image deleted successfully ...";
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

        public bool SaveIntroductionImage()
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
    public interface IIntroductionImageService
    {
        int GalleyIntroductionCount(long IntroductionID);
        IEnumerable<IntroductionImage> GetIntroductionImages();
        IEnumerable<IntroductionImage> GetIntroductionImagesByIntroID(long introID);
        IntroductionImage GetIntroductionImage(long id);
        bool CreateIntroductionImage(ref IntroductionImage introductionImage, ref string message);
        bool UpdateIntroductionImage(ref IntroductionImage introductionImage, ref string message);
        bool SaveItemImagePosition(long id, int position, ref string message);
        bool DeleteIntroductionImage(long id, ref string message, ref string filepath);
        bool SaveIntroductionImage();
    }
}
