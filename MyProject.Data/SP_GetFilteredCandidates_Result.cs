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
    
    public partial class SP_GetFilteredCandidates_Result
    {
        public Nullable<int> TotalResultsCount { get; set; }
        public Nullable<int> FilteredResultsCount { get; set; }
        public Nullable<long> RowNum { get; set; }
        public string CreatedOn { get; set; }
        public long ID { get; set; }
        public string FullName { get; set; }
        public string UCC { get; set; }
        public string Email { get; set; }
        public string CV { get; set; }
        public string Photo { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsFlaged { get; set; }
        public Nullable<bool> MarkAsRead { get; set; }
        public string Schedule { get; set; }
        public Nullable<bool> IsEmailSent { get; set; }
        public long JobOpeningID { get; set; }
        public string JobOpeningTitle { get; set; }
        public long DepartmentID { get; set; }
        public string DepartmentSlug { get; set; }
        public string DepartmentName { get; set; }
    }
}
