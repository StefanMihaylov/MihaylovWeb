using System;
using Mihaylov.Api.Site.Contracts.Helpers.Models;

namespace Mihaylov.Web.Hubs
{
    public interface IProgressReporterFactory
    {
        IProgress<ScanProgressModel> GetLoadingBarReporter(string connectionId, string jsFunctionName);
    }
}
