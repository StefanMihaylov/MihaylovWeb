using System.Collections.Generic;

namespace Mihaylov.Common;

public class ManualModuleCatalog(MenuItem home, MenuItem login, List<IUiModule> modules) : IModuleCatalog
{
    public MenuItem Home { get; } = home;

    public MenuItem Login { get; } = login;

    public IEnumerable<IUiModule> Modules { get; } = modules;
}
