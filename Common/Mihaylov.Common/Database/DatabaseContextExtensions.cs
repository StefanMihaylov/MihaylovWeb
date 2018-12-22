using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Mihaylov.Common.Database
{
    public static class DatabaseContextExtensions
    {
        //public static Exception ConvertDbEntityValidationException(this DbContext context, DbEntityValidationException e)
        //{
        //    Exception innerException = null;
        //    foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
        //    {
        //        StringBuilder message = new StringBuilder();
        //        message.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().FullName, eve.Entry.State);
        //        message.AppendLine();

        //        foreach (DbValidationError ve in eve.ValidationErrors)
        //        {
        //            message.AppendFormat("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"", ve.PropertyName, eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName), ve.ErrorMessage);
        //            message.AppendLine();
        //        }

        //        innerException = new Exception(message.ToString());
        //    }

        //    return new DbEntityValidationException(e.Message, innerException);
        //}

        //public static string GetDbFirstConnectionString(string codeFirstConnectionString, string edmxName)
        //{
        //    var ecb = new EntityConnectionStringBuilder
        //    {
        //        Metadata = $"res://*/{edmxName}.csdl|res://*/{edmxName}.ssdl|res://*/{edmxName}.msl",
        //        Provider = "System.Data.SqlClient",
        //        ProviderConnectionString = codeFirstConnectionString
        //    };

        //    return ecb.ConnectionString;
        //}

        //public static void FixEfProviderServicesProblem(this DbContext context)
        //{
        //    var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        //}
    }
}
