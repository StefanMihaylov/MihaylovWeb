using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class GroupRepository(MihaylovGearDbContext context) : IGroupRepository
{
    public async Task<Group> CreateAsync(Group model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();

        var dbModel = await context.Groups
                .Where(t => t.GroupId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Lookups.Group();
            await context.Groups.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.Adapt<Group>();
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Groups.AsNoTracking().OrderBy(x => x.Name);

        var groups = await query.ProjectToType<Group>()
                                .ToListAsync(ct);

        return groups;
    }
}
