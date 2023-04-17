using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Contact { get; set; }

        [Required]
        public string Email { get; set; }

        public long CustomerID { get; set; }
    }
}