using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetShops;

public record GetShopsQuery : IRequest<IEnumerable<Shop>>;

public class GetShopsQueryHandler : IRequestHandler<GetShopsQuery, IEnumerable<Shop>>
{
    private readonly IGearDbContext _context;

    public GetShopsQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Shop>> Handle(GetShopsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Shops.AsNoTracking().OrderBy(x => x.Name);

        var shops = await query.ProjectToType<Shop>()
                                .ToListAsync(cancellationToken);

        return shops;
    }
}