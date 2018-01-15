using System;
using System.Web.Mvc;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Mihaylov.Web.ViewModels.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers
{
    [Authorize]
    public class SiteController : BaseController
    {
        private readonly ISiteHelper siteHelper;
        private readonly IPersonsManager personManager;
        private readonly IPersonsWriter personsWriter;

        public SiteController(ISiteHelper siteHelper, IPersonsManager personManager, IPersonsWriter personsWriter,
            ILogger logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
            this.siteHelper = siteHelper;
            this.personManager = personManager;
            this.personsWriter = personsWriter;
        }

        // GET: Site
        public ActionResult Index()
        {
            var model = new PersonGridModel()
            {
                Statistics = this.personManager.GetStatictics(),
                Persons = this.personManager.GetAllPersons(true, 0, 10),
                SystemUnit = this.siteHelper.GetSystemUnit(),
            };

            return View(model);
        }

        public ActionResult Find(string url)
        {
            try
            {
                this.Logger.Debug($"Controller: hit find: {url}");

                string userName = this.siteHelper.GetUserName(url);
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return this.Json($"{url}: Error: Missing username", JsonRequestBehavior.AllowGet);
                }

                Person person = this.personManager.GetByName(userName);
                if (person == null)
                {
                    person = this.siteHelper.GetUserInfo(userName);
                }

                var personExtended = new PersonExtended(person)
                {
                    AnswerUnits = this.siteHelper.GetAllUnits(),
                    AnswerTypes = this.siteHelper.GetAllAnswerTypes()
                };

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
            this.personsWriter.AddOrUpdate(input);

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