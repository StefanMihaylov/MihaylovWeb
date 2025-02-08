using Mihaylov.Common;
using System.Text;
using Mihaylov.Api.Other.Client;
using System.Linq;
using System;

namespace Mihaylov.Web.Models.Velero
{
    public class BackupViewModel : Backup
    {
        public BackupViewModel(Backup input)
        {
            Name = input.Name;
            CreatedOn = input.CreatedOn;
            BackupItemOperationsAttempted = input.BackupItemOperationsAttempted;
            BackupItemOperationsCompleted = input.BackupItemOperationsCompleted;
            Errors = input.Errors;
            StartTimestamp = input.StartTimestamp;
            CompletionTimestamp = input.CompletionTimestamp;
            ExpirationDate = input.ExpirationDate;
            Phase = input.Phase;
            ItemsBackedUp = input.ItemsBackedUp;
            TotalItems = input.TotalItems;
            Uploads = input.Uploads;
        }

        public TimeSpan? Duration => CompletionTimestamp - StartTimestamp;

        public string PopupContent
        {
            get
            {
                var builder = new StringBuilder();

                builder.Append("<ul class='custom-popover'>");
                AddPopupLine(builder, "Name", Name);
                AddPopupLine(builder, "Delete", $"<a href='/Velero/DeleteBackup/?backup={Name}' class='button-red'><i class='bi bi-x-circle-fill'></i></a>");
                AddPopupLine(builder, "Phase", Phase?.ToString());
                AddPopupLine(builder, "Created", CreatedOn?.ToString("yyyy-MM-dd HH:mm:ss"));
                AddPopupLine(builder, "Completed", CompletionTimestamp?.ToString("yyyy-MM-dd HH:mm:ss"));
                AddPopupLine(builder, "Expiration", ExpirationDate?.ToString("yyyy-MM-dd HH:mm:ss"));
                AddPopupLine(builder, "Duration", Duration?.ToString());
                AddPopupLine(builder, "Items", $"{ItemsBackedUp ?? 0} / {TotalItems}");
                AddPopupLine(builder, "Backup", $"{BackupItemOperationsCompleted ?? 0} / {BackupItemOperationsAttempted ?? 0}");
                AddPopupLine(builder, "Errors", Errors?.ToString());
                builder.Append("</ul>");

                if (Uploads != null && Uploads.Any())
                {
                    builder.Append("<ol class='custom-popover'>");
                    foreach (var upload in Uploads)
                    {
                        AddPopupLine(builder, "PVC", upload.ClaimName);
                        builder.Append("<ul class='custom-popover'>");
                        AddPopupLine(builder, "Volume", upload.VolumeName);
                        AddPopupLine(builder, "Ceph", upload.CephName);
                        AddPopupLine(builder, "Started", upload.StartTimestamp?.ToString("yyyy-MM-dd HH:mm:ss"));
                        AddPopupLine(builder, "Duration", (upload.CompletionTimestamp - upload.StartTimestamp)?.ToString());
                        AddPopupLine(builder, "Phase", upload.Phase?.ToString());
                        AddPopupLine(builder, "Storage", upload.StorageClassName);
                        AddPopupLine(builder, "Bytes", $"{upload.BytesDone?.FormatBytes()} / {upload.TotalBytes?.FormatBytes()}");
                        AddPopupLine(builder, "Capacity", upload.Capacity?.ToString());
                        builder.Append("</ul>");
                    }

                    builder.Append("</ol>");
                }

                return builder.ToString();
            }
        }

        public static void AddPopupLine(StringBuilder builder, string key, string value)
        {
            builder.Append($"<li>{key} : {value}</li>");
        }

        public static void AddPopupLine(StringBuilder builder, string key)
        {
            builder.Append($"<li>{key}</li>");
        }
    }
}
