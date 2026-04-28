using System.Collections.Generic;
using Mihaylov.Api.Gear.Client;

namespace Mihaylov.Web.Models.Gear;

public record ListsViewModel(IEnumerable<Group> Groups, IEnumerable<Brand> Brands,
    IEnumerable<Shop> Shops, IEnumerable<Category> Categories);