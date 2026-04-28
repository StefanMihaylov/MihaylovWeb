using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Gear.Core.Application.Queries.GetInventory;
using Mihaylov.Api.Gear.Core.Domain.Enums;

namespace Mihaylov.Api.Gear.Models
{
    public class AddInventoryModel
    {
        public long Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? BrandId { get; set; }

        public int? ShopId { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public decimal? Price { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? PurchaseDate { get; set; }

        [Required]
        public ItemStatus? ItemStatus { get; set; }

        public IEnumerable<KitContentItem>? KitContents { get; set; }
    }
}
