using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record AlterGearNodeIsPackedCommand(long Id) : IRequest;

public class AlterGearNodeIsPackedCommandHandler(IGearNodeRepository repository, ILogger<AlterGearNodeIsPackedCommandHandler> logger) : IRequestHandler<AlterGearNodeIsPackedCommand>
{
    public async Task Handle(AlterGearNodeIsPackedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await repository.AlterAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in altering IsPacked of GearNode. Error: {Message}", ex.Message);
            throw;
        }
    }
}