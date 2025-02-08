using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class ScheduleResponse
    {
        public ScheduleStatistics Statistics { get; set; }

        public IEnumerable<Schedule> Schedules { get; set; }
    }
}
