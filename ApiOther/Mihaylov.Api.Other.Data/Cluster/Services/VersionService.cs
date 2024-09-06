﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Services
{
    public class VersionService : IVersionService
    {
        private readonly ILogger _logger;
        private readonly IClusterService _clusterService;
        private readonly IMemoryCache _memoryCache;

        private const string SHOW_LAST_VERSION = "show_last_version_by_application";
        private const int CACHE_DURATION = 30;

        public VersionService(ILoggerFactory loggerFactory, IClusterService clusterService, IMemoryCache memoryCache)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _clusterService = clusterService;
            _memoryCache = memoryCache;
        }

        public async Task<LastVersionModel> GetLastVersionAsync(int applicationId)
        {
            string key = GetKey(applicationId);
            if (!_memoryCache.TryGetValue(key, out LastVersionModel lastVersion))
            {
                lastVersion = await GetLastVersionOnlineAsync(applicationId).ConfigureAwait(false);

                _memoryCache.Set(key, lastVersion, TimeSpan.FromMinutes(CACHE_DURATION));
            }

            return lastVersion;
        }

        public async Task<LastVersionModel> GetLastVersionOnlineAsync(int applicationId)
        {
            var applications = await _clusterService.GetAllApplicationsAsync().ConfigureAwait(false);
            var settings = await _clusterService.GetParserSettingsAsync().ConfigureAwait(false);

            var application = applications.FirstOrDefault(a => a.Id == applicationId);
            if (application == null)
            {
                return null;
            }

            var setting = settings.Where(s => s.ApplicationId == application.Id)
                                  .OrderByDescending(s => s.Id)
                                  .FirstOrDefault();
            if (setting == null)
            {
                return null;
            }

            var configuration = new LastVersionSettings()
            {
                ApplicationId = setting.ApplicationId,
                Version = new ParseModel()
                {
                    Url = GetUrlByType(setting.VersionUrlType, application),
                    Selector = setting.VersionSelector,
                    Command = setting.VersionCommand,
                },
                ReleaseDate = new ParseModel()
                {
                    Url = GetUrlByType(setting.ReleaseDateUrlType, application),
                    Selector = setting.ReleaseDateSelector,
                    Command = setting.ReleaseDateCommand,
                }
            };

            var result = await GetLastVersionAsync(configuration).ConfigureAwait(false);

            return result;
        }

        public async Task<LastVersionModel> TestLastVersionAsync(LastVersionSettings configuration)
        {
            var result = await GetLastVersionAsync(configuration).ConfigureAwait(false);

            return result;
        }

        public void Reload(int applicationId)
        {
            string key = GetKey(applicationId);
            _memoryCache.Remove(key);
        }

        private static string GetKey(int applicationId)
        {
            return $"{SHOW_LAST_VERSION}_{applicationId}";
        }

        private async Task<LastVersionModel> GetLastVersionAsync(LastVersionSettings configuration)
        {
            var version = await GetValueAsync(configuration.Version, null).ConfigureAwait(false);
            var release = await GetValueAsync(configuration.ReleaseDate, version).ConfigureAwait(false);

            DateTime? releaseDate = ParseDate(release?.Value);

            if (string.IsNullOrEmpty(version?.Value) || !releaseDate.HasValue)
            {
                return null;
            }

            var result = new LastVersionModel()
            {
                ApplicationId = configuration.ApplicationId,
                Version = version.Value,
                ReleaseDate = releaseDate.Value.Date,
            };

            return result;
        }

        private async Task<ValueContext> GetValueAsync(ParseModel configuration, ValueContext previous)
        {
            var address = configuration?.Url;
            var inputSelector = configuration?.Selector;

            string content = previous?.Content;
            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(inputSelector))
            {
                var selectorParts = inputSelector.Split('|');
                var selector = selectorParts[0];

                string attributeName = null;
                if (selectorParts.Length > 1)
                {
                    attributeName = selectorParts[1];
                }

                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(address);

                var cells = document.QuerySelectorAll(selector)?.ToList();
                var cell = cells?.FirstOrDefault();

                if (cell == null)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(attributeName))
                {
                    content = cell?.TextContent;
                }
                else
                {
                    content = cell.Attributes.Where(a => a.Name == attributeName).FirstOrDefault()?.Value;
                }
            }

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError("Content is empty.");
                return null;
            }

            var result = RunCommand(content, configuration.Command);

            return result;
        }

        private ValueContext RunCommand(string content, string inputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand))
            {
                inputCommand = "trim^|";
            }

            var commands = inputCommand.Split('§', StringSplitOptions.RemoveEmptyEntries);

            string previousValue = content;
            string value = string.Empty;

            foreach (var command in commands)
            {
                var commandParts = command.Split('^', StringSplitOptions.RemoveEmptyEntries);
                if (commandParts.Length != 2)
                {
                    _logger.LogError($"{command} command is not valid.");

                    return null;
                }

                var commandName = commandParts[0];
                var commandParams = commandParts[1].Split('|');

                switch (commandName)
                {
                    case "trim":
                        value = Trim(previousValue, commandParams);
                        break;
                    case "split":
                        value = Split(previousValue, commandParams);
                        break;
                    default:
                        throw new ArgumentException($"Unknown command {commandName}");
                }

                previousValue = value;
            }

            return new ValueContext(content, value);
        }

        private string Trim(string text, string[] commandParams)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (commandParams.Length < 2)
            {
                _logger.LogError("Trim parameters are not collect");
                return null;
            }

            var parameterStart = commandParams[0];
            var parameterEnd = commandParams[1];

            var value = text.Trim();

            if (!string.IsNullOrEmpty(parameterStart))
            {
                value = value.TrimStart(parameterStart[0]);
            }

            if (!string.IsNullOrEmpty(parameterEnd))
            {
                value = value.TrimEnd(parameterEnd[0]);
            }

            return value;
        }

        private string Split(string text, string[] commandParams)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (commandParams.Length < 2)
            {
                _logger.LogError("Split parameters are not collect");
                return null;
            }

            var splits = text.Split(commandParams[0], StringSplitOptions.RemoveEmptyEntries);

            int index = int.Parse(commandParams[1]);
            string value = splits[index];

            return value;
        }

        private DateTime? ParseDate(string input)
        {
            DateTime? releaseDate = null;
            if (!string.IsNullOrEmpty(input) && DateTime.TryParse(input, out DateTime date))
            {
                releaseDate = date;
            }

            return releaseDate;
        }

        private string GetUrlByType(VersionUrlType? type, Application application)
        {
            if (type == null)
            {
                return null;
            }

            switch (type.Value)
            {
                case VersionUrlType.SiteUrl:
                    return application.SiteUrl;
                case VersionUrlType.ReleaseUrl:
                    return application.ReleaseUrl;
                case VersionUrlType.GithubVersionUrl:
                    return application.GithubVersionUrl;
                case VersionUrlType.ResourceUrl:
                    return application.ResourceUrl;
                default:
                    throw new ArgumentException("Unknown VersionUrlType");
            }
        }

        private class ValueContext
        {
            public string Value { get; private set; }

            public string Content { get; private set; }

            public ValueContext(string content, string value)
            {
                Content = content;
                Value = value;
            }
        }
    }
}
