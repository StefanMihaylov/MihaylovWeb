using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetInventoriesQuery(bool? IsActive) : IRequest<IEnumerable<Inventory>>;

public class GetInventoryQueriesHandler(IInventoryRepository repository, ILogger<GetCategoriesQueryHandler> logger) : IRequestHandler<GetInventoriesQuery, IEnumerable<Inventory>>
{
    public async Task<IEnumerable<Inventory>> Handle(GetInventoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(request, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetInventory failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
