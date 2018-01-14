using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Core.Managers.Dictionaries;
using Mihaylov.Core.Providers.Dictionaries;
using Mihaylov.Core.Writers.Dictionaries;
using Mihaylov.Data;
using Ninject.Modules;

namespace Mihaylov.Core
{
    public class NinjectModuleDictionariesCore : NinjectModule
    {
        private readonly string connectionString;

        public NinjectModuleDictionariesCore(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new NinjectModuleDictionariesData(this.connectionString) });

            Kernel.Bind<ICoursesInfoProvider>().To<CoursesInfoProvider>();
            Kernel.Bind<IRecordsProvider>().To<RecordsProvider>();

            Kernel.Bind<ICoursesInfoManager>().To<CoursesInfoManager>().InSingletonScope();
            Kernel.Bind<IRecordsManager>().To<RecordsManager>().InSingletonScope();

            Kernel.Bind<IRecordsWriter>().To<RecordsWriter>();
        }
    }    
}
