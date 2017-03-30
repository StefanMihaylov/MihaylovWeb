using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Web.ViewModels.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers
{
    public class SiteController : Controller
    {
        private readonly ISiteHelper siteHelper;
        private readonly IPersonsManager personManager;
        private readonly IPersonsWriter personsWriter;
        private readonly ILogger logger;

        public SiteController(ISiteHelper siteHelper, IPersonsManager personManager, IPersonsWriter personsWriter, ILogger logger)
        {
            this.siteHelper = siteHelper;
            this.personManager = personManager;
            this.personsWriter = personsWriter;
            this.logger = logger;
        }

        // GET: Site
        public ActionResult Index()
        {
            PersonStatistics statistics = this.personManager.GetStatictics();

            return View(statistics);
        }

        public ActionResult Find(string url)
        {
            this.logger.Debug($"Controller: hit find: {url}");

            string userName = this.siteHelper.GetUserName(url);

            Person person = this.personManager.GetByName(userName);
            if (person == null)
            {
                person = this.siteHelper.GetUserInfo(userName);
            }

            PersonExtended personExtended = new PersonExtended(person);
            personExtended.AnswerUnits = this.siteHelper.GetAllUnits();
            personExtended.AnswerTypes = this.siteHelper.GetAllAnswerTypes();

            return this.PartialView(personExtended);
        }

        public ActionResult Save(Person input)
        {
            this.siteHelper.AddAdditionalInfo(input);
            this.personsWriter.Add(input);
            return this.RedirectToAction("Index");
        }
    }
}