using System.Collections.Generic;
using Mihaylov.Common;

namespace Mihaylov.Web.ViewComponents
{
    public class MainNavViewModel
    {
        public MenuItem Home { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; }

        public MenuItem Login { get; set; }
    }
}
