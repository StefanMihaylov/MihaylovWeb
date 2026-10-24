using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

public interface IUiModule
{
    string Name { get; }

    void ConfigureServices(IServiceCollection services);

    IEnumerable<MenuItem> Menus { get; }
}
