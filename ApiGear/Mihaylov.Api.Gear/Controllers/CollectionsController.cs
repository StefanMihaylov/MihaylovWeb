using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateBrand;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateCategory;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateGroup;
using Mihaylov.Api.Gear.Core.Application.Commands.CreateShop;
using Mihaylov.Api.Gear.Core.Application.Queries.GetBrands;
using Mihaylov.Api.Gear.Core.Application.Queries.GetCategories;
using Mihaylov.Api.Gear.Core.Application.Queries.GetCurrencies;
using Mihaylov.Api.Gear.Core.Application.Queries.GetGroups;
using Mihaylov.Api.Gear.Core.Application.Queries.GetShops;
using Mihaylov.Api.Gear.Extensions;
using Mihaylov.Common.Host.Authorization;

namespace Mihaylov.Api.Gear.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class CollectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Categories()
        {
            var categories = await _mediator.Send<IEnumerable<Category>>(new GetCategoriesQuery());

            return Ok(categories);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public async Task<IActionResult> Category(Category input)
        {
            var category = await _mediator.Send<Category>(new CreateCategoryCommand(input));

            return Ok(category);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Currency>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Currencies()
        {
            var currencies = await _mediator.Send<IEnumerable<Currency>>(new GetCurrenciesQuery());

            return Ok(currencies);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Brand>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Brands()
        {
            var brand = await _mediator.Send<IEnumerable<Brand>>(new GetBrandsQuery());

            return Ok(brand);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Brand), StatusCodes.Status200OK)]
        public async Task<IActionResult> Brand(Brand input)
        {
            var brand = await _mediator.Send<Brand>(new CreateBrandCommand(input));

            return Ok(brand);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Group>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Groups()
        {
            var groups = await _mediator.Send<IEnumerable<Group>>(new GetGroupsQuery());

            return Ok(groups);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
        public async Task<IActionResult> Group(Group input)
        {
            var group = await _mediator.Send<Group>(new CreateGroupCommand(input));

            return Ok(group);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<Shop>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Shops()
        {
            var shops = await _mediator.Send<IEnumerable<Shop>>(new GetShopsQuery());

            return Ok(shops);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Shop), StatusCodes.Status200OK)]
        public async Task<IActionResult> Shop(Shop input)
        {
            var shop = await _mediator.Send<Shop>(new CreateShopCommand(input));

            return Ok(shop);
        }
    }
}
