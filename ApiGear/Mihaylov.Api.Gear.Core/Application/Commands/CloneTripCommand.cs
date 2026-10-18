using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CloneTripCommand(long Id) : IRequest<long>;

public class CloneTripCommandHandler(ITripRepository reposiytory, ILogger<CloneTripCommandHandler> logger) : IRequestHandler<CloneTripCommand, long>
{
    public async Task<long> Handle(CloneTripCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resultId = await reposiytory.CloneAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return resultId;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CloneTripCommand failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}