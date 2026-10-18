using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Commands;

public record CreateGroupCommand(Group Group) : IRequest<Group>;

public class CreateGroupCommandHandler(IGroupRepository repository, ILogger<CreateGroupCommandHandler> logger) : IRequestHandler<CreateGroupCommand, Group>
{
    public async Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.CreateAsync(request.Group, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in add/update Group. Error: {Message}", ex.Message);
            throw;
        }
    }
}
