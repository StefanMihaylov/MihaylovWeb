using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<IEnumerable<Category>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly IGearDbContext _context;

    public GetCategoriesQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Categories.AsNoTracking().OrderBy(x => x.Name);

        var categories = await query.ProjectToType<Category>()
                                .ToListAsync(cancellationToken);

        return categories;
    }
}
