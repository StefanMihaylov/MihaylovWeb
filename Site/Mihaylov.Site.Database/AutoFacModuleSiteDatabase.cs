using Autofac;
using Mihaylov.Site.Database.Interfaces;

namespace Mihaylov.Site.Database
{
    public class AutoFacModuleSiteDatabase : Module
    {
        private readonly string connectionString;

        public AutoFacModuleSiteDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SiteDbContext(this.connectionString)).As<ISiteDbContext>();
        }
    }
}
