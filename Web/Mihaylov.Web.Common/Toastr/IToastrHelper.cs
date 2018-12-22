using System.Collections.Generic;

namespace Mihaylov.Web.Common.Toastr
{
    public interface IToastrHelper
    {
        bool ShowCloseButton { get; set; }
        bool ShowNewestOnTop { get; set; }
        List<ToastMessage> ToastMessages { get; set; }

        ToastMessage AddToastMessage(string message, ToastType toastType);
        ToastMessage AddToastMessage(string title, string message, ToastType toastType);
    }
}