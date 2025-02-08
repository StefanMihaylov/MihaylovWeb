using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class VeleroClient : IVeleroClient
    {
        public const string VELERO_HTTP_CLIENT = "VeleroClient";

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly VeleroSettings _config;

        public VeleroClient(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IOptions<VeleroSettings> settings)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _httpClientFactory = httpClientFactory;
            _config = settings.Value;

            NormalizeConfig();
        }

        public async Task<string> GetVersionAsync()
        {
            var response = await GetResponseAsync("version --client-only").ConfigureAwait(false);

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

        public async Task<string> CreateBackupAsync(string scheduleName)
        {
            var command = $"backup create --from-schedule {scheduleName}";
            var response = await GetResponseAsync(command).ConfigureAwait(false);

            return response;
        }

        public async Task<string> DeleteBackupAsync(string backupName)
        {
            var command = $"backup delete {backupName} --confirm";
            var response = await GetResponseAsync(command).ConfigureAwait(false);

            return response;
        }

        private async Task<string> GetResponseAsync(string command)
        {
            var velero = VeleroExePath();
            if (string.IsNullOrWhiteSpace(velero))
            {
                Directory.Delete(_config.TempPath, true);
                Directory.CreateDirectory(_config.TempPath);

                var zipFilePah = GetZipFilePah();
                await DownloadVeleroAsync(zipFilePah).ConfigureAwait(false);
                UnzipVelero(zipFilePah);

                var unzipDir = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(zipFilePah));
                var zipDirectory = new DirectoryInfo($"{_config.TempPath}/{unzipDir}");
                var veleroFile = zipDirectory.GetFiles("velero*").FirstOrDefault();
                if (veleroFile == null)
                {
                    throw new ApplicationException("Velero download failed");
                }

                var exeName = Path.GetFileName(veleroFile.FullName);
                velero = $"{_config.VeleroPath}/{exeName}";                

                File.Copy(veleroFile.FullName, velero, true);
                if (!_config.CmdPath.Contains("cmd"))
                {
                    ExecuteCommand("chmod", $"+x {velero}");
                }                
            }

            var result = ExecuteCommand(velero, command);

            return result;
        }

        private async Task DownloadVeleroAsync(string zipFilePah)
        {
            var url = $"{_config.DownloadBasePath}/v{_config.DownloadVersion}/{VeleroZipFileName()}";

            using HttpClient client = _httpClientFactory.CreateClient(VELERO_HTTP_CLIENT);

            using (var stream = await client.GetStreamAsync(url))
            {
                using (var fs = new FileStream(zipFilePah, FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fs);
                }
            }
        }

        private void UnzipVelero(string zipFilePah)
        {
            using Stream stream = File.OpenRead(zipFilePah);
            using var reader = ReaderFactory.Open(stream);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    reader.WriteEntryToDirectory(_config.TempPath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true,
                        PreserveFileTime = true,
                    });
                }
            }
        }

        private string ExecuteCommand(string exePath, string command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
                var procStartInfo = new ProcessStartInfo(_config.CmdPath, $"{_config.CmdArguments} \"{exePath} {command}\"")
                {
                    // The following commands are needed to redirect the standard output. 
                    //This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true     // Do not create the black window.
                };

                _logger.LogInformation($"Executing command: {procStartInfo.FileName} {procStartInfo.Arguments}");

                var proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                string result = proc.StandardOutput.ReadToEnd();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ExecuteCommand failed. Error: {ex.Message}");
                throw;
            }
        }

        private string VeleroZipFileName() => string.Format(_config.DownloadFileName, _config.DownloadVersion);

        private string VeleroExePath()
        {
            var veleroDir = new DirectoryInfo(_config.VeleroPath);
            var valeroFile = veleroDir.GetFiles("velero*").FirstOrDefault();
            if (valeroFile == null)
            {
                return null;
            }

            return valeroFile.FullName;
        }

        private string GetZipFilePah() => $"{_config.TempPath}/{VeleroZipFileName()}";

        private void NormalizeConfig()
        {
            _config.VeleroPath = _config.VeleroPath.TrimEnd('/').TrimEnd('\\');
            _config.TempPath = _config.TempPath.TrimEnd('/').TrimEnd('\\');
            _config.DownloadBasePath = _config.DownloadBasePath.TrimEnd('/').TrimEnd('\\');

            if (!Directory.Exists(_config.VeleroPath))
            {
                Directory.CreateDirectory(_config.VeleroPath);
            }

            if (!Directory.Exists(_config.TempPath))
            {
                Directory.CreateDirectory(_config.TempPath);
            }
        }
    }
}
