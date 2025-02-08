using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Other.Models.Cluster
{
    public class CreateBackupModel
    {
        [Required]
        public string ScheduleName { get; set; }
    }
}
