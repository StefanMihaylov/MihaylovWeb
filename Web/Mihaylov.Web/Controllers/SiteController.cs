using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Client;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Site;

namespace Mihaylov.Web.Controllers
{
    public class SiteController : Controller
    {
        public const string NAME = "Site";
        private readonly ISiteApiClient _client;

        public SiteController(ISiteApiClient siteApiClient)
        {
            _client = siteApiClient;
        }

        public async Task<IActionResult> Index([FromQuery] SiteFilterModel model)
        {
            if (!model.Page.HasValue || model.Page <= 0)
            {
                model.Page = 1;
                return Redirect($"/{NAME}/{nameof(Index)}{model.ToQueryString()}");
            }

            _client.AddToken(Request.GetToken());
            PersonGrid grid = await _client.PersonsAsync(null, null, null, null, model.Page, 10).ConfigureAwait(false);
            PersonStatistics statistics = await _client.StatisticsAsync().ConfigureAwait(false);

            IEnumerable<QuizPhrase> quizPhrases = await _client.PhrasesAsync().ConfigureAwait(false);
            IEnumerable<QuizQuestion> quizQuestions = await _client.QuestionsAsync().ConfigureAwait(false);

            if (grid.Pager.PageMax > 0 && model.Page >= grid.Pager.PageMax)
            {
                model.Page = grid.Pager.PageMax - 1;
                return Redirect($"/{NAME}/{nameof(Index)}{model.ToQueryString()}");
            }

            PersonExtended personExtended = null;
            if (model.PersonId.HasValue)
            {
                personExtended = await GetPersonExtended(model.PersonId.Value).ConfigureAwait(false);
            }

            var main = new SiteMainModel()
            {
                Grid = grid,
                Person = personExtended,
                Statistics = statistics,
                AdminData = new SiteAdminModel()
                {
                    Questions = quizQuestions,
                    QuizPhrases = quizPhrases,
                }
            };

            return View(main);
        }

        //public ActionResult Find(string url)
        //{
        //    try
        //    {
        //        this.Logger.Debug($"Controller: hit find: {url}");

        //        string userName = this.siteHelper.GetUserName(url);
        //        if (string.IsNullOrWhiteSpace(userName))
        //        {
        //            return this.Json($"{url}: Error: Missing username", JsonRequestBehavior.AllowGet);
        //        }

        //        PersonExtended personExtended = this.siteHelper.GetPersonByName(userName);
        //        return this.PartialView("_Find", personExtended);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.Json($"{url}: Error: {ex.Message}", JsonRequestBehavior.AllowGet);
        //    }
        //}

        public async Task<IActionResult> PersonView(long id)
        {
            _client.AddToken(Request.GetToken());
            PersonExtended result = await GetPersonExtended(id).ConfigureAwait(false);

            return PartialView("_AddPerson", result);
        }

        public async Task<IActionResult> SavePerson(PersonModel model)
        {
            var dateOfBirthTuple = GetDateOfBirth(model.Age, model.DateOfBirth, model.DateOfBirthType);

            var request = new AddPersonModel()
            {
                Id = model.Id,
                Details = model.Details == null ? null : new AddPersonDetailModel()
                {
                    FirstName = model.Details.FirstName,
                    MiddleName = model.Details.MiddleName,
                    LastName = model.Details.LastName,
                    OtherNames = model.Details.OtherNames,
                },
                DateOfBirth = dateOfBirthTuple.Item1,
                DateOfBirthType = dateOfBirthTuple.Item2,
                CountryId = model.CountryId,
                Location = model.Location == null ? null : new AddPersonLocationModel()
                {
                    CountryStateId = model.Location.CountryStateId,
                    Region = model.Location.Region,
                    City = model.Location.City,
                    Details = model.Location.Details,
                },
                EthnicityId = model.EthnicityId,
                OrientationId = model.OrientationId,
                Comments = model.Comments,
            };

            _client.AddToken(Request.GetToken());
            var person = await _client.AddPersonAsync(request).ConfigureAwait(false);

            return Redirect($"/{NAME}/{nameof(Index)}");
        }

        public async Task<IActionResult> AccountView(long id)
        {
            _client.AddToken(Request.GetToken());
            IEnumerable<QuizAnswer> account = await _client.AnswersAsync(id).ConfigureAwait(false);

            return PartialView("_AddAccount", account);
        }

        public async Task<IActionResult> AnswerView(AnswerViewModel input)
        {
            _client.AddToken(Request.GetToken());
            AnswerExtended model = await GetAnswerExtended(input).ConfigureAwait(false);

            return PartialView("_AddAnswer", model);
        }

        public async Task<IActionResult> SaveAnswer(QuizAnswerModel model)
        {


            var request = new AddQuizAnswerModel()
            {
                Id = model.Id,
                PersonId = model.PersonId.Value,
                AskDate = model.AskDate ?? DateTime.Now,
                QuestionId = model.QuestionId.Value,
                Value = (double?)model.Value,
                UnitId = model.UnitId,
                HalfTypeId = model.HalfTypeId,
                Details = model.Details,
            };

            _client.AddToken(Request.GetToken());
            QuizAnswer answer = await _client.AddAnswerAsync(request).ConfigureAwait(false);

            var query = new SiteFilterModel()
            {
                Page = 1,
                PersonId = model.PersonId,
            };

            return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
        }

        public async Task<IActionResult> QuestionView(int? id)
        {
            var model = new QuizQuestion();

            if (id.HasValue)
            {
                _client.AddToken(Request.GetToken());
                model = await _client.QuestionAsync(id).ConfigureAwait(false);
            }

            return PartialView("_AddQuestion", model);
        }

        public async Task<IActionResult> SaveQuestion(QuizQuestionModel model)
        {
            var request = new AddQuizQuestionModel()
            {
                Id = model.Id,
                Value = model.Value,
            };

            _client.AddToken(Request.GetToken());
            QuizQuestion question = await _client.AddQuestionAsync(request).ConfigureAwait(false);

            return Redirect($"/{NAME}/{nameof(Index)}");
        }


        private async Task<PersonExtended> GetPersonExtended(long id)
        {
            Person person = await _client.PersonAsync(id).ConfigureAwait(false);

            var countries = await _client.CountriesAsync().ConfigureAwait(false);
            var countryStates = await _client.CountryStatesAsync(person.CountryId).ConfigureAwait(false);
            var ethnicities = await _client.EthnicitiesAsync().ConfigureAwait(false);
            var orientations = await _client.OrientationsAsync().ConfigureAwait(false);

            //var accountTypes = await _client.AccountTypesAsync().ConfigureAwait(false);
            //var accountStates = await _client.AccountStatesAsync().ConfigureAwait(false);

            IEnumerable<QuizAnswer> answers = new List<QuizAnswer>();
            if (person.AnswersCount > 0)
            {
                answers = await _client.AnswersAsync(person.Id).ConfigureAwait(false);
            }

            var result = new PersonExtended(person)
            {
                AnswersExtended = new AnswersExtended()
                {
                    PersonId = person.Id,
                    Answers = answers
                },

                Countries = countries,
                CountryStates = countryStates,
                Ethnicities = ethnicities,
                Orientations = orientations,
            };

            return result;
        }

        private async Task<AnswerExtended> GetAnswerExtended(AnswerViewModel input)
        {
            var answer = new QuizAnswer()
            {
                PersonId = input.PersonId.Value,
                AskDate = DateTime.UtcNow,
            };

            if (input.Id.HasValue)
            {
                answer = await _client.AnswerAsync(input.Id).ConfigureAwait(false);
            }

            var questions = await _client.QuestionsAsync().ConfigureAwait(false);
            var units = await _client.UnitsAsync().ConfigureAwait(false);
            var halfTypes = await _client.HalfTypesAsync().ConfigureAwait(false);

            var model = new AnswerExtended(answer)
            {
                Questions = questions,
                Units = units,
                HalfTypes = halfTypes
            };

            return model;
        }

        private (DateTime?, DateOfBirthType?) GetDateOfBirth(int? age, DateTime? date, DateOfBirthType? type)
        {
            if (type == null)
            {
                return (null, null);
            }
            else if (type == DateOfBirthType.YearCalculated)
            {
                if (date.HasValue)
                {
                    return (date, type);
                }
                else
                {
                    if (age.HasValue)
                    {
                        return (age.Value.GetBirthDate(), type);
                    }
                    else
                    {
                        return (null, null);
                    }
                }
            }
            else
            {
                if (date.HasValue)
                {
                    return (date, type);
                }
                else
                {
                    return (null, null);
                }
            }
        }
    }
}
