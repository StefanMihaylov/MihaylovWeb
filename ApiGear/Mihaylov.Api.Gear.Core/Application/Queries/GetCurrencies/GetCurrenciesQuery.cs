using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Queries.GetCurrencies;

public class GetCurrenciesQuery : IRequest<IEnumerable<Currency>>;

public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IEnumerable<Currency>>
{
    private readonly IGearDbContext _context;

    public GetCurrenciesQueryHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Currencies.AsNoTracking()
                                       .OrderByDescending(x => x.IsDefault)
                                       .ThenBy(x => x.CurrencyId);

        var currencies = await query.ProjectToType<Currency>()
                                .ToListAsync(cancellationToken);

        return currencies;
    }
}
