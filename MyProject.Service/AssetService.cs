using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class AssetService : IAssetsService
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AssetService(IUnitOfWork unitOfWork, IAssetsRepository assetsRepository)
        {
            this._unitOfWork = unitOfWork;
            this._assetsRepository = assetsRepository;
        }
        public IEnumerable<Asset> GetAssets(long VendorID = 0)
        {
            var Assets = _assetsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return Assets;
        }
        public IEnumerable<Asset> GetAssetsByDate(DateTime FromDate, DateTime ToDate, long VendorID = 0)
        {
            var Assets = _assetsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID && i.CreatedOn >= FromDate && i.CreatedOn <= ToDate);
            return Assets;
        }
        public Asset GetAsset(long id)
        {
            var Assets = this._assetsRepository.GetById(id);
            return Assets;
        }
        public bool CreateAsset(Asset asset, ref string message)
        {
            try
            {
                asset.IsActive = true;
                asset.IsDeleted = false;
                asset.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _assetsRepository.Add(asset);
                if (SaveAsset())
                {
                    message = "Asset added successfully...";
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

        public bool UpdateAsset(ref Asset asset, ref string message)
        {
            try
            {
                Asset Currentasset = this._assetsRepository.GetById(asset.ID);
                Currentasset.LocationID = asset.LocationID;
                Currentasset.AssetsContractorID = asset.AssetsContractorID;
                Currentasset.AssetsParentCategoryID = asset.AssetsParentCategoryID;
                Currentasset.AssetsCategoryID = asset.AssetsCategoryID;
                Currentasset.AssetsProductID = asset.AssetsProductID;
                Currentasset.DepartmentID = asset.DepartmentID;
                Currentasset.StaffID = asset.StaffID;
                Currentasset.Name = asset.Name;
                Currentasset.SerialNumber = asset.SerialNumber;
                Currentasset.Price = asset.Price;
                Currentasset.Description = asset.Description;
                Currentasset.PurchaseDate = asset.PurchaseDate;
                Currentasset.WarrantyExpiryDate = asset.WarrantyExpiryDate;
                Currentasset.PurchaseType = asset.PurchaseType;
                Currentasset.DepreciationType = asset.DepreciationType;
                Currentasset.UsefulLife = asset.UsefulLife;
                Currentasset.ResidualValue = asset.ResidualValue;
                Currentasset.RatePercentage = asset.RatePercentage;
                Currentasset.Position = asset.Position;
                _assetsRepository.Update(Currentasset);
                asset = null;
                if (SaveAsset())
                {
                    asset = Currentasset;
                    message = "Asset updated successfully...";
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

        public bool DeleteAsset(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {
                    //soft delete
                    Asset asset = _assetsRepository.GetById(id);
                    asset.IsDeleted = true;
                    _assetsRepository.Update(asset);

                    if (SaveAsset())
                    {
                        message = "Asset deleted successfully ...";
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
                    Asset asset = _assetsRepository.GetById(id);

                    _assetsRepository.Delete(asset);

                    if (SaveAsset())
                    {
                        message = "Asset deleted successfully...";
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

        public bool SaveAsset()
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
    public interface IAssetsService
    {
        Asset GetAsset(long id);
        IEnumerable<Asset> GetAssets(long VendorID = 0);
        IEnumerable<Asset> GetAssetsByDate(DateTime FromDate, DateTime ToDate, long VendorID = 0);
        bool CreateAsset(Asset asset, ref string message);
        bool UpdateAsset(ref Asset asset, ref string message);
        bool DeleteAsset(long id, ref string message, bool softdelete);
        bool SaveAsset();
    }
}
