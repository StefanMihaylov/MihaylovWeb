﻿namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Version
{
    public class LastVersionSettings
    {
        public int ApplicationId { get; set; }

        public ParseModel Version { get; set; }

        public ParseModel ReleaseDate { get; set; }
    }
}
