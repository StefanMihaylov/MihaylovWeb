using Mihaylov.Data.Interfaces;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;
using Mihaylov.Data.Repositories.Dictionaries;
using Mihaylov.Data.UnitOfWorks;
using DAL = Mihaylov.Database.Dictionaries;
using Mihaylov.Database.Interfaces;
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
            Kernel.Bind<IDictionariesDbContext>().ToConstructor(x => new DAL.DictionariesDbContext(this.connectionString));

            Kernel.Bind<IGetAllRepository<Course>>().To<CoursesRepository>();
            Kernel.Bind<IGetAllRepository<Language>>().To<LanguagesRepository>();
            Kernel.Bind<IGetAllRepository<LearningSystem>>().To<LearningSystemsRepository>();
            Kernel.Bind<IGetAllRepository<Level>>().To<LevelsRepository>();

            Kernel.Bind<IRecordsData>().To<RecordsData>();
        }
    }
}
