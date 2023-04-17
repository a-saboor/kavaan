using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class AssetsCategoryService : IAssetsCategoriesService
    {
        private readonly IAssetsCategoriesRepository _assetsCategoriesRepository;
        private readonly IAssetsProductsRepository _assetsProductsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AssetsCategoryService(IUnitOfWork unitOfWork, IAssetsCategoriesRepository assetsCategoriesRepository, IAssetsProductsRepository assetsProductsRepository)
        {
            this._unitOfWork = unitOfWork;
            this._assetsCategoriesRepository = assetsCategoriesRepository;
            this._assetsProductsRepository = assetsProductsRepository;
        }
        public IEnumerable<AssetsCategory> GetAssetsCategories(long VendorID = 0)
        {
            var AssetsCategoriess = _assetsCategoriesRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return AssetsCategoriess;
        }
        public IEnumerable<AssetsCategory> GetAssetsCategoriesByDate(DateTime startDate, DateTime endDate, long VendorID = 0)
        {
            var AssetsCategoriess = _assetsCategoriesRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID && i.CreatedOn >= startDate && i.CreatedOn <= endDate);
            return AssetsCategoriess;
        }
        public AssetsCategory GetAssetsCategory(long id)
        {
            var AssetsCategoriess = this._assetsCategoriesRepository.GetById(id);
            return AssetsCategoriess;
        }
        public IEnumerable<object> GetCategoriesForDropDown(long? id, long VendorID = 0)
        {
            var Categories = _assetsCategoriesRepository.GetAll().Where(x => x.ParentID == null && x.IsActive == true && x.IsDeleted == false && x.VendorID == VendorID);
            var dropdownList = from categories in Categories
                               where categories.ID != id
                               //where (categories.ParentCategoryID < 1 || categories.ParentCategoryID == null) && categories.ID != id
                               select new { value = categories.ID, text = categories.Name };
            return dropdownList;
        }
        public bool CreateAssetsCategory(AssetsCategory assetsCategory, ref string message)
        {
            try
            {
                AssetsCategory oldAssetsCategory = null;
                if (assetsCategory.ParentID != null && assetsCategory.ParentID > 0)
                {
                    oldAssetsCategory = _assetsCategoriesRepository.GetAssetsCategoryByNameAndParentId(assetsCategory.Name, assetsCategory.ParentID);
                }
                else
                {
                    oldAssetsCategory = _assetsCategoriesRepository.GetAssetsCategoryByName(assetsCategory.Name);
                }

                if (oldAssetsCategory == null)
                {
                    assetsCategory.IsActive = true;
                    assetsCategory.IsDeleted = false;
                    assetsCategory.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _assetsCategoriesRepository.Add(assetsCategory);
                    if (SaveAssetsCategory())
                    {
                        message = "Category added successfully...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "Category already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        public bool UpdateAssetsCategory(ref AssetsCategory assetsCategory, ref string message)
        {
            try
            {
                AssetsCategory oldAssetsCategory = null;

                if (assetsCategory.ParentID != null && assetsCategory.ParentID > 0)
                {
                    oldAssetsCategory = _assetsCategoriesRepository.GetAssetsCategoryByNameAndParentId(assetsCategory.Name, assetsCategory.ParentID, assetsCategory.ID);
                }
                else
                {
                    oldAssetsCategory = _assetsCategoriesRepository.GetAssetsCategoryByName(assetsCategory.Name, assetsCategory.ID);

                    if (oldAssetsCategory != null && oldAssetsCategory.ParentID != null)
                    {
                        oldAssetsCategory = null;
                    }

                }
                if (oldAssetsCategory == null)
                {
                    AssetsCategory CurrentCategory = _assetsCategoriesRepository.GetById(assetsCategory.ID);
                    CurrentCategory.IsActive = assetsCategory.IsActive;
                    if (CurrentCategory.AssetsCategories1.Count() > 0 && CurrentCategory.IsActive == false)
                    {
                        foreach (var item in CurrentCategory.AssetsCategories1)
                        {
                            item.IsActive = false;
                            _assetsCategoriesRepository.Update(item);
                        }
                        foreach (var item in CurrentCategory.AssetsProducts1)//parent category linked
                        {
                            item.IsActive = false;
                            _assetsProductsRepository.Update(item);
                        }
                    }
                    foreach (var item in CurrentCategory.AssetsProducts)//child category linked
                    {
                        item.IsActive = false;
                        _assetsProductsRepository.Update(item);
                    }
                    _assetsCategoriesRepository.Update(CurrentCategory);

                    if (SaveAssetsCategory())
                    {
                        assetsCategory = CurrentCategory;
                        message = "Category updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "Category already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }
        public bool DeleteAssetsCategory(long id, ref string message, bool softdelete)
        {
            bool hasChilds, hasProductLink = false;
            if (softdelete)
            {
                try
                {
                    AssetsCategory category = _assetsCategoriesRepository.GetById(id);

                    if (category.AssetsCategories1.Count() > 0)
                        hasChilds = true;
                    else
                        hasChilds = false;

                    if (category.AssetsProducts.Count() > 0 || category.AssetsProducts1.Count() > 0)
                        hasProductLink = true;
                    else
                        hasProductLink = false;

                    //When category delete, its product will be deleted
                    if (hasProductLink)
                    {
                        foreach (var item in category.AssetsProducts)//child category linked
                        {
                            item.IsDeleted = true;
                            _assetsProductsRepository.Update(item);
                        }
                        foreach (var item in category.AssetsProducts1)//parent category linked
                        {
                            item.IsDeleted = true;
                            _assetsProductsRepository.Update(item);
                        }
                    }
                    //When category delete, its child categories will be deleted
                    if (hasChilds)
                    {
                        foreach (var item in category.AssetsCategories1)
                        {
                            item.IsDeleted = true;
                            _assetsCategoriesRepository.Update(item);
                        }
                    }

                    category.IsDeleted = true;
                    _assetsCategoriesRepository.Update(category);

                    if (SaveAssetsCategory())
                    {
                        message = "Category deleted successfully ...";
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
                    AssetsCategory category = _assetsCategoriesRepository.GetById(id);

                    _assetsCategoriesRepository.Delete(category);

                    if (SaveAssetsCategory())
                    {
                        message = "Category deleted successfully ...";
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
        public bool SaveAssetsCategory()
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
    public interface IAssetsCategoriesService
    {
        AssetsCategory GetAssetsCategory(long id);
        IEnumerable<AssetsCategory> GetAssetsCategories(long VendorID = 0);
        IEnumerable<AssetsCategory> GetAssetsCategoriesByDate(DateTime startDate, DateTime endDate, long VendorID = 0);
        IEnumerable<object> GetCategoriesForDropDown(long? id, long VendorID = 0);
        bool CreateAssetsCategory(AssetsCategory assetsCategory, ref string message);
        bool UpdateAssetsCategory(ref AssetsCategory assetsCategory, ref string message);
        bool DeleteAssetsCategory(long id, ref string message, bool softdelete);
        bool SaveAssetsCategory();
    }
}
