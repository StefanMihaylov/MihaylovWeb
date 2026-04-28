using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateGroup;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateInventory;

public record CreateInventoryCommand(Inventory Item) : IRequest<long>;

public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, long>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateInventoryCommandHandler(IGearDbContext context, ILogger<CreateGroupCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<long> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var model = request.Item;
        model.Name = model.Name.Trim();
        model.Description = model.Description?.Trim();

        try
        {
            var dbModel = await _context.InventoryItems
                            .Where(t => t.InventoryItemId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Entities.InventoryItem();
                await _context.InventoryItems.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.InventoryItemId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Inventory. Error: {Message}", ex.Message);
            throw;
        }
    }
}
