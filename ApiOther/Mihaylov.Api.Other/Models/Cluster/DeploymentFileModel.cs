using Mihaylov.Api.Other.Contracts.Base;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class DeploymentFileModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(ModelConstants.AppFileNameMaxLength, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public int ApplicationId { get; set; }
    }
}
