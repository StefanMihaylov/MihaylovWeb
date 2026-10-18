using Mihaylov.Api.Gear.Core.Application.Models;

namespace Mihaylov.Api.Gear.Core.Application.Interfaces;

public interface IGroupRepository
{
    Task<Group> CreateAsync(Group model, CancellationToken ct);

    Task<IEnumerable<Group>> GetAllAsync(CancellationToken ct);
}
