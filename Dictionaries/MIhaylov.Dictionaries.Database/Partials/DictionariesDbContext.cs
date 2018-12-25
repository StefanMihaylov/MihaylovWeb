using Microsoft.EntityFrameworkCore;
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

        private string connectionString;

        public DictionariesDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }
    }
}