using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Mihaylov.Web.ViewModels.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers
{
    public class SiteController : BaseController
    {
        private readonly ISiteHelper siteHelper;
        private readonly IPersonsManager personManager;
        private readonly IPersonsWriter personsWriter;
        private readonly IUnitsManager unitManager;

        public SiteController(ISiteHelper siteHelper, IPersonsManager personManager, IPersonsWriter personsWriter,
            IUnitsManager unitManager, ILogger logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
            this.siteHelper = siteHelper;
            this.personManager = personManager;
            this.personsWriter = personsWriter;
            this.unitManager = unitManager;
        }

        // GET: Site
        public ActionResult Index()
        {
            var model = new PersonGridModel()
            {
                Statistics = this.personManager.GetStatictics(),
                Persons = this.personManager.GetAllPersons(true, 0, 10),
                SystemUnit = this.unitManager.GetAllUnits().FirstOrDefault(u => u.ConversionRate == 1.0m)?.Description,
            };

            return View(model);
        }

        public ActionResult Find(string url)
        {
            try
            {
                this.Logger.Debug($"Controller: hit find: {url}");

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
            catch (Exception ex)
            {
                return this.Json($"{url}: Error: {ex.Message}", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Save(Person input)
        {
            this.siteHelper.AddAdditionalInfo(input);
            this.personsWriter.Add(input);
            this.personManager.GetByName(input.Username);

            return this.RedirectToAction(nameof(SiteController.Index));
        }

        public ActionResult Update()
        {
            int updated = this.siteHelper.UpdatePersons();

            this.AddToastMessage(string.Empty, $"{updated} persons were updated", ToastType.Success);

            return this.RedirectToAction(nameof(SiteController.Index));
        }
    }
}