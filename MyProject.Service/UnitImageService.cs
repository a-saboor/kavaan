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
    public class UnitImageService : IUnitImageService
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitImageRepository unitImageRepository;
        public UnitImageService(IUnitImageRepository unitImageRepository, IUnitOfWork unitOfWork)
        {
            this.unitImageRepository = unitImageRepository;
            _unitOfWork = unitOfWork;
        }

        public bool Create(UnitImage unitimage, ref string message)
        {


            unitimage.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
            unitimage.IsActive = true;
            unitimage.IsDeleted = false;
            this.unitImageRepository.Add(unitimage);
            SaveUnitImages();


            message = "Unit Image Added Successfully";
            return true;
        }

        public bool DeleteUnitImage(long unitid, int position)
        {
            try
            {

                this.unitImageRepository.Delete(x => x.UnitD == unitid && x.Position == position);
                return true;
            }
            catch (Exception ex)
            {

                return false;
                throw;
            }
        }

        public bool DeleteUnitImages(long id, ref string message, ref string filepath)
        {
            try
            {
                UnitImage propertyImage = unitImageRepository.GetById(id);
                filepath = propertyImage.Image;
                unitImageRepository.Delete(propertyImage);
                if (SaveUnitImages())
                {
                    message = "Unit image deleted successfully ...";
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

        public UnitImage GetUnitImage(long id)
        {


            UnitImage propertyImage = this.unitImageRepository.GetById(id);
            return propertyImage;

        }

        public UnitImage GetUnitImageByPosition(long unitid, int position)
        {
            UnitImage propertyImage = this.unitImageRepository.GetAll().Where(x => x.UnitD == unitid && x.Position == position).FirstOrDefault();
            return propertyImage;
        }

        public IEnumerable<UnitImage> GetUnitImages(long unitid)
        {
            try
            {
                return this.unitImageRepository.GetAll().Where(x => x.UnitD == unitid&&x.IsDeleted==false);

            }
            catch (Exception)
            {

                return null;
            }

        }
        public int GalleyUnitCount(long UnitID)
        {
            var count = unitImageRepository.GetAll().Where(x => x.UnitD== UnitID).ToList().Count();
            return count;
        }
        #region Unit Service




        public bool SaveUnitImages()
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

        public bool UpdateUnitImage(ref UnitImage unitImage, ref string message)
        {
            try
            {
                unitImageRepository.Update(unitImage);
                if (SaveUnitImages())
                {
                    message = "Unit image updated successfully ...";
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
                UnitImage image = GetUnitImage(id);
                image.Position = position;

                if (UpdateUnitImage(ref image, ref message))
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

        #endregion
    }
    public interface IUnitImageService
    {
        UnitImage GetUnitImage(long id);
        UnitImage GetUnitImageByPosition(long propertyid, int position);
        IEnumerable<UnitImage> GetUnitImages(long unitid);
        bool Create(UnitImage unitimage, ref string message);
        bool UpdateUnitImage(ref UnitImage unitImage, ref string message);
        bool DeleteUnitImages(long id, ref string message, ref string filepath);
        bool DeleteUnitImage(long unitid, int position);
        int GalleyUnitCount(long UnitID);
        bool SaveItemImagePosition(long id, int position, ref string message);

    }
}
