//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentGatewaySetting
    {
        public int ID { get; set; }
        public Nullable<bool> Debug { get; set; }
        public Nullable<bool> UseSSL { get; set; }
        public Nullable<bool> IgnoreSSLError { get; set; }
        public string GatewayHost { get; set; }
        public string Version { get; set; }
        public string GatewayUrl { get; set; }
        public Nullable<bool> UseProxy { get; set; }
        public string ProxyHost { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyDomain { get; set; }
        public string MerchantID { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Salt { get; set; }
        public string Currency { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}