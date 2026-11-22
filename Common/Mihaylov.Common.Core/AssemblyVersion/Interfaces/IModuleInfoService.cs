using System.Threading.Tasks;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides the <see cref="IModuleInfo"/> of a module.
    /// </summary>
    public interface IModuleInfoService
    {
        /// <summary>
        /// The name of the controller action that returns the module info.
        /// </summary>
        public const string MODULE_ACTION_NAME = "ModuleGetInfo";

        /// <summary>
        /// Gets information about dependencies.
        /// </summary>
        public const string DEPENDENCIES_ACTION_NAME = "DependenciesGetInfo";

        /// <summary>
        /// Gets the module info.
        /// </summary>
        /// <returns>The <see cref="IModuleInfo"/> of the module.</returns>
        Task<IModuleInfo> GetInfoAsync();

        /// <summary>
        /// Gets the dependency graph for the current module.
        /// </summary>
        /// <returns>A task that resolves to the root dependency node for the current module.</returns>
        Task<DependencyNode> GetDependenciesAsync();
    }
}
