using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
	{
		public VendorRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<Vendor> GetAll(bool isApproved)
		{
			var vendors = this.DbContext.Vendors.Where(c => c.IsApproved == isApproved && c.IsDeleted == false).ToList();
			return vendors;
		}

		public IEnumerable<Vendor> GetAllForApproval()
		{
			var vendors = this.DbContext.Vendors.Where(c => c.IsApproved == null && c.IsDeleted == false).ToList();
			return vendors;
		}

		public Vendor GetVendorByName(string name)
		{
			var vendor = this.DbContext.Vendors.Where(c => c.Name == name && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
			return vendor;
		}

		public Vendor GetVendorByEmail(string email)
		{
			var vendor = this.DbContext.Vendors.Where(c => c.Email == email && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
			return vendor;
		}

		public Vendor GetVendorByContact(string contact)
		{
			var vendor = this.DbContext.Vendors.Where(c => c.Contact == contact && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
			return vendor;
		}

		public Vendor GetVendorBySlug(string slug)
		{
			var vendor = this.DbContext.Vendors.Where(c => c.Slug == slug && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
			return vendor;
		}

		public Vendor GetVendorByName(string name, string email, long id = 0)
		{
			var vendor = this.DbContext.Vendors.Where(c => (c.Name == name || c.Email == email) && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return vendor;
		}

		//public IEnumerable<SP_GetVendorFilters_Result> GetVendorFilters(long vendorId, string lang)
		//{
		//	var filters = this.DbContext.SP_GetVendorFilters(vendorId, lang).ToList();
		//	return filters;
		//}

	

		//public IEnumerable<SP_GetVendorBrands_Result> GetVendorBrands(long vendorId, string lang)
		//{
		//	var Brands = this.DbContext.SP_GetVendorBrands(vendorId, lang).ToList();
		//	return Brands;
		//}

		public IEnumerable<SP_GetVendorCategories_Result> GetVendorCategories(long vendorId, string lang)
		{
			var Categories = this.DbContext.SP_GetVendorCategories(vendorId, lang).ToList();
			return Categories;
		}
	}

	public interface IVendorRepository : IRepository<Vendor>
	{
		IEnumerable<Vendor> GetAll(bool isApproved);
		IEnumerable<Vendor> GetAllForApproval();
		Vendor GetVendorByName(string name);
		Vendor GetVendorByContact(string contact);
		Vendor GetVendorBySlug(string slug);
		Vendor GetVendorByEmail(string email);
		Vendor GetVendorByName(string name, string email, long id = 0);
		//IEnumerable<SP_GetVendorFilters_Result> GetVendorFilters(long vendorId, string lang);
		//IEnumerable<SP_GetVendorBrands_Result> GetVendorBrands(long vendorId, string lang);
		IEnumerable<SP_GetVendorCategories_Result> GetVendorCategories(long vendorId, string lang);
	
	}
}
