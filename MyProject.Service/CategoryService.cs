using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
		{
			this._categoryRepository = categoryRepository;
			this._unitOfWork = unitOfWork;
		}


		#region ICategoryService Members

		public IEnumerable<Category> GetCategories()
		{
			var categories = _categoryRepository.GetAll().Where(i => i.IsDeleted == false);
			return categories;
		}
		public IEnumerable<Category> GetCategoriesByDate(DateTime FromDate, DateTime ToDate, int id = 0)
		{
			var categories = _categoryRepository.GetCategoryByDate(FromDate, ToDate);
			return categories;
		}
		public IEnumerable<Category> GetCategoriesForApp()
		{
			var categories = _categoryRepository.GetAll().Where(i => i.IsDeleted == false && i.IsActive == true);
			return categories;
		}

		public int GetCategoriesForDashboard()
		{
			var categories = _categoryRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
			return categories.Count();
		}

		public Category GetCategory(long id)
		{
			var category = _categoryRepository.GetById(id);
			return category;
		}

		public Category GetCategoryBySlug(string slug)
		{
			var category = _categoryRepository.GetCategoryBySlug(slug);
			return category;
		}

		public bool CreateCategory(Category category, ref string message)
		{
			try
			{
				string messsage = null;
				category.IsActive = true;
				category.IsDeleted = false;
				category.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_categoryRepository.Add(category);
				if (SaveCategory())
				{
					message = "Department added successfully ...";
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

		public bool UpdateCategory(ref Category category, ref string message, bool updater = true)
		{
			try
			{
				if (_categoryRepository.GetCategoryByName(category.CategoryName, category.ID) == null)
				{
					if (updater)
					{
						Category CurrentCategory = _categoryRepository.GetById(category.ID);

						CurrentCategory.CategoryName = category.CategoryName;
						CurrentCategory.CategoryNameAr = category.CategoryNameAr;
						CurrentCategory.Image = category.Image;
						CurrentCategory.Slug = category.Slug;
						CurrentCategory.Description = category.Description;
						CurrentCategory.DescriptionAR = category.DescriptionAR;
						CurrentCategory.IsDefault = category.IsDefault;
						CurrentCategory.IsParentCategoryDeleted = category.IsParentCategoryDeleted;

						category = null;

						_categoryRepository.Update(CurrentCategory);
						if (SaveCategory())
						{
							category = CurrentCategory;
							message = "Department updated successfully ...";
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
						_categoryRepository.Update(category);
						if (SaveCategory())
						{
							message = "Department updated successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later...";
							return false;
						}
					}

				}
				else
				{
					message = "Department already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteCategory(long id, ref string message, ref bool hasChilds, bool softDelete = true)
		{
			try
			{
				Category category = _categoryRepository.GetById(id);
				if (category.Category1.Count() > 0)
					hasChilds = true;
				else
					hasChilds = false;
				if (softDelete)
				{
					category.IsDeleted = true;

					_categoryRepository.Update(category);
				}
				else
				{
					_categoryRepository.Delete(category);
				}
				if (SaveCategory())
				{
					_categoryRepository.UpdateDeletedCategoryChilds(id);
					message = "Department deleted successfully ...";
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


		public IEnumerable<object> GetCategoriesForDropDown(long? id)
		{
			var Categories = _categoryRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from categories in Categories
							   where categories.ID != id
							   //where (categories.ParentCategoryID < 1 || categories.ParentCategoryID == null) && categories.ID != id
							   select new { value = categories.ID, text = categories.CategoryName };
			return dropdownList;
		}

		public IEnumerable<object> GetCategoriesForDropDown(string lang = "en")
		{
			var categories = GetCategories().Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from Category in categories
							   select new { value = Category.ID, text = lang == "en" ? Category.CategoryName : Category.CategoryNameAr };
			return dropdownList;
		}

		public IEnumerable<SP_GetCategories_Result> GetCategories(string imageServer, string lang)
		{
			var Categories = _categoryRepository.GetCategories(imageServer, lang);
			return Categories;
		}

		public IEnumerable<SP_GetFilteredCategories_Result> GetFilteredCategories(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var Categories = _categoryRepository.GetFilteredCategories(search, pageSize, pageNumber, sortBy, lang, imageServer);
			return Categories;
		}

		public bool UpdateDeletedCategoryChilds(long categoryId)
		{
			try
			{
				_categoryRepository.UpdateDeletedCategoryChilds(categoryId);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public bool SaveCategory()
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

	public interface ICategoryService
	{
		IEnumerable<Category> GetCategories();
		IEnumerable<Category> GetCategoriesForApp();
		Category GetCategory(long id);
		IEnumerable<Category> GetCategoriesByDate(DateTime FromDate, DateTime ToDate, int id = 0);
		int GetCategoriesForDashboard();
		Category GetCategoryBySlug(string slug);
		bool CreateCategory(Category category, ref string message);
		bool UpdateCategory(ref Category category, ref string message, bool updater = true);
		bool DeleteCategory(long id, ref string message, ref bool hasChilds, bool softDelete = true);
		bool SaveCategory();

		IEnumerable<object> GetCategoriesForDropDown(long? id);
		IEnumerable<object> GetCategoriesForDropDown(string lang = "en");
		IEnumerable<SP_GetCategories_Result> GetCategories(string imageServer, string lang);
		IEnumerable<SP_GetFilteredCategories_Result> GetFilteredCategories(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
		bool UpdateDeletedCategoryChilds(long categoryId);
	}
}
