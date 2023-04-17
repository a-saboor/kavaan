using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
     public class VendorWithdrawalrequestRepository:  RepositoryBase<VendorWithdrawalRequest>, IVendorWithdrawalrequestRepository
    {
        public VendorWithdrawalrequestRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
    }
    public interface IVendorWithdrawalrequestRepository : IRepository<VendorWithdrawalRequest>
    {

    }
}
