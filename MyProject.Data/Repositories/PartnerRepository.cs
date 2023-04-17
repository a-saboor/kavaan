using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Repositories
{
	class PartnerRepository : RepositoryBase<Partner>, IPartnerRepository
	{
		public PartnerRepository(IDbFactory dbFactory)
		   : base(dbFactory) { }

		public Partner GetPartnersByName(string name, long id = 0)
		{
			var partner = this.DbContext.Partners.Where(c => c.Name == name && c.ID != id && c.IsDeleted == false).FirstOrDefault();
			return partner;
		}

		public IEnumerable<SP_GetFilteredPartners_Result> GetFilteredPartners(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var partners = this.DbContext.SP_GetFilteredPartners(search, pageSize, pageNumber, sortBy, lang, imageServer).ToList();
			return partners;
		}

	}
	public interface IPartnerRepository : IRepository<Partner>
	{
		Partner GetPartnersByName(string name, long id = 0);

		IEnumerable<SP_GetFilteredPartners_Result> GetFilteredPartners(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

	}
}
