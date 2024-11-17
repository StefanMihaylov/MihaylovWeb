using System.Collections.Generic;
using Mihaylov.Common.Database.Models;

namespace Mihaylov.Api.Other.Database.Cluster.Models
{
    public class Application : Entity
    {
        public int ApplicationId { get; set; }

        public int? Order {  get; set; }

        public string Name { get; set; }

        public string SiteUrl { get; set; }        

        public string ReleaseUrl { get; set; }

        public string GithubVersionUrl { get; set; }

        public string ResourceUrl { get; set; }

        public byte DeploymentId { get; set; }

        public Deployment Deployment { get; set; }

        public string Notes { get; set; }

        public IEnumerable<ApplicationVersion> Versions { get; set; }

        public IEnumerable<ApplicationPod> Pods { get; set; }

        public IEnumerable<DeploymentFile> Files { get; set; }

        public IEnumerable<ParserSetting> ParserSettings { get; set; }

        public Application()
        {
            Versions = new List<ApplicationVersion>();
            Pods = new List<ApplicationPod>();
            Files = new List<DeploymentFile>();
            ParserSettings = new List<ParserSetting>();
        }
    }
}
