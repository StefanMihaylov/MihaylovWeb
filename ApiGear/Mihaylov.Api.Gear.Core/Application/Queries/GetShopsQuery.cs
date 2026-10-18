using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetShopsQuery : IRequest<IEnumerable<Shop>>;

public class GetShopsQueryHandler(IShopRepository repository, ILogger<GetShopsQueryHandler> logger) : IRequestHandler<GetShopsQuery, IEnumerable<Shop>>
{
    public async Task<IEnumerable<Shop>> Handle(GetShopsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetShops failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}