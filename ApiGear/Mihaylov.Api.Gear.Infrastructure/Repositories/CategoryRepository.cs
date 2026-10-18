using Mapster;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Models;
using Mihaylov.Api.Gear.Infrastructure.Persistence;

namespace Mihaylov.Api.Gear.Infrastructure.Repositories;

public class CategoryRepository(MihaylovGearDbContext context) : ICategoryRepository
{
    public async Task<Category> CreateAsync(Category model, CancellationToken ct)
    {
        model.Name = model.Name.Trim();

        var dbModel = await context.Categories
                .Where(t => t.CategoryId == model.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

        if (dbModel == null)
        {
            dbModel = new Core.Domain.Lookups.Category();
            await context.Categories.AddAsync(dbModel).ConfigureAwait(false);
        }

        dbModel = model.Adapt(dbModel);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return dbModel.Adapt<Category>();
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct)
    {
        var query = context.Categories.AsNoTracking().OrderBy(x => x.Name);

        var categories = await query.ProjectToType<Category>()
                                .ToListAsync(ct);

        return categories;
    }
}
