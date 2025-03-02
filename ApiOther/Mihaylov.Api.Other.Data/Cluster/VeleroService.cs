using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Velero;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class VeleroService : IVeleroService
    {
        private readonly IKubernetesHelper _kubernetesHelper;
        private readonly IVeleroClient _veleroClient;

        public VeleroService(IKubernetesHelper kubernetesHelper, IVeleroClient veleroClient)
        {
            _kubernetesHelper = kubernetesHelper;
            _veleroClient = veleroClient;
        }

        public Task<string> GetExeVersionAsync()
        {
            return _veleroClient.GetVersionAsync();
        }

        public Task<string> CreateBackupAsync(string scheduleName)
        {
            return _veleroClient.CreateBackupAsync(scheduleName);
        }

        public Task<string> DeleteBackupAsync(string backupName)
        {
            return _veleroClient.DeleteBackupAsync(backupName);
        }

        public async Task<ScheduleResponse> GetSchedulesAsync()
        {
            var schedules = await _kubernetesHelper.GetVeleroSchedulesAsync().ConfigureAwait(false);
            var backups = await _kubernetesHelper.GetVeleroBackupsAsync().ConfigureAwait(false);
            var dataUploads = await _kubernetesHelper.GetDataUploadsAsync().ConfigureAwait(false);
            var pvcs = await _kubernetesHelper.GetPersistanceVolumeClaimsAsync(null).ConfigureAwait(false);
            var pvs = await _kubernetesHelper.GetPersistanceVolumesAsync().ConfigureAwait(false);

            var pvcDic = pvcs.ToDictionary(g => $"{g.Namespace}_{g.Name}", g => g);
            var pvDic = pvs.ToDictionary(g => $"{g.Namespace}_{g.Claim}", g => g);

            var backupDic = backups.GroupBy(s => s.ScheduleName)
                            .Select(g => new
                            {
                                Schedule = g.Key,
                                Backups = g.OrderByDescending(b => b.CreatedOn).ToList()
                            })
                            .ToDictionary(g => g.Schedule, g => new BackupDetails(g.Backups, false));

            var uploadDic = dataUploads.GroupBy(s => s.Backup)
                            .Select(g => new
                            {
                                Backup = g.Key,
                                Uploads = g.OrderByDescending(b => b.CreatedOn).ToList()
                            })
                            .ToDictionary(g => g.Backup, g => g.Uploads);

            var scheduleList = new List<Schedule>();
            foreach (var schedule in schedules)
            {
                if (backupDic.ContainsKey(schedule.Name))
                {
                    var backupDetails = backupDic[schedule.Name];
                    backupDetails.Used = true;

                    scheduleList.Add(new Schedule()
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

                        Backups = backupDetails.Backups.Select(b => MapBackup(b)).ToList(),
                    });
                }
            }

            var oldSchedules = backupDic.Where(b => !b.Value.Used)
                                         .ToDictionary(b => b.Key, b => b.Value.Backups);

            foreach (var oldSchedule in oldSchedules)
            {
                scheduleList.Add(new Schedule()
                {
                    Name = oldSchedule.Key,
                    Backups = oldSchedule.Value.Select(b => MapBackup(b)).ToList(),
                });
            }

            foreach (var schedule in scheduleList)
            {
                foreach (var backup in schedule.Backups)
                {
                    if (uploadDic.ContainsKey(backup.Name))
                    {
                        var uploadList = uploadDic[backup.Name];

                        backup.Uploads = uploadList.Select(u =>
                        {
                            PersistentVolumeClaim pvc = null;
                            PersistentVolume pv = null;

                            var pvcKey = $"{u.SourceNamespace}_{u.SourcePVC}";
                            if (pvcDic.ContainsKey(pvcKey))
                            {
                                pvc = pvcDic[pvcKey];

                                var pvKey = $"{pvc.Namespace}_{pvc.Name}";
                                if (pvDic.ContainsKey(pvKey))
                                {
                                    pv = pvDic[pvKey];
                                }
                            }

                            return new DataUpload()
                            {
                                ClaimName = u.SourcePVC,
                                VolumeName = pv?.Name,
                                CephName = pv?.ImageName,
                                Phase = u.Phase,
                                TotalBytes = u.TotalBytes,
                                BytesDone = u.BytesDone,
                                StartTimestamp = u.StartTimestamp,
                                CompletionTimestamp = u.CompletionTimestamp,
                                Capacity = pvc?.Capacity,
                                StorageClassName = pvc?.StorageClassName
                            };
                        }).ToList();
                    }
                }
            }

            var result = new ScheduleResponse()
            {
                Statistics = new ScheduleStatistics()
                {
                    ScheduleCount = schedules.Count(),
                    TotalBackupCount = GetBackupCount(backups, null, null),
                    LastWeekBackupCount = GetBackupCount(backups, 7, null),
                    LastDayBackupCount = GetBackupCount(backups, 1, null),
                    TotalSuccessfulBackupCount = GetBackupCount(backups, null, true),
                    LastWeekSuccessfulBackupCount = GetBackupCount(backups, 7, true),
                    LastDaySuccessfulBackupCount = GetBackupCount(backups, 1, true),
                },
                Schedules = scheduleList.OrderByDescending(s => s.LastBackup).ToList(),
            };

            return result;
        }

        private static Backup MapBackup(KubernetesBackup b)
        {
            return new Backup()
            {
                Name = b.Name,
                CreatedOn = b.CreatedOn,
                ExpirationDate = b.ExpirationDate,
                Phase = b.Phase,
                ItemsBackedUp = b.ItemsBackedUp,
                TotalItems = b.TotalItems,
                BackupItemOperationsAttempted = b.BackupItemOperationsAttempted,
                BackupItemOperationsCompleted = b.BackupItemOperationsCompleted,
                Errors = b.Errors,
                StartTimestamp = b.StartTimestamp,
                CompletionTimestamp = b.CompletionTimestamp,
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
    }

    internal class BackupDetails
    {
        public IEnumerable<KubernetesBackup> Backups { get; }

        public bool Used { get; set; }

        public BackupDetails(IEnumerable<KubernetesBackup> backups, bool used)
        {
            Backups = backups;
            Used = used;
        }
    }
}
