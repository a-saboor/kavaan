using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class VendorPackagesService : IVendorPackagesService
    {
        private readonly IVendorPackagesRepository _vendorPackagesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public VendorPackagesService(IVendorPackagesRepository vendorPackagesRepository, IUnitOfWork unitOfWork)
        {
            this._vendorPackagesRepository = vendorPackagesRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<VendorPackage> GetVendorPackages()
        {
            var packages = _vendorPackagesRepository.GetAll().Where(d => d.IsDeleted == false);
            return packages;
        }
        public VendorPackage GetByID(long id)
        {
            var packages = _vendorPackagesRepository.GetById(id);
            return packages;
        }

        public VendorPackage GetVendorPackagesByID(long id)
        {
            var packages = _vendorPackagesRepository.GetById(id);
            return packages;
        }
        public IEnumerable<object> GetPackageForDropDown(string lang = "en")
        {
            var packages = GetVendorPackages().Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from package in packages
                               select new { value = package.ID, text = lang == "en" ? package.Name : package.NameAr };
            return dropdownList;
        }
        public bool CreateVendorPackages(VendorPackage vendorPackages, ref string message)
        {
            try
            {
                    vendorPackages.IsActive = true;
                    vendorPackages.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _vendorPackagesRepository.Add(vendorPackages);
                    if (SaveVendorPackages())
                    {
                        message = "Vendor Packages added successfully ...";
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

        public bool UpdateVendorPackages(ref VendorPackage vendorPackage, ref string message)
        {
            try
            {
                    VendorPackage CurrentVendorPackage = _vendorPackagesRepository.GetById(vendorPackage.ID);

                    CurrentVendorPackage.IsActive = vendorPackage.IsActive;
                    _vendorPackagesRepository.Update(CurrentVendorPackage);
                    if (SaveVendorPackages())
                    {
                        vendorPackage = CurrentVendorPackage;
                        message = "Vendor Package updated successfully ...";
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

        public bool DeleteVendorPackages(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {

                    //soft delete
                    VendorPackage Vendor = _vendorPackagesRepository.GetById(id);
                    Vendor.IsDeleted = true;
                    _vendorPackagesRepository.Update(Vendor);

                    if (SaveVendorPackages())
                    {
                        message = "Vendor Package deleted successfully ...";
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
                    VendorPackage vendor = _vendorPackagesRepository.GetById(id);

                    _vendorPackagesRepository.Delete(vendor);

                    if (SaveVendorPackages())
                    {
                        message = "Vendor Package deleted successfully ...";
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

        public bool SaveVendorPackages()
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
    public interface IVendorPackagesService
    {
        IEnumerable<VendorPackage> GetVendorPackages();
        VendorPackage GetByID(long id);
        VendorPackage GetVendorPackagesByID(long id);
        IEnumerable<object> GetPackageForDropDown(string lang = "en");
        bool CreateVendorPackages(VendorPackage vendorPackage, ref string message);
        bool UpdateVendorPackages(ref VendorPackage vendorPackage, ref string message);
        bool DeleteVendorPackages(long id, ref string message, bool softdelete);
        bool SaveVendorPackages();
    }
}
