using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class ClusterService : IClusterService
    {
        private readonly ILogger _logger;
        private readonly IApplicationRepository _appRepository;
        private readonly IPodRepository _podRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IVersionRepository _versionRepository;
        private readonly IParserSettingRepository _parserRepository;
        private readonly IMemoryCache _memoryCache;

        private const string SHOW_ALL_APPLICATIONS = "show_all_applications";
        private const string SHOW_ALL_SETTINGS = "show_all_parser_settings";
        private const int CACHE_DURATION = 30;

        public ClusterService(ILoggerFactory loggerFactory, IApplicationRepository appRepository,
            IPodRepository podRepository, IFileRepository fileRepository, IVersionRepository versionRepository,
            IParserSettingRepository parserRepository, IMemoryCache memoryCache)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _appRepository = appRepository;
            _podRepository = podRepository;
            _fileRepository = fileRepository;
            _versionRepository = versionRepository;
            _parserRepository = parserRepository;
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

        public Task<IEnumerable<VersionHistory>> GetVersionsAsync(int applicationId, int size)
        {
            return _versionRepository.GetVersionsAsync(applicationId, size);
        }

        public async Task<AppVersion> AddOrUpdateVersionAsync(AppVersion model, int applicationId)
        {
            var oldVersions = await _versionRepository.GetAllVersionsAsync(applicationId).ConfigureAwait(false);
            var oldVersion = oldVersions.Where(v => v.Version == model.Version && v.ReleaseDate == model.ReleaseDate).FirstOrDefault();
            if (oldVersion != null)
            {
                _logger.LogInformation($"Duplicate version detected: {model.Version} for applicationId: {applicationId}");
                return oldVersion;
            }

            var version = await _versionRepository.AddOrUpdateAsync(model, applicationId).ConfigureAwait(false);
            ClearCache();

            return version;
        }

        public async Task<IEnumerable<ParserSetting>> GetParserSettingsAsync()
        {
            if (!_memoryCache.TryGetValue(SHOW_ALL_SETTINGS, out IEnumerable<ParserSetting> settings))
            {
                settings = await _parserRepository.GetAllAsync().ConfigureAwait(false);

                _memoryCache.Set(SHOW_ALL_SETTINGS, settings, TimeSpan.FromMinutes(CACHE_DURATION));
            }

            return settings;
        }

        public async Task<ParserSetting> AddOrUpdateParserSettingAsync(ParserSetting model)
        {
            var settings = await _parserRepository.AddOrUpdateAsync(model).ConfigureAwait(false);
            ClearCacheSettings();

            return settings;
        }

        private void ClearCache()
        {
            _memoryCache.Remove(SHOW_ALL_APPLICATIONS);
        }

        private void ClearCacheSettings()
        {
            _memoryCache.Remove(SHOW_ALL_SETTINGS);
        }
    }
}
