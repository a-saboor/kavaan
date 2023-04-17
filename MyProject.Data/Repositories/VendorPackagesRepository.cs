using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
    public class VendorPackagesRepository : RepositoryBase<VendorPackage>, IVendorPackagesRepository 
    {   
        public VendorPackagesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
    public interface IVendorPackagesRepository : IRepository<VendorPackage>
    {


    }
}
