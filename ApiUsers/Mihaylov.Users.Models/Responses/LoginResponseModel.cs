namespace Mihaylov.Users.Models.Responses
{
    public class LoginResponseModel
    {
        public bool Succeeded { get; set; }

        public bool IsLockedOut { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public LoginResponseModel(bool succeeded, bool isLockedOut, string userName, string token)
        {
            Succeeded = succeeded;
            IsLockedOut = isLockedOut;
            UserName = userName;
            Token = token;
        }
    }
}
