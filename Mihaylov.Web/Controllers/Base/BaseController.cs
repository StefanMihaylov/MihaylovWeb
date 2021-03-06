﻿using System;
using System.Web.Mvc;
using Mihaylov.Web.Common.Toastr;
using Ninject.Extensions.Logging;

namespace Mihaylov.Web.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        public BaseController(ILogger logger, IToastrHelper toaster)
        {
            this.Logger = logger;
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