﻿namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusConfiguration
    {
        public string BaseUrl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RepositoryName { get; set; }

        public int SkippedVersionCount { get; set; }

        public int SkippedVersionMonthsAge { get; set; }
    }
}