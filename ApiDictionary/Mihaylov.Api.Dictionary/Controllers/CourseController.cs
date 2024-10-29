using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Dictionary.Contracts.Managers;
using Mihaylov.Api.Dictionary.Contracts.Models;

namespace Mihaylov.Api.Dictionary.Controllers
{
    // [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class CourseController : ControllerBase
    {
        private readonly ICourseManager _manager;

        public CourseController(ICourseManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Course>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Courses()
        {
            IEnumerable<Course> courses = await _manager.GetAllCoursesAsync().ConfigureAwait(false);

            return Ok(courses);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LearningSystem>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LearningSystems()
        {
            IEnumerable<LearningSystem> LearningSystems = await _manager.GetAllLearningSystemsAsync().ConfigureAwait(false);

            return Ok(LearningSystems);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Level>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Levels()
        {
            IEnumerable<Level> levels = await _manager.GetAllLevelsAsync().ConfigureAwait(false);

            return Ok(levels);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Language>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Languages()
        {
            IEnumerable<Language> languages = await _manager.GetAllLanguagesAsync().ConfigureAwait(false);

            return Ok(languages);
        }
    }
}
