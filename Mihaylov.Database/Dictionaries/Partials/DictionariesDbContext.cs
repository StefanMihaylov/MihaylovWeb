using System.Data.Entity.Validation;
using Mihaylov.Common.Database;
using Mihaylov.Database.Interfaces;

namespace Mihaylov.Database.Dictionaries
{
    public partial class DictionariesDbContext : IDictionariesDbContext
    {
        public DictionariesDbContext(string connectionString)
            : base(DatabaseContextExtensions.GetDbFirstConnectionString(connectionString, "Dictionaries.DictionariesDbModel"))
        {
            this.FixEfProviderServicesProblem();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw this.ConvertDbEntityValidationException(ex);
            }
        }
    }
}