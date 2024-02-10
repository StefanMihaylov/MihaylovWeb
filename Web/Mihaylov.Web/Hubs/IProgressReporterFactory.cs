using System;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Web.Hubs
{
    public interface IProgressReporterFactory
    {
        IProgress<ScanProgressModel> GetLoadingBarReporter(string connectionId, string jsFunctionName);
    }
}
