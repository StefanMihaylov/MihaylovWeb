using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class ShopRepository(MihaylovGearDbContext context) : IShopRepository
{
    public async Task<Shop> CreateAsync(Shop model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();

        var dbModel = await context.Shops
                .Where(t => t.ShopId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Lookups.Shop();
            await context.Shops.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.Adapt<Shop>();
    }

    public async Task<IEnumerable<Shop>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Shops.AsNoTracking().OrderBy(x => x.Name);

        var shops = await query.ProjectToType<Shop>()
                                .ToListAsync(ct);

        return shops;
    }
}
