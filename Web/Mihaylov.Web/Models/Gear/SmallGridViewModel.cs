using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record SmallGridViewModel(IEnumerable<ISmallItem> Items, string Title, string SaveActionUrl);
