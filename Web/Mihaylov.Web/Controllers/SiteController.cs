using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Core.Models;
using Mihaylov.Site.Data.Models;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Mihaylov.Web.Models.Site;
using Ninject.Extensions.Logging;

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
            IPhrasesManager phrasesManager, IPhrasesWriter phrasesWriter, ILogger logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
            this.siteHelper = siteHelper;
            this.personManager = personManager;
            this.personsWriter = personsWriter;
            this.phrasesManager = phrasesManager;
            this.phrasesWriter = phrasesWriter;
        }

        // GET: Site
        public IActionResult Index()
        {
            var model = new PersonGridModel()
            {
                Statistics = this.personManager.GetStatictics(),
                Persons = this.personManager.GetAllPersons(true, 0, 10),
                SystemUnit = this.siteHelper.GetSystemUnit(),
                Phrases = this.phrasesManager.GetAllPhrases(),
            };

            return View(model);
        }

        public IActionResult Find(string url)
        {
            try
            {
                this.Logger.Debug($"Controller: hit find: {url}");

                string userName = this.siteHelper.GetUserName(url);
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return this.Json($"{url}: Error: Missing username");
                }

                PersonExtended personExtended = this.siteHelper.GetPersonByName(userName);
                return this.PartialView("_Find",personExtended);
            }
            catch (Exception ex)
            {
                return this.Json($"{url}: Error: {ex.Message}");
            }
        }

        public IActionResult AddPhrase(string phrase)
        {
            if (!string.IsNullOrWhiteSpace(phrase))
            {
                var newPhrase = new Phrase(0, phrase.Trim(), null);
                this.phrasesWriter.AddOrUpdate(newPhrase);
            }

            return this.RedirectToAction(nameof(SiteController.Index));
        }

        public IActionResult Save(Person input)
        {
            this.siteHelper.AddAdditionalInfo(input);
            this.personsWriter.AddOrUpdate(input);

            return this.RedirectToAction(nameof(SiteController.Index));
        }

        public IActionResult Update()
        {
            int updated = this.siteHelper.UpdatePersons();

            this.AddToastMessage(string.Empty, $"{updated} persons were updated", ToastType.Success);

            return this.RedirectToAction(nameof(SiteController.Index));
        }
    }
}