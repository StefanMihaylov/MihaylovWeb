using Mihaylov.Database.Interfaces;
using Mihaylov.Database.Site;
using Ninject.Modules;

namespace Mihaylov.Database
{
    public class NinjectModuleDatabase : NinjectModule
    {
        private readonly string connectionString;

        public NinjectModuleDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Kernel.Bind<ISiteDbContext>().ToConstructor(x => new SiteDbContext(this.connectionString)).InSingletonScope();
        }
    }
}
