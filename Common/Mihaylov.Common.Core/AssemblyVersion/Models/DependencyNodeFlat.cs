namespace Mihaylov.Common
{
    /// <summary>
    /// Represents a dependency node in a flat structure.
    /// </summary>
    public class DependencyNodeFlat : IDependencyNodeFlat
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dependency version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the target framework.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the kind of dependency.
        /// </summary>
        public DependencyKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent node.
        /// </summary>
        public int? ParentId { get; set; }


        /// <summary>
        /// Initializes a new instance of the DependencyNodeFlat class.
        /// </summary>
        public DependencyNodeFlat()
        {            
        }

        /// <summary>
        /// Initializes a new flattened dependency node.
        /// </summary>
        /// <param name="item">The dependency node whose properties are copied.</param>
        /// <param name="id">The unique identifier of the dependency node.</param>
        /// <param name="parentId">The identifier of the parent dependency node, or <see langword="null"/> if it has no parent.</param>
        public DependencyNodeFlat(DependencyNode item, int id, int? parentId)
            : this(id, item.Name, item.Version, item.Framework, item.Kind, parentId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyNodeFlat"/> class from a dependency node.
        /// </summary>
        /// <param name="item">The dependency node to copy.</param>
        public DependencyNodeFlat(IDependencyNodeFlat item)
            : this(item.Id, item.Name, item.Version, item.Framework, item.Kind, item.ParentId)
        {
        }

        internal DependencyNodeFlat(int id, string name, string version, string framework, DependencyKind kind, int? parentId)
        {
            Id = id;
            Name = name;
            Version = version;
            Framework = framework;
            Kind = kind;
            ParentId = parentId;
        }
    }
}
