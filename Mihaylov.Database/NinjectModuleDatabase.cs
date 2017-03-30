using Mihaylov.Database.Models.Interfaces;
using Ninject.Modules;
using Ninject.Web.Common;

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
            Kernel.Bind<IMihaylovDbContext>().ToConstructor(x => new MihaylovDbContext(this.connectionString)).InThreadScope();
        }
    }
}
