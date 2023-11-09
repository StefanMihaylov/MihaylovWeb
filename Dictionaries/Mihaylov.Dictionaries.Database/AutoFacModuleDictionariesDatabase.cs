using Autofac;
using Mihaylov.Dictionaries.Database.Interfaces;
using Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Database
{
    public class AutoFacModuleDictionariesDatabase : Module
    {
        private readonly string connectionString;

        public AutoFacModuleDictionariesDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DictionariesDbContext(this.connectionString)).As<IDictionariesDbContext>();
        }
    }
}
