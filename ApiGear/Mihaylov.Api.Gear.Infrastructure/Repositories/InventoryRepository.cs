using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Core.Application.Queries;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class InventoryRepository(MihaylovGearDbContext context) : IInventoryRepository
{
    public async Task<long> CreateAsync(Inventory model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();
        model.Description = model.Description?.Trim();

        var dbModel = await context.InventoryItems
                .Where(t => t.InventoryItemId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Entities.InventoryItem();
            await context.InventoryItems.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.InventoryItemId;
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync(GetInventoriesQuery request, CancellationToken ct)
    {
        var query = context.InventoryItems.AsNoTracking()
                                   .Include(x => x.Shop)
                                   .Include(x => x.Brand)
                                   .Include(x => x.Category)
                                   .Include(x => x.Currency)
                                   .AsQueryable();

        if (request.IsActive.HasValue && request.IsActive.Value)
        {
            query = query.Where(x => x.ItemStatusId == (int)ItemStatus.Active);
        }

        var inverntoryItems = await query.OrderByDescending(x => x.PurchaseDate)
                                         .ThenByDescending(x => x.Category.Name)
                                         .ThenBy(x => x.Name)
                                         .ProjectToType<Inventory>()
                                         .ToListAsync(ct);

        return inverntoryItems;
    }
}
