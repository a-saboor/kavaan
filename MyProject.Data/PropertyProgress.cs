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
    
    public partial class PropertyProgress
    {
        public long ID { get; set; }
        public Nullable<long> PropertyID { get; set; }
        public Nullable<float> ProgressesPercent { get; set; }
        public bool IsActive { get; set; }
        public int ProgressId { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual Progress Progress { get; set; }
        public virtual Property Property { get; set; }
    }
}