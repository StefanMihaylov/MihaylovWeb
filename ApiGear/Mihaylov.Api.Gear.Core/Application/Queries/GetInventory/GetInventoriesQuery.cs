using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;

public record GetInventoriesQuery(bool? IsActive) : IRequest<IEnumerable<Inventory>>;

public class GetInventoryQueriesHandler : IRequestHandler<GetInventoriesQuery, IEnumerable<Inventory>>
{
    private readonly IGearDbContext _context;

    public GetInventoryQueriesHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inventory>> Handle(GetInventoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.InventoryItems.AsNoTracking()
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
                                         .ToListAsync(cancellationToken);

        return inverntoryItems;
    }
}
