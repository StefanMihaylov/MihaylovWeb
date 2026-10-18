using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateBrandCommand(Brand Brand) : IRequest<Brand>;

public class CreateBrandCommandHandler(IBrandRepository repository, ILogger<CreateBrandCommandHandler> logger) : IRequestHandler<CreateBrandCommand, Brand>
{
    public async Task<Brand> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Brand, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Brand. Error: {Message}", ex.Message);
            throw;
        }
    }
}
