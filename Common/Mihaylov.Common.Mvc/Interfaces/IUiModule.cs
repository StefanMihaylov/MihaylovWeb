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
    /// Gets the server and UI module info of a UI module.
    /// </summary>
    /// <param name="factory">Creates the scope used to resolve IModuleInfoService</param>
    /// <returns>
    /// The <see cref="ComponentModuleInfo"/> with the server info and the UI info read from the module's assembly.
    /// If the server cannot be read, the server info has only the module name.
    /// </returns>
    Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory);

    /// <summary>
    /// Gets the component dependencies from the specified service scope factory.
    /// </summary>
    /// <param name="factory">The service scope factory used to resolve the component dependencies.</param>
    /// <returns>A task that resolves to the component dependencies for the current scope.</returns>
    Task<ComponentDependencies> GetDependenciesAsync(IServiceScopeFactory factory);
}
