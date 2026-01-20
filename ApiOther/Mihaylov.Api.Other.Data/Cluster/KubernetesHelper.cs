using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;
using Mihaylov.Api.Other.Data.Cluster.Models;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class KubernetesHelper : IKubernetesHelper
    {
        private readonly KubernetesSettings _config;
        private readonly KubernetesClientConfiguration _kubeConfig;

        public KubernetesHelper(IOptions<KubernetesSettings> settings)
        {
            _config = settings.Value;
            _kubeConfig = null;

            if (!string.IsNullOrWhiteSpace(_config.ServiceHost) &&
               !string.IsNullOrWhiteSpace(_config.ServicePort))
            {
                _kubeConfig = KubernetesClientConfiguration.InClusterConfig();
            }

            if (_kubeConfig == null && !string.IsNullOrWhiteSpace(_config.ConfigPath))
            {
                _kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile(_config.ConfigPath);
            }

            if (_kubeConfig == null)
            {
                throw new Exception("Kubernetes configuration is not valid.");
            }
        }

        public async Task<IEnumerable<KubeNamespace>> GetNamespacesAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.CoreV1.ListNamespaceAsync().ConfigureAwait(false);
            var namespaces = list.Items.Select(n => new KubeNamespace()
            {
                Id = Guid.Parse(n.Metadata.Uid),
                Name = n.Metadata.Name,
                CreatedOn = n.Metadata.CreationTimestamp,
                Labels = n.Metadata.Labels
            })
            .ToList();

            return namespaces;
        }

        public async Task<IEnumerable<StorageClass>> GetStorageClassesAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.StorageV1.ListStorageClassAsync().ConfigureAwait(false);

            var storageClasses = list.Items.Select(sc => new StorageClass()
            {
                Id = Guid.Parse(sc.Metadata.Uid),
                Name = sc.Metadata.Name,
                CreatedOn = sc.Metadata.CreationTimestamp,
                Parameters = sc.Parameters,
                Provisioner = sc.Provisioner,
                ReclaimPolicy = sc.ReclaimPolicy,
            })
            .ToList();

            return storageClasses;
        }

        public async Task<IEnumerable<PersistentVolumeClaim>> GetPersistanceVolumeClaimsAsync(string namespaceName)
        {
            V1PersistentVolumeClaimList list;
            using Kubernetes client = GetClient();
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                list = await client.CoreV1.ListPersistentVolumeClaimForAllNamespacesAsync().ConfigureAwait(false);
            }
            else
            {
                list = await client.CoreV1.ListNamespacedPersistentVolumeClaimAsync(namespaceName).ConfigureAwait(false);
            }

            var aaa = list.Items.Last();

            var pvcs = list.Items.Select(pvc => new PersistentVolumeClaim()
            {
                Id = Guid.Parse(pvc.Metadata.Uid),
                Name = pvc.Metadata.Name,
                CreatedOn = pvc.Metadata.CreationTimestamp,
                Labels = pvc.Metadata.Labels,
                Namespace = pvc.Metadata.NamespaceProperty,
                Status = pvc.Status.Phase,
                Capacity = Normalize(pvc.Status.Capacity?.Where(kv => kv.Key == "storage").Select(kv => kv.Value).FirstOrDefault()?.ToString()),
                VolumeName = pvc.Spec.VolumeName,
                VolumeMode = pvc.Spec.VolumeMode,
                StorageClassName = pvc.Spec.StorageClassName,
                AccessMode = pvc.Status.AccessModes?.FirstOrDefault(),

            }).ToList();

            return pvcs;
        }

        public async Task<IEnumerable<PersistentVolume>> GetPersistanceVolumesAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.CoreV1.ListPersistentVolumeAsync().ConfigureAwait(false);

            var pvs = list.Items.Select(pv => new PersistentVolume()
            {
                Id = Guid.Parse(pv.Metadata.Uid),
                Name = pv.Metadata.Name,
                CreatedOn = pv.Metadata.CreationTimestamp,
                Labels = pv.Metadata.Labels,
                Capacity = Normalize(pv.Spec.Capacity.Where(kv => kv.Key == "storage").Select(kv => kv.Value).FirstOrDefault()?.ToString()),
                AccessMode = pv.Spec.AccessModes.FirstOrDefault(),
                ReclaimPolicy = pv.Spec.PersistentVolumeReclaimPolicy,
                StorageClassName = pv.Spec.StorageClassName,
                Status = pv.Status.Phase,
                Claim = pv.Spec.ClaimRef?.Name,
                ClaimId = Guid.Parse(pv.Spec.ClaimRef?.Uid),
                Namespace = pv.Spec.ClaimRef?.NamespaceProperty,
                Path = pv.Spec.HostPath?.Path,
                csiVolumeName = pv.Spec.Csi?.VolumeHandle,
                VolumeAttributes = pv.Spec.Csi?.VolumeAttributes,
                ImageName = pv.Spec.Csi?.VolumeAttributes?.FirstOrDefault(a => a.Key == "imageName" || a.Key == "subvolumeName").Value,
                ImagePool = pv.Spec.Csi?.VolumeAttributes?.FirstOrDefault(a => a.Key == "pool").Value,
            })
            .ToList();

            return pvs;
        }

        public async Task<IEnumerable<CustomResource>> GetCustomResourcesAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.ListCustomResourceDefinitionAsync().ConfigureAwait(false);

            var crds = list.Items.Select(cr => new CustomResource()
            {
                Name = cr.Metadata.Name,
                CreatedOn = cr.Metadata.CreationTimestamp,
                Group = cr.Spec.Group,
                Kind = cr.Spec.Names.Kind,
                Plural = cr.Spec.Names.Plural,
                Version = cr.Spec.Versions.FirstOrDefault()?.Name
            })
            .OrderBy(cr => cr.Group)
            .ThenBy(cr => cr.Kind)
            .ToList();

            return crds;
        }

        public async Task<IEnumerable<KubernetesSchedule>> GetVeleroSchedulesAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.ListClusterCustomObjectAsync<ScheduleModelList>("velero.io", "v1", "schedules").ConfigureAwait(false);

            var shedules = list.Items.Select(s => new KubernetesSchedule()
            {
                Name = s.Metadata.Name,
                CreatedOn = s.Metadata.CreationTimestamp,

                Schedule = s.Spec.Schedule,
                Paused = s.Spec.Paused,
                CsiSnapshotTimeout = s.Spec.Template.CsiSnapshotTimeout,
                IncludedNamespaces = s.Spec.Template.IncludedNamespaces,
                ExcludedResources = s.Spec.Template.ExcludedResources,
                MatchLabels = s.Spec.Template.LabelSelector?.MatchLabels,
                ItemOperationTimeout = s.Spec.Template.ItemOperationTimeout,
                SnapshotMoveData = s.Spec.Template.SnapshotMoveData,
                StorageLocation = s.Spec.Template.StorageLocation,
                Expiration = s.Spec.Template.Ttl,

                LastBackup = s.Status.LastBackup,
                Phase = Enum.TryParse(s.Status.Phase, true, out SchedulePhaseType phase) ? phase : null,
            })
            .OrderBy(s => s.Name)
            .ToList();

            return shedules;
        }

        public async Task<IEnumerable<KubernetesBackup>> GetVeleroBackupsAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.ListClusterCustomObjectAsync<BackupModelList>("velero.io", "v1", "backups").ConfigureAwait(false);

            var backups = list.Items.Select(b => new KubernetesBackup()
            {
                Name = b.Metadata.Name,
                CreatedOn = b.Metadata.CreationTimestamp,
                
                ScheduleName = b.Metadata.Labels.Where(a => a.Key == "velero.io/schedule-name").Select(a => a.Value).FirstOrDefault(),
                BackupItemOperationsAttempted = b.Status.BackupItemOperationsAttempted,
                BackupItemOperationsCompleted = b.Status.BackupItemOperationsCompleted,
                Errors = b.Status.Errors,
                StartTimestamp = b.Status.StartTimestamp,
                CompletionTimestamp = b.Status.CompletionTimestamp,
                ExpirationDate = b.Status.Expiration,
                Phase = Enum.TryParse(b.Status.Phase, true, out BackupPhaseType phase) ? phase : null,
                ItemsBackedUp = b.Status.Progress?.ItemsBackedUp,
                TotalItems = b.Status.Progress?.TotalItems,
            })
             .OrderBy(b => b.Name)
             .ToList();

            return backups;
        }

        public async Task<IEnumerable<DataUploadModel>> GetDataUploadsAsync()
        {
            using Kubernetes client = GetClient();
            var list = await client.ListClusterCustomObjectAsync<DataUploadModelList>("velero.io", "v2alpha1", "datauploads").ConfigureAwait(false);

            var uploads = list.Items.Select(a => new DataUploadModel()
            {
                Name = a.Metadata.Name,
                CreatedOn = a.Metadata.CreationTimestamp,
                Backup = a.Metadata.OwnerReferences?.FirstOrDefault()?.Name,
                BackupStorageLocation = a.Spec.BackupStorageLocation,
                OperationTimeout = a.Spec.OperationTimeout,
                SourcePVC = a.Spec.SourcePVC,
                SourceNamespace = a.Spec.SourceNamespace,
                SnapshotType = a.Spec.SnapshotType,
                CompletionTimestamp = a.Status.CompletionTimestamp,
                SnapshotID = a.Status.SnapshotID,
                StartTimestamp = a.Status.StartTimestamp,
                Phase = Enum.TryParse(a.Status.Phase, true, out DataUploadPhaseType phase) ? phase : null,
                TotalBytes = a.Status.Progress.TotalBytes,
                BytesDone = a.Status.Progress.BytesDone,
            })
            .ToList();

            return uploads;
        }

        // var aaaa = JsonSerializer.Serialize(list, new JsonSerializerOptions() { WriteIndented = true });

        private Kubernetes GetClient()
        {
            return new Kubernetes(_kubeConfig);
        }

        public static string Normalize(string input)
        {
            NumberStyles numberStyles = NumberStyles.Float | NumberStyles.AllowThousands;
            IFormatProvider formatProvider = NumberFormatInfo.CurrentInfo;

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            input = input.Trim();

            var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
            var decimalSeparator = Convert.ToChar(numberFormatInfo.NumberDecimalSeparator);
            var groupSeparator = Convert.ToChar(numberFormatInfo.NumberGroupSeparator);

            var found = false;
            int num;
            for (num = 0; num < input.Length; num++)
            {
                char current = input[num];
                if (!(char.IsDigit(current) || current == decimalSeparator || current == groupSeparator))
                {
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                throw new FormatException($"No byte indicator found in value '{input}'.");
            }                

            int lastNumber = num;
            string numberPart = input.Substring(0, lastNumber).Trim();

            if (!double.TryParse(numberPart, numberStyles, formatProvider, out double number))
            {
                throw new FormatException($"No number found in value '{input}'.");
            }            

            string sizePart = input.Substring(lastNumber, input.Length - lastNumber).Trim();
            
            return $"{number} {sizePart}B";
        }
    }
}
