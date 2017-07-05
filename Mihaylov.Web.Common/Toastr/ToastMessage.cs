using System;

namespace Mihaylov.Web.Common.Toastr
{
    [Serializable]
    public class ToastMessage
    {
        public ToastMessage()
        {
        }

        public ToastMessage(string title, string message, ToastType toastType)
        {
            this.Title = title;
            this.Message = message;
            this.ToastType = toastType;
            this.IsSticky = false;

            if (this.ToastType == ToastType.Error)
            {
                this.IsSticky = true;
            }
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public ToastType ToastType { get; set; }

        public bool IsSticky { get; set; }
    }
}
