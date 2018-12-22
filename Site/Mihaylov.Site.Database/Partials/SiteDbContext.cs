using System;
using Mihaylov.Common.Database;
using System.Text;
using Mihaylov.Site.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mihaylov.Site.Database.Models
{
    public partial class SiteDbContext : ISiteDbContext
    {
        //public SiteDbContext(string connectionString)
        //    : base(DatabaseContextExtensions.GetDbFirstConnectionString(connectionString, "Site.SiteDbModel")) => this.FixEfProviderServicesProblem();

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

        public SiteDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }
    }
}