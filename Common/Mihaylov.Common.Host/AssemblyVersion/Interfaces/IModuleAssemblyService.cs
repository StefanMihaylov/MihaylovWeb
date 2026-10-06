namespace Mihaylov.Common;

/// <summary>
/// Defines a service that provides metadata about a module's assembly.
/// </summary>
public interface IModuleAssemblyService
{
    /// <summary>
    /// Gets information about the module.
    /// </summary>
    /// <returns>An IModuleInfo that represents the module's metadata and runtime information.</returns>
    IModuleInfo GetModuleInfo();
}