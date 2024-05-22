using Mihaylov.Api.Other.Contracts.Show;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Concerts
{
    public class TicketProviderModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.TicketProviderNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }

        [MaxLength(ModelConstants.TicketProviderUrlMaxLength)]
        public string Url { get; set; }
    }
}
