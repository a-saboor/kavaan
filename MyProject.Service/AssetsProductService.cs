using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class AssetsProductService : IAssetsProductsService
    {
        private readonly IAssetsProductsRepository _assetsProductsRepository;
        private readonly IAssetsCategoriesRepository _assetsCategoriesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AssetsProductService(IUnitOfWork unitOfWork, IAssetsProductsRepository assetsProductsRepository, IAssetsCategoriesRepository assetsCategoriesRepository)
        {
            this._unitOfWork = unitOfWork;
            this._assetsProductsRepository = assetsProductsRepository;
            this._assetsCategoriesRepository = assetsCategoriesRepository;
        }
        public IEnumerable<AssetsProduct> GetAssetsProducts(long VendorID = 0)
        {
            var AssetsProductss = _assetsProductsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return AssetsProductss;
        }
        public IEnumerable<AssetsProduct> GetAssetsProductsByDate(DateTime startDate, DateTime endDate, long VendorID = 0)
        {
            var AssetsProductss = _assetsProductsRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID && i.CreatedOn >= startDate && i.CreatedOn <= endDate);
            return AssetsProductss;
        }
        public AssetsProduct GetAssetsProduct(long id)
        {
            var AssetsProductss = this._assetsProductsRepository.GetById(id);
            return AssetsProductss;
        }
        public IEnumerable<object> GetAssetsCategoryForDropDown(long? id)
        {
            var Categories = _assetsCategoriesRepository.GetAll().Where(x => x.ParentID == null && x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from categories in Categories
                               where categories.ID != id
                               //where (categories.ParentCategoryID < 1 || categories.ParentCategoryID == null) && categories.ID != id
                               select new { value = categories.ID, text = categories.Name };
            return dropdownList;
        }
        public IEnumerable<object> GetAssetsProductForDropDownByAssetCategoryID(long? id)
        {
            var assetProduct = _assetsProductsRepository.GetAll().Where(x => x.AssetsCategoryID == id && x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from assetprod in assetProduct
                               select new { value = assetprod.ID, text = assetprod.Name };
            return dropdownList;
        }
        public IEnumerable<object> GetAssetsChildCategoryForDropDown(long? id)
        {
            var Categories = _assetsCategoriesRepository.GetAll().Where(x => x.ParentID != null && x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from categories in Categories
                               where categories.ID != id
                               //where (categories.ParentCategoryID < 1 || categories.ParentCategoryID == null) && categories.ID != id
                               select new { value = categories.ID, text = categories.Name, parentID = categories.ParentID };
            return dropdownList;
        }

        public List<AssetsCategory> GetAssetsChildCategory(long? id)
        {
            var Categories = _assetsCategoriesRepository.GetAll().Where(x => x.ParentID != null && x.IsActive == true && x.IsDeleted == false).ToList();
            return Categories;
        }
        public bool CreateAssetsProduct(AssetsProduct assetsProduct, ref string message)
        {
            try
            {
                
                    assetsProduct.IsActive = true;
                    assetsProduct.IsDeleted = false;
                    assetsProduct.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _assetsProductsRepository.Add(assetsProduct);
                    if (SaveAssetsProduct())
                    {
                        message = "Product added successfully...";
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
        public bool UpdateAssetsProduct(ref AssetsProduct assetsProduct, ref string message)
        {
            try
            {
                    AssetsProduct currentProduct = _assetsProductsRepository.GetById(assetsProduct.ID);
                    currentProduct.IsActive = assetsProduct.IsActive;
                
                    _assetsProductsRepository.Update(currentProduct);
                    if (SaveAssetsProduct())
                    {
                        assetsProduct = currentProduct;
                        message = "Product updated successfully ...";
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
        public bool DeleteAssetsProduct(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    AssetsProduct assetsProduct= _assetsProductsRepository.GetById(id);
                    //When category delete, its product will be deleted
                    //*
                    assetsProduct.IsDeleted = true;
                    _assetsProductsRepository.Update(assetsProduct);

                    if (SaveAssetsProduct())
                    {
                        message = "Product deleted successfully ...";
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
                    AssetsProduct product = _assetsProductsRepository.GetById(id);

                    _assetsProductsRepository.Delete(product);

                    if (SaveAssetsProduct())
                    {
                        message = "Product deleted successfully ...";
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
        public bool SaveAssetsProduct()
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
    public interface IAssetsProductsService
    {
        AssetsProduct GetAssetsProduct(long id);
        IEnumerable<AssetsProduct> GetAssetsProducts(long VendorID = 0);
        IEnumerable<AssetsProduct> GetAssetsProductsByDate(DateTime startDate, DateTime endDate, long VendorID = 0);
        IEnumerable<object> GetAssetsProductForDropDownByAssetCategoryID(long? id);
        IEnumerable<object> GetAssetsCategoryForDropDown(long? id);
        IEnumerable<object> GetAssetsChildCategoryForDropDown(long? id);
        List<AssetsCategory> GetAssetsChildCategory(long? id);
        bool CreateAssetsProduct(AssetsProduct assetsProduct, ref string message);
        bool UpdateAssetsProduct(ref AssetsProduct assetsProduct, ref string message);
        bool DeleteAssetsProduct(long id, ref string message, ref bool hasProductLink, bool softdelete);
        bool SaveAssetsProduct();
    }
}
