using System;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Account
{
	public class CustomerViewModel
	{
        public long ID { get; set; }
        //public Nullable<long> CountryID { get; set; }
        //public Nullable<long> CityID { get; set; }
        //public Nullable<long> AreaID { get; set; }
        public string Country { get; set; }
        public string CustomerCity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Logo { get; set; }
        //public Nullable<int> OTP { get; set; }
        public string Contact { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> Points { get; set; }
        public string AccountType { get; set; }
        public Nullable<int> Salt { get; set; }
        public string Password { get; set; }
        public string AuthorizationCode { get; set; }
        public Nullable<System.DateTime> AuthorizationExpiry { get; set; }
        public Nullable<long> ReferredBy { get; set; }
        public Nullable<bool> IsBookingNoticationAllowed { get; set; }
        public Nullable<bool> IsPushNotificationAllowed { get; set; }
        public Nullable<bool> IsGuest { get; set; }
        public Nullable<bool> IsContactVerified { get; set; }
        public Nullable<bool> IsEmailVerified { get; set; }
        public Nullable<bool> IsOTPAccess { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string PoBox { get; set; }
        public string ZipCode { get; set; }
        public string CNICNo { get; set; }
        public Nullable<System.DateTime> CNICExpiry { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> PassportExpiry { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}