using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class VendorServicesService : IVendorServicesService
    {
        private readonly IVendorServiceRepository _vendorServicesRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        public VendorServicesService(IUnitOfWork unitOfWork, IServiceRepository serviceRepository, IVendorServiceRepository vendorServicesRepository)
        {
            this._unitOfWork = unitOfWork;
            this._vendorServicesRepository = vendorServicesRepository;
            this._serviceRepository = serviceRepository;
        }
        public IEnumerable<Data.VendorService> GetVendorServices(long vendorID)
        {
            var vendorServices = _vendorServicesRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == vendorID);
            return vendorServices;
        }
        public Data.VendorService GetVendorService(long id)
        {
            var vendorService = this._vendorServicesRepository.GetById(id);
            return vendorService;
        }
        public Data.VendorService GetVendorServiceByVendorAndServiceID(long ServiceID, long VendorID)
        {
            var vendorService = this.GetVendorServices(VendorID).FirstOrDefault(x => x.ServiceID == ServiceID);
            return vendorService;
        }
        //public IEnumerable<object> GetDepartmentsForDropDown(string lang = "en")
        //{
        //    var Departments = GetDepartments().Where(x => x.IsActive == true && x.IsDeleted == false);
        //    var dropdownList = from departments in Departments
        //                       select new { value = departments.ID, text = lang == "en" ? departments.Name : departments.Name };
        //    return dropdownList;
        //}
        public bool CreateVendorServices(Data.VendorService vendorServices, ref string message)
        {
            try
            {

                vendorServices.IsActive = true;
                vendorServices.IsDeleted = false;
                vendorServices.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _vendorServicesRepository.Add(vendorServices);
                if (SaveVendorServices())
                {
                    message = "Vendor Service added successfully...";
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
        public bool UpdateVendorServices(ref Data.VendorService vendorServices, ref string message)
        {
            try
            {
                Data.VendorService currentvendorService = _vendorServicesRepository.GetById(vendorServices.ID);
                currentvendorService.IsActive = currentvendorService.IsActive;

                _vendorServicesRepository.Update(vendorServices);
                if (SaveVendorServices())
                {
                    vendorServices = currentvendorService;
                    message = "Vendor updated successfully ...";
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
        public bool DeleteVendorServices(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    Data.VendorService vendorService = _vendorServicesRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                            // delete code here.....
                    //*
                    vendorService.IsDeleted = true;
                    _vendorServicesRepository.Update(vendorService);

                    if (SaveVendorServices())
                    {
                        message = "Vendor service deleted successfully ...";
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
                    Data.VendorService vendorService = _vendorServicesRepository.GetById(id);

                    _vendorServicesRepository.Delete(vendorService);

                    if (SaveVendorServices())
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
        }
        public bool SaveVendorServices()
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
    public interface IVendorServicesService
    {
        Data.VendorService GetVendorService(long id);
        Data.VendorService GetVendorServiceByVendorAndServiceID(long ServiceID, long VendorID);
        IEnumerable<Data.VendorService> GetVendorServices(long vendorID);
        //IEnumerable<object> GetDepartmentsForDropDown(string lang = "en");
        bool CreateVendorServices(Data.VendorService vendorService, ref string message);
        bool UpdateVendorServices(ref Data.VendorService vendorService, ref string message);
        bool DeleteVendorServices(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveVendorServices();
    }
}
