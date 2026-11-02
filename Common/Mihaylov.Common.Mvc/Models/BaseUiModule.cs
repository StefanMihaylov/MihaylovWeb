using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Base class for UI modules. It holds the name, identity flag and partial view name,
    /// and leaves the services, menus and module info to the derived class.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <param name="isIdentity">True if this is the identity module that handles the login.</param>
    /// <param name="partialViewName">The name of the partial view of the module, or null when it has none.</param>
    public abstract class BaseUiModule(string name, bool isIdentity = false, string partialViewName = null) : IUiModule
    {
        /// <inheritdoc />
        public string Name => name;

        /// <inheritdoc />
        public bool IsIdentity => isIdentity;

        /// <inheritdoc />
        public string PartialViewName => partialViewName;

        /// <inheritdoc />
        public abstract void ConfigureServices(IServiceCollection services);
        
        /// <inheritdoc />
        public abstract Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory);

        /// <inheritdoc />
        public abstract IEnumerable<MenuItem> Menus { get; }
    }
}
