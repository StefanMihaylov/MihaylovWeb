using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Home.Models.Home
{
    public class ModuleDependenciesModel
    {
        public string ModuleName { get; }

        public DependencyNode Server { get; }

        public DependencyNode Ui { get; }

        public ModuleDependenciesModel(string moduleName, ComponentDependencies input)
        {
            ModuleName = moduleName;
            Server = input?.ServerDependencies;
            Ui = input?.UiDependencies;
        }
    }
}
