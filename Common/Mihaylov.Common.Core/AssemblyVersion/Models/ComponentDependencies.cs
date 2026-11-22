namespace Mihaylov.Common
{
    /// <summary>
    /// Represents the dependencies required by a component for its server and user interface.
    /// </summary>
    public class ComponentDependencies
    {
        /// <summary>
        /// Gets the dependency node for server-side (API) dependencies.
        /// </summary>
        public DependencyNode ServerDependencies { get; }

        /// <summary>
        /// Gets the dependency node representing the UI dependencies.
        /// </summary>
        public DependencyNode UiDependencies { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentDependencies"/> class.
        /// </summary>
        /// <param name="serverDependencies">The dependency node for server dependencies.</param>
        /// <param name="uiDependencies">The dependency node for UI dependencies.</param>
        public ComponentDependencies(DependencyNode serverDependencies, DependencyNode uiDependencies)
        {
            ServerDependencies = serverDependencies;
            UiDependencies = uiDependencies;
        }
    }
}
