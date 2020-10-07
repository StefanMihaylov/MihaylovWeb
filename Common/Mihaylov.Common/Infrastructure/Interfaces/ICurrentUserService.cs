namespace Mihaylov.Common.Infrastructure.Interfaces
{
    public interface ICurrentUserService
    {
        string GetId();

        string GetUserName();
    }
}