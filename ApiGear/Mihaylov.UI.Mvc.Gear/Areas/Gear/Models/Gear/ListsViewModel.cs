using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear;

public record ListsViewModel(IEnumerable<Group> Groups, IEnumerable<Brand> Brands,
    IEnumerable<Shop> Shops, IEnumerable<Category> Categories);