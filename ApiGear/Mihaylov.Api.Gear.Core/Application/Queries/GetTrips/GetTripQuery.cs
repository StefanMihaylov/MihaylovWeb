using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

public record GetTripQuery(long Id, bool? NotPacked) : IRequest<TripFull>;

public class GetTripQueryHandler : IRequestHandler<GetTripQuery, TripFull>
{
    private readonly IGearDbContext _context;

    public GetTripQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<TripFull> Handle(GetTripQuery request, CancellationToken cancellationToken)
    {
        var trip = await _context.Trips.AsNoTracking()
                            .Where(x => x.TripId == request.Id)
                            .ProjectToType<Trip>()
                            .FirstOrDefaultAsync(cancellationToken);

        if (trip is null)
        {
            throw new KeyNotFoundException($"Trip with id {request.Id} not found.");
        }

        var nodeQuery = _context.GearNodes.AsNoTracking()
                                .Include(n => n.Category)
                                .Include(n => n.Group)
                                .Include(n => n.InventoryItem).ThenInclude(a => a.Brand)
                                .Include(n => n.Children).ThenInclude(c => c.Children)
                            .Where(x => x.TripId == request.Id)
                            .AsQueryable();

        if (request.NotPacked.HasValue && request.NotPacked.Value)
        {
            nodeQuery = nodeQuery.Where(node =>
                (node.Quantity > 0 && !node.IsPacked && !node.IsExcluded) ||
                (node.Quantity == 0 && node.Children.Any(child =>
                    child.Quantity > 0 && !child.IsPacked && !child.IsExcluded)) ||
                (node.Quantity == 0 && node.Children.Any(child =>
                    child.Quantity == 0 && child.Children.Any(grChild =>
                        grChild.Quantity > 0 && !grChild.IsPacked && !grChild.IsExcluded))
                )
            );
        }

        var nodes = await nodeQuery.ProjectToType<GearNode>()
                                   .ToListAsync(cancellationToken)
                                   .ConfigureAwait(false);

        var result = trip.Adapt<TripFull>();
        result.Nodes = nodes;

        return result;
    }
}
