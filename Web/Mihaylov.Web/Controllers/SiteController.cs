using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Core.Models;
using Mihaylov.Site.Data.Models;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Mihaylov.Web.Models.Site;

namespace Mihaylov.Web.Controllers
{
    [Authorize]
    public class SiteController : BaseController
    {
        private readonly ISiteHelper siteHelper;
        private readonly IPersonsManager personManager;
        private readonly IPersonsWriter personsWriter;
        private readonly IPhrasesManager phrasesManager;
        private readonly IPhrasesWriter phrasesWriter;

        public SiteController(ISiteHelper siteHelper, IPersonsManager personManager, IPersonsWriter personsWriter,
            IPhrasesManager phrasesManager, IPhrasesWriter phrasesWriter, ILoggerFactory logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
            this.siteHelper = siteHelper;
            this.personManager = personManager;
            this.personsWriter = personsWriter;
            this.phrasesManager = phrasesManager;
            this.phrasesWriter = phrasesWriter;
        }

        // GET: Site
        public async Task<IActionResult> Index()
        {
            var model = new PersonGridModel()
            {
                Statistics = await this.personManager.GetStaticticsAsync().ConfigureAwait(false),
                Persons = await this.personManager.GetAllPersonsAsync(true, 0, 10).ConfigureAwait(false),
                SystemUnit = this.siteHelper.GetSystemUnit(),
                Phrases = this.phrasesManager.GetAllPhrases(),
            };

            return View(model);
        }

        public async Task<IActionResult> Find(string url)
        {
            try
            {
                this.Logger.LogDebug($"Controller: hit find: {url}");

                string userName = this.siteHelper.GetUserName(url);
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return this.Json($"{url}: Error: Missing username");
                }

                PersonExtended personExtended = await this.siteHelper.GetPersonByNameAsync(userName).ConfigureAwait(false);
                return this.PartialView("_Find",personExtended);
            }
            catch (Exception ex)
            {
                return this.Json($"{url}: Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> AddPhrase(string phrase)
        {
            if (!string.IsNullOrWhiteSpace(phrase))
            {
                var newPhrase = new Phrase(0, phrase.Trim(), null);
                await this.phrasesWriter.AddOrUpdateAsync(newPhrase).ConfigureAwait(false);
            }

            return this.RedirectToAction(nameof(SiteController.Index));
        }

        public async Task<IActionResult> Save(Person input)
        {
            this.siteHelper.AddAdditionalInfo(input);
            await this.personsWriter.AddOrUpdateAsync(input).ConfigureAwait(false);

            return this.RedirectToAction(nameof(SiteController.Index));
        }

        public async Task<IActionResult> Update()
        {
            int updated = await this.siteHelper.UpdatePersonsAsync().ConfigureAwait(false);

            this.AddToastMessage(string.Empty, $"{updated} persons were updated", ToastType.Success);

            return this.RedirectToAction(nameof(SiteController.Index));
        }
    }
}