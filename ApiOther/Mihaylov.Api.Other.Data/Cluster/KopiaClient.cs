using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;
using Mihaylov.Api.Other.Data.Cluster.Models;

namespace Mihaylov.Api.Other.Data.Cluster;

public class KopiaClient : IKopiaClient
{
    private readonly ILogger _logger;
    private readonly IProcessHelper _processHelper;
    private readonly KopiaSettings _config;
    private readonly JsonSerializerOptions _jsonOptions;

    public KopiaClient(ILoggerFactory loggerFactory, IProcessHelper processHelper, IOptions<KopiaSettings> settings)
    {
        _logger = loggerFactory.CreateLogger(this.GetType());
        _processHelper = processHelper;
        _config = settings.Value;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        _config.KopiaPath = _config.KopiaPath.TrimEnd('/').TrimEnd('\\');
    }

    public IEnumerable<KopiaSnapshot> GetSnapshots(KopiaConnectModel context)
    {
        Connect(context);

        var list = GetResponse("snapshot list --all -i --json", true);
        var snapshotsList = JsonSerializer.Deserialize<KopiaSnapshotModel[]>(list, _jsonOptions);

        var snapshots = snapshotsList.Select(s => new KopiaSnapshot
        {
            SnapshotId = s.Id,
            Date = s.EndTime,
            KopiaId = s.RootEntry.Obj,
            Path = string.Join("/", s.Source.Path.Split('/').Skip(2)),
            Size = s.RootEntry.Summ.Size,
            Files = s.RootEntry.Summ.Files,
            Dirs = s.RootEntry.Summ.Dirs,
            NumFailed = s.RootEntry.Summ.NumFailed,
            LocationName = context.LocationName,
            VolumeNamespace = context.VolumeNamespace,
        }).ToList();

        Disconnect();

        return snapshots;
    }

    public void DeleteSnapshot(KopiaConnectModel context, string id)
    {
        Connect(context);

        var response = GetResponse($"snapshot delete --unsafe-ignore-source {id}", true);

        Disconnect();
    }

    private void Connect(KopiaConnectModel context)
    {
        var url = new Uri(context.StorageUrl);

        var arguments = new Dictionary<string, string>
        {
            { "bucket", context.Bucket },
            { "endpoint", url.Host },
            { "access-key", context.ClientId },
            { "secret-access-key", context.ClientSecret },
            { "disable-tls", null },
            { "disable-tls-verification", null },
            { "prefix", $"kopia/{context.VolumeNamespace}/" },
            { "password", context.Password },
            { "no-check-for-updates", null },
           // { "log-level", "debug" },
        };

        var command = $"repository connect s3 {string.Join(" ", arguments.Select(kv => GetArgument(kv)))}";
        var result = GetResponse(command, false);
    }

    private void Disconnect()
    {
        var logout = GetResponse("repository disconnect", true);
    }

    private static string GetArgument(KeyValuePair<string, string> kv)
    {
        var value = $"--{kv.Key}";
        if (!string.IsNullOrWhiteSpace(kv.Value))
        {
            value = $"{value}=\"{kv.Value}\"";
        }

        return value;
    }

    private string GetResponse(string command, bool hideResponse)
    {
        _logger.LogInformation("Start Kopia exe command 'command'");

        var kopiaPath = KopiaExePath();

        _logger.LogInformation($"Kopia path: '{kopiaPath}'");
        var result = _processHelper.ExecuteCommand(kopiaPath, command);

        _logger.LogInformation($"Kopia '{command}' command response: {(hideResponse == false ? result : $"<lenght:{result.Length}>")}");

        return result;
    }

    private string KopiaExePath()
    {
        var kopiaDir = new DirectoryInfo(_config.KopiaPath);
        var kopiaFile = kopiaDir.GetFiles("kopia*").FirstOrDefault();
        if (kopiaFile == null)
        {
            return null;
        }

        return kopiaFile.FullName;
    }
}
