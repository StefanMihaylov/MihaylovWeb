namespace Mihaylov.Common
{
    /// <summary>
    /// Specifies the kinds of dependencies.
    /// </summary>
    public enum DependencyKind
    {
        /// <summary>
        /// A project dependency.
        /// </summary>
        Project = 1,

        /// <summary>
        /// A package dependency.
        /// </summary>
        Package,

        /// <summary>
        /// Provides Swagger documentation for the API.
        /// </summary>
        Swagger,
    }
}
