using System.Collections.Generic;

namespace Mihaylov.Common;

public interface IModuleCatalog
{
    public MenuItem Home { get; }

    public MenuItem Login { get; }

    IEnumerable<IUiModule> Modules { get; }
}
