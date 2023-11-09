namespace Mihaylov.Users.Models.Responses
{
    public class LoginResponseModel
    {
        public bool Succeeded { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public LoginResponseModel(bool succeeded, string userName, string token)
        {
            this.Succeeded = succeeded;
            this.UserName = userName;
            this.Token = token;
        }
    }
}
