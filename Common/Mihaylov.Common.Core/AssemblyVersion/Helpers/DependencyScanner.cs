using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Scans an assembly and its declared dependencies to build a dependency tree.
    /// </summary>
    public static class DependencyScanner
    {
        private static readonly ConcurrentDictionary<Assembly, Lazy<DependencyNode>> Cache
            = new ConcurrentDictionary<Assembly, Lazy<DependencyNode>>();

        /// <summary>
        /// Scans the assembly containing <typeparamref name="T"/> for dependencies.
        /// </summary>
        /// <typeparam name="T">A reference type in the assembly to scan.</typeparam>
        /// <returns>The dependency node for the scanned assembly.</returns>
        public static DependencyNode Scan<T>() where T : class
        {
            var assembly = typeof(T).Assembly;
            var result = Scan(assembly);

            return result;
        }

        /// <summary>
        /// Scans an assembly and its dependencies, returning the resulting dependency node.
        /// </summary>
        /// <remarks>The result is cached for each assembly.</remarks>
        /// <param name="root">The assembly to scan.</param>
        /// <returns>The dependency node representing the scanned assembly and its dependencies.</returns>
        public static DependencyNode Scan(Assembly root)
        {
            var lazyNode = Cache.GetOrAdd(root, assembly =>
            {
                return new Lazy<DependencyNode>(() =>
                {
                    var cache = new Dictionary<string, DependencyNode>();
                    return ScanCore(assembly, cache);
                });
            });

            return lazyNode.Value;
        }

        internal static DependencyNode ScanCore(Assembly assembly, IDictionary<string, DependencyNode> cache)
        {
            var versionInfo = VersionHelper.GetVersionInfo(assembly);
            var name = versionInfo.ModuleName;

            if (cache.TryGetValue(name, out var cached))
            {
                return cached;
            }

            cache[name] = null; // cycle guard

            var meta = assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                               .ToDictionary(a => a.Key, a => a.Value ?? "", StringComparer.Ordinal);

            if (!meta.ContainsKey("PackageReferences"))
            {
                return null; // not one of our projects
            }

            var children = new List<DependencyNode>();
            AddReferences(meta, "PackageReferences", DependencyKind.Package, children);
            AddReferences(meta, "SwaggerReferences", DependencyKind.Swagger, children);

            var node = new DependencyNode(name, versionInfo.Version, versionInfo.Framework, DependencyKind.Project, children);
            cache[name] = node;

            meta.TryGetValue("ProjectReferences", out var rawProjects);
            if (!string.IsNullOrWhiteSpace(rawProjects))
            {
                var projects = rawProjects.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var projectName in projects)
                {
                    try
                    {
                        var childAssembly = Assembly.Load(new AssemblyName(projectName));
                        var childNode = ScanCore(childAssembly, cache);
                        if (childNode != null)
                        {
                            children.Add(childNode);
                        }
                    }
                    catch
                    {
                        children.Add(new DependencyNode(projectName, DependencyKind.Project));
                    }
                }
            }

            return node;
        }

        private static void AddReferences(Dictionary<string, string> meta, string key, DependencyKind kind, IList<DependencyNode> target)
        {
            if (!meta.TryGetValue(key, out var raw))
            {
                return;
            }

            var items = raw.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                var parts = item.Split('|');

                var name = parts[0];
                var version = parts.Length > 1 ? parts[1] : null;

                target.Add(new DependencyNode(name, version, null, kind));
            }
        }
    }
}
