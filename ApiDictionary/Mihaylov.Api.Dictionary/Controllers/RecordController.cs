using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Dictionary.Contracts.Managers;
using Mihaylov.Api.Dictionary.Contracts.Models;
using Mihaylov.Api.Dictionary.Contracts.Writers;
using Mihaylov.Api.Dictionary.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Dictionary.Controllers
{
    // [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class RecordController : ControllerBase
    {
        private readonly IRecordManager _manager;
        private readonly IRecordWriter _writer;

        public RecordController(IRecordManager manager, IRecordWriter writer)
        {
            _manager = manager;
            _writer = writer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Record>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Records([FromQuery] RecordSearchModel searchModel)
        {
            IEnumerable<Record> records = await _manager.GetRecordsAsync(searchModel).ConfigureAwait(false);

            return Ok(records);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddRecord")]
        [ProducesResponseType(typeof(Record), StatusCodes.Status200OK)]
        public async Task<IActionResult> Record(AddRecordModel input)
        {
            var model = new Record()
            {
                Id = input.Id ?? 0,
                CourseId = input.CourseId.Value,
                ModuleNumber = input.ModuleNumber,
                RecordTypeId = input.RecordTypeId,
                RecordType = null,
                Original = input.Original,
                Translation = input.Translation,
                Comment = input.Comment,
                PrepositionId = input.PrepositionId,
                Preposition = null,
            };

            Record record = await _writer.AddRecordAsync(model).ConfigureAwait(false);

            return Ok(record);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecordType>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RecordTypes()
        {
            IEnumerable<RecordType> recordTypes = await _manager.GetAllRecordTypesAsync().ConfigureAwait(false);

            return Ok(recordTypes);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Preposition>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Prepositions()
        {
            IEnumerable<Preposition> prepositions = await _manager.GetAllPrepositionsAsync().ConfigureAwait(false);

            return Ok(prepositions);
        }
    }
}
