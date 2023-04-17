using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.CustomNotification
{
    public class CustomNotificationViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Module { get; set; }
        public long CarID { get; set; }
        public List<int> Customers { get; set; }
    }
}