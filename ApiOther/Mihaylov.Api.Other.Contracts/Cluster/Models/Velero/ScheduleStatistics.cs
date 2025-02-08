namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Velero
{
    public class ScheduleStatistics
    {
        public int ScheduleCount { get; set; }

        public int TotalBackupCount { get; set; }

        public int LastWeekBackupCount { get; set; }

        public int LastDayBackupCount { get; set; }

        public int TotalSuccessfulBackupCount { get; set; }

        public int LastWeekSuccessfulBackupCount { get; set; }

        public int LastDaySuccessfulBackupCount { get; set; }
    }
}
