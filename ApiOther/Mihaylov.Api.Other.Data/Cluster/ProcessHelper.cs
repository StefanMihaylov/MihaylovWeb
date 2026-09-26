using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;

namespace Mihaylov.Api.Other.Data.Cluster;

public class ProcessHelper : IProcessHelper
{
    private readonly ILogger _logger;
    private readonly ProcessSettings _config;

    public ProcessHelper(ILoggerFactory loggerFactory, IOptions<ProcessSettings> settings)
    {
        _logger = loggerFactory.CreateLogger(this.GetType());
        _config = settings.Value;
    }

    public string ExecuteCommand(string exePath, string command)
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
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true     // Do not create the black window.
            };

            _logger.LogInformation($"Executing command: {procStartInfo.FileName} {procStartInfo.Arguments}");

            using var proc = new Process()
            {
                StartInfo = procStartInfo
            };
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();
            var error = proc.StandardError.ReadToEnd();

            proc.WaitForExit();

            if (proc.ExitCode != 0 && !string.IsNullOrWhiteSpace(error))
            {
                _logger.LogError($"Command execution error: {error}");
                throw new ApplicationException($"Command execution error: {error}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"ExecuteCommand failed. Error: {ex.Message}");
            throw;
        }
    }
}
