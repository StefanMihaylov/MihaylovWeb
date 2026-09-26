using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetParentGearNodes;

public record GetParentGearNodesQuery(long TripId) : IRequest<IEnumerable<GearNodeFlat>>;

public class GetParentGearNodesQueryHandler : IRequestHandler<GetParentGearNodesQuery, IEnumerable<GearNodeFlat>>
{
    private readonly IGearDbContext _context;
    public GetParentGearNodesQueryHandler(IGearDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<GearNodeFlat>> Handle(GetParentGearNodesQuery request, CancellationToken cancellationToken)
    {
        var nodeTypes = new[]
        {
            (int)NodeType.Group,
            (int)NodeType.Category
        };

        IEnumerable<long> parents = await _context.GearNodes
                                    .Where(n => n.TripId == request.TripId)
                                    .Where(n => nodeTypes.Contains(n.NodeTypeId))
                                    .Select(n => n.GearNodeId)
                                    .ToListAsync(cancellationToken)
                                    .ConfigureAwait(false);

        var parentNodes = await _context.GearNodes
                             .Where(n => n.TripId == request.TripId)
                             .Where(n => parents.Contains(n.GearNodeId))
                             .OrderBy(n => n.ParentId ?? 0)
                             .ProjectToType<GearNodeFlat>()
                             .ToListAsync(cancellationToken)
                             .ConfigureAwait(false);

        return parentNodes;
    }
}

