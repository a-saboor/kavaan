using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Appointment
{
    public class AppointmentsViewModel
    {
        public string Type { get; set; }
        public string TypeAr { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }
}