using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.JobCandidate
{
    public class JobCandidateViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string VISAStatus { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Dob { get; set; }
        public string TotalExperience { get; set; }
        public string UAEExperience { get; set; }
        public string IndustryExperience { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string NoticePeriod { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string MaritalStatus { get; set; }
        public string DrivingLicense { get; set; }
        public Nullable<long> JobID { get; set; }
        public List<CandidateExperinceViewModel> Experiences { get; set; }
        public List<CandidateEducationViewModel> Education { get; set; }
    }

    public class CandidateExperinceRootViewModel
    {
        public List<CandidateExperinceViewModel> Experiences { get; set; }
    }
    public class CandidateExperinceViewModel
    {
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public bool IsCurrentlyWorking { get; set; }
    }

    public class CandidateEducationRootViewModel
    {
        public List<CandidateEducationViewModel> Educations { get; set; }
    }
    
    public class CandidateEducationViewModel
    {
        public string Degree { get; set; }
        public string Institute { get; set; }
        public string YearOfPassing { get; set; }
    }

   
}