using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Category GetCategoryByName(string name, long id = 0)
        {
            var user = this.DbContext.Categories.Where(c => c.CategoryName == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
            return user;
        }

        public Category GetCategoryBySlug(string slug)
        {
            var user = this.DbContext.Categories.Where(c => c.Slug == slug && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
            return user;
        }

        public IEnumerable<SP_GetCategories_Result> GetCategories(string imageServer, string lang)
        {
            var Categories = this.DbContext.SP_GetCategories(lang, imageServer).ToList();
            return Categories;
        }

        public IEnumerable<SP_GetFilteredCategories_Result> GetFilteredCategories(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var categories = this.DbContext.SP_GetFilteredCategories(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
            return categories;
        }

        public List<Category> GetCategoryByDate(DateTime FromDate, DateTime ToDate, int id = 0)
        {
            var categories = this.DbContext.Categories.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsDeleted == false).ToList();
            return categories;
        }

        public bool UpdateDeletedCategoryChilds(long categoryId)
        {
            try
            {
                DbContext.PR_UpdateDeletedCategoryChilds(categoryId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetCategoryByName(string name, long id = 0);
        Category GetCategoryBySlug(string slug);
        IEnumerable<SP_GetCategories_Result> GetCategories(string imageServer, string lang);
        IEnumerable<SP_GetFilteredCategories_Result> GetFilteredCategories(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);
        List<Category> GetCategoryByDate(DateTime FromDate, DateTime ToDate, int id = 0);
        bool UpdateDeletedCategoryChilds(long categoryId);
    }
}
