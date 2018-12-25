using Autofac;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Core.Managers;
using Mihaylov.Dictionaries.Core.Writers;
using Mihaylov.Dictionaries.Data;

namespace Mihaylov.Dictionaries.Core
{
    public class AutofacModuleDictionariesCore : Module
    {
        private readonly string connectionString;

        public AutofacModuleDictionariesCore(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleDictionariesData(this.connectionString));

            builder.RegisterType<CoursesInfoManager>().As<ICoursesInfoManager>().SingleInstance();
            builder.RegisterType<RecordsManager>().As<IRecordsManager>().SingleInstance();

            builder.RegisterType<RecordsWriter>().As<IRecordsWriter>();
        }
    }    
}
