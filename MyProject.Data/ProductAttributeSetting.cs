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
    
    public partial class ProductAttributeSetting
    {
        public long ID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public Nullable<long> AttributeID { get; set; }
        public Nullable<bool> ProductPageVisiblity { get; set; }
        public Nullable<bool> VariationUsage { get; set; }
    
        public virtual Attribute Attribute { get; set; }
        public virtual Product Product { get; set; }
    }
}