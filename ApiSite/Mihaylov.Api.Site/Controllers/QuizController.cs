using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Extensions;
using Mihaylov.Api.Site.Models;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mihaylov.Api.Site.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class QuizController : ControllerBase
    {
        private readonly IQuizManager _manager;
        private readonly IQuizWriter _writer;

        public QuizController(IQuizManager quizManager, IQuizWriter quizWriter)
        {
            _manager = quizManager;
            _writer = quizWriter;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuizQuestion>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Questions()
        {
            IEnumerable<QuizQuestion> questions = await _manager.GetAllQuestionsAsync().ConfigureAwait(false);

            return Ok(questions);
        }

        [HttpGet]
        [ProducesResponseType(typeof(QuizQuestion), StatusCodes.Status200OK)]
        public async Task<IActionResult> Question(int id)
        {
            QuizQuestion question = await _manager.GetQuestionAsync(id).ConfigureAwait(false);

            return Ok(question);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddQuestion")]
        [ProducesResponseType(typeof(QuizQuestion), StatusCodes.Status200OK)]
        public async Task<IActionResult> Question(AddQuizQuestionModel input)
        {
            var model = new QuizQuestion()
            {
                Id = input.Id ?? 0,
                Value = input.Value,
            };

            QuizQuestion question = await _writer.AddOrUpdateQuestionAsync(model).ConfigureAwait(false);

            return Ok(question);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuizPhrase>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Phrases()
        {
            IEnumerable<QuizPhrase> phrases = await _manager.GetAllPhrasesAsync().ConfigureAwait(false);

            return Ok(phrases);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddPhrase")]
        [ProducesResponseType(typeof(QuizPhrase), StatusCodes.Status200OK)]
        public async Task<IActionResult> Phrase(AddQuizPhraseModel input)
        {
            var model = new QuizPhrase()
            {
                Id = input.Id ?? 0,
                Text = input.Text,
                OrderId = input.OrderId,
            };

            QuizPhrase phrase = await _writer.AddOrUpdatePhraseAsync(model).ConfigureAwait(false);

            return Ok(phrase);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HalfType>), StatusCodes.Status200OK)]
        public async Task<IActionResult> HalfTypes()
        {
            IEnumerable<HalfType> halfTypes = await _manager.GetAllHalfTypesAsync().ConfigureAwait(false);

            return Ok(halfTypes);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnitShort>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Units()
        {
            IEnumerable<UnitShort> units = await _manager.GetAllUnitsAsync().ConfigureAwait(false);

            return Ok(units);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuizAnswer>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Answers(long personId)
        {
            IEnumerable<QuizAnswer> answers = await _manager.GetQuizAnswersAsync(personId).ConfigureAwait(false);

            return Ok(answers);
        }

        [HttpGet]
        [ProducesResponseType(typeof(QuizAnswer), StatusCodes.Status200OK)]
        public async Task<IActionResult> Answer(long id)
        {
            QuizAnswer answer = await _manager.GetQuizAnswerAsync(id).ConfigureAwait(false);

            return Ok(answer);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddAnswer")]
        [ProducesResponseType(typeof(QuizAnswer), StatusCodes.Status200OK)]
        public async Task<IActionResult> Answer(AddQuizAnswerModel input)
        {
            var model = new QuizAnswer()
            {
                Id = input.Id ?? 0,
                PersonId = input.PersonId.Value,
                QuestionId = input.QuestionId.Value,
                Question = null,
                AskDate = input.AskDate.Value,
                Value = input.Value,
                UnitId = input.UnitId,
                Unit = null,
                ConvertedValue = null,
                ConvertedUnit = null,
                HalfTypeId = input.HalfTypeId,
                HalfType = null,
                Details = input.Details,
            };

            QuizAnswer answer = await _writer.AddOrUpdateAnswersAsync(model).ConfigureAwait(false);

            return Ok(answer);
        }
    }
}
