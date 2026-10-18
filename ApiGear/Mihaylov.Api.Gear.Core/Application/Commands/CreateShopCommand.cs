using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateShopCommand(Shop Shop) : IRequest<Shop>;

public class CreateShopCommandHandler(IShopRepository repository, ILogger<CreateShopCommandHandler> logger) : IRequestHandler<CreateShopCommand, Shop>
{
    public async Task<Shop> Handle(CreateShopCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Shop, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Shop. Error: {Message}", ex.Message);
            throw;
        }
    }
}
