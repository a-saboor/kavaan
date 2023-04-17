using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class AssetsContractorService : IAssetsContractorsService
    {
        private readonly IAssetContractorsRepository _assetsContractorsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AssetsContractorService(IUnitOfWork unitOfWork, IAssetContractorsRepository assetsContractorsRepository)
        {
            this._unitOfWork = unitOfWork;
            this._assetsContractorsRepository = assetsContractorsRepository;
        }
        public IEnumerable<AssetsContractor> GetAssetsContractors(long VendorID = 0)
        {
            var AssetsContractorss = _assetsContractorsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return AssetsContractorss;
        }
        public IEnumerable<AssetsContractor> GetAssetsContractorsByDate(DateTime startDate, DateTime endDate, long VendorID = 0)
        {
            var AssetsContractorss = _assetsContractorsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID && i.CreatedOn >= startDate && i.CreatedOn <= endDate);
            return AssetsContractorss;
        }
        public AssetsContractor GetAssetsContractor(long id)
        {
            var AssetsContractorss = this._assetsContractorsRepository.GetById(id);
            return AssetsContractorss;
        }
        public IEnumerable<object> GetContractorsForDropDown(string lang = "en", long VendorID = 0)
        {
            var contractractors = GetAssetsContractors(VendorID).Where(x => x.IsActive == true && x.IsDeleted==false);
            var dropdownList = from contractor in contractractors
                               select new { value = contractor.ID, text = lang == "en" ? contractor.Name : contractor.Name };
            return dropdownList;
        }
        public bool CreateAssetsContractor(AssetsContractor assetsContractor, ref string message)
        {
            try
            {

                assetsContractor.IsActive = true;
                assetsContractor.IsDeleted = false;
                assetsContractor.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _assetsContractorsRepository.Add(assetsContractor);
                if (SaveAssetsContractor())
                {
                    message = "Contractor added successfully...";
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
        public bool UpdateAssetsContractor(ref AssetsContractor assetsContractor, ref string message)
        {
            try
            {
                AssetsContractor currentContractor = _assetsContractorsRepository.GetById(assetsContractor.ID);
                currentContractor.IsActive = assetsContractor.IsActive;

                _assetsContractorsRepository.Update(currentContractor);
                if (SaveAssetsContractor())
                {
                    assetsContractor = currentContractor;
                    message = "Contractor updated successfully ...";
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
        public bool DeleteAssetsContractor(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    AssetsContractor assetsContractor = _assetsContractorsRepository.GetById(id);
                    //When category delete, its Contractor will be deleted
                    //*
                    assetsContractor.IsDeleted = true;
                    _assetsContractorsRepository.Update(assetsContractor);

                    if (SaveAssetsContractor())
                    {
                        message = "Contractor deleted successfully ...";
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
                    AssetsContractor Contractor = _assetsContractorsRepository.GetById(id);

                    _assetsContractorsRepository.Delete(Contractor);

                    if (SaveAssetsContractor())
                    {
                        message = "Contractor deleted successfully ...";
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
        public bool SaveAssetsContractor()
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
    public interface IAssetsContractorsService
    {
        AssetsContractor GetAssetsContractor(long id);
        IEnumerable<AssetsContractor> GetAssetsContractors(long VendorID = 0);
        IEnumerable<AssetsContractor> GetAssetsContractorsByDate(DateTime startDate, DateTime endDate, long VendorID = 0);
        IEnumerable<object> GetContractorsForDropDown(string lang = "en", long VendorID = 0);
        bool CreateAssetsContractor(AssetsContractor assetsContractor, ref string message);
        bool UpdateAssetsContractor(ref AssetsContractor assetsContractor, ref string message);
        bool DeleteAssetsContractor(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveAssetsContractor();
    }
}
