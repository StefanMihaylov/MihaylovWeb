using System.Data.Entity.Validation;
using Mihaylov.Common.Database;
using Mihaylov.Dictionaries.Database.Interfaces;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class DictionariesDbContext : IDictionariesDbContext
    {
        //public DictionariesDbContext(string connectionString)
        //    : base(DatabaseContextExtensions.GetDbFirstConnectionString(connectionString, "Dictionaries.DictionariesDbModel"))
        //{
        //    this.FixEfProviderServicesProblem();
        //}

        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        throw this.ConvertDbEntityValidationException(ex);
        //    }
        //}
    }
}