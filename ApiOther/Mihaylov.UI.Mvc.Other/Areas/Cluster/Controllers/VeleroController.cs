using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Cluster.Models.Velero;

namespace Mihaylov.UI.Mvc.Other.Areas.Cluster.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Area("Cluster")]
    public class VeleroController(IOtherApiClient client) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ScheduleResponse images = await client.SchedulesAsync().ConfigureAwait(false);

            var veleroVersion = string.Empty;

            try
            {
                veleroVersion = await client.ExeVersionAsync().ConfigureAwait(false);
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

            var model = new SchedulesMainModel()
            {
                Statistics = images.Statistics,
                Schedules = images.Schedules.Select(s => new ScheduleViewModel(s)).ToList(),
                Dates = dates,
                VeleroVersion = veleroVersion,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SnapshotIndex()
        {
            SnapshotResponse snapshots = await client.SnapshotsAsync().ConfigureAwait(false);

            return View(snapshots);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBackup(string schedule)
        {
            var request = new CreateBackupModel() { ScheduleName = schedule };
            await client.CreateBackupAsync(request).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBackup(string backup)
        {
            await client.DeleteBackupAsync(backup).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSnapshot(string id)
        {
            await client.DeleteSnapshotAsync(id).ConfigureAwait(false);

            return RedirectToAction(nameof(SnapshotIndex));
        }
    }
}
