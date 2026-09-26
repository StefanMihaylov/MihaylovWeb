using System.Collections.Generic;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IKopiaClient
    {
        IEnumerable<KopiaSnapshot> GetSnapshots(KopiaConnectModel context);

        void DeleteSnapshot(KopiaConnectModel context, string id);
    }
}
