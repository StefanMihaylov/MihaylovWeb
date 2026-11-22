namespace Mihaylov.Common
{
    /// <summary>
    /// Represents a dependency node in a flat data structure.
    /// </summary>
    public interface IDependencyNodeFlat
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the dependency version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        public string Framework { get; }

        /// <summary>
        /// Gets the kind of dependency.
        /// </summary>
        DependencyKind Kind { get; }

        /// <summary>
        /// Gets the identifier of the parent node.
        /// </summary>
        int? ParentId { get; }
    }
}