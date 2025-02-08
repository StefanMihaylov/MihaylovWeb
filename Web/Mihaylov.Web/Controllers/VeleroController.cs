using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Velero;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class VeleroController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public VeleroController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _client.AddToken(Request.GetToken());
            ScheduleResponse images = await _client.SchedulesAsync().ConfigureAwait(false);

            var veleroVersion = string.Empty;
            
            try
            {
                veleroVersion = await _client.ExeVersionAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
            }

            var dates = images.Schedules.SelectMany(s => s.Backups)
                                        .Where(b => b.CreatedOn.HasValue)
                                        .Select(b => b.CreatedOn.Value.Date)
                                        .Distinct()
                                        .OrderByDescending(d => d)
                                        .ToList();

            var model = new SchedulesMainModel
            {
                Statistics = images.Statistics,
                Schedules = images.Schedules.Select(s => new ScheduleViewModel(s)).ToList(),
                Dates = dates,
                VeleroVersion = veleroVersion,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBackup(string schedule)
        {
            _client.AddToken(Request.GetToken());
            var request = new CreateBackupModel() { ScheduleName = schedule };
            await _client.CreateBackupAsync(request).ConfigureAwait(false);


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBackup(string backup)
        {
            _client.AddToken(Request.GetToken());
            await _client.DeleteBackupAsync(backup).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
    }
}
