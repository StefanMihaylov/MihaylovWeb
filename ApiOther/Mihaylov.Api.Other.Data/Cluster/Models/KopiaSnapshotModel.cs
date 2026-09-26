using System;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class KopiaSnapshotModel
{
    public string Id { get; set; }

    public DateTime EndTime { get; set; }

    public KopiaSnapshotModelSource Source { get; set; }    

    public KopiaSnapshotModelRootEntry RootEntry { get; set; }
}

public class KopiaSnapshotModelRootEntry
{
    public string Obj { get; set; }

    public KopiaSnapshotModelSummary Summ { get; set; }
}

public class KopiaSnapshotModelSummary
{
    public long Size { get; set; }

    public long Files { get; set; }

    public long Dirs { get; set; }

    public long NumFailed { get; set; }
}

public class KopiaSnapshotModelSource
{
    public string Path { get; set; }
}
