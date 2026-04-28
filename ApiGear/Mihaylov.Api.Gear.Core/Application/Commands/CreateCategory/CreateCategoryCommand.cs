using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mapster;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetCategories;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateCategory;

public record CreateCategoryCommand(Category Category) : IRequest<Category>;


public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateCategoryCommandHandler(IGearDbContext context, ILogger<CreateCategoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var model = request.Category;
        model.Name = model.Name.Trim();

        try
        {
            var dbModel = await _context.Categories
                            .Where(t => t.CategoryId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Lookups.Category();
                await _context.Categories.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.Adapt<Category>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Category. Error: {Message}", ex.Message);
            throw;
        }
    }
}
