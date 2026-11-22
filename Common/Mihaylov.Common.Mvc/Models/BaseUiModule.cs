using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Base class for UI modules. It holds the name, identity flag and partial view name,
    /// and leaves the services, menus and module info to the derived class.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <param name="serviceType">The service that reads the module info from the server.</param>
    /// <param name="isIdentity">True if this is the identity module that handles the login.</param>
    /// <param name="partialViewName">The name of the partial view of the module, or null when it has none.</param>
    public abstract class BaseUiModule(string name, Type serviceType, bool isIdentity = false, string partialViewName = null) : IUiModule
    {
        private readonly Type _serviceType = serviceType;

        /// <inheritdoc />
        public string Name => name;

        /// <inheritdoc />
        public bool IsIdentity => isIdentity;

        /// <inheritdoc />
        public string PartialViewName => partialViewName;

        /// <inheritdoc />
        public abstract void ConfigureServices(IServiceCollection services);

        /// <inheritdoc />
        public abstract IEnumerable<MenuItem> Menus { get; }

        /// <inheritdoc />
        public virtual async Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory)
        {
            ValidateServiceType();

            IModuleInfo serverData;
            try
            {
                serverData = await UseServiceAsync(factory, s => s.GetInfoAsync()).ConfigureAwait(false);
            }
            catch (Exception)
            {
                serverData = new ModuleInfo(Name, null, null, null, null, null);
            }

            var uiInfo = VersionHelper.GetVersionInfo(this.GetType().Assembly);

            var orders = Menus?.Select(m => m.Order).ToList();

            return new ComponentModuleInfo(serverData, uiInfo, orders);
        }

        /// <inheritdoc />
        public virtual async Task<ComponentDependencies> GetDependenciesAsync(IServiceScopeFactory factory)
        {
            ValidateServiceType();

            DependencyNode serverData;
            try
            {
                serverData = await UseServiceAsync(factory, s => s.GetDependenciesAsync()).ConfigureAwait(false);
            }
            catch (Exception)
            {
                serverData = new DependencyNode(Name, DependencyKind.Project);
            }

            var uiInfo = DependencyScanner.Scan(this.GetType().Assembly);

            return new ComponentDependencies(serverData, uiInfo);
        }

        private void ValidateServiceType()
        {
            if (_serviceType == null)
            {
                throw new InvalidOperationException($"Module '{Name}' has no service type.");
            }

            if (!typeof(IModuleInfoService).IsAssignableFrom(_serviceType))
            {
                throw new ArgumentException($"Type '{_serviceType.FullName}' must implement IModuleInfoService.", nameof(_serviceType));
            }
        }

        private async Task<T> UseServiceAsync<T>(IServiceScopeFactory factory, Func<IModuleInfoService, Task<T>> action)
        {
            using var scope = factory.CreateScope();
            var provider = scope.ServiceProvider;

            var service = (IModuleInfoService)provider.GetRequiredService(_serviceType);

            return await action(service).ConfigureAwait(false);
        }
    }
}
