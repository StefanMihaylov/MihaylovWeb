using System;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.UI.Mvc.Gear.Areas.Gear.Models.Gear
{
    public class TripAddViewModel
    {
        public long? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int? Year { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
