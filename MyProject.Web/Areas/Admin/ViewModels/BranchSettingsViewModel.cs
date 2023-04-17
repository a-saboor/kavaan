using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.Areas.Admin.ViewModels
{
    public class BranchSettingsViewModel
    {
        public long ID { get; set; }
        public Nullable<long> BusinessSettingID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string NameAr { get; set; }
        public string Contact { get; set; }
        public string Contact2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string MapIframe { get; set; }
        public string StreetAddress { get; set; }
        public string StreetAddressAr { get; set; }
        public string Days { get; set; }
    }
}