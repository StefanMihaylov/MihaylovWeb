using System;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Account : IMapFrom<DAL.Account> // IHaveCustomMappings
    {
        public Account()
        {
        }

        //public void CreateMappings(IMapperConfigurationExpression configuration)
        //{
        //    throw new NotImplementedException();
        //}

        public Guid Id { get; set; }

        public AccountType AccountType { get; set; }

        public string Username { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime LastOnlineDate { get; set; }

        public bool IsAccountDisabled { get; set; }
    }
}
