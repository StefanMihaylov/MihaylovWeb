using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Velero;

namespace Mihaylov.Api.Other.Data.Cluster;

public class VeleroService(IKubernetesHelper kubernetesHelper, IVeleroClient veleroClient,
    IKopiaClient kopiaClient, IMemoryCache cache) : IVeleroService
{
    private const string ORPHANED_SNAPSHOTS = "orphanedSnapshots";
    private const string KOPIA_PASSWORD = "kopiaPassword";
    private const string STORAGE_CREDENTIALS = "StorageCredentials";

    public Task<string> GetExeVersionAsync()
    {
        return Task.FromResult(veleroClient.GetVersion());
    }

    public Task<string> CreateBackupAsync(string scheduleName)
    {
        return Task.FromResult(veleroClient.CreateBackup(scheduleName));
    }

    public Task<string> DeleteBackupAsync(string backupName)
    {
        return Task.FromResult(veleroClient.DeleteBackup(backupName));
    }

    public async Task DeleteSnapshotAsync(string id)
    {
        if (!cache.TryGetValue(ORPHANED_SNAPSHOTS, out IEnumerable<KopiaSnapshot> snapshots))
        {
            return;
        }

        var snapshot = snapshots.Where(s => s.SnapshotId == id).FirstOrDefault();
        if (snapshot == null)
        {
            return;
        }

        var locations = await kubernetesHelper.GetVeleroBackupStorageLocationsAsync().ConfigureAwait(false);
        var location = locations.FirstOrDefault(a => a.Name == snapshot.LocationName);
        if (location == null)
        {
            return;
        }

        var credentials = await GetStorageCredentials(location).ConfigureAwait(false);
        var kopiaPassword = await GetKopiaPasswordAsync().ConfigureAwait(false);
        var context = GetKopiaContext(location, snapshot.VolumeNamespace, credentials, kopiaPassword);

        kopiaClient.DeleteSnapshot(context, snapshot.KopiaId);
    }

    public async Task<ScheduleResponse> GetSchedulesAsync()
    {
        var context = await InitializeScheduleContextAsync().ConfigureAwait(false);

        var scheduleList = new List<Schedule>();
        foreach (var schedule in context.AllSchedules)
        {
            if (context.Backups.TryGetValue(schedule.Name, out BackupDetails backupDetails))
            {
                backupDetails.Used = true;

                var shedule = MapSchedule(schedule);
                shedule.Backups = backupDetails.Backups.Select(b => MapBackup(b, context)).ToList();

                scheduleList.Add(shedule);
            }
        }

        var oldSchedules = context.Backups.Where(b => !b.Value.Used)
                                     .ToDictionary(b => b.Key, b => b.Value.Backups);

        foreach (var oldSchedule in oldSchedules)
        {
            scheduleList.Add(new Schedule()
            {
                Name = oldSchedule.Key,
                Backups = oldSchedule.Value.Select(b => MapBackup(b, context)).ToList(),
            });
        }

        var result = new ScheduleResponse()
        {
            Schedules = scheduleList.OrderByDescending(s => s.LastBackup).ToList(),
            Statistics = new ScheduleStatistics()
            {
                ScheduleCount = context.AllSchedules.Count(),
                TotalBackupCount = GetBackupCount(context.AllBackups, null, null),
                LastWeekBackupCount = GetBackupCount(context.AllBackups, 7, null),
                LastDayBackupCount = GetBackupCount(context.AllBackups, 1, null),
                TotalSuccessfulBackupCount = GetBackupCount(context.AllBackups, null, true),
                LastWeekSuccessfulBackupCount = GetBackupCount(context.AllBackups, 7, true),
                LastDaySuccessfulBackupCount = GetBackupCount(context.AllBackups, 1, true),
            }
        };

        return result;
    }

    public async Task<SnapshotResponse> GetSnapshortsAsync()
    {
        var context = await InitializeKopiaContextAsync();

        var uploads = context.Uploads.ToDictionary(d => d.SnapshotID, d => d);
        var snapshots = context.Snapshots.ToDictionary(d => d.SnapshotId, d => d);

        var orphanedSnapshots = context.Snapshots.Where(s => !uploads.ContainsKey(s.SnapshotId))
                                                 .OrderByDescending(s => s.Date)
                                                 .ToList();

        var orphanedUploads = context.Uploads.Where(s => !snapshots.ContainsKey(s.SnapshotID))
                                         .OrderByDescending(s => s.Backup)
                                         .Select(s => s.Backup)
                                         .ToList();

        cache.Set(ORPHANED_SNAPSHOTS, orphanedSnapshots, TimeSpan.FromMinutes(30));

        var result = new SnapshotResponse()
        {
            OrphanedSnapshots = orphanedSnapshots,
            OrphanedUploads = orphanedUploads,
            Statistics = new SnapshotStatistics()
            {
                TotalUploadCount = context.Uploads.Count(),
                TotalSnapshotCount = context.Snapshots.Count(),
                OrchanedSnapshotCount = orphanedSnapshots.Count()
            }
        };

        return result;
    }


    private static Schedule MapSchedule(KubernetesSchedule schedule)
    {
        return new Schedule()
        {
            Name = schedule.Name,
            CreatedOn = schedule.CreatedOn,
            Cron = schedule.Schedule,
            Paused = schedule.Paused,
            CsiSnapshotTimeout = schedule.CsiSnapshotTimeout,
            LastBackup = schedule.LastBackup,
            Phase = schedule.Phase,
            IncludedNamespaces = schedule.IncludedNamespaces,
            ExcludedResources = schedule.ExcludedResources,
            Expiration = schedule.Expiration,
            MatchLabels = schedule.MatchLabels?.Select(kv => $"{kv.Key} : {kv.Value}").FirstOrDefault(),
            ItemOperationTimeout = schedule.ItemOperationTimeout,
            SnapshotMoveData = schedule.SnapshotMoveData,
            StorageLocation = schedule.StorageLocation,

            Backups = null,
        };
    }

    private static Backup MapBackup(KubernetesBackup input, ScheduleContext context)
    {
        var backup = new Backup()
        {
            Name = input.Name,
            CreatedOn = input.CreatedOn,
            ExpirationDate = input.ExpirationDate,
            Phase = input.Phase,
            ItemsBackedUp = input.ItemsBackedUp,
            TotalItems = input.TotalItems,
            BackupItemOperationsAttempted = input.BackupItemOperationsAttempted,
            BackupItemOperationsCompleted = input.BackupItemOperationsCompleted,
            Errors = input.Errors,
            StartTimestamp = input.StartTimestamp,
            CompletionTimestamp = input.CompletionTimestamp,
        };

        if (context.Uploads.TryGetValue(backup.Name, out var uploadList))
        {
            backup.Uploads = uploadList.Select(u => MapDataUpload(u, context)).ToList();
        }

        return backup;
    }

    private static DataUpload MapDataUpload(DataUploadModel dataUpload, ScheduleContext context)
    {
        PersistentVolumeClaim pvc = null;
        PersistentVolume pv = null;

        var pvcKey = $"{dataUpload.SourceNamespace}_{dataUpload.SourcePVC}";
        if (context.Pvcs.TryGetValue(pvcKey, out PersistentVolumeClaim pvcValue))
        {
            pvc = pvcValue;

            var pvKey = $"{pvc.Namespace}_{pvc.Name}";
            if (context.Pvs.TryGetValue(pvKey, out PersistentVolume pvValue))
            {
                pv = pvValue;
            }
        }

        return new DataUpload()
        {
            ClaimName = dataUpload.SourcePVC,
            Phase = dataUpload.Phase,
            TotalBytes = dataUpload.TotalBytes,
            BytesDone = dataUpload.BytesDone,
            StartTimestamp = dataUpload.StartTimestamp,
            CompletionTimestamp = dataUpload.CompletionTimestamp,

            VolumeName = pv?.Name,
            CephName = pv?.ImageName,

            Capacity = pvc?.Capacity,
            StorageClassName = pvc?.StorageClassName
        };
    }

    private int GetBackupCount(IEnumerable<KubernetesBackup> backups, int? days, bool? isSuccessful)
    {
        var query = backups;

        if (days.HasValue)
        {
            query = query.Where(b => b.CreatedOn >= DateTime.UtcNow.AddDays(-days.Value));
        }

        if (isSuccessful.HasValue && isSuccessful.Value)
        {
            query = query.Where(b => b.Phase == BackupPhaseType.Completed);
        }

        return query.Count();
    }


    private async Task<ScheduleContext> InitializeScheduleContextAsync()
    {
        var schedules = await kubernetesHelper.GetVeleroSchedulesAsync().ConfigureAwait(false);
        var backups = await kubernetesHelper.GetVeleroBackupsAsync().ConfigureAwait(false);
        var dataUploads = await kubernetesHelper.GetDataUploadsAsync().ConfigureAwait(false);
        var pvcs = await kubernetesHelper.GetPersistanceVolumeClaimsAsync(null).ConfigureAwait(false);
        var pvs = await kubernetesHelper.GetPersistanceVolumesAsync().ConfigureAwait(false);

        var context = new ScheduleContext
        {
            AllSchedules = schedules,
            AllBackups = backups,
            Pvcs = pvcs.ToDictionary(g => $"{g.Namespace}_{g.Name}", g => g),
            Pvs = pvs.ToDictionary(g => $"{g.Namespace}_{g.Claim}", g => g),

            Backups = backups.GroupBy(s => s.ScheduleName)
                            .Select(g => new
                            {
                                Schedule = g.Key,
                                Backups = g.OrderByDescending(b => b.CreatedOn).ToList()
                            })
                            .ToDictionary(g => g.Schedule, g => new BackupDetails(g.Backups, false)),

            Uploads = dataUploads.GroupBy(s => s.Backup)
                            .Select(g => new
                            {
                                Backup = g.Key,
                                Uploads = g.OrderByDescending(b => b.CreatedOn).ToList()
                            })
                            .ToDictionary(g => g.Backup, g => g.Uploads)
        };

        return context;
    }

    private async Task<KopiaContext> InitializeKopiaContextAsync()
    {
        var dataUploads = await kubernetesHelper.GetDataUploadsAsync().ConfigureAwait(false);

        var snapshots = await GetAllSnapshotsAsync().ConfigureAwait(false);

        var context = new KopiaContext()
        {
            Uploads = dataUploads,
            Snapshots = snapshots,
        };

        return context;
    }

    private async Task<IEnumerable<KopiaSnapshot>> GetAllSnapshotsAsync()
    {
        var locations = await kubernetesHelper.GetVeleroBackupStorageLocationsAsync().ConfigureAwait(false);
        var repositories = await kubernetesHelper.GetBackupRepositoriesAsync().ConfigureAwait(false);

        var kopiaPassword = await GetKopiaPasswordAsync().ConfigureAwait(false);

        var snapshots = new List<KopiaSnapshot>();
        foreach (var location in locations)
        {
            var credentials = await GetStorageCredentials(location).ConfigureAwait(false);

            var currentRepositories = repositories.Where(r => r.BackupStorageLocation == location.Name).ToList();
            foreach (var repository in currentRepositories)
            {
                if (!string.IsNullOrEmpty(repository.VolumeNamespace))
                {
                    var kopiaContext = GetKopiaContext(location, repository.VolumeNamespace, credentials, kopiaPassword);

                    var currentSnapshots = kopiaClient.GetSnapshots(kopiaContext);
                    snapshots.AddRange(currentSnapshots);
                }
            }
        }

        return snapshots;
    }

    private static KopiaConnectModel GetKopiaContext(BackupStorageLocationModel location, string volumeNamespace, Credentials credentials, string kopiaPassword)
    {
        return new KopiaConnectModel
        {
            LocationName = location.Name,
            StorageUrl = location.ConfigPublicUrl,
            Bucket = location.Bucket,
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            Password = kopiaPassword,
            VolumeNamespace = volumeNamespace,
        };
    }

    private async Task<string> GetKopiaPasswordAsync()
    {
        if (!cache.TryGetValue(KOPIA_PASSWORD, out string kopiaPassword))
        {
            kopiaPassword = await kubernetesHelper.GetSecretAsync("velero", "velero-repo-credentials", "repository-password").ConfigureAwait(false);

            if (!string.IsNullOrEmpty(kopiaPassword))
            {
                cache.Set(KOPIA_PASSWORD, kopiaPassword, TimeSpan.FromMinutes(60));
            }
        }

        return kopiaPassword;
    }

    private async Task<Credentials> GetStorageCredentials(BackupStorageLocationModel location)
    {
        if (!cache.TryGetValue(STORAGE_CREDENTIALS, out Credentials credentials))
        {
            var locationCredsBody = await kubernetesHelper.GetSecretAsync(location.Namespace, location.SecretName, location.SecretKey).ConfigureAwait(false);
            var locationCredsArray = locationCredsBody.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            if (locationCredsArray.Length < 2)
            {
                return null;
            }

            credentials = new Credentials()
            {
                ClientId = locationCredsArray[0].Split('=', StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault(),
                ClientSecret = locationCredsArray[1].Split('=', StringSplitOptions.RemoveEmptyEntries).Skip(1).FirstOrDefault(),
            };

            cache.Set(STORAGE_CREDENTIALS, credentials, TimeSpan.FromMinutes(60));
        }

        return credentials;
    }
}

internal class BackupDetails(IEnumerable<KubernetesBackup> backups, bool used)
{
    public IEnumerable<KubernetesBackup> Backups { get; } = backups;

    public bool Used { get; set; } = used;
}

internal class ScheduleContext
{
    public IEnumerable<KubernetesSchedule> AllSchedules { get; set; }

    public IEnumerable<KubernetesBackup> AllBackups { get; set; }

    public IDictionary<string, PersistentVolumeClaim> Pvcs { get; set; }

    public IDictionary<string, PersistentVolume> Pvs { get; set; }

    public IDictionary<string, BackupDetails> Backups { get; set; }

    public IDictionary<string, List<DataUploadModel>> Uploads { get; set; }
}

internal class KopiaContext
{
    public IEnumerable<DataUploadModel> Uploads { get; set; }

    public IEnumerable<KopiaSnapshot> Snapshots { get; set; }
}