using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Gear.Core.Application.Interfaces;
using Mihaylov.Api.Gear.Core.Application.Queries.GetGroups;

namespace Mihaylov.Api.Gear.Core.Application.Commands.CreateGroup;

public record CreateGroupCommand(Group Group) : IRequest<Group>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Group>
{
    private readonly IGearDbContext _context;
    private readonly ILogger _logger;

    public CreateGroupCommandHandler(IGearDbContext context, ILogger<CreateGroupCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var model = request.Group;
        model.Name = model.Name.Trim();

        try
        {
            var dbModel = await _context.Groups
                            .Where(t => t.GroupId == model.Id)
                            .FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (dbModel == null)
            {
                dbModel = new Domain.Lookups.Group();
                await _context.Groups.AddAsync(dbModel).ConfigureAwait(false);
            }

            dbModel = model.Adapt(dbModel);

            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return dbModel.Adapt<Group>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in add/update Group. Error: {Message}", ex.Message);
            throw;
        }
    }
}
