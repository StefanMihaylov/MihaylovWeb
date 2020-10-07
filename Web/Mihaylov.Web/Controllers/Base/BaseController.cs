using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mihaylov.Web.Common.Toastr;

namespace Mihaylov.Web.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public BaseController(ILoggerFactory loggerFactory, IToastrHelper toaster)
        {
            this.Logger = loggerFactory.CreateLogger(this.GetType().Name);
            this.Toaster = toaster;
        }

        protected ILogger Logger { get; private set; }
        private IToastrHelper Toaster { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            IToastrHelper toastr = this.TempData["Toastr"] as ToastrHelper;
            toastr = toastr ?? this.Toaster;

            var toastMessage = toastr.AddToastMessage(title, message, toastType);

            this.TempData["Toastr"] = toastr;

            return toastMessage;
        }

//        protected override void OnException(ExceptionContext filterContext)
//        {
//#if (DEBUG)
//            return;
//#else
//            try
//            {
//                var exception = filterContext.Exception;
//                this.Logger.Error(exception, "Error in {0}", filterContext.Controller);
//            }
//            catch (Exception)
//            {
//                // skip logging
//            }
//            finally
//            {
//               // this.UpdateContextResult(filterContext);
//            }
//#endif
//        }
    }
}