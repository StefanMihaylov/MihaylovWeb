using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateCategoryCommand(Category Category) : IRequest<Category>;

public class CreateCategoryCommandHandler(ICategoryRepository repository, ILogger<CreateCategoryCommandHandler> logger) : IRequestHandler<CreateCategoryCommand, Category>
{
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Category, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Category. Error: {Message}", ex.Message);
            throw;
        }
    }
}
