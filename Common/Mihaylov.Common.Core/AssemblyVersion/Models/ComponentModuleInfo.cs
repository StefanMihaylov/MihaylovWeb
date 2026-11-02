namespace Mihaylov.Common
{
    /// <summary>
    /// Groups the <see cref="IModuleInfo"/> of a component's server side (the API) with the
    /// <see cref="IModuleInfo"/> of its UI side (the Razor module in the web host), so both can be shown or compared together.
    /// </summary>
    public class ComponentModuleInfo
    {
        /// <summary>
        /// Gets the metadata of the component's server module (API).
        /// </summary>
        public IModuleInfo ServerModuleInfo { get; }

        /// <summary>
        /// Gets the metadata of the component's UI module.
        /// </summary>
        public IModuleInfo UiModuleInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentModuleInfo"/> class.
        /// </summary>
        /// <param name="serverModuleInfo">The server module metadata.</param>
        /// <param name="uiModuleInfo">The UI module metadata.</param>
        public ComponentModuleInfo(IModuleInfo serverModuleInfo, IModuleInfo uiModuleInfo)
        {
            ServerModuleInfo = serverModuleInfo;
            UiModuleInfo = uiModuleInfo;
        }
    }
}
