using System.Net;
using System.Net.Http;

namespace Mihaylov.Common.Generic.Servises.Models
{
    public class BaseHttpResponse
    {
        public string Body { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public HttpStatusCode StatusCode { get; set; }


        public BaseHttpResponse(string body, HttpResponseMessage response)
        {
            Body = body;
            IsSuccessStatusCode = response.IsSuccessStatusCode;
            StatusCode = response.StatusCode;
        }
    }
}
