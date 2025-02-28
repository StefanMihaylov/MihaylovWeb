using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;

namespace Mihaylov.Web.Models.Velero
{
    public class ScheduleViewModel : Schedule
    {
        public ScheduleViewModel(Schedule input)
        {
            Name = input.Name;
            CreatedOn = input.CreatedOn;
            Cron = input.Cron;
            Paused = input.Paused;
            CsiSnapshotTimeout = input.CsiSnapshotTimeout;
            LastBackup = input.LastBackup;
            Phase = input.Phase;
            IncludedNamespaces = input.IncludedNamespaces;
            ExcludedResources = input.ExcludedResources;
            ItemOperationTimeout = input.ItemOperationTimeout;
            MatchLabels = input.MatchLabels;
            Expiration = input.Expiration;
            SnapshotMoveData = input.SnapshotMoveData;
            StorageLocation = input.StorageLocation;

            Backups = null;
            BackupExtended = input.Backups.Select(b => new BackupViewModel(b));
        }

        public IEnumerable<BackupViewModel> BackupExtended { get; }


        private BackupViewModel LastBackupModel => BackupExtended?.FirstOrDefault();

        public TimeSpan? Duration => LastBackupModel?.Duration;

        public long? Size => GetSize(LastBackupModel?.Uploads);

        public long? Capacity => LastBackupModel?.Uploads?.Sum(a => a.Capacity.GetBytes());

        public string StorageClassType
        {
            get
            {
                var storageClassName = LastBackupModel?.Uploads?.FirstOrDefault()?.StorageClassName;
                if (string.IsNullOrEmpty(storageClassName))
                {
                    return string.Empty;
                }

                return storageClassName switch
                {
                    "rook-ceph-storage" => "Block",
                    "rook-ceph-filesystem" => "File",
                    _ => "unknown",
                };
            }
        }

        public long? SizeStandardDeviation
        {
            get
            {
                var sizeList = BackupExtended.Where(b => b.Uploads != null)
                                             .Select(b => GetSize(b.Uploads))
                                             .Where(s => s.HasValue)
                                             .Select(s => (decimal)s.Value)
                                             .ToList();

                if (sizeList == null || sizeList.Count < 2)
                {
                    return null;
                }

                decimal average = sizeList.Average();
                decimal sumOfSquaresOfDifferences = sizeList.Sum(val => (val - average) * (val - average));

                double standartDeviation = Math.Sqrt((double)(sumOfSquaresOfDifferences / (sizeList.Count - 1)));

                return (long)standartDeviation;
            }
        }

        public int? SizeDeviation
        {
            get
            {
                if (!Size.HasValue || !SizeStandardDeviation.HasValue)
                {
                    return null;
                }

                var result = (decimal)SizeStandardDeviation.Value / Size.Value;
                return (int)(result * 100);
            }
        }

        public string PopupContent
        {
            get
            {
                var builder = new StringBuilder();

                builder.Append("<ul class='custom-popover'>");
                BackupViewModel.AddPopupLine(builder, "Name", Name);
                BackupViewModel.AddPopupLine(builder, "Phase", Phase?.ToString());
                BackupViewModel.AddPopupLine(builder, "Created", CreatedOn?.ToString("yyyy-MM-dd HH:mm:ss"));
                BackupViewModel.AddPopupLine(builder, "CRON", Cron);
                BackupViewModel.AddPopupLine(builder, "Paused", Paused?.ToString());
                BackupViewModel.AddPopupLine(builder, "LastBackup", LastBackup?.ToString("yyyy-MM-dd HH:mm:ss"));
                BackupViewModel.AddPopupLine(builder, "CsiTimeout", CsiSnapshotTimeout);
                BackupViewModel.AddPopupLine(builder, "ItemTimeout", ItemOperationTimeout);
                BackupViewModel.AddPopupLine(builder, "MatchLabels", MatchLabels);
                BackupViewModel.AddPopupLine(builder, "Expiration", Expiration);
                BackupViewModel.AddPopupLine(builder, "SnapshotMoveData", SnapshotMoveData.ToString());
                BackupViewModel.AddPopupLine(builder, "StorageLocation", StorageLocation);

                if (IncludedNamespaces != null && IncludedNamespaces.Any())
                {
                    BackupViewModel.AddPopupLine(builder, "IncludedNamespaces");
                    builder.Append("<ul class='custom-popover'>");
                    foreach (var namespaceName in IncludedNamespaces)
                    {
                        BackupViewModel.AddPopupLine(builder, namespaceName);
                    }

                    builder.Append("</ul>");
                }

                if (ExcludedResources != null && ExcludedResources.Any())
                {
                    BackupViewModel.AddPopupLine(builder, "ExcludedResources");
                    builder.Append("<ul class='custom-popover'>");
                    foreach (var resource in ExcludedResources)
                    {
                        BackupViewModel.AddPopupLine(builder, resource);
                    }

                    builder.Append("</ul>");
                }

                builder.Append("</ul>");

                return builder.ToString();
            }
        }

        private long? GetSize(IEnumerable<DataUpload> uploads)
        {
            if (uploads == null || !uploads.Any())
            {
                return null;
            }

            var query = uploads.Where(f => (f.TotalBytes ?? f.BytesDone).HasValue);
            if (!query.Any())
            {
                return null;
            }

            long result = query.Sum(a => a.TotalBytes ?? a.BytesDone.Value);
            return result;
        }
    }
}
