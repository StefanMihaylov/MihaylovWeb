using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Database;
using Mihaylov.Common.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Mihaylov.Api.Site.Data.Repositories
{
    public class LookupTablesRepository : ILookupTablesRepository
    {
        private readonly SiteDbContext _context;

        public LookupTablesRepository(SiteDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync()
        {
            var ethnicities = await this._context.Ethnicities
                                                 .To<Ethnicity>()
                                                 .ToListAsync()
                                                 .ConfigureAwait(false);

            return ethnicities;
        }

        public async Task<IEnumerable<Orientation>> GetAllOrientationsAsync()
        {
            var preference = await this._context.Orientations
                                                .To<Orientation>()
                                                .ToListAsync()
                                                .ConfigureAwait(false);
            return preference;
        }

        public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync()
        {
            var accountTypes = await this._context.AccountTypes
                                                .To<AccountType>()
                                                .ToListAsync()
                                                .ConfigureAwait(false);
            return accountTypes;
        }

        //public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        //{
        //    var units = await this._context.Units
        //                                   .Select(Unit.FromDb)
        //                                   .ToListAsync()
        //                                   .ConfigureAwait(false);    
        //    return units;
        //}
    }
}
