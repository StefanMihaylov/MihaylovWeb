using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mihaylov.GoogleDrive;
using Mihaylov.GoogleDrive.Interfaces;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers
{
    public class HomeController : BaseController
    {
      //  private IGoogleDriveApiHelper googleDrive;
 
        public HomeController(ILogger logger, IToastrHelper toaster)//, IGoogleDriveApiHelper google)
            : base(logger, toaster)
        {
           // this.googleDrive = google;
        }

        public ActionResult Index()
        {
            this.AddToastMessage("Congratulations", "You made it all the way here!", ToastType.Success);
          
            return View();
        }

        public ActionResult About()
        {
            this.AddToastMessage("Redirected message", "This message has been redirected", ToastType.Error);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        //http://localhost:57756/Home/geturl?folder=IMG101_0142_p_tiles&file=IMG101_0142_p_t_1f_01_01.jpg
        public string GetUrl(string folder, string file)
        {
            return null; // this.googleDrive.GetUrl(folder, file);
        }
    }
}