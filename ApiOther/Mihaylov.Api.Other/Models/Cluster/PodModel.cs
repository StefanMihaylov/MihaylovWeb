using Mihaylov.Api.Other.Contracts.Base;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class PodModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.AppPodNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public int ApplicationId { get; set; }
    }
}
