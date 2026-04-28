using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Domain.Entities;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CloneTrip;

public record CloneTripCommand(long Id) : IRequest<long>;

public class CloneTripCommandHandler : IRequestHandler<CloneTripCommand, long>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CloneTripCommandHandler(IGearDbContext context, ILogger<CloneTripCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<long> Handle(CloneTripCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var sourceTrip = await _context.Trips.AsNoTracking()
                                .Include(t => t.Nodes)                
                                .FirstOrDefaultAsync(t => t.TripId == request.Id)
                                .ConfigureAwait(false);

            if (sourceTrip == null)
            {
                throw new Exception("Source trip not found");
            }

            var newTrip = new Trip
            {
                Title = sourceTrip.Title,
                Year = DateTime.Now.Year,
                Notes = sourceTrip.Notes,
                CreatedAt = DateTime.Now,
            };

            _context.Trips.Add(newTrip);
            await _context.SaveChangesAsync();

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

                var newNode = new GearNode
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

                _context.GearNodes.Add(newNode);
                await _context.SaveChangesAsync();

                idMap.Add(sourceNode.GearNodeId, newNode.GearNodeId);
            }

            await transaction.CommitAsync();

            return newTrip.TripId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}