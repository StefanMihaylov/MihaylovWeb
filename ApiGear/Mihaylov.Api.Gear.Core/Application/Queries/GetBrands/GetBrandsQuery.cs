using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetBrands
{
    public record GetBrandsQuery : IRequest<IEnumerable<Brand>>;

    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IEnumerable<Brand>>
    {
        private readonly IGearDbContext _context;

        public GetBrandsQueryHandler(IGearDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Brands.AsNoTracking().OrderBy(x => x.Name);

            var brands = await query.ProjectToType<Brand>()
                                    .ToListAsync(cancellationToken);

            return brands;
        }
    }
}
