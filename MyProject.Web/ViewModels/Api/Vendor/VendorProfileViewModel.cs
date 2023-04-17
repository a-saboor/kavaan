using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Vendor
{
    public class VendorProfileViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string emiratesID { get; set; }
        public string trnNo { get; set; }
        public string licenseNo { get; set; }
        public string website { get; set; }
        public long Bankid { get; set; }
        public string accountName { get; set; }
        public string accountNo { get; set; }
        public string iban { get; set; }
        public long vendorSectionid { get; set; }
        public long vendorIndustryid { get; set; }
        public long vendorTypeid { get; set; }
        public long countryid { get; set; }
        public long cityid { get; set; }
    }
}