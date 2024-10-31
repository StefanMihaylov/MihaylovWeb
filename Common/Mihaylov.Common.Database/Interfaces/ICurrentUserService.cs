namespace Mihaylov.Common.Database.Interfaces
{
    public interface ICurrentUserService
    {
        string GetId();

        string GetUserName();
    }
}
