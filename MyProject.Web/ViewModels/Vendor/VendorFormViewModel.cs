using System;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Vendor
{
	public class VendorFormViewModel
	{
		[Required]
		public long ID { get; set; }
		[Required]
		public string VendorCode { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string NameAr { get; set; }
		[Required]
		public string Slug { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
		public string Email { get; set; }
		[Required]
		public string Logo { get; set; }
        //[Required]
        //public string CoverImage { get; set; }
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
		public Nullable<decimal> Longitude { get; set; }

		public Nullable<decimal> Latitude { get; set; }

		public decimal Commission { get; set; }
		[Required]
		public string License { get; set; }
		
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
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
		public string BankName { get; set; }
		public long BankID { get; set; }
		public string AccountHolderName { get; set; }
		public string BankAccountNumber { get; set; }
		public string IBAN { get; set; }
        public String OpeningTime { get; set; }
        public String ClosingTime { get; set; }
		public string TermsAndConditionWebEn { get; set; }
		public string TermsAndConditionWebAr { get; set; }
		public double ServingKilometer { get; set; }


		public bool EnableOrganizerModule { get; set; }
		public bool EnableAcademyModule { get; set; }
		public bool EnableEcommerceModule { get; set; }
		public bool EnableFacilityModule { get; set; }
	}
}