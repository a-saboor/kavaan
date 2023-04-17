using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    class VendorTransactionHistoryRepository : RepositoryBase<VendorTransactionHistory>, IVendorTransactionHistoryRepository
    {
        public VendorTransactionHistoryRepository(IDbFactory dbFactory)
        : base(dbFactory) { }
    }
    public interface IVendorTransactionHistoryRepository : IRepository<VendorTransactionHistory>
    {

    }
}
