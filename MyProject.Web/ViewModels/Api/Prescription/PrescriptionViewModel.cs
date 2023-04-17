using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Prescription
{
    public class PrescriptionViewModel
    {
        [Required]
        public string Description { get; set; }
    }
}