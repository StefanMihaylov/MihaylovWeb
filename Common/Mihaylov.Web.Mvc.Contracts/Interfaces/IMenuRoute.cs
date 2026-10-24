namespace Mihaylov.Common;

public interface IMenuRoute
{
    string Area { get; }

    string Controller { get; }

    string Action { get; }
}
