using MyProject.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{

	class ServiceRatingRepository : RepositoryBase<ServiceRating>, IServiceRatingRepository
	{
		public ServiceRatingRepository(IDbFactory dbFactory)
			: base(dbFactory) { }

		public IEnumerable<ServiceRating> GetServiceRatings(long serviceid, bool? IsApproved)
		{
			var ServiceRatings = this.DbContext.ServiceRatings.Where(c => c.ServiceID == serviceid && (!IsApproved.HasValue || c.IsApproved == IsApproved)).ToList();
			return ServiceRatings;
		}

		public IEnumerable<ServiceRating> GetServiceRatings(long serviceid)
		{
			var ServiceRatings = this.DbContext.ServiceRatings.Where(c => c.ServiceID == serviceid).ToList();
			return ServiceRatings;
		}

		public ServiceRating GetServiceRatingByServiceBooking(long serviceBookingID)
		{
			var ServiceRating = this.DbContext.ServiceRatings.FirstOrDefault(c => c.ServiceBookingID == serviceBookingID);
			return ServiceRating;
		}

		public List<SP_GetserviceRatings_Result> GetServiceRatingsbyServiceID(long serviceId)
		{
			var ServiceRatings = this.DbContext.SP_GetserviceRatings(serviceId).ToList();
			return ServiceRatings;
		}
	}

	public interface IServiceRatingRepository : IRepository<ServiceRating>
	{
		IEnumerable<ServiceRating> GetServiceRatings(long tournamentID, bool? IsApproved);
		IEnumerable<ServiceRating> GetServiceRatings(long tournamentID);
		ServiceRating GetServiceRatingByServiceBooking(long serviceBookingID);
		List<SP_GetserviceRatings_Result> GetServiceRatingsbyServiceID(long serviceId);
	}
}
