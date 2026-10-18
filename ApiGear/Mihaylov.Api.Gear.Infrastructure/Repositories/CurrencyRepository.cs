using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class CurrencyRepository(MihaylovGearDbContext context) : ICurrencyRepository
{
    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Currencies.AsNoTracking()
                               .OrderByDescending(x => x.IsDefault)
                               .ThenBy(x => x.CurrencyId);

        var currencies = await query.ProjectToType<Currency>()
                                .ToListAsync(ct);

        return currencies;
    }
}
