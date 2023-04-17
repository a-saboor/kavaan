using MyProject.Data.Infrastructure;

namespace MyProject.Data.Repositories
{
    public class EmailSettingRepository : RepositoryBase<EmailSetting>, IEmailSettingRepository
    {
        public EmailSettingRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IEmailSettingRepository : IRepository<EmailSetting>
    {

    }
}
