using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record DeleteGearNodeCommand(long Id) : IRequest;

public class DeleteGearNodeCommandHandler(IGearNodeRepository repository, ILogger<DeleteGearNodeCommandHandler> logger) : IRequestHandler<DeleteGearNodeCommand>
{
    public async Task Handle(DeleteGearNodeCommand request, CancellationToken cancellationToken)
    {
		try
		{
            await repository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
		catch (Exception ex)
		{
            logger.LogError(ex, "Error in delete GearNode. Error: {Message}", ex.Message);
            throw;
        }
    }
}