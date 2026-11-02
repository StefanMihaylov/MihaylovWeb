using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

/// <summary>
/// A UI module that adds its own pages, menu items and services to the web host.
/// </summary>
public interface IUiModule
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether this is the identity module that handles the login.
    /// </summary>
    bool IsIdentity { get; }

    /// <summary>
    /// Gets the name of the partial view of the module, or null when it has none.
    /// </summary>
    string PartialViewName { get; }

    /// <summary>
    /// Registers the services of the module.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// Gets the menu items of the module.
    /// </summary>
    IEnumerable<MenuItem> Menus { get; }

    /// <summary>
    /// Gets the server and UI module info of the module.
    /// </summary>
    /// <param name="factory">Creates the scope used to resolve the services.</param>
    /// <returns>The <see cref="ComponentModuleInfo"/> of the module.</returns>
    Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory);
}
