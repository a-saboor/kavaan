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
    
    public partial class CandidateEducation
    {
        public long ID { get; set; }
        public Nullable<long> CandidateID { get; set; }
        public string Degree { get; set; }
        public string Institute { get; set; }
        public string PassingYear { get; set; }
    
        public virtual JobCandidate JobCandidate { get; set; }
    }
}