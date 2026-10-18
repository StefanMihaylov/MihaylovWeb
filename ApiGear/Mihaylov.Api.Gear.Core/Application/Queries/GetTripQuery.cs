using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetTripQuery(long Id, bool? NotPacked) : IRequest<TripFull>;

public class GetTripQueryHandler(ITripRepository repository, ILogger<GetTripQueryHandler> logger) : IRequestHandler<GetTripQuery, TripFull>
{
    public async Task<TripFull> Handle(GetTripQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetByIdAsync(request, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetTrip failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
