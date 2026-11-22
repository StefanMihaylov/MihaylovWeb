using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear;

public record SmallGridViewModel(IEnumerable<ISmallItem> Items, string Title, string SaveActionUrl);
