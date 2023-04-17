using System.Collections.Generic;
using System.Linq;

namespace MyProject.Web.ViewModels.Api.Vendor
{
    public class VendorServicesViewModel
    {
        public List<VendorServiceList> vendorServiceLists = new List<VendorServiceList>();

        public VendorServicesViewModel(List<Data.Service> services, List<Data.VendorService> vendorServices)
        {
            foreach (var item in services)
            {
                VendorServiceList vendorServiceItem = new VendorServiceList();
                vendorServiceItem.ID = item.ID;
                vendorServiceItem.Name = item.Name;
                vendorServiceItem.Type = item.Type;
                vendorServiceItem.Position = item.Position;
                vendorServiceItem.Thumbnail = item.Thumbnail;
                vendorServiceItem.Category = item.Category;
                vendorServiceItem.CreatedOn = item.CreatedOn;
                vendorServiceItem.IsChecked = vendorServices.FirstOrDefault(x => x.ServiceID == vendorServiceItem.ID) != null ? true : false;

                this.vendorServiceLists.Add(vendorServiceItem);
            }
        }
    }
    public class VendorServiceList : Data.Service
    {
        public bool IsChecked { get; set; } = false;
    }
}