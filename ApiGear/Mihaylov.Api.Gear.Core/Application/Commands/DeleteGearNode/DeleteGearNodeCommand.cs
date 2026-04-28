using MediatR;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;

namespace Mihaylov.Api.Gear.Core.Application.Commands.DeleteGearNode;

public record DeleteGearNodeCommand(long Id) : IRequest;

public class DeleteGearNodeCommandHandler : IRequestHandler<DeleteGearNodeCommand>
{
    private readonly IGearDbContext _context;

    public DeleteGearNodeCommandHandler(IGearDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteGearNodeCommand request, CancellationToken cancellationToken)
    {
        var node = await _context.GearNodes.AsNoTracking()
                            .Where(g => g.GearNodeId == request.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

        if (node == null)
        {
            throw new Exception($"GearNode with id {request.Id} not found.");
        }

        _context.GearNodes.Remove(node);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}