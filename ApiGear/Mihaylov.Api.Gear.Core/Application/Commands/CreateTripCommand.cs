using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateTripCommand(Trip Trip) : IRequest<Trip>;

public class CreateTripCommandHandler(ITripRepository repository, ILogger<CreateTripCommandHandler> logger) : IRequestHandler<CreateTripCommand, Trip>
{
    public async Task<Trip> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Trip, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Trip. Error: {Message}", ex.Message);
            throw;
        }
    }
}