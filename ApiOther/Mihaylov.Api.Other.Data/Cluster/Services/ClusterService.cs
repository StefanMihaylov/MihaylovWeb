using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Services
{
    public class ClusterService : IClusterService
    {
        private readonly ILogger _logger;
        private readonly IApplicationRepository _appRepository;
        private readonly IPodRepository _podRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IVersionRepository _versionRepository;
        private readonly IMemoryCache _memoryCache;

        private const string SHOW_ALL_APPLICATIONS = "show_all_applications";
        private const int CACHE_DURATION = 30;

        public ClusterService(ILoggerFactory loggerFactory, IApplicationRepository appRepository,
            IPodRepository podRepository, IFileRepository fileRepository, IVersionRepository versionRepository,
            IMemoryCache memoryCache)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _appRepository = appRepository;
            _podRepository = podRepository;
            _fileRepository = fileRepository;
            _versionRepository = versionRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<ApplicationExtended>> GetAllApplicationsAsync()
        {
            if (!_memoryCache.TryGetValue(SHOW_ALL_APPLICATIONS, out IEnumerable<ApplicationExtended> applications))
            {
                applications = await _appRepository.GetAllAsync().ConfigureAwait(false);

                _memoryCache.Set(SHOW_ALL_APPLICATIONS, applications, TimeSpan.FromMinutes(CACHE_DURATION));
            }

            return applications;
        }

        public async Task<ApplicationExtended> AddOrUpdateApplicationAsync(Application model)
        {
            var application = await _appRepository.AddOrUpdateAsync(model).ConfigureAwait(false);
            ClearCache();

            return application;
        }

        public async Task<Pod> AddOrUpdatePodAsync(Pod model, int applicationId)
        {
            var pod = await _podRepository.AddOrUpdateAsync(model, applicationId).ConfigureAwait(false);
            ClearCache();

            return pod;
        }

        public async Task<DeploymentFile> AddOrUpdateFileAsync(DeploymentFile model, int applicationId)
        {
            var file = await _fileRepository.AddOrUpdateAsync(model, applicationId).ConfigureAwait(false);
            ClearCache();

            return file;
        }

        public async Task<AppVersion> AddOrUpdateVersionAsync(AppVersion model, int applicationId)
        {
            var version = await _versionRepository.AddOrUpdateAsync(model, applicationId).ConfigureAwait(false);
            ClearCache();

            return version;
        }

        private void ClearCache()
        {
            _memoryCache.Remove(SHOW_ALL_APPLICATIONS);
        }
    }
}
