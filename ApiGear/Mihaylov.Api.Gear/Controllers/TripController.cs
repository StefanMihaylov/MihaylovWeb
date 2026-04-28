using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Gear.Core.Application.Commands.CloneTrip;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateGearNode;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateTrip;
using Mihaylov.Api.Gear.Core.Application.Commands.DeleteGearNode;
using Mihaylov.Api.Gear.Core.Application.Queries.GetTrips;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Extensions;
using Mihaylov.Api.Gear.Models;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Gear.Controllers;

[JwtAuthorize(Roles = UserConstants.AdminRole)]
[Route("api/[controller]/[action]")]
[ApiController]
[TypeFilter(typeof(ErrorFilter))]
[Produces("application/json")]
[ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
public class TripController : ControllerBase
{
    private readonly IMediator _mediator;

    public TripController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet()]
    [SwaggerOperation(OperationId = "TripList")]
    [ProducesResponseType(typeof(IEnumerable<Trip>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        var trips = await _mediator.Send<IEnumerable<Trip>>(new GetTripsQuery());

        return Ok(trips);
    }

    [HttpGet()]
    [SwaggerOperation(OperationId = "TripGet")]
    [ProducesResponseType(typeof(TripFull), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(long id, bool? notPacked)
    {
        var trip = await _mediator.Send<TripFull>(new GetTripQuery(id, notPacked));

        return Ok(trip);
    }

    [HttpPost()]
    [SwaggerOperation(OperationId = "TripAdd")]
    [ProducesResponseType(typeof(Trip), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddTripModel input)
    {
        var request = new Trip
        {
            Id = input.Id ?? 0,
            Title = input.Title ?? string.Empty,
            Year = input.Year ?? DateTime.UtcNow.Year,
            Notes = input.Notes,            
            CreatedAt = input.CreatedAt ?? DateTime.Now,
        };

        var trip = await _mediator.Send<Trip>(new CreateTripCommand(request));

        return Ok(trip);
    }

    [HttpPost()]
    [SwaggerOperation(OperationId = "TripNode")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> Node(AddGearNode input)
    {
        var request = new GearNode
        {
            Id = input.Id ?? 0,
            TripId = input.TripId ?? 0,
            ParentId = input.ParentId,
            NodeType = input.NodeType ?? NodeType.Item,
            GroupId = input.GroupId,
            CategoryId = input.CategoryId,
            InventoryItemId = input.InventoryItemId,
            Quantity = input.Quantity ?? 0,
            IsPacked = input.IsPacked ?? false,
            IsExcluded = input.IsExcluded ?? false,
            IsRequired = input.IsRequired ?? false
        };

        var trip = await _mediator.Send<long>(new CreateGearNodeCommand(request));

        return Ok(trip);
    }

    [HttpPut]
    [SwaggerOperation(OperationId = "TripNodePacked")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<IActionResult> NodePacked(long nodeId)
    {
        await _mediator.Send(new AlterGearNodeIsPackedCommand(nodeId));
        
        return Ok();
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "TripClone")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> Clone(long id)
    {
        var trip = await _mediator.Send<long>(new CloneTripCommand(id));
        return Ok(trip);
    }

    [HttpDelete]
    [SwaggerOperation(OperationId = "TripNodeDelete")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(long nodeId)
    {
        await _mediator.Send(new DeleteGearNodeCommand(nodeId)).ConfigureAwait(false);

        return Ok();
    }
}
