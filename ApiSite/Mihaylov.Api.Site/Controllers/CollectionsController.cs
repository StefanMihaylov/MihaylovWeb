using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;

namespace Mihaylov.Api.Site.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionsManager _manager;

        public CollectionsController(ICollectionsManager manager)
        {
            this._manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllCountries()
        {
            var countries = _manager.GetAllCountries();
            return Ok(countries);
        }

        [HttpGet]
        public IActionResult GetAllStates(int id)
        {
            var states = _manager.GetAllStatesByCountryId(id);
            return Ok(states);
        }

        [HttpGet]
        public IActionResult GetAllEthnicities()
        {
            var ethnicities = _manager.GetAllEthnicities();
            return Ok(ethnicities);
        }

        [HttpGet]
        public IActionResult GetAllOrientations()
        {
            var orientations = _manager.GetAllOrientations();
            return Ok(orientations);
        }

        [HttpGet]
        public IActionResult GetAllAccountTypes()
        {
            var accountTypes = _manager.GetAllAccountTypes();
            return Ok(accountTypes);
        }

        [HttpGet]
        public IActionResult GetAllUnits()
        {
            var units = _manager.GetAllUnits();
            return Ok(units);
        }
    }
}
