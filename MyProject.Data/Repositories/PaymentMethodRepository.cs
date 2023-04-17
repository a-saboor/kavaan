using MyProject.Data.Infrastructure;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
	{
		public PaymentMethodRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public PaymentMethod GetPaymentMethodByName(string name, long id = 0)
		{
			var user = this.DbContext.PaymentMethods.Where(c => c.Method == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return user;
		}
	}

	public interface IPaymentMethodRepository : IRepository<PaymentMethod>
	{
		PaymentMethod GetPaymentMethodByName(string name, long id = 0);
	}
}
