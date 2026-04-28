using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateInventory;
using Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;
using Mihaylov.Api.Gear.Core.Domain.Enums;
using Mihaylov.Api.Gear.Extensions;
using Mihaylov.Api.Gear.Models;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Gear.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [SwaggerOperation(OperationId = "InventoryList")]
        [ProducesResponseType(typeof(IEnumerable<Inventory>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(bool? isActive)
        {
            var inventories = await _mediator.Send<IEnumerable<Inventory>>(new GetInventoriesQuery(isActive));

            return Ok(inventories);
        }

        [HttpPost()]
        [SwaggerOperation(OperationId = "InventoryAdd")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AddInventoryModel input)
        {
            var request = new Inventory
            {
                Id = input.Id,
                Name = input.Name ?? string.Empty,
                Description = input.Description,
                BrandId = input.BrandId,
                ShopId = input.ShopId,
                CategoryId = input.CategoryId ?? 0,
                Price = input.Price,
                CurrencyId = input.CurrencyId,
                PurchaseDate = input.PurchaseDate,
                ItemStatus = input.ItemStatus ?? ItemStatus.Active,
                KitContents = input.KitContents
            };

            var inventoryId = await _mediator.Send<long>(new CreateInventoryCommand(request));

            return Ok(inventoryId);
        }
    }
}
