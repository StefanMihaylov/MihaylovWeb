using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class BrandRepository(MihaylovGearDbContext context) : IBrandRepository
{
    public async Task<Brand> CreateAsync(Brand model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();


        var dbModel = await context.Brands
                        .Where(t => t.BrandId == model.Id)
                        .FirstOrDefaultAsync(ct)
                        .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Lookups.Brand();
            await context.Brands.AddAsync(dbModel, ct).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.Adapt<Brand>();
    }

    public async Task<IEnumerable<Brand>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Brands.AsNoTracking().OrderBy(x => x.Name);

        var brands = await query.ProjectToType<Brand>()
                                .ToListAsync(ct);

        return brands;
    }
}
