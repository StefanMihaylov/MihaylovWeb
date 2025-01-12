using System;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class Response<T> where T : class
    {
        public T Data { get; set; }

        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }


        public Response(T data) : this(data, true, null)
        {
        }

        public Response(string error) : this(default, false, error)
        {
        }

        private Response(T data, bool isSuccessful, string errorMessage)
        {
            Data = data;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public T GetResponse() => IsSuccessful ? Data : throw new ApplicationException(ErrorMessage);
    }
}
