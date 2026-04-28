using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

public record GetTripsQuery : IRequest<IEnumerable<Trip>>;

public class GetTripsQueryHandler : IRequestHandler<GetTripsQuery, IEnumerable<Trip>>
{
    private readonly IGearDbContext _context;

    public GetTripsQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trip>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Trips.AsNoTracking().OrderByDescending(x => x.CreatedAt);

        var trips = await query.ProjectToType<Trip>()
                                .ToListAsync(cancellationToken);

        return trips;
    }
}
