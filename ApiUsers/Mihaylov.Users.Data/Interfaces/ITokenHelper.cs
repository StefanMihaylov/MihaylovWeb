using System.Collections.Generic;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Interfaces
{
    public interface ITokenHelper
    {
        string GetToken(User user, IEnumerable<string> roles, int? customClaimTypes);
    }
}