using System;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Vendor
{
	public class VendorEditViewModel
	{
		[Required]
		public long ID { get; set; }
		[Required]
		public string VendorCode { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string NameAr { get; set; }
		
		public string Slug { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
		public string Email { get; set; }
		public string Logo { get; set; }
		public string CoverImage { get; set; }
		[Required]
		public string Contact { get; set; }
		[Required]
		public string Mobile { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string IDNo { get; set; }
		
		public string TRN { get; set; }
	
		public string Website { get; set; }
		[Required]
		public decimal Commission { get; set; }
		[Required]
		public string License { get; set; }
		[Required]
		public long BankID { get; set; }
		public string BankName { get; set; }
		[Required]
		public string BankAccountNumber { get; set; }
		public Nullable<decimal> Longitude { get; set; }
		
		public Nullable<decimal> Latitude { get; set; }
		
		public string FAX { get; set; }
		
		public string About { get; set; }
		[Required]
		public string AboutAr { get; set; }
		[Required]
		public Nullable<long> CountryID { get; set; }
		[Required]
		public Nullable<long> CityID { get; set; }
		public Nullable<long> VendorPackageID { get; set; }
		public Nullable<long> VendorSectionID { get; set; }
		public Nullable<long> VendorTypeID { get; set; }
		public Nullable<long> VendorIndustryID { get; set; }
		public bool? IsApproved { get; set; }
		public string AccountHolderName { get; set; }
		public string IBAN { get; set; }
	}
}