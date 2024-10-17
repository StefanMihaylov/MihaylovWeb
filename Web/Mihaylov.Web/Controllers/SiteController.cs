using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Client;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Areas.Identity.Pages.Account;
using Mihaylov.Web.Models.Configs;
using Mihaylov.Web.Models.Site;

namespace Mihaylov.Web.Controllers
{
    public class SiteController : Controller
    {
        public const string NAME = "Site";
        
        private readonly ISiteApiClient _client;
        private readonly SiteConfig _settings;

        public SiteController(ISiteApiClient siteApiClient, IOptions<SiteConfig> siteSettings)
        {
            _client = siteApiClient;
            _settings = siteSettings.Value;
        }

        public async Task<IActionResult> Index([FromQuery] SiteFilterModel model)
        {
            _client.AddToken(Request.GetToken());
            PersonGrid grid = await _client.PersonsAsync(model, 10).ConfigureAwait(false);
            PersonStatistics statistics = await _client.StatisticsAsync().ConfigureAwait(false);

            IEnumerable<QuizPhrase> quizPhrases = await _client.PhrasesAsync().ConfigureAwait(false);
            IEnumerable<QuizQuestion> quizQuestions = await _client.QuestionsAsync().ConfigureAwait(false);

            PersonExtended personExtended = null;
            NewPersonViewModel newPersonFilter = null;
            if (model.PersonId.HasValue)
            {
                personExtended = await GetPersonExtended(model.PersonId.Value).ConfigureAwait(false);
            }
            else if (model.IsNewPerson == true)
            {
                var request = new NewPersonModel()
                {
                    AccountTypeId = model.AccountTypeId,
                    Username = model.AccountName,
                    IsPreview = true,
                };

                Person newPerson = await _client.NewPersonAsync(request).ConfigureAwait(false);
                personExtended = await GetPersonExtended(newPerson).ConfigureAwait(false);

                var accountTypes = await _client.AccountTypesAsync().ConfigureAwait(false);

                newPersonFilter = new NewPersonViewModel()
                {
                    AccountTypeId = model.AccountTypeId,
                    AccountName = model.AccountName,
                    IsPreview = false,
                    AccountTypes = accountTypes,
                };
            }

            await FilterExtend(model).ConfigureAwait(false);

            var main = new SiteMainModel()
            {
                Filter = model,
                NewPersonFilter = newPersonFilter,
                Person = personExtended,
                Grid = grid,
                Statistics = statistics,
                AdminData = new SiteAdminModel()
                {
                    Questions = quizQuestions,
                    QuizPhrases = quizPhrases,
                },
                OtherTabModel = new OtherTabModel()
                {
                    ApiUrl = _settings.SiteApiBaseUrl.TrimEnd('/'),
                    TokenName = LoginModel.COOKIE_NAME,
                }
            };

            return View(main);
        }

        public IActionResult Search(SiteFilterModel query)
        {
            return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
        }

        public async Task<IActionResult> NewPersonView()
        {
            _client.AddToken(Request.GetToken());
            var accountTypes = await _client.AccountTypesAsync().ConfigureAwait(false);

            var model = new NewPersonViewModel()
            {
                AccountTypeId = 3,
                AccountName = null,
                IsPreview = true,
                AccountTypes = accountTypes,
            };

            return PartialView("_AddNewPerson", model);
        }

        [HttpPost]
        public async Task<IActionResult> NewPerson(NewPersonViewModel input)
        {
            var query = new SiteFilterModel()
            {
                Page = null,
                AccountTypeId = input.AccountTypeId,
                AccountNameExact = input.AccountName,
                AccountName = null,
                StatusId = null,
                Name = null,
                PersonId = null,
                IsNewPerson = null,
            };

            _client.AddToken(Request.GetToken());

            if (input.IsPreview == false)
            {
                var request = new NewPersonModel()
                {
                    AccountTypeId = input.AccountTypeId,
                    Username = input.AccountName,
                    IsPreview = input.IsPreview,
                };

                Person newPerson = await _client.NewPersonAsync(request).ConfigureAwait(false);

                query.PersonId = newPerson.Id;
                query.AccountName = query.AccountNameExact;
                query.AccountNameExact = null;

                return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
            }

            PersonGrid grid = await _client.PersonsAsync(query, null).ConfigureAwait(false);

            query.AccountName = query.AccountNameExact;

            if (grid.Data.Count() == 1)
            {
                query.PersonId = grid.Data.First().Id;

                return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
            }

            query.IsNewPerson = true;
            return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
        }

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
                CreatedOn = model.CreatedOn,
            };

            _client.AddToken(Request.GetToken());
            var person = await _client.AddPersonAsync(request).ConfigureAwait(false);

            return Redirect($"/{NAME}/{nameof(Index)}");
        }

        public async Task<IActionResult> AccountView(AccountViewModel input)
        {
            _client.AddToken(Request.GetToken());
            AccountExtended model = await GetAccountExtended(input).ConfigureAwait(false);

            return PartialView("_AddAccount", model);
        }

        public async Task<IActionResult> SaveAccount(AccountModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorQuery = new SiteFilterModel()
                {
                    Page = 1,
                    PersonId = model.PersonId,
                };

                return Redirect($"/{NAME}/{nameof(Index)}{errorQuery.ToQueryString()}");
            }

            var request = new AddAccountModel()
            {
                Id = model.Id,
                PersonId = model.PersonId,
                AccountTypeId = model.AccountTypeId.Value,
                StatusId = model.StatusId,
                Username = model.Username,
                DisplayName = model.DisplayName,
                AskDate = model.AskDate.Value,
                CreateDate = model.CreateDate,
                LastOnlineDate = model.LastOnlineDate,
                ReconciledDate = model.ReconciledDate,
                Details = model.Details,
            };

            _client.AddToken(Request.GetToken());
            Account account = await _client.AddAccountAsync(request).ConfigureAwait(false);

            var query = new SiteFilterModel()
            {
                Page = 1,
                PersonId = model.PersonId,
            };

            return Redirect($"/{NAME}/{nameof(Index)}{query.ToQueryString()}");
        }

        public async Task<IActionResult> AnswerView(AnswerViewModel input)
        {
            _client.AddToken(Request.GetToken());
            AnswerExtended model = await GetAnswerExtended(input).ConfigureAwait(false);

            return PartialView("_AddAnswer", model);
        }

        public async Task<IActionResult> SaveAnswer(QuizAnswerModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorQuery = new SiteFilterModel()
                {
                    Page = 1,
                    PersonId = model.PersonId,
                };

                return Redirect($"/{NAME}/{nameof(Index)}{errorQuery.ToQueryString()}");
            }

            var request = new AddQuizAnswerModel()
            {
                Id = model.Id,
                PersonId = model.PersonId.Value,
                AskDate = model.AskDate ?? DateTime.UtcNow,
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

        public async Task<IActionResult> MergeView([FromQuery] MergeRequestModel input)
        {
            _client.AddToken(Request.GetToken());
            Person personFrom = await _client.PersonAsync(input.From.Value).ConfigureAwait(false);
            Person personTo = await _client.PersonAsync(input.To.Value).ConfigureAwait(false);

            var result = new MergeViewModel()
            {
                PersonFrom = personFrom,
                PersonTo = personTo,
            };

            return View("MergePerson", result);
        }

        public async Task<IActionResult> SaveMerge([FromForm] MergeModel input)
        {
            var request = new PersonMerge()
            {
                From = input.From,
                To = input.To,
            };

            foreach (var check in input.Checks)
            {
                switch (check)
                {
                    case 1: request.FirstName = true; break;
                    case 2: request.MiddleName = true; break;
                    case 3: request.LastName = true; break;
                    case 4: request.OtherNames = true; break;
                    case 5: request.EthnicityId = true; break;
                    case 6: request.OrientationId = true; break;
                    case 7: request.Comments = true; break;
                    case 8: request.DateOfBirth = true; break;
                    case 9: request.DateOfBirthType = true; break;
                    case 10: request.CountryId = true; break;
                    case 11: request.CountryStateId = true; break;
                    case 12: request.Region = true; break;
                    case 13: request.City = true; break;
                    case 14: request.Details = true; break;
                    case 15: request.Accounts = true; break;
                    case 16: request.Answers = true; break;
                    default:
                        throw new ArgumentException($"Unknown checkbox {check}");
                }
            }

            _client.AddToken(Request.GetToken());
            Person person = await _client.MergePersonAsync(request).ConfigureAwait(false);

            return Redirect($"/{NAME}/{nameof(Index)}");
        }


        private async Task FilterExtend(SiteFilterModel model)
        {
            var accountTypes = await _client.AccountTypesAsync().ConfigureAwait(false);
            var accountStates = await _client.AccountStatesAsync().ConfigureAwait(false);

            model.AccountTypes = accountTypes;
            model.AccountStates = accountStates;
        }

        private async Task<PersonExtended> GetPersonExtended(long id)
        {
            Person person = await _client.PersonAsync(id).ConfigureAwait(false);

            PersonExtended result = await GetPersonExtended(person).ConfigureAwait(false);

            return result;
        }

        private async Task<PersonExtended> GetPersonExtended(Person person)
        {
            var countries = await _client.CountriesAsync().ConfigureAwait(false);
            var countryStates = await _client.CountryStatesAsync(person.CountryId).ConfigureAwait(false);
            var ethnicities = await _client.EthnicitiesAsync().ConfigureAwait(false);
            var orientations = await _client.OrientationsAsync().ConfigureAwait(false);

            IEnumerable<QuizAnswer> answers = new List<QuizAnswer>();
            if (person.AnswersCount > 0)
            {
                answers = await _client.AnswersAsync(person.Id).ConfigureAwait(false);
            }

            var result = new PersonExtended(person)
            {
                AnswersExtended = new AnswersExtended(person.Id, answers),
                Countries = countries,
                CountryStates = countryStates,
                Ethnicities = ethnicities,
                Orientations = orientations,
            };
            return result;
        }

        private async Task<AccountExtended> GetAccountExtended(AccountViewModel input)
        {
            Account account = new Account()
            {
                PersonId = input.PersonId.Value,
                AskDate = DateTime.UtcNow,
            };

            if (input.Id.HasValue)
            {
                account = await _client.AccountAsync(input.Id).ConfigureAwait(false);
            }

            var accountTypes = await _client.AccountTypesAsync().ConfigureAwait(false);
            var accountStates = await _client.AccountStatesAsync().ConfigureAwait(false);

            var model = new AccountExtended(account)
            {
                AccountTypes = accountTypes,
                AccountStates = accountStates,
            };
            return model;
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
            else
            {
                var answers = await _client.AnswersAsync(answer.PersonId).ConfigureAwait(false);
                if (answers.Any())
                {
                    var lastAskDate = answers.OrderByDescending(a => a.AskDate).First().AskDate;
                    answer.AskDate = lastAskDate.AddSeconds(1);
                }
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
