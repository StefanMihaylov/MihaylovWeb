using System.Collections.Generic;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Data.Repositories;
using Ninject.Modules;

namespace Mihaylov.Data
{
    public class NinjectModuleData : NinjectModule
    {
        private readonly string connectionString;

        public NinjectModuleData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new Mihaylov.Database.NinjectModuleDatabase(this.connectionString) });

            Kernel.Bind<IGetAllRepository<AnswerType>>().To<AnswerTypeRepository>();
            Kernel.Bind<IGetAllRepository<Ethnicity>>().To<EthnicitiesRepository>();
            Kernel.Bind<IGetAllRepository<Orientation>>().To<OrientationsRepository>();
            Kernel.Bind<IGetAllRepository<Unit>>().To<UnitsRepository>();
            Kernel.Bind<ICountriesRepository>().To<CountriesRepository>();
            Kernel.Bind<IPersonsRepository>().To<PersonsRepository>();
        }
    }
}
