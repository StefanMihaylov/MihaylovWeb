using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateInventoryCommand(Inventory Item) : IRequest<long>;

public class CreateInventoryCommandHandler(IInventoryRepository repository, ILogger<CreateGroupCommandHandler> logger) : IRequestHandler<CreateInventoryCommand, long>
{
    public async Task<long> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Item, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Inventory. Error: {Message}", ex.Message);
            throw;
        }
    }
}
