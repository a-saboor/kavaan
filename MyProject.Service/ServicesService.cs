using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class ServicesService : IServicesService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            this._serviceRepository = serviceRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Data.Service> GetServices()
        {
            var services = _serviceRepository.GetAll().Where(d => d.IsDeleted == false);
            return services;
        }

        public Data.Service GetService(long id)
        {
            var service = _serviceRepository.GetById(id);
            return service;
        }

        public Data.Service GetServiceBySlug(string slug)
        {
            var Service = _serviceRepository.GetServiceBySlug(slug);
            return Service;
        }

        public IEnumerable<Data.Service> GetServicesByCategoryID(long id)
        {
            var services = _serviceRepository.GetAll().Where(x=>x.CategoryID==id && x.IsDeleted==false && x.IsActive==true);
            return services;
        } 
        public IEnumerable<Data.Service> GetServicesByDate(DateTime FromDate, DateTime ToDate, int id = 0)
        {
            var services = _serviceRepository.GetServicesByDate(FromDate, ToDate);
            return services;
        }
        public bool CreateSerivce(Data.Service service, ref string message )
        {
            try
            {

                service.IsActive = true;
                service.IsDeleted = false;
                service.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _serviceRepository.Add(service);
                if (SaveService())
                {
                    message = "Serivce added successfully...";
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

        public bool UpdateService(ref Data.Service service , ref string message)
        {
            try
            {
                _serviceRepository.Update(service);
                if (SaveService())
                {
                    message = "Service updated successfully...";
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

        public bool DeleteService(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {
                    //soft delete
                    Data.Service service = _serviceRepository.GetById(id);
                    service.IsDeleted = true;
                    _serviceRepository.Update(service);

                    if (SaveService())
                    {
                        message = "Service deleted successfully ...";
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
            else
            {

                //hard delete
                try
                {
                    Data.Service service = _serviceRepository.GetById(id);

                    _serviceRepository.Delete(service);

                    if (SaveService())
                    {
                        message = "Service deleted successfully...";
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
        }

        public bool SaveService()
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
        public IEnumerable<SP_GetFilteredServices_Result> GetFilteredServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, long? CategoryID)
        {
            var services = _serviceRepository.GetFilteredServices(search, pageSize, pageNumber, sortBy, lang, imageServer, CategoryID );
            return services;
        }
        public IEnumerable<SP_GetPopularService_Result> GetPopularServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var services = _serviceRepository.GetPopularServices(search, pageSize, pageNumber, sortBy, lang, imageServer).OrderByDescending(i=>i.CountServices);
            return services;
        }

    }
    public interface IServicesService
    {
        IEnumerable<Data.Service> GetServices();
        IEnumerable<Data.Service> GetServicesByCategoryID(long id);

        IEnumerable<Data.Service> GetServicesByDate(DateTime FromDate, DateTime ToDate, int id = 0);
        Data.Service GetService(long id);
        Data.Service GetServiceBySlug(string slug);
        bool CreateSerivce(Data.Service service, ref string message);
        bool UpdateService(ref Data.Service service, ref string message);
        bool DeleteService(long id, ref string message, bool softdelete);
        bool SaveService();
        IEnumerable<SP_GetPopularService_Result> GetPopularServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
        IEnumerable<SP_GetFilteredServices_Result> GetFilteredServices(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, long? CategoryID);
    }
}
