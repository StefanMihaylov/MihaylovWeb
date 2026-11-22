using System;
using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods for converting dependency node hierarchies.
    /// </summary>
    public static class DependencyExtensions
    {
        /// <summary>
        /// Gets the prefix used for local package names.
        /// </summary>
        /// <remarks>Used to identify packages associated with Mihaylov.</remarks>
        public const string LOCAL_PACKAGE_PREFIX = "Mihaylov";

        /// <summary>
        /// Determines whether a NuGet package name uses the local package prefix.
        /// </summary>
        /// <param name="packageName">The package name to evaluate.</param>
        /// <returns><see langword="true"/> if the package name starts with the local package prefix; otherwise, <see langword="false"/>.</returns>
        public static bool IsNugetLocal(string packageName)
        {
            return packageName?.StartsWith(LOCAL_PACKAGE_PREFIX) ?? false;
        }

        /// <summary>
        /// Converts a collection of dependency trees into one flat collection. The ids run on across all roots.
        /// </summary>
        /// <param name="input">The dependency trees (roots) to flatten.</param>
        /// <returns>The flat nodes, or an empty collection if <paramref name="input"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentNullException">An element of <paramref name="input"/> is <see langword="null"/>.</exception>
        public static IEnumerable<DependencyNodeFlat> ToFlatModel(this IEnumerable<DependencyNode> input)
        {
            var result = new List<DependencyNodeFlat>();

            if (input == null)
            {
                return result;
            }

            var index = 0;
            foreach (var root in input)
            {
                index = ConvertToFlat(result, root, index, null);
            }

            return result;
        }

        /// <summary>
        /// Converts a dependency tree into a flat collection.
        /// </summary>
        /// <param name="input">The root of the tree to convert.</param>
        /// <returns>The flat nodes, or an empty collection if <paramref name="input"/> is <see langword="null"/>.</returns>
        public static IEnumerable<DependencyNodeFlat> ToFlatModel(this DependencyNode input)
        {
            var result = new List<DependencyNodeFlat>();

            if (input == null)
            {
                return result;
            }

            var index = 0;
            ConvertToFlat(result, input, index, null);

            return result;
        }

        /// <summary>
        /// Rebuilds the dependency trees from their flat representation (the reverse of
        /// <see cref="ToFlatModel(IEnumerable{DependencyNode})"/>).
        /// </summary>
        /// <param name="input">The flat nodes. They can be in any order; the order of the children (and of the roots) is the order of the nodes in the input.</param>
        /// <returns>The root nodes, or an empty collection if <paramref name="input"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentException">The input has duplicate ids, a missing parent, or has nodes but no root.</exception>
        public static IEnumerable<DependencyNode> ToTreeModels(this IEnumerable<IDependencyNodeFlat> input)
        {
            var roots = new List<DependencyNode>();

            if (input == null)
            {
                return roots;
            }

            var nodes = new Dictionary<int, NodeModel>();
            foreach (var item in input)
            {
                if (nodes.ContainsKey(item.Id))
                {
                    throw new ArgumentException($"Duplicate node id {item.Id}.", nameof(input));
                }

                nodes.Add(item.Id, new NodeModel(item, new List<DependencyNode>()));
            }

            foreach (var item in input)
            {
                var node = nodes[item.Id];

                if (item.ParentId == null)
                {
                    roots.Add(new DependencyNode(node.Input, node.Dependencies));
                    continue;
                }

                if (!nodes.TryGetValue(item.ParentId.Value, out var parent))
                {
                    throw new ArgumentException($"Parent {item.ParentId} of node {item.Id} was not found.", nameof(input));
                }

                parent.Dependencies.Add(new DependencyNode(node.Input, node.Dependencies));
            }

            if (nodes.Count > 0 && roots.Count == 0)
            {
                throw new ArgumentException("The input has no root node.", nameof(input));
            }

            return roots;
        }

        /// <summary>
        /// Rebuilds a single dependency tree from its flat representation (the reverse of
        /// <see cref="ToFlatModel(DependencyNode)"/>).
        /// </summary>
        /// <param name="input">The flat nodes of one tree. They can be in any order.</param>
        /// <returns>The root node, or <see langword="null"/> if <paramref name="input"/> is <see langword="null"/> or empty.</returns>
        /// <exception cref="ArgumentException">The input is invalid (see <see cref="ToTreeModels"/>) or has more than one root.</exception>
        public static DependencyNode ToTreeModel(this IEnumerable<IDependencyNodeFlat> input)
        {
            var roots = input.ToTreeModels();
            if (roots.Count() > 1)
            {
                throw new ArgumentException("The input has more than one root node.", nameof(input));
            }

            return roots.FirstOrDefault();
        }

        private static int ConvertToFlat(ICollection<DependencyNodeFlat> result, DependencyNode input, int index, int? parentId)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var count = index;

            var parentNode = new DependencyNodeFlat(input, count, parentId);
            count++;
            result.Add(parentNode);

            if (input.Dependencies?.Any() == true)
            {
                foreach (var child in input.Dependencies)
                {
                    var nextCount = ConvertToFlat(result, child, count, parentNode.Id);
                    count = nextCount;
                }
            }

            return count;
        }

        private class NodeModel
        {
            public IDependencyNodeFlat Input { get; }

            public IList<DependencyNode> Dependencies { get;}

            public NodeModel(IDependencyNodeFlat input, List<DependencyNode> dependencies)
            {
                Input = input;
                Dependencies = dependencies;
            }
        }
    }
}
