namespace Mihaylov.Common.Generic.Servises.Models
{
    public class ApiConfig
    {
        public string BaseUrl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ApiKey { get; set; }

        public ApiConfig(string baseUrl, string username, string password, string apiKey)
        {
            BaseUrl = baseUrl;
            Username = username;
            Password = password;
            ApiKey = apiKey;
        }
    }
}
