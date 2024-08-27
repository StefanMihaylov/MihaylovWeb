namespace Mihaylov.Api.Other.Database.Cluster.Models
{
    public class ApplicationPod
    {
        public int ApplicationPodId { get; set; }

        public int ApplicationId { get; set; }

        public string Name { get; set; }

        public Application Application { get; set; }
    }
}
