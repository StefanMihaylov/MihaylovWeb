using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetBrands;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateBrand;

public record CreateBrandCommand(Brand Brand) : IRequest<Brand>;


public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Brand>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateBrandCommandHandler(IGearDbContext context, ILogger<CreateBrandCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Brand> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var model = request.Brand;
        model.Name = model.Name.Trim();

        try
        {
            var dbModel = await _context.Brands
                            .Where(t => t.BrandId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Lookups.Brand();
                await _context.Brands.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.Adapt<Brand>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Brand. Error: {Message}", ex.Message);
            throw;
        }
    }
}
