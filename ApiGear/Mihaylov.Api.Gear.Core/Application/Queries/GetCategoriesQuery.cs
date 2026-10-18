using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetCategoriesQuery : IRequest<IEnumerable<Category>>;

public class GetCategoriesQueryHandler(ICategoryRepository repository, ILogger<GetCategoriesQueryHandler> logger) : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetCategories failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
