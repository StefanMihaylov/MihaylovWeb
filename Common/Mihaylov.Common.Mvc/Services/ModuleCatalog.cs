using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

/// <summary>
/// The default <see cref="IModuleCatalog"/>. It reads the modules registered in the service container.
/// </summary>
/// <param name="modules">The registered UI modules.</param>
public class ModuleCatalog(IEnumerable<IUiModule> modules) : IModuleCatalog
{
    /// <inheritdoc />
    public IEnumerable<IUiModule> Modules { get; } = modules;

    /// <inheritdoc />
    public MenuItem Home => Modules.Where(m => m.Menus != null).SelectMany(a => a.Menus).SingleOrDefault(m => m?.IsHomeMenu == true);

    /// <inheritdoc />
    public IUiModule Login => Modules.SingleOrDefault(m => m.IsIdentity);

    /// <inheritdoc />
    public IEnumerable<IUiModule> MenuModules => Modules.Where(m => !m.IsIdentity && m.Menus?.Any() == true).ToList();

    /// <inheritdoc />
    public async Task<IEnumerable<ComponentModuleInfo>> GetModuleVersionsAsync(IServiceScopeFactory factory)
    {
        var result = new List<ComponentModuleInfo>();

        foreach (var module in Modules)
        {
            var moduleInfo = await module.GetModuleInfoAsync(factory).ConfigureAwait(false);
            result.Add(moduleInfo);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ComponentDependencies>> GetModuleDependenciesAsync(IServiceScopeFactory factory)
    {
        var result = new List<ComponentDependencies>();

        foreach (var module in Modules)
        {
            var dependencies = await module.GetDependenciesAsync(factory).ConfigureAwait(false);
            result.Add(dependencies);
        }

        return result;
    }
}
