using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateGearNode;

public record CreateGearNodeCommand(GearNode Gear) : IRequest<long>;

public class CreateGearNodeCommandHandler : IRequestHandler<CreateGearNodeCommand, long>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateGearNodeCommandHandler(IGearDbContext context, ILogger<CreateGearNodeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<long> Handle(CreateGearNodeCommand request, CancellationToken cancellationToken)
    {
        var model = request.Gear;

        try
        {
            var dbModel = await _context.GearNodes
                            .Where(t => t.GearNodeId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Entities.GearNode();
                await _context.GearNodes.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.GearNodeId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update GearNode. Error: {Message}", ex.Message);
            throw;
        }
    }
}
