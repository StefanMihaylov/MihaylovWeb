namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Configs
{
    public class KubernetesSettings
    {
        public string ServiceHost { get; set; }

        public string ServicePort { get; set; }

        public string ConfigPath { get; set; }
    }
}
