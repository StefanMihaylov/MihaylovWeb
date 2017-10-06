using System;
using System.Data.Entity.Validation;
using System.Text;
using Mihaylov.Database.Interfaces;

namespace Mihaylov.Database.Site
{
    public partial class SiteDbContext : ISiteDbContext
    {
        public SiteDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Exception innerException = null;
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    StringBuilder message = new StringBuilder();
                    message.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().FullName, eve.Entry.State);
                    message.AppendLine();

                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        message.AppendFormat("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"", ve.PropertyName, eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName), ve.ErrorMessage);
                        message.AppendLine();
                    }

                    innerException = new Exception(message.ToString());
                }

                throw new DbEntityValidationException(e.Message, innerException);
            }
        }
    }
}