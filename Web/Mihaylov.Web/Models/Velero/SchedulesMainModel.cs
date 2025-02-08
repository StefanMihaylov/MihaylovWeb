using System;
using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Velero
{
    public class SchedulesMainModel
    {
        public ScheduleStatistics Statistics { get; set; }

        public IEnumerable<ScheduleViewModel> Schedules { get; set; }

        public IEnumerable<DateTime> Dates { get; set; }

        public string VeleroVersion { get; set; }
    }
}
