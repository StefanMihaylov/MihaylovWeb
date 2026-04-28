using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateGearNode;

public record AlterGearNodeIsPackedCommand(long Id) : IRequest;

public class AlterGearNodeIsPackedCommandHandler : IRequestHandler<AlterGearNodeIsPackedCommand>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public AlterGearNodeIsPackedCommandHandler(IGearDbContext context, ILogger<AlterGearNodeIsPackedCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(AlterGearNodeIsPackedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dbModel = await _context.GearNodes
                            .Where(t => t.GearNodeId == request.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                throw new Exception($"GearNode with id {request.Id} not found.");
            }

            dbModel.IsPacked = !dbModel.IsPacked;

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in altering IsPacked of GearNode. Error: {Message}", ex.Message);
            throw;
        }
    }
}