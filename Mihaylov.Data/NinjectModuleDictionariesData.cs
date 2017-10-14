using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;
using Mihaylov.Data.Repositories.Dictionaries;
using Ninject.Modules;

namespace Mihaylov.Data
{
    public class NinjectModuleDictionariesData : NinjectModule
    {
        private readonly string connectionString;

        public NinjectModuleDictionariesData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new Mihaylov.Database.NinjectModuleDatabase(this.connectionString) });

            Kernel.Bind<IGetAllRepository<Course>>().To<CoursesRepository>();
            Kernel.Bind<IGetAllRepository<Language>>().To<LanguagesRepository>();
            Kernel.Bind<IGetAllRepository<LearningSystem>>().To<LearningSystemsRepository>();
            Kernel.Bind<IGetAllRepository<Level>>().To<LevelsRepository>();
            Kernel.Bind<IGetAllRepository<PrepositionType>>().To<PrepositionTypesRepository>();
            Kernel.Bind<IGetAllRepository<Record>>().To<RecordsRepository>();
            Kernel.Bind<IGetAllRepository<RecordType>>().To<RecordTypesRepository>();
        }
    }
}
