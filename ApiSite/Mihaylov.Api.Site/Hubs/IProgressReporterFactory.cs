using Mihaylov.Api.Site.Contracts.Hubs;
using System;

namespace Mihaylov.Api.Site.Hubs
{
    public interface IProgressReporterFactory
    {
        IProgress<UpdateProgressBarModel> GetLoadingBarReporter(string connectionId, string jsFunctionName);
    }
}
