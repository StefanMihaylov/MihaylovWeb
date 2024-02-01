using System;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.WebUI.Hubs
{
    public interface IProgressReporterFactory
    {
        IProgress<ScanProgressModel> GetLoadingBarReporter(string connectionId, string jsFunctionName);
    }
}
