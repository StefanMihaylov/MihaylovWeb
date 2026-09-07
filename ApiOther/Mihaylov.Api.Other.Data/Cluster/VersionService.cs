using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Version;

namespace Mihaylov.Api.Other.Data.Cluster
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
            _logger = loggerFactory.CreateLogger(GetType());
            _clusterService = clusterService;
            _memoryCache = memoryCache;
        }

        public async Task<LastVersionModel> GetLastVersionAsync(int applicationId, bool? reload)
        {
            string key = GetKey(applicationId);

            if (reload == true)
            {
                _memoryCache.Remove(key);
            }

            if (!_memoryCache.TryGetValue(key, out LastVersionModel lastVersion))
            {
                lastVersion = await GetLastVersionOnlineAsync(applicationId).ConfigureAwait(false);

                if (lastVersion != null)
                {
                    _memoryCache.Set(key, lastVersion, TimeSpan.FromMinutes(CACHE_DURATION));
                }
            }

            return lastVersion;
        }

        public async Task<LastVersionModel> GetLastVersionOnlineAsync(int applicationId)
        {
            try
            {
                var applications = await _clusterService.GetAllApplicationsAsync().ConfigureAwait(false);
                var application = applications.FirstOrDefault(a => a.Id == applicationId);
                if (application == null)
                {
                    return null;
                }

                var settings = await _clusterService.GetParserSettingsAsync().ConfigureAwait(false);

                ParserSetting setting;
                if (application.ParserSettingId.HasValue)
                {
                    setting = settings.Single(s => s.Id == application.ParserSettingId.Value);
                }
                else
                {
                    setting = settings.Where(s => s.ApplicationId == application.Id)
                                      .OrderByDescending(s => s.Id)
                                      .FirstOrDefault();
                }

                if (setting == null)
                {
                    return null;
                }

                var configuration = new LastVersionSettings()
                {
                    ApplicationId = application.Id,
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

                var result = await ComputeLastVersionAsync(configuration).ConfigureAwait(false);

                _logger.LogInformation("GetLastVersionOnline succeeded. ApplicationId: {applicationId}, Version: {version}, ReleaseDate: {releaseDate:yyyy.MM.dd}",
                    applicationId, result.Version, result.ReleaseDate);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetLastVersionOnline failed. Error: {error}", ex.Message);
                throw;
            }
        }

        public async Task<LastVersionModel> TestLastVersionAsync(LastVersionSettings configuration)
        {
            var result = await ComputeLastVersionAsync(configuration).ConfigureAwait(false);

            return result;
        }

        private static string GetKey(int applicationId)
        {
            return $"{SHOW_LAST_VERSION}_{applicationId}";
        }

        private async Task<LastVersionModel> ComputeLastVersionAsync(LastVersionSettings configuration)
        {
            ValueContext version = await GetValueAsync(configuration.Version, null).ConfigureAwait(false);
            ValueContext release = await GetValueAsync(configuration.ReleaseDate, version).ConfigureAwait(false);

            DateTime? releaseDate = ParseDate(release?.Value);

            var result = new LastVersionModel()
            {
                ApplicationId = configuration.ApplicationId,
                Version = version?.Value,
                ReleaseDate = releaseDate,
                IsSuccessful = !string.IsNullOrEmpty(version?.Value) && releaseDate.HasValue,
                RawVersion = $"{version?.Value} {{{version?.Content}}}",
                RawReleaseDate = $"{release?.Value} {{{release?.Content}}}"
            };

            return result;
        }

        private async Task<ValueContext> GetValueAsync(ParseModel configuration, ValueContext previous)
        {
            var address = configuration?.Url;
            var inputSelector = configuration?.Selector;

            var content = previous?.Content;

            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(inputSelector))
            {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(address).ConfigureAwait(false);
                await document.WaitForReadyAsync();

                IEnumerable<IElement> cells = document.QuerySelectorAll(inputSelector)?.ToList();

                if(cells.Any() == false) // try to load the html with js
                {
                    string html = await LoadWithPlaywright(address, inputSelector).ConfigureAwait(false);

                    document = await context.OpenAsync(req => req.Content(html)).ConfigureAwait(false);
                    cells = document.QuerySelectorAll(inputSelector)?.ToList();
                }

                _logger.LogInformation($"Document loaded from '{address}'. Size: {document.DocumentElement?.OuterHtml.Length}. Found: {cells.Count()} selector matches.");

                var cellCount = cells.Count();
                if (cellCount == 0)
                {
                    _logger.LogError("no selector match.");
                    return null;
                }
                else if (cellCount == 1)
                {
                    var cell = cells?.FirstOrDefault();
                    content = new ContentContext(cell?.TextContent, cells);
                }
                else
                {
                    content = new ContentContext(null, cells);
                }
            }

            if (string.IsNullOrEmpty(content.Content) && (content.Cells == null))
            {
                _logger.LogError("Content is empty.");
                return null;
            }

            var result = RunCommand(content, configuration.Command);

            return result;
        }

        private ValueContext RunCommand(ContentContext content, string inputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand))
            {
                inputCommand = "trim^|";
            }

            var commands = inputCommand.Split('§', StringSplitOptions.RemoveEmptyEntries);

            ContentContext previousContent = content;
            ContentContext value = null;

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
                    case "index":
                        value = Index(previousContent, commandParams);
                        break;
                    case "con":
                        value = Contains(previousContent, commandParams);
                        break;
                    case "atr":
                        value = Attribute(previousContent, commandParams);
                        break;
                    case "trim":
                        value = Trim(previousContent, commandParams);
                        break;
                    case "split":
                        value = Split(previousContent, commandParams);
                        break;
                    case "substr":
                        value = SubString(previousContent, commandParams);
                        break;
                    default:
                        throw new ArgumentException($"Unknown command {commandName}");
                }

                previousContent = value;
            }

            return new ValueContext(content, value?.Content);
        }

        private ContentContext Index(ContentContext context, string[] commandParams)
        {
            if (context == null || context.Cells.Count() < 1)
            {
                return null;
            }

            var parameter = commandParams[0];
            if (!int.TryParse(parameter, out int index))
            {
                throw new ArgumentException($"Index parameter {parameter} is not a valid integer.");
            }

            var cell = context.Cells.GetItemByIndex(index);

            return new ContentContext(cell?.TextContent, [cell]);
        }

        private ContentContext Contains(ContentContext context, string[] commandParams)
        {
            if (context.Cells == null || context.Cells.Count() == 0)
            {
                return null;
            }

            var parameter = commandParams[0];
            var cell = context.Cells.Where(c => c.TextContent.Contains(parameter, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            return new ContentContext(cell?.TextContent, [cell]);
        }

        private ContentContext Attribute(ContentContext context, string[] commandParams)
        {
            if (context.Cells.Count() != 1)
            {
                return null;
            }

            var cell = context.Cells.First();
            var attributeName = commandParams[0];
            string content = cell.Attributes.Where(a => a.Name == attributeName).FirstOrDefault()?.Value;

            return new ContentContext(content, null);
        }

        private ContentContext Trim(ContentContext context, string[] commandParams)
        {
            if (string.IsNullOrWhiteSpace(context?.Content))
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

            var value = context.Content.Trim();

            if (!string.IsNullOrEmpty(parameterStart))
            {
                value = value.TrimStart(parameterStart.ToArray());
            }

            if (!string.IsNullOrEmpty(parameterEnd))
            {
                value = value.TrimEnd(parameterEnd.ToArray());
            }

            return new ContentContext(value, null);
        }

        private ContentContext Split(ContentContext context, string[] commandParams)
        {
            if (string.IsNullOrWhiteSpace(context?.Content))
            {
                return null;
            }

            if (commandParams.Length < 2)
            {
                _logger.LogError("Split parameters are not collect");
                return null;
            }

            var splits = context.Content.Split(commandParams[0], StringSplitOptions.RemoveEmptyEntries);

            int index = int.Parse(commandParams[1]);
            if (index < 0 || index >= splits.Length)
            {
                _logger.LogError($"Index {index} is out of range for splits array of length {splits.Length}.");
                return null;
            }

            string value = splits[index];

            return new ContentContext(value, null);
        }

        private ContentContext SubString(ContentContext context, string[] commandParams)
        {
            if (string.IsNullOrWhiteSpace(context?.Content))
            {
                return null;
            }

            if (commandParams.Length < 2)
            {
                _logger.LogError("Split parameters are not collect");
                return null;
            }

            if(!int.TryParse(commandParams[0], out int startIndex) 
                || !int.TryParse(commandParams[1], out int length))
            {
                _logger.LogError("Invalid substring parameters");
                return null;
            }   

            var result = context.Content.Substring(startIndex, length);

            return new ContentContext(result, null);
        }

        private DateTime? ParseDate(string input)
        {
            DateTime? releaseDate = null;
            if (!string.IsNullOrEmpty(input))
            {
                if (DateTime.TryParse(input, out DateTime date))
                {
                    releaseDate = date.Date;
                }
                else
                {
                    string[] formats = {
                        "yyyy-MM-dd HH:mm:ss 'UTC'",
                        "yyyy-MM-ddTHH:mm:ssZ",
                        "yyyy-MM-ddTHH-mm-ssZ"
                    };

                    if (DateTime.TryParseExact(input,
                                   formats,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                                   out DateTime dateExact))
                    {
                        releaseDate = dateExact.Date;
                    }
                }
            }

            return releaseDate;
        }

        private async Task<string> LoadWithPlaywright(string address, string inputSelector)
        {
            // 1. Get rendered HTML with Playwright
            using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
            var options = new BrowserTypeLaunchOptions()
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            };
            await using var browser = await playwright.Chromium.LaunchAsync(options).ConfigureAwait(false);
            // var page = await browser.NewPageAsync().ConfigureAwait(false);

            var pageContext = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
            });

            var page = await pageContext.NewPageAsync();

            await page.GotoAsync(address, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 10000
            }).ConfigureAwait(false);

            // Wait for Angular to finish rendering the tags
            await page.WaitForSelectorAsync(inputSelector);

            var html = await page.ContentAsync();

            return html;
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


        private record ValueContext(ContentContext Content, string Value);

        private record ContentContext(string Content, IEnumerable<IElement> Cells)
        {
            public override string ToString()
            {
                var cellsCount = Cells?.Any() == true ? $"/{Cells.Count()} cells" : string.Empty; 

                return $"{Content ?? "?"}{cellsCount}";
            }
        };
    }
}
