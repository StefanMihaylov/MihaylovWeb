using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateGearNodeCommand(GearNode Gear) : IRequest<long>;

public class CreateGearNodeCommandHandler(IGearNodeRepository repository, ILogger<CreateGearNodeCommandHandler> logger) : IRequestHandler<CreateGearNodeCommand, long>
{
    public async Task<long> Handle(CreateGearNodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Gear, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update GearNode. Error: {Message}", ex.Message);
            throw;
        }
    }
}
