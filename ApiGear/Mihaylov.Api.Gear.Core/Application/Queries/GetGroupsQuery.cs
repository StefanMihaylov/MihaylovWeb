using MediatR;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Queries;

public record GetGroupsQuery : IRequest<IEnumerable<Group>>;

public class GetGroupsQueryHandler(IGroupRepository repository, ILogger<GetGroupsQueryHandler> logger) : IRequestHandler<GetGroupsQuery, IEnumerable<Group>>
{
    public async Task<IEnumerable<Group>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetGroups failed. Message: {Message}", ex.Message);
            throw;
        }
    }
}
