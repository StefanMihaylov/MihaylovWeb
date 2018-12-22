using Mihaylov.Site.Data.Models;
using Mihaylov.Site.Data.Repositories;
using Mihaylov.Site.Data.Interfaces;
using DAL = Mihaylov.Site.Database;
using Autofac;

namespace Mihaylov.Site.Data
{
    public class AutoFacModuleSiteData : Module
    {
        private readonly string connectionString;

        public AutoFacModuleSiteData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DAL.AutoFacModuleSiteDatabase(this.connectionString));

            builder.RegisterType<AnswerTypeRepository>().As<IGetAllRepository<AnswerType>>();
            builder.RegisterType<EthnicitiesRepository>().As<IGetAllRepository<Ethnicity>>();
            builder.RegisterType<OrientationsRepository>().As<IGetAllRepository<Orientation>>();
            builder.RegisterType<UnitsRepository>().As<IGetAllRepository<Unit>>();
            builder.RegisterType<CountriesRepository>().As<ICountriesRepository>();
            builder.RegisterType<PersonsRepository>().As<IPersonsRepository>();
            builder.RegisterType<PhrasesRepository>().As<IPhrasesRepository>();
        }
    }
}
