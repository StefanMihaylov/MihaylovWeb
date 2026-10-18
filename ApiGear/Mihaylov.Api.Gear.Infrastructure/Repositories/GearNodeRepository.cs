using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class GearNodeRepository(MihaylovGearDbContext context) : IGearNodeRepository
{
    public async Task<long> CreateAsync(GearNode model, CancellationToken ct)
    {
        var dbModel = await context.GearNodes
                .Where(t => t.GearNodeId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Entities.GearNode();
            await context.GearNodes.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.GearNodeId;
    }

    public async Task AlterAsync(long noteId, CancellationToken ct)
    {
        var dbModel = await context.GearNodes
                .Where(t => t.GearNodeId == noteId)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            throw new Exception($"GearNode with id {noteId} not found.");
        }

        dbModel.IsPacked = !dbModel.IsPacked;

        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task DeleteAsync(long noteId, CancellationToken ct)
    {
        var node = await context.GearNodes.AsNoTracking()
                    .Where(g => g.GearNodeId == noteId)
                    .FirstOrDefaultAsync(ct)
                    .ConfigureAwait(false);

        if (node == null)
        {
            throw new Exception($"GearNode with id {noteId} not found.");
        }

        context.GearNodes.Remove(node);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<GearNodeFlat>> GetByTripIdAsync(long tripId, CancellationToken ct)
    {
        var nodeTypes = new[]
{
            (int)NodeType.Group,
            (int)NodeType.Category
        };

        IEnumerable<long> parents = await context.GearNodes
                                    .Where(n => n.TripId == tripId)
                                    .Where(n => nodeTypes.Contains(n.NodeTypeId))
                                    .Select(n => n.GearNodeId)
                                    .ToListAsync(ct)
                                    .ConfigureAwait(false);

        var parentNodes = await context.GearNodes
                             .Where(n => n.TripId == tripId)
                             .Where(n => parents.Contains(n.GearNodeId))
                             .OrderBy(n => n.ParentId ?? 0)
                             .ProjectToType<GearNodeFlat>()
                             .ToListAsync(ct)
                             .ConfigureAwait(false);

        return parentNodes;
    }
}
