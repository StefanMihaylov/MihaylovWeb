using System;
using System.Web.Mvc;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public BaseController(ILogger logger)
        {
            this.Logger = logger;
        }

        protected ILogger Logger { get; private set; }

        protected override void OnException(ExceptionContext filterContext)
        {
#if (DEBUG)
            return;
#else
            try
            {
                var exception = filterContext.Exception;
                this.Logger.Error(exception, "Error in {0}", filterContext.Controller);
            }
            catch (Exception)
            {
                // skip logging
            }
            finally
            {
               // this.UpdateContextResult(filterContext);
            }
#endif
        }
    }
}