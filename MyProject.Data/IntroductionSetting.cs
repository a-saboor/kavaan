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
    
    public partial class IntroductionSetting
    {
        public long ID { get; set; }
        public Nullable<long> CountryID { get; set; }
        public Nullable<long> CityID { get; set; }
        public string City { get; set; }
        public string Heading { get; set; }
        public string HeadingAr { get; set; }
        public string Paragraph { get; set; }
        public string ParagraphAr { get; set; }
        public Nullable<int> Position { get; set; }
        public string Type { get; set; }
    
        public virtual City City1 { get; set; }
        public virtual Country Country { get; set; }
    }
}
