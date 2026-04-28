using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateTrip;

public record CreateTripCommand(Trip Trip) : IRequest<Trip>;

public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, Trip>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateTripCommandHandler(IGearDbContext context, ILogger<CreateTripCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Trip> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var model = request.Trip;
        model.Title = model.Title.Trim();
        model.Notes = model.Notes?.Trim();

        try
        {
            var dbModel = await _context.Trips
                            .Where(t => t.TripId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Entities.Trip();
                await _context.Trips.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.Adapt<Trip>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Trip. Error: {Message}", ex.Message);
            throw;
        }
    }
}