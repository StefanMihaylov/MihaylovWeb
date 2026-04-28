using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetGroups;

public record GetGroupsQuery : IRequest<IEnumerable<Group>>;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IEnumerable<Group>>
{
    private readonly IGearDbContext _context;

    public GetGroupsQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Group>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Groups.AsNoTracking().OrderBy(x => x.Name);

        var groups = await query.ProjectToType<Group>()
                                .ToListAsync(cancellationToken);

        return groups;
    }
}
