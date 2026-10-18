using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetTripsQuery : IRequest<IEnumerable<Trip>>;

public class GetTripsQueryHandler(ITripRepository repository, ILogger<GetTripsQueryHandler> logger) : IRequestHandler<GetTripsQuery, IEnumerable<Trip>>
{
    public async Task<IEnumerable<Trip>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetTrips failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
