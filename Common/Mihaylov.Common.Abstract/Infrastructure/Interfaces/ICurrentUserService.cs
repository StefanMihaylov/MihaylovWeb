namespace Mihaylov.Common.Abstract.Infrastructure.Interfaces
{
    public interface ICurrentUserService
    {
        string GetId();

        string GetUserName();
    }
}