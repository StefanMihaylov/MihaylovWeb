namespace Mihaylov.Users.Data.Repository.Models
{
    public class LoginResponseModel
    {
        public bool Succeeded { get; set; }

        public string Token { get; set; }

        public LoginResponseModel(bool succeeded, string token)
        {
            this.Succeeded = succeeded;
            this.Token = token;
        }
    }
}
