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
    
    public partial class Blog
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Slug { get; set; }
        public string TitleDescription { get; set; }
        public string TitleDescriptionAr { get; set; }
        public string MobileDescription { get; set; }
        public string MobileDescriptionAr { get; set; }
        public string BannerImage { get; set; }
        public string Video { get; set; }
        public string Author { get; set; }
        public Nullable<System.DateTime> PublishedOn { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookURL { get; set; }
        public string LinkedinURL { get; set; }
        public string InstagramURL { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public Nullable<int> Position { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> PostedDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}