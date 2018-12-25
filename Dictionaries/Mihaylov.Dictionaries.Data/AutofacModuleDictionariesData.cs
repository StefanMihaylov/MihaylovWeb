using Autofac;
using Mihaylov.Data.Repositories.Dictionaries;
using Mihaylov.Data.UnitOfWorks;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Dictionaries.Database;

namespace Mihaylov.Dictionaries.Data
{
    public class AutofacModuleDictionariesData : Module
    {
        private readonly string connectionString;

        public AutofacModuleDictionariesData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutoFacModuleDictionariesDatabase(this.connectionString));

            builder.RegisterType<CoursesRepository>().As<IGetAllRepository<Course>>();
            builder.RegisterType<LanguagesRepository>().As<IGetAllRepository<Language>>();
            builder.RegisterType<LearningSystemsRepository>().As<IGetAllRepository<LearningSystem>>();
            builder.RegisterType<LevelsRepository>().As<IGetAllRepository<Level>>();

            builder.RegisterType<RecordsData>().As<IRecordsData>();
        }
    }
}
