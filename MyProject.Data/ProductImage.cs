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
    
    public partial class ProductImage
    {
        public long ID { get; set; }
        public Nullable<long> ProductID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<System.DateTime> CraetedOn { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
