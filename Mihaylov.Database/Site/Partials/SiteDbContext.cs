using System;
using System.Data.Entity.Validation;
using Mihaylov.Common.Database;
using System.Text;
using Mihaylov.Database.Interfaces;

namespace Mihaylov.Database.Site
{
    public partial class SiteDbContext : ISiteDbContext
    {
        public SiteDbContext(string connectionString)
            : base(connectionString)
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