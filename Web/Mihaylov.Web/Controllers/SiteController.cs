using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Client;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Site;

namespace Mihaylov.Web.Controllers
{
    public class SiteController : Controller
    {
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
                return Redirect($"/Site/index{model.ToQueryString()}");
            }

            _client.AddToken(Request.GetToken());
            PersonGrid grid = await _client.PersonsAsync(null, null, null, null, model.Page, 10).ConfigureAwait(false);
            PersonStatistics statistics = await _client.StatisticsAsync().ConfigureAwait(false);
            IEnumerable<QuizPhrase> quizPhrases = await _client.PhrasesAsync().ConfigureAwait(false);

            if (grid.Pager.PageMax > 0 && model.Page >= grid.Pager.PageMax)
            {
                model.Page = grid.Pager.PageMax - 1;
                return Redirect($"/Site/index{model.ToQueryString()}");
            }

            var main = new SiteMainModel()
            {
                Grid = grid,
                Statistics = statistics,
                QuizPhrases = quizPhrases,
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

        public async Task<IActionResult> Edit(long id)
        {
            _client.AddToken(Request.GetToken());
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
                Answers = answers,

                Countries = countries,
                CountryStates = countryStates,
                Ethnicities = ethnicities,
                Orientations = orientations,
            };

            return PartialView("_AddPerson", result);
        }

        public async Task<IActionResult> Account(long id)
        {
            _client.AddToken(Request.GetToken());
            IEnumerable<QuizAnswer> account = await _client.AnswersAsync(id).ConfigureAwait(false);

            return PartialView("_AddAccount", account);
        }

        public async Task<IActionResult> AnswerView(long? id, long? personId)
        {
            _client.AddToken(Request.GetToken());
            QuizAnswer answer = await _client.AnswerAsync(id).ConfigureAwait(false);

            var questions = await _client.QuestionsAsync().ConfigureAwait(false);
            var units = await _client.UnitsAsync().ConfigureAwait(false);
            var halfTypes = await _client.HalfTypesAsync().ConfigureAwait(false);

            var model = new AnswerExtended(answer)
            {
                Questions = questions,
                Units = units,
                HalfTypes = halfTypes
            };

            return PartialView("_AddAnswer", model);
        }

        public async Task<IActionResult> SaveAnswer(AnswerModel model)
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

            return RedirectToAction(nameof(Index));
        }
    }
}
