using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Core.Application.Queries;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class TripRepository(MihaylovGearDbContext context) : ITripRepository
{
    public async Task<Trip> CreateAsync(Trip model, CancellationToken ct)
    {
        model.Title = model.Title.Trim();
        model.Notes = model.Notes?.Trim();

        var dbModel = await context.Trips
                .Where(t => t.TripId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Entities.Trip();
            await context.Trips.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.Adapt<Trip>();
    }

    public async Task<IEnumerable<Trip>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Trips.AsNoTracking().OrderByDescending(x => x.CreatedAt);

        var trips = await query.ProjectToType<Trip>()
                                .ToListAsync(ct);

        return trips;
    }

    public async Task<TripFull> GetByIdAsync(GetTripQuery request, CancellationToken ct)
    {
        var trip = await context.Trips.AsNoTracking()
                    .Where(x => x.TripId == request.Id)
                    .ProjectToType<Trip>()
                    .FirstOrDefaultAsync(ct);

        if (trip is null)
        {
            throw new KeyNotFoundException($"Trip with id {request.Id} not found.");
        }

        var nodeQuery = context.GearNodes.AsNoTracking()
                                .Include(n => n.Category)
                                .Include(n => n.Group)
                                .Include(n => n.InventoryItem).ThenInclude(a => a!.Brand)
                                .Include(n => n.Children).ThenInclude(c => c.Children)
                            .Where(x => x.TripId == request.Id)
                            .AsQueryable();

        if (request.NotPacked.HasValue && request.NotPacked.Value)
        {
            nodeQuery = nodeQuery.Where(node =>
                (node.Quantity > 0 && !node.IsPacked && !node.IsExcluded) ||
                (node.Quantity == 0 && node.Children.Any(child =>
                    child.Quantity > 0 && !child.IsPacked && !child.IsExcluded)) ||
                (node.Quantity == 0 && node.Children.Any(child =>
                    child.Quantity == 0 && child.Children.Any(grChild =>
                        grChild.Quantity > 0 && !grChild.IsPacked && !grChild.IsExcluded))
                )
            );
        }

        var nodes = await nodeQuery.ProjectToType<GearNode>()
                                   .ToListAsync(ct)
                                   .ConfigureAwait(false);

        var result = trip.Adapt<TripFull>();
        result.Nodes = nodes;

        return result;
    }

    public async Task<long> CloneAsync(long tripId, CancellationToken ct)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var sourceTrip = await context.Trips.AsNoTracking()
                                .Include(t => t.Nodes)
                                .FirstOrDefaultAsync(t => t.TripId == tripId, ct)
                                .ConfigureAwait(false);

            if (sourceTrip == null)
            {
                throw new Exception("Source trip not found");
            }

            var newTrip = new Core.Domain.Entities.Trip
            {
                Title = sourceTrip.Title,
                Year = DateTime.Now.Year,
                Notes = sourceTrip.Notes,
                CreatedAt = DateTime.Now,
            };

            context.Trips.Add(newTrip);
            await context.SaveChangesAsync(ct);

            // Clone the GearNodes
            var idMap = new Dictionary<long, long>();
            foreach (var sourceNode in sourceTrip.Nodes.OrderBy(n => n.GearNodeId))
            {
                long? parentId = null;
                if (sourceNode.ParentId.HasValue)
                {
                    if (idMap.TryGetValue(sourceNode.ParentId.Value, out long value))
                    {
                        parentId = value;
                    }
                }

                var newNode = new Core.Domain.Entities.GearNode
                {
                    GearNodeId = 0,
                    TripId = newTrip.TripId,
                    ParentId = parentId,
                    IsPacked = false,

                    NodeTypeId = sourceNode.NodeTypeId,
                    GroupId = sourceNode.GroupId,
                    CategoryId = sourceNode.CategoryId,
                    InventoryItemId = sourceNode.InventoryItemId,
                    Quantity = sourceNode.Quantity,
                    IsExcluded = sourceNode.IsExcluded,
                    IsRequired = sourceNode.IsRequired,
                };

                context.GearNodes.Add(newNode);
                await context.SaveChangesAsync(ct);

                idMap.Add(sourceNode.GearNodeId, newNode.GearNodeId);
            }

            await transaction.CommitAsync(ct);

            return newTrip.TripId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
