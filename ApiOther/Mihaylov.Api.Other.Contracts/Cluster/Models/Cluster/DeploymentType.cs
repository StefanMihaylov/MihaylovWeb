﻿namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster
{
    public enum DeploymentType : byte
    {
        Yaml = 1,

        HelmChart = 2,

        Command = 3,
    }
}
