using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Common
{
    /// <summary>
    /// Represents a dependency and its nested dependencies.
    /// </summary>
    public class DependencyNode
    {
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
        /// Gets or sets the nodes that this node depends on.
        /// </summary>
        public IEnumerable<DependencyNode> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dependency is local.
        /// </summary>
        public bool IsLocal { get; set; }


        /// <summary>
        /// Initializes a new instance of the DependencyNode class.
        /// </summary>
        public DependencyNode()
        {
        }

        /// <summary>
        /// Initializes a new dependency node.
        /// </summary>
        /// <param name="name">The name of the dependency.</param>
        /// <param name="version">The version of the dependency.</param>
        /// <param name="framework">The framework of the dependency.</param>
        /// <param name="kind">The kind of dependency.</param>
        /// <param name="dependencies">The dependencies of this node.</param>
        public DependencyNode(string name, string version, string framework, DependencyKind kind, IEnumerable<DependencyNode> dependencies)
        {
            Name = name;
            Version = version;
            Framework = framework;
            Kind = kind;
            Dependencies = dependencies;
            IsLocal = DependencyExtensions.IsNugetLocal(name);
        }

        /// <summary>
        /// Initializes a new dependency node.
        /// </summary>
        /// <param name="name">The name of the dependency.</param>
        /// <param name="version">The version of the dependency.</param>
        /// <param name="framework">The framework of the dependency.</param>
        /// <param name="kind">The kind of dependency.</param>
        public DependencyNode(string name, string version, string framework, DependencyKind kind)
           : this(name, version, framework, kind, new List<DependencyNode>())
        {          
        }

        /// <summary>
        /// Initializes a new dependency node when the package cannot be loaded
        /// </summary>
        /// <param name="name">The name of the dependency.</param>
        /// <param name="kind">The kind of dependency.</param>
        public DependencyNode(string name, DependencyKind kind) 
            : this(name, "<not loaded>", null, kind)
        {
        }

        /// <summary>
        /// Initializes a dependency node from flat dependency information and its dependencies.
        /// </summary>
        /// <param name="input">The flat dependency information used to initialize the node.</param>
        /// <param name="dependencies">The dependencies associated with the node.</param>
        public DependencyNode(IDependencyNodeFlat input, IEnumerable<DependencyNode> dependencies)
            : this(input.Name, input.Version, input.Framework, input.Kind, dependencies)
        {
        }

        /// <summary>
        /// Returns a string that represents the dependency.
        /// </summary>
        /// <returns>The dependency name and version with its Local or Nuget source.</returns>
        public override string ToString()
        {
            return $"{Name} {Version} [{Dependencies.Count()}] ({(IsLocal ? "Local" : "Nuget")})";
        }
    }
}
