using System;
using System.Collections.Generic;

namespace Mihaylov.Web.Common.Toastr
{
    [Serializable]
    public class ToastrHelper : IToastrHelper
    {
        public bool ShowNewestOnTop { get; set; }

        public bool ShowCloseButton { get; set; }

        public List<ToastMessage> ToastMessages { get; set; }

        public ToastrHelper()
        {
            ShowNewestOnTop = false;
            ShowCloseButton = false;
            ToastMessages = new List<ToastMessage>();
        }

        public ToastMessage AddToastMessage(string message, ToastType toastType)
        {
            return this.AddToastMessage(string.Empty, message, toastType);
        }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {
            var toast = new ToastMessage(title, message, toastType);
            ToastMessages.Add(toast);
            return toast;
        }
    }
}
