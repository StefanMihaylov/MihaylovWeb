using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ISiteHelper
    {
        Person GetUserInfo(string username);
    }
}