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
        public const string ACTION_NAME = "ModuleGetInfo";

        /// <summary>
        /// Gets the module info.
        /// </summary>
        /// <returns>The <see cref="IModuleInfo"/> of the module.</returns>
        Task<IModuleInfo> GetInfoAsync();
    }
}
