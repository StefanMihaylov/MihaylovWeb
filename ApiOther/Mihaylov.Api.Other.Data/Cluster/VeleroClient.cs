using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class VeleroClient : IVeleroClient
    {
        private readonly ILogger _logger;
        private readonly IProcessHelper _processHelper;
        private readonly IFileWrapper _file;
        private readonly VeleroSettings _config;

        public VeleroClient(ILoggerFactory loggerFactory, IProcessHelper processHelper, IFileWrapper file, IOptions<VeleroSettings> settings)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _processHelper = processHelper;
            _file = file;
            _config = settings.Value;

            _config.VeleroPath = _config.VeleroPath.TrimEnd('/').TrimEnd('\\');
        }

        public string GetVersion()
        {
            var response = GetResponse("version --client-only");

            var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                return response;
            }

            var lineParts = lines[1].Split(":", StringSplitOptions.RemoveEmptyEntries);
            if (lineParts.Length < 2)
            {
                return response;
            }

            var result = lineParts[1].Trim().TrimStart('V').TrimStart('v');
            return result;
        }

        public string CreateBackup(string scheduleName)
        {
            var response = GetResponse($"backup create --from-schedule {scheduleName}");

            return response;
        }

        public string DeleteBackup(string backupName)
        {
            var response = GetResponse($"backup delete {backupName} --confirm");

            return response;
        }

        
        private string GetResponse(string command)
        {
            _logger.LogInformation("Start velero exe command 'command'");

            var veleroPath = _file.GetFilePath(_config.VeleroPath, "velero*");
            if (string.IsNullOrWhiteSpace(veleroPath))
            {
                throw new Exception("velero.exe not found");
            }

            _logger.LogInformation($"Before Execute '{command}' command");

            var result = _processHelper.ExecuteCommand(veleroPath, command);

            _logger.LogInformation($"'{command}' command response: {result}");

            return result;
        }
    }
}
