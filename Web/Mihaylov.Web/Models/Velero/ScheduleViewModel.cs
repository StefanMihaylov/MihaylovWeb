using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mihaylov.Api.Other.Client;

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

        private DataUpload LastUpload => LastBackupModel?.Uploads
                                                        ?.OrderByDescending(u => u.Capacity)
                                                        ?.FirstOrDefault();
        public TimeSpan? Duration => LastBackupModel?.Duration;

        public long? Size => LastUpload?.TotalBytes ?? LastUpload?.BytesDone;

        public string Capacity => LastUpload?.Capacity;

        public string StorageClassType
        {
            get
            {
                var storageClassName = LastUpload?.StorageClassName;
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
    }
}
