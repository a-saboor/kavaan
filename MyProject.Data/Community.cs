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
    
    public partial class Community
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Community()
        {
            this.CommunityFeatures = new HashSet<CommunityFeature>();
            this.CommunityFeatures1 = new HashSet<CommunityFeature>();
            this.CommunityFeatures2 = new HashSet<CommunityFeature>();
            this.CommunityFeatures3 = new HashSet<CommunityFeature>();
            this.CommunityFeatures4 = new HashSet<CommunityFeature>();
            this.CommunityFeatures5 = new HashSet<CommunityFeature>();
            this.CommunityFeatures6 = new HashSet<CommunityFeature>();
            this.CommunityFeatures7 = new HashSet<CommunityFeature>();
            this.CommunityFeatures8 = new HashSet<CommunityFeature>();
            this.CommunityFeatures9 = new HashSet<CommunityFeature>();
            this.CommunityFeatures10 = new HashSet<CommunityFeature>();
            this.CommunityFeatures11 = new HashSet<CommunityFeature>();
            this.CommunityFeatures12 = new HashSet<CommunityFeature>();
            this.CommunityFeatures13 = new HashSet<CommunityFeature>();
            this.CommunityFeatures14 = new HashSet<CommunityFeature>();
            this.CommunityFeatures15 = new HashSet<CommunityFeature>();
            this.CommunityFeatures16 = new HashSet<CommunityFeature>();
            this.CommunityFeatures17 = new HashSet<CommunityFeature>();
            this.CommunityFeatures18 = new HashSet<CommunityFeature>();
            this.CommunityImages = new HashSet<CommunityImage>();
            this.CommunityImages1 = new HashSet<CommunityImage>();
            this.CommunityImages2 = new HashSet<CommunityImage>();
            this.CommunityImages3 = new HashSet<CommunityImage>();
            this.CommunityImages4 = new HashSet<CommunityImage>();
            this.CommunityImages5 = new HashSet<CommunityImage>();
            this.CommunityImages6 = new HashSet<CommunityImage>();
            this.CommunityImages7 = new HashSet<CommunityImage>();
            this.CommunityImages8 = new HashSet<CommunityImage>();
            this.CommunityImages9 = new HashSet<CommunityImage>();
            this.CommunityImages10 = new HashSet<CommunityImage>();
            this.CommunityImages11 = new HashSet<CommunityImage>();
            this.CommunityImages12 = new HashSet<CommunityImage>();
            this.CommunityImages13 = new HashSet<CommunityImage>();
            this.CommunityImages14 = new HashSet<CommunityImage>();
            this.CommunityImages15 = new HashSet<CommunityImage>();
            this.CommunityImages16 = new HashSet<CommunityImage>();
            this.CommunityImages17 = new HashSet<CommunityImage>();
            this.CommunityImages18 = new HashSet<CommunityImage>();
            this.CommunityImages19 = new HashSet<CommunityImage>();
            this.CommunityFeatures19 = new HashSet<CommunityFeature>();
        }
    
        public long ID { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<long> CountryID { get; set; }
        public Nullable<long> CItyID { get; set; }
        public Nullable<long> AreaID { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Area Area { get; set; }
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures8 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures9 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures10 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures11 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures12 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures13 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures14 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures15 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures16 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures17 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures18 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages8 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages9 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages10 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages11 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages12 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages13 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages14 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages15 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages16 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages17 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages18 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityImage> CommunityImages19 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommunityFeature> CommunityFeatures19 { get; set; }
        public virtual Country Country { get; set; }
    }
}
