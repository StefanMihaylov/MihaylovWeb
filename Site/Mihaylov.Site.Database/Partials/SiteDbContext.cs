using System;
using System.Data.Entity.Validation;
using Mihaylov.Common.Database;
using System.Text;
using Mihaylov.Site.Database.Interfaces;

namespace Mihaylov.Site.Database
{
    public partial class SiteDbContext : ISiteDbContext
    {
        public SiteDbContext(string connectionString)
            : base(DatabaseContextExtensions.GetDbFirstConnectionString(connectionString, "Site.SiteDbModel")) => this.FixEfProviderServicesProblem();

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