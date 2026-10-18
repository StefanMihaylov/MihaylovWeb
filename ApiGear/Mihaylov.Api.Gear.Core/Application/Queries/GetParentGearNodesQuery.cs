using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetParentGearNodesQuery(long TripId) : IRequest<IEnumerable<GearNodeFlat>>;

public class GetParentGearNodesQueryHandler(IGearNodeRepository repository, ILogger<GetParentGearNodesQueryHandler> logger) : IRequestHandler<GetParentGearNodesQuery, IEnumerable<GearNodeFlat>>
{
    public async Task<IEnumerable<GearNodeFlat>> Handle(GetParentGearNodesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetByTripIdAsync(request.TripId, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetParentGearNodes failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}