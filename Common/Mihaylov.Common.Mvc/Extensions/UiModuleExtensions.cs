using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Extension methods for <see cref="IUiModule"/>.
    /// </summary>
    public static class UiModuleExtensions
    {
        /// <summary>
        /// Gets the server and UI module info of a UI module.
        /// </summary>
        /// <typeparam name="T">The service that reads the module info from the server.</typeparam>
        /// <param name="module">The UI module.</param>
        /// <param name="factory">Creates the scope used to resolve <typeparamref name="T"/>.</param>
        /// <returns>
        /// The <see cref="ComponentModuleInfo"/> with the server info and the UI info read from the module's assembly.
        /// If the server cannot be read, the server info has only the module name.
        /// </returns>
        public static async Task<ComponentModuleInfo> GetModelInfoAsync<T>(this IUiModule module, IServiceScopeFactory factory) where T : IModuleInfoService
        {
            using var scope = factory.CreateScope();
            var provider = scope.ServiceProvider;

            var service = provider.GetRequiredService<T>();

            IModuleInfo serverData;
            try
            {
                serverData = await service.GetInfoAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                serverData = new ModuleInfo(module.Name, null, null, null, null, null);
            }

            var assembly = module.GetType().Assembly;
            var uiInfo = VersionHelper.GetVersionInfo(assembly);

            return new ComponentModuleInfo(serverData, uiInfo);
        }
    }
}
