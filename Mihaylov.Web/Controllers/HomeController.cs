using System.Web.Mvc;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}