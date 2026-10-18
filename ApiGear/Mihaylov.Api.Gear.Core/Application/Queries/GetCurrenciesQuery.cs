using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public class GetCurrenciesQuery : IRequest<IEnumerable<Currency>>;

public class GetCurrenciesQueryHandler(ICurrencyRepository repository, ILogger<GetCurrenciesQueryHandler> logger) : IRequestHandler<GetCurrenciesQuery, IEnumerable<Currency>>
{
    public async Task<IEnumerable<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetCurrencies failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
