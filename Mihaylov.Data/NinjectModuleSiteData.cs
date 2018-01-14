using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Data.Repositories.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;
using Ninject.Modules;

namespace Mihaylov.Data
{
    public class NinjectModuleSiteData : NinjectModule
    {
        private readonly string connectionString;

        public NinjectModuleSiteData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            // Kernel.Load(new INinjectModule[] { new Mihaylov.Database.NinjectModuleDatabase(this.connectionString) });
            Kernel.Rebind<ISiteDbContext>().ToConstructor(x => new DAL.SiteDbContext(this.connectionString));

            Kernel.Bind<IGetAllRepository<AnswerType>>().To<AnswerTypeRepository>();
            Kernel.Bind<IGetAllRepository<Ethnicity>>().To<EthnicitiesRepository>();
            Kernel.Bind<IGetAllRepository<Orientation>>().To<OrientationsRepository>();
            Kernel.Bind<IGetAllRepository<Unit>>().To<UnitsRepository>();
            Kernel.Bind<ICountriesRepository>().To<CountriesRepository>();
            Kernel.Bind<IPersonsRepository>().To<PersonsRepository>();
        }
    }
}
