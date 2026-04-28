using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Gear;

public class InventoryViewModel
{
    public long? Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public int? BrandId { get; set; }

    public int? ShopId { get; set; }

    [Required]
    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }
    public int? CurrencyId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    [Required]
    public int ItemStatus { get; set; }

    public IEnumerable<KitItemViewModel> KitContents { get; set; }
}

public class KitItemViewModel
{
    public string Name { get; set; }
}
