using Mihaylov.Database.Dictionaries;
using Mihaylov.Database.Interfaces;
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
           // Kernel.Rebind<ISiteDbContext>().ToConstructor(x => new SiteDbContext(this.connectionString)).InSingletonScope();
            Kernel.Rebind<IDictionariesDbContext>().ToConstructor(x => new DictionariesDbContext(this.connectionString)).InSingletonScope();
        }
    }
}
