using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetShops;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateShop;

public record CreateShopCommand(Shop Shop) : IRequest<Shop>;

public class CreateShopCommandHandler : IRequestHandler<CreateShopCommand, Shop>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateShopCommandHandler(IGearDbContext context, ILogger<CreateShopCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Shop> Handle(CreateShopCommand request, CancellationToken cancellationToken)
    {
        var model = request.Shop;
        model.Name = model.Name.Trim();

        try
        {
            var dbModel = await _context.Shops
                            .Where(t => t.ShopId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Lookups.Shop();
                await _context.Shops.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.Adapt<Shop>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Shop. Error: {Message}", ex.Message);
            throw;
        }
    }
}
