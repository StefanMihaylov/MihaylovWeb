using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace Mihaylov.Core
{
    public class NinjectModuleCore : NinjectModule
    {
        private readonly string connectionString;
        private readonly string url;

        public NinjectModuleCore(string connectionString, string url)
        {
            this.connectionString = connectionString;
            this.url = url;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new NinjectModuleSiteCore(this.connectionString, this.url) });
        }
    }
}
