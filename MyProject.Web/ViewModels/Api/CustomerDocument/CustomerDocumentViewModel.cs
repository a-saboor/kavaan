using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Web.ViewModels.Api.CustomerDocument
{
    public class CustomerDocumentViewModel
    {
        //public long ID { get; set; }

        public long? ID { get; set; }
        public long CustomerDocumentTypeID { get; set; }
        public long CustomerRelationID { get; set; }
        public string ExpiryDate { get; set; }
    }
}