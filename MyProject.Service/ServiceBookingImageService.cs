using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class ServiceBookingImageService : IServiceBookingImageService
    {
        private readonly IServiceBookingImageRepository _serviceBookingImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBookingImageService(IServiceBookingImageRepository serviceBookingImageRepository, IUnitOfWork unitOfWork)
        {
            this._serviceBookingImageRepository = serviceBookingImageRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<ServicebookingImage> GetServiceBookingImagesByBookingID(long servicebookingid)
        {
            var areas = _serviceBookingImageRepository.GetAll().Where(x => x.ServiceBookingID == servicebookingid);
            return areas;
        }
        public bool CreateServiceBookingImage(ServicebookingImage data, ref string message)
        {
            try
            {
                _serviceBookingImageRepository.Add(data);
                if (SaveImage())
                {
                    message = "Image added successfully ...";
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
        public bool SaveImage()
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
    }
    public interface IServiceBookingImageService
    {
        IEnumerable<ServicebookingImage> GetServiceBookingImagesByBookingID(long servicebookingid);
        bool CreateServiceBookingImage(ServicebookingImage data, ref string message);
        bool SaveImage();
    }
}
