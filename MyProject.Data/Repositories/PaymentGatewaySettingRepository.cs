using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
	public interface IPaymentGatewaySettingRepository : IRepository<PaymentGatewaySetting>
	{

	}
	public class PaymentGatewaySettingRepository : RepositoryBase<PaymentGatewaySetting>, IPaymentGatewaySettingRepository
	{
		public PaymentGatewaySettingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
	}
}
