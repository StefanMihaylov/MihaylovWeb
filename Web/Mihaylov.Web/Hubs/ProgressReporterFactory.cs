using Microsoft.AspNetCore.SignalR;
using Mihaylov.Site.Media.Models;
using System;

namespace Mihaylov.Web.Hubs
{
    public class ProgressReporterFactory : IProgressReporterFactory
    {
        private readonly IHubContext<ScanProgressHub> _progressHubContext;

        public ProgressReporterFactory(IHubContext<ScanProgressHub> progressHubContext)
        {
            _progressHubContext = progressHubContext;
        }

        public IProgress<ScanProgressModel> GetLoadingBarReporter(string connectionId, string jsFunctionName)
        {
            if (connectionId == null)
            {
                return new Progress<ScanProgressModel>();
            }

            var progress = new Progress<ScanProgressModel>(current =>
            {
                var client = _progressHubContext.Clients.Client(connectionId);
                client.SendAsync(jsFunctionName, current);
            });

            return progress;
        }
    }
}
