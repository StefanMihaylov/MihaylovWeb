﻿using System;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusResponseAsset
    {
        public string Id { get; set; }

        public string DownloadUrl { get; set; }

        public DateTime LastModified { get; set; }
    }
}