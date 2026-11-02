using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

/// <summary>
/// Provides the registered UI modules of the web host.
/// </summary>
public interface IModuleCatalog
{
    /// <summary>
    /// Gets all registered UI modules.
    /// </summary>
    IEnumerable<IUiModule> Modules { get; }

    /// <summary>
    /// Gets the menu item that is marked as the home menu, or null when there is none.
    /// </summary>
    public MenuItem Home { get; }

    /// <summary>
    /// Gets the identity module that handles the login, or null when there is none.
    /// </summary>
    public IUiModule Login { get; }

    /// <summary>
    /// Gets the modules that show menu items. The identity module is not included.
    /// </summary>
    IEnumerable<IUiModule> MenuModules { get; }

    /// <summary>
    /// Gets the server and UI module info of every module.
    /// </summary>
    /// <param name="factory">Creates the scope used to resolve the module info services.</param>
    /// <returns>One <see cref="ComponentModuleInfo"/> for each module in <see cref="Modules"/>.</returns>
    Task<IEnumerable<ComponentModuleInfo>> GetModuleVersionsAsync(IServiceScopeFactory factory);
}
