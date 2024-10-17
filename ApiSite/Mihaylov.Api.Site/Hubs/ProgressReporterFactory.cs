using Microsoft.AspNetCore.SignalR;
using Mihaylov.Api.Site.Contracts.Hubs;
using System;

namespace Mihaylov.Api.Site.Hubs
{
    public class ProgressReporterFactory : IProgressReporterFactory
    {
        private readonly IHubContext<UpdateProgressHub> _progressHubContext;

        public ProgressReporterFactory(IHubContext<UpdateProgressHub> progressHubContext)
        {
            _progressHubContext = progressHubContext;
        }

        public IProgress<UpdateProgressBarModel> GetLoadingBarReporter(string connectionId, string jsFunctionName)
        {
            if (connectionId == null)
            {
                return new Progress<UpdateProgressBarModel>();
            }

            var progress = new Progress<UpdateProgressBarModel>(current =>
            {
                var client = _progressHubContext.Clients.Client(connectionId);
                client.SendAsync(jsFunctionName, current);
            });

            return progress;
        }
    }
}
